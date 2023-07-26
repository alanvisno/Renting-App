using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCore.Security.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
    }
}
