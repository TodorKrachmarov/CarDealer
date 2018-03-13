namespace CarDealer.Services.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ReviewSaleModel
    {
        public int CustomerId { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; }

        public int CarId { get; set; }

        [Display(Name = "Car")]
        public string CarName { get; set; }

        public int Discount { get; set; }

        public double Price { get; set; }

        [Display(Name = "Final Car Price")]
        public double FinalPrice { get; set; }

        public bool IsYoungDriver { get; set; }
    }
}
