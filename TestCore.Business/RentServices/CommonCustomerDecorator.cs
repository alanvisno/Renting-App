using TestCore.Data.Entities;

namespace TestCore.Business
{
    public class CommonCustomerDecorator : RentServiceDecorator
    {
        public CommonCustomerDecorator(IRentServices rentService, bool isPremium)
            : base(rentService, isPremium)
        {
        }

        public override async Task<decimal> ReturnCar(Rent rent, DateTime returnDate)
        {
            if (!_isPremium)
            {
                var difference = GetDifference(returnDate, rent.ReturnDate, rent.Car.PricePerDay, 0);
                var updatedRent = await _rentService.ReturnCar(rent, returnDate, difference);

                var total = updatedRent.Recharge + updatedRent.BasePrice;

                return total;
            }

            return 0;
        }


    }
}
