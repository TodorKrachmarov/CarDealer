namespace CarDealer.Data.Models
{
    using System.Collections.Generic;

    public class Car
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public List<CarPart> Parts { get; set; } =new List<CarPart>();

        public List<Sale> Sales { get; set; } = new List<Sale>();
    }
}
