using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestCore.Data.Entities
{
    public class BaseEntity
    {
        [Column("ID")]
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
    }
}
