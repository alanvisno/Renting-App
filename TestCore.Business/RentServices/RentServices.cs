using Microsoft.EntityFrameworkCore;
using TestCore.Business.Models;
using TestCore.Data;
using TestCore.Data.Entities;

namespace TestCore.Business
{
    public class RentServices : IRentServices
    {
        private readonly CoreDataContext _context;

        public RentServices(CoreDataContext context)
        {
            _context = context;
        }

        public static bool AreDatesMatching(DateTime deliveryDate, DateTime returnDate, List<RentDate> list)
        {
            return list.Where(x => 
                deliveryDate > x.ReturnDate ||
                returnDate < x.DeliveryDate).Any();
        }

        public static decimal GetDifference(DateTime realReturnDate, DateTime returnDate, decimal price)
        {
            if (realReturnDate <= returnDate)
            {
                //No recharge
                return 0;
            }
            var days = (int)Math.Ceiling((decimal)(realReturnDate - returnDate).TotalDays);
            return days * price;
        }

        public static decimal CalculatePrice(DateTime deliveryDate, DateTime returnDate, decimal price)
        {
            var days = (int)Math.Ceiling((decimal)(returnDate - deliveryDate).TotalDays);
            return days * price;
        }

        public async Task<bool> HasAlreadyARent(Guid customerId, DateTime deliveryDate, DateTime returnDate)
        {
            var rents = await _context.Rents.Where(x =>
                x.Customer.ID == customerId &&
                x.DeliveryDate > DateTime.Now)
                .Select(x => new RentDate { DeliveryDate = x.DeliveryDate, ReturnDate = x.ReturnDate }).ToListAsync();
            return AreDatesMatching(deliveryDate, returnDate, rents);
        }

        public async Task<List<Rent>> GetRents(Guid customerId)
        {
            return await _context.Rents.Include(x => x.Car).Where(x => x.Customer.ID == customerId).ToListAsync();
        }

        public async Task<Car?> GetCar(Guid carId)
        {
            return await _context.Cars.FindAsync(carId);
        }

        public async Task<Customer?> GetCustomer(Guid customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task<Rent?> GetRent(Guid rentId)
        {
            return await _context.Rents.Include(x => x.Car).FirstOrDefaultAsync(x => x.ID == rentId);
        }

        public void Create(Customer customer, Car car, decimal pricePerDay, DateTime deliveryDate, DateTime returnDate)
        {
            try
            {
                var rent = new Rent()
                {
                    Customer = customer,
                    Car = car,
                    BasePrice = CalculatePrice(deliveryDate, returnDate, pricePerDay),
                    DeliveryDate = deliveryDate,
                    ReturnDate = returnDate
                };
                var state = new RentStatus.RentStatus(rent);
                var result = _context.Rents.Add(rent);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async void ReturnCar(Rent rent, DateTime returnDate)
        {
            try
            {
                var state = new RentStatus.RentStatus(rent);
                var stateMessgae = state.MarkReturned();
                //rent.ReturnLogDate = DateTime.Now;
                if (stateMessgae.success)
                { 
                    
                }
                state.Rent.ReturnProcessDate = returnDate;
                state.Rent.Recharge = GetDifference(returnDate, rent.ReturnDate, rent.Car.PricePerDay);
                var result = _context.Rents.Update(state.Rent);
                _context.SaveChanges();
            }
            catch(DbUpdateException ex) 
            {
                throw new Exception(ex.Message);          
            }
        }

        public async void Cancel(Rent rent)
        {
            try
            {
                var state = new RentStatus.RentStatus(rent);
                state.Cancel();
                var result = _context.Rents.Update(state.Rent);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async void Using(Rent rent)
        {
            try
            {
                var state = new RentStatus.RentStatus(rent);
                state.MarkUsing();
                var result = _context.Rents.Update(state.Rent);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
