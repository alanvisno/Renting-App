
using TestCore.Data.DTO;

namespace TestCore.Business
{
    public interface ICarServices
    {
        Task<List<CarDTO>> SearchAvailable(DateTime startDate, DateTime endDate);
        void Create(string description, decimal pricePerDay, DateTime patentDate);
    }
}
