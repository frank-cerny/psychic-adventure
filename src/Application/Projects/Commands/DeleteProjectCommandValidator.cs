using FluentValidation;
using System;
using System.Threading.Tasks;
using System.Threading;
using bike_selling_app.Application.Common.Interfaces;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace bike_selling_app.Application.Projects.Commands
{
    public class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        private IApplicationDbContext _context;
        public DeleteProjectCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            RuleFor(p => p.projectId).MustAsync(ProjectIdMustExist).WithMessage("Invalid request. Project id must exist before deleting.");
        }

        public async Task<bool> ProjectIdMustExist(int id, CancellationToken cancellationToken)
        {
            var projects = await _context.GetAllProjects();
            return !projects.Select(p => p.Id).ToList().Contains(id);
        }
    }
}