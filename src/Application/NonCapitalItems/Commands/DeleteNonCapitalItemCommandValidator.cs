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
    public class DeleteNonCapitalItemCommandValidator : AbstractValidator<DeleteNonCapitalItemCommand>
    {
        private readonly IApplicationDbContext _context;
        public DeleteNonCapitalItemCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            RuleFor(req => req.NonCapitalItemId).MustAsync(ShouldHaveValidNonCapitalItemId).WithMessage("Invalid non-capital item id passed for delete");
        }

        public async Task<bool> ShouldHaveValidNonCapitalItemId(int id, CancellationToken cancellationToken)
        {
            var nonCapitalItems = await _context.GetAllNonCapitalItems();
            return (nonCapitalItems.Count(nci => nci.Id == id) == 0);
        }
    }
}
