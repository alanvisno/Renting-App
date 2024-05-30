using Microsoft.EntityFrameworkCore;
using TestCore.Data.Models;
using TestCore.Data;
using TestCore.Data.DTO;
using TestCore.Data.Entities;

namespace TestCore.Business
{
    public class RentServices : IRentServices
    {
        private readonly ICoreDataContext _context;

        public RentServices(ICoreDataContext context)
        {
            _context = context;
        }

        public static bool AreDatesMatching(DateTime deliveryDate, DateTime returnDate, List<RentDateDTO> list)
        {
            return list.Any(x =>
                (deliveryDate <= x.ReturnDate && returnDate >= x.DeliveryDate));
        }

        public static decimal CalculatePrice(DateTime deliveryDate, DateTime returnDate, decimal price)
        {
            var days = (int)Math.Ceiling((decimal)(returnDate - deliveryDate).TotalDays);
            return days * price;
        }

        public bool HasAlreadyARent(long customerId, DateTime deliveryDate, DateTime returnDate)
        {
            var rents = _context.Rents.Where(x =>
                x.Customer.BusinessID == customerId &&
                x.DeliveryDate > DateTime.Now)
                .Select(x => new RentDateDTO { DeliveryDate = x.DeliveryDate, ReturnDate = x.ReturnDate }).ToList();
            return AreDatesMatching(deliveryDate, returnDate, rents);
        }

        public async Task<List<RentDTO>> GetRents(long customerId)
        {
            return await _context.Rents
                .Where(x => x.Customer.BusinessID == customerId)
                .Select(x => new RentDTO
                {
                    BusinessID = x.BusinessID,
                    CustomerID = x.Customer.BusinessID,
                    CustomerName = x.Customer.FullName,
                    CardID = x.Car.BusinessID,
                    CarDescription = x.Car.Description,
                    CarPrice = x.Car.PricePerDay,
                    BasePrice = x.BasePrice,
                    DeliveryDate = x.DeliveryDate,
                    ReturnDate = x.ReturnDate,
                    ReturnLogDate = x.ReturnLogDate,
                    ReturnProcessDate = x.ReturnProcessDate,
                    Recharge = x.Recharge
                })
                .ToListAsync();
        }

        public async Task<Car?> GetCar(long carId)
        {
            return await _context.Cars.FirstOrDefaultAsync(x => x.BusinessID == carId);
        }

        public async Task<Customer?> GetCustomer(long customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.BusinessID == customerId);
        }

        public async Task<Rent?> GetRent(long rentId)
        {
            return await _context.Rents.Include(x => x.Car).FirstOrDefaultAsync(x => x.BusinessID == rentId);
        }

        public async Task CreateRentAsync(Customer customer, Car car, decimal pricePerDay, DateTime deliveryDate, DateTime returnDate)
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
                _context.Rents.Add(rent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new Exception("Error while creating.");
            }
        }

        public async Task<Rent> ReturnCar(Rent rent, DateTime returnDate, decimal recharge)
        {
            try
            {
                rent.ReturnLogDate = DateTime.Now;
                rent.ReturnProcessDate = returnDate;
                rent.Recharge = recharge;
                var result = _context.Rents.Update(rent);
                _context.SaveChanges();

                return result.Entity;
            }
            catch(DbUpdateException) 
            {
                throw new Exception("Error while updating.");
            }
        }
    }
}
