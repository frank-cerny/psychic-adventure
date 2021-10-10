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
            // RuleFor(req => req.ExpenseItem.ParentItemId).MustAsync(HasValidParentItemId).WithMessage("Expense item must have a valid parent id");
            RuleFor(req => req.ExpenseItem).MustAsync(HasValidNameDateCombo).WithMessage("Name/Date combination must be unique across all expense items");
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

        public async Task<bool> HasValidParentItemId(int parentId, CancellationToken cancellationToken)
        {
            // Check each item type individually
            var capitalItems = await _context.GetAllCapitalItems();
            if (capitalItems.Select(ci => ci.Id).Contains(parentId))
            {
                return true;
            }
            var nonCapitalItems = await _context.GetAllNonCapitalItems();
            if (nonCapitalItems.Select(nc => nc.Id).Contains(parentId))
            {
                return true;
            }
            var revenueItems = await _context.GetAllRevenueItems();
            if (revenueItems.Select(ri => ri.Id).Contains(parentId))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> HasValidNameDateCombo(ExpenseItemRequestDto item, CancellationToken cancellationToken)
        {
            // TODO - Can we make this better?
            // We incorporate this check here due to the nature of async fluent assertions
            DateTime temp = new DateTime();
            if (!DateTime.TryParse(item.DatePurchased, out temp))
            {
                return false;
            }
            // Since this is a create, NO other object in the database can have the same name/date (update is a bit different)
            var expenseItems = await _context.GetAllExpenseItems();
            var shortRequestDatetime = temp.ToShortDateString();
            return expenseItems.SingleOrDefault(ei => ei.Name.Equals(item.Name) && ei.DatePurchased.ToShortDateString().Equals(shortRequestDatetime)) == null;
        }

        // TODO - Should we ensure all non-null fields are null?
    }
}
