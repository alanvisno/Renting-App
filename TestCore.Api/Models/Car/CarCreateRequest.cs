using System.ComponentModel.DataAnnotations;

namespace TestCore.Api.Models.Car
{
    public class CarCreateRequest
    {
        public string Description { get; set; }

        public decimal PricePerDay { get; set; }

        [DataType(DataType.Date)]
        public DateTime PatentDate { get; set; } 
    }
}
