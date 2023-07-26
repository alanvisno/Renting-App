namespace TestCore.Api.Validators.CarValidators
{
    public class CarCreateValidatorBase : ICarCreateValidatorBase
    {
        public DateTime DateNow()
        {
            return DateTime.Now;
        }

        public int YearsPatent()
        {
            return -5;
        }

        public decimal MoreThanPrice()
        {
            return 0;
        }
    }
}
