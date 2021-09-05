using FluentValidation;
using System;
using System.Threading.Tasks;
using System.Threading;
using bike_selling_app.Application.Common.Interfaces;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace bike_selling_app.Application.Bikes.Commands
{
    public class CreateBikeCommandValidator : AbstractValidator<CreateBikeCommand>
    {
        private readonly IApplicationDbContext _context;
        public CreateBikeCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            RuleFor(req => req.bike.DatePurchased).Must(HasValidDateString).WithMessage("Invalid date string. Date string must be a valid date.");
            RuleFor(req => req.bike.ProjectId).MustAsync(HasValidProject).WithMessage("Invalid project id. Project must exist");
        }

        public bool HasValidDateString(string datetime)
        {
            DateTime temp = new DateTime();
            if (!DateTime.TryParse(datetime, out temp))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> HasValidProject(int? projectId, CancellationToken cancellationToken)
        {
            if (!projectId.HasValue)
            {
                return true;
            }
            var project = await _context.GetProjectById(projectId.Value);
            return (project != null);
        }

        // TODO - Create validator to ensure unique entries
    }
}
