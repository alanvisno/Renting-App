using FluentValidation;
using TestCore.Api.Models.Car;
using TestCore.Api.Validators.CarValidators;

namespace TestCore.Api.Validators
{
    public class CarCreateValidator : AbstractValidator<CarCreateRequest>
    {
        private readonly ICarCreateValidatorBase _baseValues;

        public CarCreateValidator(ICarCreateValidatorBase baseValues)
        {
            _baseValues = baseValues;

            RuleFor(x => x.Description).NotEmpty()
                .WithMessage("Description must not be empty");
            RuleFor(x => x.PatentDate).Must(x => x > _baseValues.DateNow().AddYears(_baseValues.YearsPatent()))
                .WithMessage("Patent Date must be newer than " + Math.Abs(_baseValues.YearsPatent()) + " years");
            RuleFor(x => x.PricePerDay).Must(x => x > _baseValues.MoreThanPrice())
                .WithMessage("Price Per Day must be more than " + _baseValues.MoreThanPrice());
        }
    }
}
