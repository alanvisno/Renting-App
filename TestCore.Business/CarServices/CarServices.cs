using Microsoft.EntityFrameworkCore;
using TestCore.Data;
using TestCore.Data.DTO;
using TestCore.Data.Entities;

namespace TestCore.Business
{
    public class CarServices : ICarServices
    {
        private readonly ICoreDataContext _context;

        public CarServices(ICoreDataContext context)
        {
            _context = context;
        }

        public async Task<List<CarDTO>> SearchAvailable(DateTime startDate, DateTime endDate)
        {
            return await _context.Cars
                .Where(car => !_context.Rents.Any(rental => rental.Car.ID == car.ID && 
                                                  rental.DeliveryDate <= endDate && 
                                                  rental.ReturnDate >= startDate))
                .Select(car => new CarDTO
                {
                    BusinessID = car.BusinessID,
                    Description = car.Description,
                    PricePerDay = car.PricePerDay
                })
                .ToListAsync();
        }

        public void Create(string description, decimal pricePerDay, DateTime patentDate)
        {
            try
            {
                var lastCar = _context.Cars.OrderByDescending(c => c.BusinessID).FirstOrDefault();
                var newBusinessId = (lastCar != null ? lastCar.BusinessID : 0) + 1;
                _context.Cars.Add(new Car()
                {
                    BusinessID = newBusinessId,
                    Description = description.Trim(),
                    PricePerDay = pricePerDay,
                    PatentDate = patentDate
                });
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw new Exception("Error while creating.");
            }
        }
    }
}
