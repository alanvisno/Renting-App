using TestCore.Data.DTO;

namespace TestCore.Api.Models.Customer
{
    public class CustomerListResponse
    {
        public List<CustomerDTO> Customers { get; set; }
    }
}
