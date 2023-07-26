using TestCore.Data.Entities;

namespace TestCore.Business
{
    public interface IRentServices
    {
        void CreateRent(Customer customer, Car car, decimal pricePerDay, DateTime deliveryDate, DateTime returnDate);
        Task<bool> HasAlreadyARent(Guid customerId, DateTime deliveryDate, DateTime returnDate);
        void ReturnCar(Rent rent, DateTime returnDate);
        Task<List<Rent>> GetRents(Guid customerId);
        Task<Rent?> GetRent(Guid rentId);
        Task<Customer?> GetCustomer(Guid customerId);
        Task<Car?> GetCar(Guid carId);
    }
}
