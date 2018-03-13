namespace CarDealer.App.Controllers
{
    using CarDealer.App.Models.Car;
    using CarDealer.Services;
    using CarDealer.Services.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Linq;

    [Route("cars")]
    public class CarController : Controller
    {
        private readonly ICarService carService;
        private readonly IPartService partService;
        private readonly ILogService logService;

        public CarController(ICarService carService, IPartService partService, ILogService logService)
        {
            this.carService = carService;
            this.partService = partService;
            this.logService = logService;
        }
        
        public IActionResult All()
        {
            var cars = this.carService.AllCars();

            return this.View(new AllCarsByMake
            {
                Cars = cars
            });
        }

        [Route("{make}")]
        public IActionResult ByMake(string make)
        {
            var cars = this.carService.CarsFromMake(make);

            return this.View(new AllCarsByMake
            {
                Cars = cars,
                Make = make
            });
        }

        [Route("{id}/parts")]
        public IActionResult SingleCar(int id)
        {
            var car = this.carService.CarInfo(id);

            return this.View(car);
        }

        [Authorize]
        [Route("create")]
        public IActionResult Create()
        {
            var carModel = new AddCarModel
            {
                Parts = this.partService.AllParts().Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList()
            };

            return View(carModel);
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult Create(AddCarModel carModel)
        {

            if (!ModelState.IsValid)
            {
                carModel.Parts = this.partService.AllParts().Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();
                return View(carModel);
            }

            this.carService.Create(carModel.Make, carModel.Model, carModel.TravelledDistance, carModel.PartsIds);

            this.logService.Create(User.Identity.Name, "Create", "Car");

            return RedirectToAction(nameof(All));
        }
    }
}
