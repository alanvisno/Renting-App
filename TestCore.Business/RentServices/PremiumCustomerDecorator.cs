using TestCore.Data.Entities;

namespace TestCore.Business
{
    public class PremiumCustomerDecorator : RentServiceDecorator
    {
        public PremiumCustomerDecorator(IRentServices rentService, bool isPremium)
            : base(rentService, isPremium)
        {
        }

        private readonly static decimal _discount = 0.9m;
        private readonly static int _daysAllowed = 1;

        public override async Task<decimal> ReturnCar(Rent rent, DateTime returnDate)
        {
            if (_isPremium)
            {
                var difference = GetDifference(returnDate, rent.ReturnDate, rent.Car.PricePerDay, _daysAllowed);
                var updatedRent = await _rentService.ReturnCar(rent, returnDate, difference);

                var total = updatedRent.Recharge + updatedRent.BasePrice;
                total *= _discount;

                return total;
            }

            return 0;
        }
    }
}
