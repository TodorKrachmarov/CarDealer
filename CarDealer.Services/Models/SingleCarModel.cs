namespace CarDealer.Services.Models
{
    using System.Collections.Generic;

    public class SingleCarModel
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public long TravelledDistance { get; set; }

        public List<PartModel> Parts { get; set; } = new List<PartModel>();
    }
}
