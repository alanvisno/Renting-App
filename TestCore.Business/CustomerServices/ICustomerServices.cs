using TestCore.Data.DTO;
using TestCore.Data.Entities;

namespace TestCore.Business.CustomerServices
{
    public interface ICustomerServices
    {
        Task<List<CustomerDTO>> Search(string name);
    }
}
