using Microsoft.EntityFrameworkCore;
using TestCore.Data;
using TestCore.Data.DTO;
using TestCore.Data.Entities;

namespace TestCore.Business.CustomerServices
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICoreDataContext _context;

        public CustomerServices(ICoreDataContext context)
        {
            _context = context;
        }

        public async Task<List<CustomerDTO>> Search(string name)
        {
            return await _context.Customers.Where(x => EF.Functions.Like(x.FullName, $"%{name.Trim()}%"))
                .Select(x => new CustomerDTO
                {
                    BusinessID = x.BusinessID,
                    FullName = x.FullName
                })
                .ToListAsync();
        }
    }
}
