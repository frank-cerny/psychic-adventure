using FluentValidation;
using System;
using System.Threading.Tasks;
using System.Threading;
using bike_selling_app.Application.Common.Interfaces;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace bike_selling_app.Application.NonCapitalItems.Commands
{
    public class UpdateNonCapitalItemCommandValidator : AbstractValidator<UpdateNonCapitalItemCommand>
    {
        private readonly IApplicationDbContext _context;
        public UpdateNonCapitalItemCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            RuleFor(req => req.NonCapitalItem.DatePurchased).Must(HasValidDateString).WithMessage("Invalid date string. Date string must be a valid date.");
            RuleFor(req => req).MustAsync(HasValidNameDateCombo).WithMessage("Name/Date combination must be unique across all non-capital items");
            RuleFor(req => req.NonCapitalItem.ExpenseItemIds).MustAsync(ShouldHaveValidChildrenIds).WithMessage("All expense item ids must exist");
            RuleFor(req => req.NonCapitalItem).Must(HasAllRequiredValues).WithMessage("Name/Date Purchased cannot be null");
            RuleFor(req => req.NonCapitalItemId).MustAsync(ShouldHaveValidNonCapitalItemId).WithMessage("Invalid non-capital item id passed for update");
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

        public async Task<bool> HasValidNameDateCombo(UpdateNonCapitalItemCommand request, CancellationToken cancellationToken)
        {
            // We incorporate this check here due to the nature of async fluent assertions
            DateTime temp = new DateTime();
            if (!DateTime.TryParse(request.NonCapitalItem.DatePurchased, out temp))
            {
                return false;
            }
            // Since this is a create, NO other object in the database can have the same name/date (update is a bit different)
            var nonCapitalItems = await _context.GetAllNonCapitalItems();
            var shortRequestDatetime = temp.ToShortDateString();
            return nonCapitalItems.SingleOrDefault(ei => ei.Name.Equals(request.NonCapitalItem.Name) && ei.DatePurchased.ToShortDateString().Equals(shortRequestDatetime) && request.NonCapitalItemId != ei.Id) == null;
        }

        public bool HasAllRequiredValues(NonCapitalItemRequestDto item)
        {
            return (item.Name != null && item.DatePurchased != null);
        }

        public async Task<bool> ShouldHaveValidChildrenIds(IList<int> ids, CancellationToken cancellationToken)
        {
            var expenseItems = await _context.GetAllExpenseItems();
            var expenseItemIds = expenseItems.Select(e => e.Id).ToList();
            foreach (int id in ids)
            {
                if (!expenseItemIds.Contains(id))
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> ShouldHaveValidNonCapitalItemId(int id, CancellationToken cancellationToken)
        {
            var nonCapitalItems = await _context.GetAllNonCapitalItems();
            return (nonCapitalItems.Count(nci => nci.Id == id) != 0);
        }
    }
}
