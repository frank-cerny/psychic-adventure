using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace bike_selling_app.Application.Projects.Commands
{
    public class UpdateProjectCommand : IRequest<Project>
    {
        public int projectId { get; set; }
        public ProjectRequestDto project { get; set; }
    }

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Project>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<Project> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var bikes = await context.GetAllBikes();

            // Get the current project based on the id
            var currentProject = await context.GetProjectById(request.projectId);
            // TODO - Get all updated items here
            currentProject.Description = request.project.Description;
            currentProject.Title = request.project.Title;
            // If dates given are not null, parse them
            if (request.project.DateStarted != null)
            {
                currentProject.DateStarted = DateTime.Parse(request.project.DateStarted);
            }
            else
            {
                currentProject.DateStarted = null;
            }
            if (request.project.DateEnded != null)
            {
                currentProject.DateEnded = DateTime.Parse(request.project.DateEnded);
            }
            else 
            {
                currentProject.DateEnded = null;
            }
            // Set list of bikes to empty (this handles not adding duplicates and removing entires no longer required)
            currentProject.Bikes = new List<Bike>();
            // Convert list of ids (for bikes and all items to lists of entities) (Lists default to empty, so we can append straight away)
            foreach (int id in request.project.BikeIds)
            {
                // If the project already contains the bike, do not add it again
                if (!currentProject.Bikes.Select(b => b.Id).ToList().Contains(id))
                {
                    // The validator has validated that ALL ids exist in the database
                    var bike = bikes.SingleOrDefault(b => b.Id == id);
                    currentProject.Bikes.Add(bike);
                }
            }
            // Add all non-capital items
            currentProject.NonCapitalItems.Clear();
            var nonCapitalItems = await context.GetAllNonCapitalItems();
            foreach (int id in request.project.NonCapitalItemIds)
            {
                // Only add the id once
                if (!currentProject.NonCapitalItems.Select(ei => ei.Id).ToList().Contains(id))
                {
                    var nonCapitalItem = nonCapitalItems.SingleOrDefault(ei => ei.Id == id);
                    currentProject.NonCapitalItems.Add(nonCapitalItem);
                }
            }
            // TODO - Add item related ones (capital item, revenue item, non capital item)
            await context.SaveChangesAsync(CancellationToken.None);
            // This id ONLY exists once changes are saved (otherwise the id has not been created yet)
            return currentProject;
        }
    }
}
