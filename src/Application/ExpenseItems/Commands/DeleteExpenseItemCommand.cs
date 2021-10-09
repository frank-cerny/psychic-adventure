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
    public class DeleteExpenseItemCommand : IRequest<ExpenseItem>
    {
        public int ExpenseItemId { get; set; }
    }

    public class DeleteExpenseItemCommandHandler : IRequestHandler<DeleteExpenseItemCommand, ExpenseItem>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public DeleteExpenseItemCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<ExpenseItem> Handle(DeleteExpenseItemCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var allExpenseItems = await context.GetAllExpenseItems();
            var itemToDelete = allExpenseItems.SingleOrDefault(e => e.Id == request.ExpenseItemId);
            context.RemoveExpenseItem(itemToDelete);
            await context.SaveChangesAsync(cancellationToken);
            return itemToDelete;
        }
    }
}
