using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestCore.Data.Entities
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
            ID = Guid.NewGuid();
        }

        [Column("BusinessID")]
        [Required]
        public long BusinessID { get; set; }

        [Column("Premium")]
        [Required]
        public bool Premium { get; set; }

        [Column("FullName")]
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [Column("Document")]
        [Required]
        [MaxLength(20)]
        public string Document { get; set; }

        [Column("Nationality")]
        [MaxLength(3)]
        [Required]
        public string Nationality { get; set; }
    }
}
