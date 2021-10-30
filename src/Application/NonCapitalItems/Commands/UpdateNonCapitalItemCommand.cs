using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace bike_selling_app.Application.NonCapitalItems.Commands
{
    public class UpdateNonCapitalItemCommand : IRequest<NonCapitalItem>
    {
        public NonCapitalItemRequestDto NonCapitalItem { get; set; }
        public int NonCapitalItemId { get; set; }
    }

    public class UpdateNonCapitalItemCommandHandler : IRequestHandler<UpdateNonCapitalItemCommand, NonCapitalItem>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public UpdateNonCapitalItemCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        
        public async Task<NonCapitalItem> Handle(UpdateNonCapitalItemCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var newItem = _mapper.Map<NonCapitalItem>(request.NonCapitalItem);
            var allNonCapitalItems = await context.GetAllNonCapitalItems();
            var currentItem = allNonCapitalItems.SingleOrDefault(nci => nci.Id == request.NonCapitalItemId);
            // Update all values
            currentItem.Name = newItem.Name;
            currentItem.Description = newItem.Description;
            currentItem.DatePurchased = newItem.DatePurchased;
            currentItem.UnitCost = newItem.UnitCost;
            currentItem.UnitsPurchased = newItem.UnitsPurchased;
            currentItem.UnitsRemaining = newItem.UnitsRemaining;
            currentItem.ProjectId = newItem.ProjectId;
            // Now update any expense items (clear old ones first)
            currentItem.ExpenseItems.Clear();
            var expenseItems = await context.GetAllExpenseItems();
            foreach (int id in request.NonCapitalItem.ExpenseItemIds)
            {
                // Only add the id once
                if (!currentItem.ExpenseItems.Select(ei => ei.Id).ToList().Contains(id))
                {
                    var expenseItem = expenseItems.SingleOrDefault(ei => ei.Id == id);
                    currentItem.ExpenseItems.Add(expenseItem);
                }
            }
            await context.SaveChangesAsync(cancellationToken);
            return currentItem;
        }
    }
}
