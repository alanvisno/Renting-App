﻿using System.ComponentModel.DataAnnotations;

namespace TestCore.Api.Models.Rent
{
    public class RentReturnRequest
    {
        [Required]
        public long RentId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
    }
}
