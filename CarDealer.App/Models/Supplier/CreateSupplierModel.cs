﻿using System.ComponentModel.DataAnnotations;

namespace CarDealer.App.Models.Supplier
{
    public class CreateSupplierModel
    {
        [Required]
        [MinLength(4, ErrorMessage ="{0} must be at least 4 symbols")]
        [MaxLength(50, ErrorMessage = "{0} can not be more than 50 symbols")]
        public string Name { get; set; }

        [Required]
        public bool IsImporter { get; set; }
    }
}
