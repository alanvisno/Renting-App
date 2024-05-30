using TestCore.Data.DTO;
using TestCore.Data.Entities;

namespace TestCore.Api.Models.Car
{
    public class CarListResponse
    {
        public List<CarDTO> List { get; set; }
    }
}
