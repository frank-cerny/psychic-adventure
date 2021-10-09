using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace bike_selling_app.Application.ExpenseItems.Commands
{
    public class CreateExpenseItemCommand : IRequest<ExpenseItem>
    {
        public ExpenseItemRequestDto ExpenseItem { get; set; }
    }

    public class CreateExpenseItemCommandHandler : IRequestHandler<CreateExpenseItemCommand, ExpenseItem>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public CreateExpenseItemCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<ExpenseItem> Handle(CreateExpenseItemCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            ExpenseItem newItem = _mapper.Map<ExpenseItem>(request.ExpenseItem);
            context.AddExpenseItem(newItem);
            await context.SaveChangesAsync(cancellationToken);
            return newItem;
        }
    }
}
