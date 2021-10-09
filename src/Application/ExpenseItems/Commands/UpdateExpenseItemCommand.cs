using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace bike_selling_app.Application.ExpenseItems.Commands
{
    public class UpdateExpenseItemCommand : IRequest<ExpenseItem>
    {
        public ExpenseItemRequestDto ExpenseItem { get; set; }
        public int ExpenseItemId { get; set; }
    }

    public class UpdateExpenseItemCommandHandler : IRequestHandler<UpdateExpenseItemCommand, ExpenseItem>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public UpdateExpenseItemCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<ExpenseItem> Handle(UpdateExpenseItemCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            // Get the current item from the database
            var allExpenseItems = await context.GetAllExpenseItems();
            var currentItem = allExpenseItems.SingleOrDefault(ei => ei.Id == request.ExpenseItemId);
            // Now update all fields based on passed in entity
            var newItem = _mapper.Map<ExpenseItem>(request.ExpenseItem);
            currentItem.Name = newItem.Name;
            currentItem.Description = newItem.Description;
            currentItem.ParentItemId = newItem.ParentItemId;
            currentItem.UnitCost = newItem.UnitCost;
            currentItem.Units = newItem.Units;
            currentItem.DatePurchased = newItem.DatePurchased;
            await context.SaveChangesAsync(cancellationToken);
            return currentItem;
        }
    }
}
