using System.ComponentModel.DataAnnotations;

namespace TestCore.Api.Models.Rent
{
    public class RentCreateRequest
    {
        [Required]
        public long CustomerId { get; set; }

        [Required]
        public long CarId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }
    }
}
