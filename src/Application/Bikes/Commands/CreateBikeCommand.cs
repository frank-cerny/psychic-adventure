using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace bike_selling_app.Application.Bikes.Commands
{
    public class CreateBikeCommand : IRequest<Bike>
    {
        public BikeRequestDto bike { get; set; }
    }

    public class CreateBikeCommandHandler : IRequestHandler<CreateBikeCommand, Bike>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public CreateBikeCommandHandler(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task<Bike> Handle(CreateBikeCommand request, CancellationToken cancellationToken)
        {
            var context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            // Parse the object to a bike and add to the database
            Bike newBike = _mapper.Map<Bike>(request.bike);
            context.AddBike(newBike);
            await context.SaveChangesAsync(CancellationToken.None);
            // This id ONLY exists once changes are saved (otherwise the id has not been created yet)
            return newBike;
        }
    }
}
