using TestCore.Data.Entities;

namespace TestCore.Business.CustomerServices
{
    public interface ICustomerServices
    {
        Task<List<Customer>> Search(string name);
    }
}
