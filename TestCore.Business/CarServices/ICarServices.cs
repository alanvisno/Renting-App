
namespace TestCore.Business
{
    public interface ICarServices
    {
        Task<List<Data.Entities.Car>> SearchAvailable(DateTime startDate, DateTime endDate);
        void Create(string description, decimal pricePerDay, DateTime patentDate);
    }
}
