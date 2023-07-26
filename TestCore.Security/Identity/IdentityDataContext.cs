using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TestCore.Security.Identity
{
    public class IdentityDataContext : IdentityDbContext<User>
    {
        public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
        {
        }
    }

    public record AuthenticateRequest(string UserName, string Password);
}
