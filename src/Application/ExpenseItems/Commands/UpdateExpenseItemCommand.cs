using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace bike_selling_app.Application.ExpenseItems.Commands
{
    public class UpdateExpenseItemCommand : IRequest<ExpenseItem>
    {
        public ExpenseItemDto ExpenseItem { get; set; }
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

        // TODO
        public async Task<ExpenseItem> Handle(UpdateExpenseItemCommand request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
