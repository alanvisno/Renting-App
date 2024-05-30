using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestCore.Data.Entities
{
    public class Car : BaseEntity
    {
        public Car()
        {
            ID = Guid.NewGuid();
            LogDate = DateTime.Now;
        }

        [Column("BusinessID")]
        [Required]
        public long BusinessID { get; set; }

        [MaxLength(20)]
        [Column("Description")]
        [Required]
        public string Description { get; set; }

        [Column("LogDate")]
        [Required]
        [JsonIgnore]
        public DateTime LogDate { get; set; }

        [Column("PatentDate")]
        [Required]
        [JsonIgnore]
        public DateTime PatentDate { get; set; }

        [Column("PricePerHour")]
        [Required]
        public decimal PricePerDay { get; set; }
    }
}
