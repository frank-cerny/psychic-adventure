using FluentValidation;
using System;
using System.Threading.Tasks;
using System.Threading;
using bike_selling_app.Application.Common.Interfaces;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace bike_selling_app.Application.Projects.Commands
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdateProjectCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            RuleFor(req => req.project).Must(HasValidDateStrings).WithMessage("Invalid date string. Date string must be a valid date or null.");
            RuleFor(req => req.project.BikeIds).MustAsync(HasValidBikeIds).WithMessage("All bike ids must exist");
            // Reference: https://stackoverflow.com/questions/20529085/fluentvalidation-rule-for-multiple-properties
            RuleFor(req => req).MustAsync(HasUniqueTitle).WithMessage("Project title must be unique!");
            RuleFor(req => req.project.NonCapitalItemIds).MustAsync(HasValidNonCapitalIds).WithMessage("All non-capital item ids must be valid");
        }

        public bool HasValidDateStrings(ProjectRequestDto dto)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            // If start date and end date are null, they are valid
            if (dto.DateStarted == null && dto.DateEnded == null)
            {
                return true;
            }
            // If only end date is null, attempt to parse start date
            else if (dto.DateEnded == null)
            {
                if (!DateTime.TryParse(dto.DateStarted, out startDate))
                {
                    return false;
                }
            }
            // If only date started is null, not valid
            else if (dto.DateStarted == null)
            {
                return false;
            }
            // This means both date started and date ended are not null
            else 
            {
                if (!DateTime.TryParse(dto.DateStarted, out startDate))
                {
                    return false;
                }
                if (!DateTime.TryParse(dto.DateEnded, out endDate))
                {
                    return false;
                }
                // Validate that end date comes after or the same as start date
                if (startDate > endDate)
                {
                    return false;
                }
            }
            return true;
        } 
    
        public async Task<bool> HasValidNonCapitalIds(IList<int> ids, CancellationToken cancellationToken)
        {
            var allNonCapitalItems = await _context.GetAllNonCapitalItems();
            foreach (int id in ids)
            {
                if (allNonCapitalItems.Count(nic => nic.Id == id) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> HasValidBikeIds(IList<int> ids, CancellationToken cancellationToken)
        {
            var bikes = await _context.GetAllBikes();
            foreach (int id in ids)
            {
                if (bikes.Count(b => b.Id == id) == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> HasUniqueTitle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            if (request.project.Title == null || request.project.Title.Equals(""))
            {
                return false;
            }
            var projects = await _context.GetAllProjects();
            // If title has not changed, the title is valid
            var currentProject = projects.SingleOrDefault(p => p.Title.Equals(request.project.Title) && p.Id == request.projectId);
            if (currentProject != null)
            {
                return true;
            }
            return !projects.Select(p => p.Title).Contains(request.project.Title);
        }
    }
}