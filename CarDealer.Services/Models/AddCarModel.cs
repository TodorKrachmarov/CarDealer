namespace CarDealer.Services.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddCarModel
    {
        [Required]
        [MinLength(2, ErrorMessage ="{0} must be at least {1} symbols!")]
        [MaxLength(30, ErrorMessage = "{0} can not be more than {1} symbols!")]
        public string Make { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "{0} must be at least {1} symbols!")]
        [MaxLength(30, ErrorMessage = "{0} can not be more than {1} symbols!")]
        public string Model { get; set; }

        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "{0} must be a positive number!")]
        public long TravelledDistance { get; set; }

        [Required]
        public List<int> PartsIds { get; set; } = new List<int>();

        public List<SelectListItem> Parts { get; set; } = new List<SelectListItem>();
    }
}
