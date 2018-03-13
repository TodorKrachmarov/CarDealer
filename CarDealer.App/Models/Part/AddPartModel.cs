namespace CarDealer.App.Models.Part
{
    using CarDealer.Services.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddPartModel
    {
        [Required(ErrorMessage = "Name can not be empty!")]
        [MinLength(4)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price can not be empty!")]
        [Range(1, double.MaxValue,
            ErrorMessage = "Price must be a positive number!")]
        public double Price { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Quantity must be a positive number!")]
        public int? Quantity { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }

        public List<SupplierModel> Suppliers { get; set; } = new List<SupplierModel>();
    }
}
//string name, double price, int supplierId, int quantity