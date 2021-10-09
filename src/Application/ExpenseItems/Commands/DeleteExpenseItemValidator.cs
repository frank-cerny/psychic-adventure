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
    // TODO
    public class DeleteExpenseItemCommandValidator : AbstractValidator<DeleteExpenseItemCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteExpenseItemCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
        }
    }
}
