using TestCore.Data.DTO;
using TestCore.Data.Entities;

namespace TestCore.Business
{
    public interface IRentServices
    {
        Task CreateRentAsync(Customer customer, Car car, decimal pricePerDay, DateTime deliveryDate, DateTime returnDate);
        bool HasAlreadyARent(long customerId, DateTime deliveryDate, DateTime returnDate);
        Task<Rent> ReturnCar(Rent rent, DateTime returnDate, decimal recharge);
        Task<List<RentDTO>> GetRents(long customerId);
        Task<Rent?> GetRent(long rentId);
        Task<Customer?> GetCustomer(long customerId);
        Task<Car?> GetCar(long carId);
    }
}
