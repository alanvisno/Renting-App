namespace TestCore.Api.Validators.CarValidators
{
    public interface ICarCreateValidatorBase
    {
        DateTime DateNow();

        int YearsPatent();

        decimal MoreThanPrice();
    }
}
