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
    public class DeleteCapitalItemCommand : IRequest<CapitalItem>
    {
        public int CapitalItemId { get; set; }
    }

    public class DeleteCapitalItemCommandHandler : IRequestHandler<DeleteCapitalItemCommand, CapitalItem>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public DeleteCapitalItemCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<CapitalItem> Handle(DeleteCapitalItemCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var allCapitalItems = await context.GetAllCapitalItems();
            // The item is guranteet to exist because of the validator
            var oldCapitalItem = allCapitalItems.SingleOrDefault(ci => ci.Id == request.CapitalItemId);
            context.RemoveCapitalItem(oldCapitalItem);
            await context.SaveChangesAsync(cancellationToken);
            return oldCapitalItem;
        }
    }
}
