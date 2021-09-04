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
    public class UpdateBikeCommand : IRequest<Bike>
    {
        public BikeRequestDto bike { get; set; }
        public int bikeId { get; set; }
    }

    public class UpdateBikeCommandHandler : IRequestHandler<UpdateBikeCommand, Bike>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public UpdateBikeCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<Bike> Handle(UpdateBikeCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            // Parse the object to a bike and add to the database
            Bike newBike = _mapper.Map<Bike>(request.bike);
            // Get the old bike from the database based on id and update its fields
            var bikes = await context.GetAllBikes();
            var oldBike = bikes.SingleOrDefault(b => b.Id == request.bikeId);
            oldBike.Make = newBike.Make;
            oldBike.Model = newBike.Model;
            oldBike.PurchasedFrom = newBike.PurchasedFrom;
            oldBike.PurchasePrice = newBike.PurchasePrice;
            oldBike.SerialNumber = newBike.SerialNumber;
            // TODO - Test if this actually lets us update project reference
            oldBike.ProjectId = newBike.ProjectId;
            oldBike.DatePurchased = newBike.DatePurchased;
            await context.SaveChangesAsync(CancellationToken.None);
            // This id ONLY exists once changes are saved (otherwise the id has not been created yet)
            return oldBike;
        }
    }
}
