using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace bike_selling_app.Application.CapitalItems.Commands
{
    public class CreateCapitalItemCommand : IRequest<CapitalItem>
    {
        public CapitalItemRequestDto CapitalItem { get; set; }
    }

    public class CreateCapitalItemCommandHandler : IRequestHandler<CreateCapitalItemCommand, CapitalItem>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public CreateCapitalItemCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        // TODO
        public async Task<CapitalItem> Handle(CreateCapitalItemCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            // NonCapitalItem newItem = _mapper.Map<NonCapitalItem>(request.NonCapitalItem);
            // // Add expense items as well (all items exist based on validator)
            // var expenseItems = await context.GetAllExpenseItems();
            // foreach (int id in request.NonCapitalItem.ExpenseItemIds)
            // {
            //     // TODO do we need to update the foreign key of the expense item here?
            //     // Only add the id once
            //     if (!newItem.ExpenseItems.Select(ei => ei.Id).ToList().Contains(id))
            //     {
            //         var expenseItem = expenseItems.SingleOrDefault(ei => ei.Id == id);
            //         newItem.ExpenseItems.Add(expenseItem);
            //     }
            // }
            // context.AddNonCapitalItem(newItem);
            // await context.SaveChangesAsync(cancellationToken);
            return null;
        }
    }
}
