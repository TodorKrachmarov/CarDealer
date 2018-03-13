namespace CarDealer.App.Models.Car
{
    using System.Collections.Generic;
    using CarDealer.Services.Models;

    public class AllCarsByMake
    {
        public IEnumerable<CarModel> Cars { get; set; }

        public string Make { get; set; }
    }
}
