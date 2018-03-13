using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.App.Models.Sale
{
    public class CreateSaleModel
    {
        [Required]
        public int Customer { get; set; }

        public List<SelectListItem> Customers { get; set; } = new List<SelectListItem>();

        [Required]
        public int Car { get; set; }

        public List<SelectListItem> Cars { get; set; } = new List<SelectListItem>();

        [Required]
        [Range(0, 35, ErrorMessage ="{0} must be beetween 0% and 35%.")]
        public int Discount { get; set; }
    }
}
