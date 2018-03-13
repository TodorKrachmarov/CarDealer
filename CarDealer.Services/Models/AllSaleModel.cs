namespace CarDealer.Services.Models
{
    public class AllSaleModel
    {
        public int Id { get; set; }

        public string CarModel { get; set; }

        public string CustomerName { get; set; }

        public double? Price { get; set; }

        public double? PriceWithDis { get; set; }

        public double Discount { get; set; }
    }
}
