using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace bike_selling_app.Application.Bikes.Commands
{
    public class DeleteBikeCommand : IRequest<Bike>
    {
        public int bikeId { get; set; }
    }

    public class DeleteBikeCommandHandler : IRequestHandler<DeleteBikeCommand, Bike>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public DeleteBikeCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<Bike> Handle(DeleteBikeCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var bikes = await context.GetAllBikes();
            var bike = bikes.SingleOrDefault(b => b.Id == request.bikeId);
            context.RemoveBike(bike);
            await context.SaveChangesAsync(cancellationToken);
            return bike;
        }
    }
}