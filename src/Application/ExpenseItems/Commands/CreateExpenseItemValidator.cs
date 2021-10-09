using FluentValidation;
using System;
using System.Threading.Tasks;
using System.Threading;
using bike_selling_app.Application.Common.Interfaces;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace bike_selling_app.Application.ExpenseItems.Commands
{
    public class CreateExpenseItemCommandValidator : AbstractValidator<CreateExpenseItemCommand>
    {
        private readonly IApplicationDbContext _context;
        public CreateExpenseItemCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            RuleFor(req => req.ExpenseItem.DatePurchased).Must(HasValidDateString).WithMessage("Invalid date string. Date string must be a valid date.");
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

        // TODO - Should have valid parent item!


        // TODO - Create validator to ensure unique entries
    }
}
