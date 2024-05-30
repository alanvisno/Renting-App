using FluentValidation.TestHelper;
using TestCore.Api.Models.Car;
using TestCore.Api.Validators;
using TestCore.Api.Validators.CarValidators;

namespace TestCore.Test.Car
{
    public class CarCreateValidatorShould
    {
        private readonly CarCreateValidator Validator;
        private readonly static ICarCreateValidatorBase _baseValues = new FixedCarCreateValidatorBase(DateTime.Now, -5, 0);

        public CarCreateValidatorShould()
        {
            Validator = new CarCreateValidator(_baseValues);
        }

        [Theory]
        [TestCase(null)]
        [TestCase("")]
        public void Should_have_error_for_Description(string description)
        {
            var model = new CarCreateRequest { Description = description };
            var result = Validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Theory]
        [TestCase("Description")]
        public void Should_not_have_error_for_Description(string description)
        {
            var model = new CarCreateRequest { Description = description };
            var result = Validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
        }

        [Theory]
        [TestCase(5, 7)]
        [TestCase(6, 5)]
        [TestCase(1, 6)]
        public void Should_have_error_for_PatentDate(int month, int years)
        {
            var patentDate = _baseValues.DateNow().AddYears(-years).AddMonths(-month);
            var model = new CarCreateRequest { PatentDate = patentDate };
            var result = Validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PatentDate);
        }

        [Theory]
        [TestCase(1, 0)]
        [TestCase(11, 4)]
        [TestCase(1, 2)]
        [TestCase(1, 1)]
        public void Should_not_have_error_for_PatentDay(int month, int years)
        {
            var patentDate = _baseValues.DateNow().AddYears(-years).AddMonths(-month);
            var model = new CarCreateRequest { PatentDate = patentDate };
            var result = Validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PatentDate);
        }

        [Theory]
        [TestCase(0.1)]
        [TestCase(20)]
        [TestCase(40)]
        [TestCase(65)]
        public void Should_not_have_error_for_PricePerHour(decimal price)
        {
            var model = new CarCreateRequest { PricePerDay = price };
            var result = Validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.PricePerDay);
        }

        [Theory]
        [TestCase(0)]
        public void Should_have_error_for_PricePerHour(decimal price)
        {
            var model = new CarCreateRequest { PricePerDay = price };
            var result = Validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.PricePerDay);
        }
    }
}
