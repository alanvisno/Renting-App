using Microsoft.EntityFrameworkCore;
using TestCore.Data.Entities;

namespace TestCore.Data
{
    public class CoreDataContext : DbContext
    {

        public CoreDataContext(DbContextOptions<CoreDataContext> options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
