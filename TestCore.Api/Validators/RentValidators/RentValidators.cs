using FluentValidation;
using TestCore.Api.Models.Car;
using TestCore.Api.Models.Rent;

namespace TestCore.Api.Validators.RentValidators
{
    public class RentCreateValidator : AbstractValidator<RentCreateRequest>
    {
        public RentCreateValidator()
        {
            RuleFor(x => x.CustomerId).NotNull()
                .WithMessage("Customer must not be empty");
            RuleFor(x => x.DeliveryDate).Must(p => p > DateTime.Now.Date)
                .WithMessage("Delivery Date must not be today");
            RuleFor(x => x.ReturnDate).GreaterThan(x => x.DeliveryDate)
                .WithMessage("Return Date must be after Delivery");
        }
    }
}
