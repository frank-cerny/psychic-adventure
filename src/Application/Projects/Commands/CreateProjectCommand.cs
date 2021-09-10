using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace bike_selling_app.Application.Projects.Commands
{
    public class CreateProjectCommand : IRequest<Project>
    {
        public ProjectRequestDto project { get; set; }
    }

    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Project>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public CreateProjectCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var bikes = await context.GetAllBikes();
            // TODO - Get all items here
            var newProject = new Project();
            newProject.Description = request.project.Description;
            // If dates given are not null, parse them
            if (request.project.DateStarted != null)
            {
                newProject.DateStarted = DateTime.Parse(request.project.DateStarted);
            }
            if (request.project.DateEnded != null)
            {
                newProject.DateEnded = DateTime.Parse(request.project.DateEnded);
            }
            // Convert list of ids (for bikes and all items to lists of entities) (Lists default to empty, so we can append straight away)
            foreach (int id in request.project.BikeIds)
            {
                // The validator has validated that ALL ids exist in the database
                var bike = bikes.SingleOrDefault(b => b.Id == id);
                newProject.Bikes.Add(bike);
            }
            // TODO - Add item related ones (capital item, revenue item, non capital item)
            context.AddProject(newProject);
            await context.SaveChangesAsync(CancellationToken.None);
            // This id ONLY exists once changes are saved (otherwise the id has not been created yet)
            return newProject;
        }
    }
}
