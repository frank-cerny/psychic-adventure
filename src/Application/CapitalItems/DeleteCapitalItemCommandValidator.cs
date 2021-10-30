using FluentValidation;
using System;
using System.Threading.Tasks;
using System.Threading;
using bike_selling_app.Application.Common.Interfaces;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace bike_selling_app.Application.CapitalItems.Commands
{
    public class DeleteCapitalItemCommandValidator : AbstractValidator<DeleteCapitalItemCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteCapitalItemCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            RuleFor(req => req.CapitalItemId).MustAsync(ShouldHaveValidCapitalItemId).WithMessage("Invalid capital item id passed for delete");
        }

        public async Task<bool> ShouldHaveValidCapitalItemId(int id, CancellationToken cancellationToken)
        {
            var capitalItems = await _context.GetAllCapitalItems();
            return (capitalItems.Count(ci => ci.Id == id) != 0);
        }
    }
}
