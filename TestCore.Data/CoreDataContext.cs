using Microsoft.EntityFrameworkCore;
using TestCore.Data.Entities;

namespace TestCore.Data
{
    public class CoreDataContext : DbContext
    {
        //public CoreDataContext(DbContextOptions<CoreDataContext> options) : base(options)
        //{
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseSqlServer("Data Source=SQL5103.site4now.net;Initial Catalog=db_a5dc32_cinematest;User Id=db_a5dc32_cinematest_admin;Password=3792Cinema");
        //}

        //public CoreDataContext()
        //{

        //}

        public CoreDataContext(DbContextOptions<CoreDataContext> options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
