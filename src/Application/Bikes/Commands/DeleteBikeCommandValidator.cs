using FluentValidation;
using System;
using System.Threading.Tasks;
using System.Threading;
using bike_selling_app.Application.Common.Interfaces;
using System.Linq;

namespace bike_selling_app.Application.Bikes.Commands
{
    public class DeleteBikeCommandValidator : AbstractValidator<DeleteBikeCommand>
    {
        private IApplicationDbContext _context;
        public DeleteBikeCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            RuleFor(b => b.bikeId).MustAsync(BikeIdMustExist).WithMessage("Invalid request. Bike id must exist before deleting.");
        }

        public async Task<bool> BikeIdMustExist(int id, CancellationToken cancellationToken)
        {
            var bikes = await _context.GetAllBikes();
            return (bikes.SingleOrDefault(b => b.Id == id) != null);
        }

    }
}