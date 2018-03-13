namespace CarDealer.App.Controllers
{
    using CarDealer.Services;
    using Models.Sale;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using CarDealer.Services.Models;

    [Route("sales")]

    public class SaleController : Controller
    {
        private readonly ISaleService saleService;
        private readonly ICarService carService;
        private readonly ICustomerService customerService;
        private readonly ILogService logService;

        public SaleController(ISaleService saleService, ICustomerService customerService, ICarService carService, ILogService logService)
        {
            this.saleService = saleService;
            this.carService = carService;
            this.customerService = customerService;
            this.logService = logService;
        }

        public IActionResult All()
        {
            var sales = this.saleService.GetAllSales();

            return this.View(new ListAllSalesModel { Sales = sales });
        }

        [Route("{id}")]
        public IActionResult SingleSale(int id)
        {
            var sale = this.saleService.GetSaleById(id);

            return this.View(sale);
        }

        [Route("discounted")]
        public IActionResult AllWithDiscount()
        {
            var sales = this.saleService.GetSalesWithDiscount();

            return this.View(new ListAllSalesModel { Sales = sales });
        }

        [Route("discounted/{percent}")]
        public IActionResult AllWithDiscountPercent(double percent)
        {
            var sales = this.saleService.GetSalesWithSpesificDiscount(percent);

            return this.View(new ListAllSalesModel { Sales = sales });
        }

        [Authorize]
        [Route(nameof(Create))]
        public IActionResult Create()
        {
            var model = new CreateSaleModel
            {
                Cars = this.GetCarsToSelectListItem(),
                Customers = this.GetCustomersToSelectListItem()
            };

            return View(model);
        }

        [Authorize]
        [Route(nameof(Confirm))]
        public IActionResult Confirm(CreateSaleModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Cars = this.GetCarsToSelectListItem();
                model.Customers = this.GetCustomersToSelectListItem();

                return View(nameof(Create), new { model });
            }

            var sale = this.saleService.ReviewSale(model.Customer, model.Car, model.Discount);

            TempData["carId"] = model.Car;
            TempData["customerId"] = model.Customer;
            TempData["discount"] = sale.Discount;

            return View(sale);
        }

        [Authorize]
        [Route(nameof(Add))]
        public IActionResult Add()
        {
            var carId = int.Parse(TempData["carId"].ToString());
            var customerId = int.Parse(TempData["customerId"].ToString());
            var discount = int.Parse(TempData["discount"].ToString());
            
            var success = this.saleService.Create(carId, customerId, discount);

            if (!success)
            {
                TempData["error"] = "Unexpected error has occurred. Sale could not be created!";
            }
            else
            {
                this.logService.Create(User.Identity.Name, "Create", "Sale");
            }
            return this.RedirectToAction(nameof(All));
        }

        private List<SelectListItem> GetCarsToSelectListItem()
            => this.carService
            .SaleCar()
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToList();

        private List<SelectListItem> GetCustomersToSelectListItem()
            => this.customerService
            .SaleCustomers()
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToList();
    }
}
