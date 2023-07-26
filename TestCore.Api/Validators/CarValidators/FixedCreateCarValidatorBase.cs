namespace TestCore.Api.Validators.CarValidators
{
    public class FixedCarCreateValidatorBase : ICarCreateValidatorBase
    {
        private readonly DateTime _datetime;
        private readonly int _years;
        private readonly decimal _price;

        public FixedCarCreateValidatorBase(DateTime datetime, int years, decimal price) 
        { 
            _datetime = datetime;
            _years = years;
            _price = price;
        }

        public DateTime DateNow()
        {
            return _datetime;
        }

        public int YearsPatent()
        { 
            return _years;
        }

        public decimal MoreThanPrice()
        { 
            return _price;
        }
    }
}
