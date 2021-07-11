using bike_selling_app.Application.Common.Interfaces;
using bike_selling_app.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace bike_selling_app.Application.Bikes.Commands
{
    public class CreateBikeCommand : IRequest<int>
    {
        public BikeRequestDto bike { get; set; }
    }

    public class CreateTodoItemCommandHandler : IRequestHandler<CreateBikeCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateTodoItemCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateBikeCommand request, CancellationToken cancellationToken)
        {
            // Parse the object to a bike and add to the database
            Bike newBike = _mapper.Map<Bike>(request.bike);
            await _context.AddBike(newBike);
            return newBike.Id;
        }
    }
}
