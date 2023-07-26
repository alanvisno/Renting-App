using System.ComponentModel.DataAnnotations;

namespace TestCore.Api.Models.Customer
{
    public class CustomerSearchRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
