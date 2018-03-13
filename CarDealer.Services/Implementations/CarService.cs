namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Services.Models;
    using Microsoft.EntityFrameworkCore;
    using CarDealer.Data.Models;

    public class CarService : ICarService
    {
        private readonly ApplicationDbContext db;

        public CarService(ApplicationDbContext db)
        {
            this.db = db;
        }


        public IEnumerable<CarModel> CarsFromMake(string make)
        {
            return this.db
                .Cars
                .Where(c => c.Make == make)
                .Select(c => new CarModel
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToList();
        }

        public SingleCarModel CarInfo(int Id)
        {
            var car = this.db
                .Cars
                .Where(c => c.Id == Id)
                .Include(c => c.Parts)
                .ThenInclude(p => p.Part)
                .Select(c => new SingleCarModel
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    Parts = c.Parts
                        .Select(p => new PartModel
                        {
                            Name = p.Part.Name,
                            Price = p.Part.Price
                        }).ToList()
                })
                .FirstOrDefault();

            return car;
        }

        public IEnumerable<CarModel> AllCars()
        {
            return this.db
               .Cars
               .Select(c => new CarModel
               {
                   Id = c.Id,
                   Make = c.Make,
                   Model = c.Model,
                   TravelledDistance = c.TravelledDistance
               })
               .ToList();
        }

        public void Create(string make, string model, long trDistance, List<int> parts)
        {
            var car = new Car
            {
                Make = make,
                Model = model,
                TravelledDistance = trDistance
            };

            foreach (var id in parts)
            {
                var part = this.db.Parts.FirstOrDefault(p => p.Id == id);

                if (part == null)
                {
                    continue;
                }

                car.Parts.Add(new CarPart { PartId = id, Part = part });
            }

            this.db.Cars.Add(car);
            this.db.SaveChanges();
        }

        public IEnumerable<AddSaleCarModel> SaleCar()
            => this.db
                   .Cars
                   .Select(c => new AddSaleCarModel
                   {
                       Id = c.Id,
                       Name = $"{c.Make} {c.Model}"
                   })
                   .ToList();
    }
}
