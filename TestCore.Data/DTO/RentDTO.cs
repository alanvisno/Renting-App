using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCore.Data.Entities;

namespace TestCore.Data.DTO
{
    public class RentDTO
    {
        public long BusinessID { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public long CardID { get; set; }
        public string CarDescription { get; set; }
        public decimal CarPrice { get; set; }
        public decimal BasePrice { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? ReturnLogDate { get; set; }
        public DateTime? ReturnProcessDate { get; set; }
        public decimal? Recharge { get; set; }
    }
}
