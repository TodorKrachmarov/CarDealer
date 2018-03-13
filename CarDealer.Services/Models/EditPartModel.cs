namespace CarDealer.Services.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EditPartModel
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Price can not be empty!")]
        [Range(1, double.MaxValue,
            ErrorMessage = "Price must be a positive number!")]
        public double Price { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Quantity must be a positive number!")]
        public int? Quantity { get; set; }
    }
}
