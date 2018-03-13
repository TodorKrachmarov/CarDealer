namespace CarDealer.App.Models.Part
{
    using CarDealer.Services.Models;
    using System.Collections.Generic;

    public class AllPartsModel
    {
        public IEnumerable<PartModel> Parts { get; set; } = new List<PartModel>();
    }
}
