using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TestCore.Data.Entities
{
    public class Rent : BaseEntity
    {
        public Rent()
        {
            ID = Guid.NewGuid();
            LogDate = DateTime.Now;
            Recharge = 0;
            State = 0;
        }

        [Column("State")]
        [Required]
        public int State { get; set; }

        [Column("LogDate")]
        [Required]
        public DateTime LogDate { get; set; }

        [Column("DeliveryDate")]
        [Required]
        public DateTime DeliveryDate { get; set; }

        [Column("BasePrice")]
        [Required]
        public decimal BasePrice { get; set; }

        [ForeignKey("CarID")]
        [Required]
        public Car Car { get; set; }

        [ForeignKey("CustomerID")]
        [Required]
        [JsonIgnore]
        public Customer Customer { get; set; }

        [Column("ReturnLogDate")]
        public DateTime ReturnLogDate { get; set; }

        [Column("ReturnDate")]
        public DateTime ReturnDate { get; set; }

        [Column("ReturnProcessDate")]
        public DateTime ReturnProcessDate { get; set; }

        [Column("Recharge")]
        public decimal Recharge { get; set; }
    }
}
