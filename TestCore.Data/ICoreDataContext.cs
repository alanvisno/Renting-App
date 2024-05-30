using Microsoft.EntityFrameworkCore;
using TestCore.Data.Entities;

namespace TestCore.Data
{
    public interface ICoreDataContext
    {
        DbSet<Rent> Rents { get; set; }
        DbSet<Car> Cars { get; set; }
        DbSet<Customer> Customers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
    }
}
