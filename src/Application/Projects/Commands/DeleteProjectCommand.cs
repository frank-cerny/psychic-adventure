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
    public class DeleteProjectCommand : IRequest<Project>
    {
        public int projectId { get; set; }
    }

    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Project>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public DeleteProjectCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<Project> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var project = await context.GetProjectById(request.projectId);
            context.RemoveProject(project);
            await context.SaveChangesAsync(CancellationToken.None);
            // This id ONLY exists once changes are saved (otherwise the id has not been created yet)
            return project;
        }
    }
}
