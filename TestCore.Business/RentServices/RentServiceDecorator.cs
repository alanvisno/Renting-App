using TestCore.Data.Entities;

namespace TestCore.Business
{
    public abstract class RentServiceDecorator
    {
        protected readonly IRentServices _rentService;
        protected readonly bool _isPremium;

        protected RentServiceDecorator(IRentServices rentService, bool isPremium)
        {
            _rentService = rentService;
            _isPremium = isPremium;
        }

        public abstract Task<decimal> ReturnCar(Rent rent, DateTime returnDate);

        public static decimal GetDifference(DateTime realReturnDate, DateTime returnDate, decimal price, int daysAllowed)
        {
            if (realReturnDate <= returnDate)
            {
                //No recharge
                return 0;
            }
            var days = (int)Math.Ceiling((decimal)(realReturnDate - returnDate).TotalDays);
            return (days - daysAllowed) * price;
        }
    }
}
