using System.ComponentModel.DataAnnotations;

namespace TestCore.Api.Models.Car
{
    public class CarListRequest
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
    }
}
