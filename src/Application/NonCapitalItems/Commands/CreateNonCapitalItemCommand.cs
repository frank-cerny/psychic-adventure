using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace bike_selling_app.Application.NonCapitalItems.Commands
{
    public class CreateNonCapitalItemCommand : IRequest<NonCapitalItem>
    {
        public NonCapitalItemRequestDto NonCapitalItem { get; set; }
    }

    public class CreateNonCapitalItemCommandHandler : IRequestHandler<CreateNonCapitalItemCommand, NonCapitalItem>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public CreateNonCapitalItemCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        // TODO 
        public async Task<NonCapitalItem> Handle(CreateNonCapitalItemCommand request, CancellationToken cancellationToken)
        {
            // var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            // ExpenseItem newItem = _mapper.Map<ExpenseItem>(request.ExpenseItem);
            // context.AddExpenseItem(newItem);
            // await context.SaveChangesAsync(cancellationToken);
            // return newItem;
            return null;
        }
    }
}
