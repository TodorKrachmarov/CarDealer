namespace CarDealer.App.Models.Customer
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateCustomerModel
    {
        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
    }
}
