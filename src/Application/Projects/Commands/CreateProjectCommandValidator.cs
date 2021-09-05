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
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        private readonly IApplicationDbContext _context;
        public CreateProjectCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            RuleFor(req => req.project.DateStarted).Must(HasValidDateString).WithMessage("Invalid date string. Date string must be a valid date or null.");
            RuleFor(req => req.project.DateEnded).Must(HasValidDateString).WithMessage("Invalid date string. Date string must be a valid date or null.");
            RuleFor(req => req.project.BikeIds).MustAsync(HasValidBikeIds).WithMessage("All bike ids must exist");
        }

        // TODO - Add validation for all item based things

        public bool HasValidDateString(string datetime)
        {
            if (datetime == null)
            {
                return true;
            }
            DateTime temp = new DateTime();
            if (!DateTime.TryParse(datetime, out temp))
            {
                return false;
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
    }
}