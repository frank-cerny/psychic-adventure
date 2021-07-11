using FluentValidation;
using System;

namespace bike_selling_app.Application.Bikes.Commands
{
    public class CreateBikeCommandValidator : AbstractValidator<CreateBikeCommand>
    {
        public CreateBikeCommandValidator()
        {
            RuleFor(req => req.bike.DatePurchased).Must(HasValidDateString).WithMessage("Invalid date string. Date string must be a valid date.");
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
    }
}
