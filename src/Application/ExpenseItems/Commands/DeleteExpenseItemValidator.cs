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
    public class DeleteExpenseItemCommandValidator : AbstractValidator<DeleteExpenseItemCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteExpenseItemCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            RuleFor(r => r.ExpenseItemId).MustAsync(HasValidExpenseId).WithMessage("Expense Item to be deleted cannot be found.");
        }
        public async Task<bool> HasValidExpenseId(int itemId, CancellationToken cancellationToken)
        {
            var allExpenseItems = await _context.GetAllExpenseItems();
            return allExpenseItems.Select(e => e.Id).Contains(itemId);
        }
    }
}
