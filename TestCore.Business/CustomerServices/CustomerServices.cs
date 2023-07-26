using Microsoft.EntityFrameworkCore;
using TestCore.Data;
using TestCore.Data.Entities;

namespace TestCore.Business.CustomerServices
{
    public class CustomerServices : ICustomerServices
    {
        private readonly CoreDataContext _context;

        public CustomerServices(CoreDataContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> Search(string name)
        {
            return await _context.Customers.Where(
                x => x.FullName.ToLower().Contains(name.Trim().ToLower())).ToListAsync();
        }
    }
}
