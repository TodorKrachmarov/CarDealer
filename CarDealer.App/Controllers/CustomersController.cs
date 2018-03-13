namespace CarDealer.App.Controllers
{
    using CarDealer.App.Models.Customer;
    using CarDealer.Services;
    using CarDealer.Services.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CustomersController : Controller
    {
        private readonly ICustomerService customerService;
        private readonly ILogService logService;

        public CustomersController(ICustomerService customerService, ILogService logService)
        {
            this.customerService = customerService;
            this.logService = logService;
        }

        public IActionResult All(string order)
        {
            var orderDirection = order.ToLower() == "assending"
                ? OrderDirection.Ascending
                : OrderDirection.Descending;

            var result = this.customerService.OrderedCustomers(orderDirection);

            return this.View(new AllCustomersModel
            {
                Customers = result,
                OrderDirection = orderDirection
            });
        }

        [Route("customers/{id}")]
        public IActionResult SingleCustomer(int id)
        {
            var customer = this.customerService.FindCustomer(id);

            return this.View(customer);
        }

        [Authorize]
        [Route("customers/create")]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost("customers/create")]
        public IActionResult Create(CreateCustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            this.customerService.Create(model.Name, model.BirthDate);

            this.logService.Create(User.Identity.Name, "Create", "Customer");

            return RedirectToAction(nameof(All), new { order = "assending" });
        }

        [Authorize]
        [Route("customers/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var model = this.customerService.FindById(id);

            if (model == null)
            {
                TempData["error"] = "The customer you are trying to edit does not exist!";
                return RedirectToAction(nameof(All), new { order = "assending" });
            }

            return this.View(model);
        }

        [Authorize]
        [HttpPost("customers/edit/{id}")]
        public IActionResult Edit(int id, EditCustomerModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var success = this.customerService.Edit(id, model.Name, model.BirthDate);

            if (!success)
            {
                TempData["error"] = "The customer you are trying to edit does not exist!";
            }
            else
            {
                this.logService.Create(User.Identity.Name, "Edit", "Customer");
            }

            return RedirectToAction(nameof(All), new { order = "assending" });
        }
    }
}
