using Microsoft.EntityFrameworkCore;
using System;
using TestCore.Data;
using TestCore.Data.Entities;

namespace TestCore.Business
{
    public class CarServices : ICarServices
    {
        private readonly CoreDataContext _context;

        public CarServices(CoreDataContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> SearchAvailable(DateTime startDate, DateTime endDate)
        {
                var query = from car in _context.Cars
                            where !_context.Rents.Any(rent =>
                                  (startDate <= rent.ReturnDate && endDate >= rent.DeliveryDate)
                                  && rent.Car.ID == car.ID)
                            select new Car
                            {
                                ID = car.ID,
                                Description = car.Description,
                                PricePerDay = car.PricePerDay
                            };

                return await query.ToListAsync();
        }

        public void Create(string description, decimal pricePerDay, DateTime patentDate)
        {
            try
            {
                _context.Cars.Add(new Car()
                {
                    Description = description.Trim(),
                    PricePerDay = pricePerDay,
                    PatentDate = patentDate
                });
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
