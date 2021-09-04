using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using bike_selling_app.Application.Common.Interfaces;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace bike_selling_app.Application.Bikes.Commands
{
    public class UpdateBikeCommandValidator : AbstractValidator<UpdateBikeCommand>
    {
        private IApplicationDbContext _context;
        public UpdateBikeCommandValidator(IServiceScopeFactory scopeFactory)
        {
            _context = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IApplicationDbContext>();
            RuleFor(req => req.bike.DatePurchased).Must(HasValidDateString).WithMessage("Invalid date string. Date string must be a valid date.");
            RuleFor(req => req.bike).Must(HasAllNonNullFieldsWhereApplicable).WithMessage("All fields must be non-null (except ProjectId)");
            RuleFor(b => b.bikeId).MustAsync(BikeIdMustExist).WithMessage("Invalid request. Bike id must exist when updating");
        }

        public bool HasValidDateString(string datetime)
        {
            DateTime temp = new DateTime();
            if (!DateTime.TryParse(datetime, out temp))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> BikeIdMustExist(int id, CancellationToken cancellationToken)
        {
            var bikes = await _context.GetAllBikes();
            return (bikes.SingleOrDefault(b => b.Id == id) != null);
        }

        public bool HasAllNonNullFieldsWhereApplicable(BikeRequestDto bike)
        {
            return (bike.DatePurchased != null & bike.Make != null & bike.Model != null & bike.PurchasedFrom != null & bike.SerialNumber != null);
        }
    }
}
