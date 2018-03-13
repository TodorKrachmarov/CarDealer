namespace CarDealer.App.Controllers
{
    using CarDealer.App.Models.Supplier;
    using CarDealer.Services;
    using CarDealer.Services.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("suppliers")]
    public class SupplierController : Controller
    {
        private readonly ISupplierService supplierService;
        private readonly ILogService logService;

        public SupplierController(ISupplierService supplierService, ILogService logService)
        {
            this.supplierService = supplierService;
            this.logService = logService;
        }

        public IActionResult AllSuppliers()
        {
            var suppliers = this.supplierService.ListSuppliers();

            return View(suppliers);
        }

        [Route("{filter}")]
        public IActionResult All(string filter)
        {
            bool isImporter = filter != "local";

            var suppliers = this.supplierService.FilterSuppliers(isImporter);

            return this.View(new AllSuppliersModel
            {
                Suppliers = suppliers,
                IsImporter = isImporter
            });
        }

        [Authorize]
        [Route(nameof(Create))]
        public IActionResult Create()
            => this.View();

        [Authorize]
        [HttpPost(nameof(Create))]
        public IActionResult Create(CreateSupplierModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            this.supplierService.Create(model.Name, model.IsImporter);

            this.logService.Create(User.Identity.Name, "Create", "Supplier");

            return RedirectToAction(nameof(AllSuppliers));
        }

        [Authorize]
        [Route("edit/{id}")]
        public IActionResult Edit( int id)
        {
            var supplier = this.supplierService.FindToEdit(id);

            if (supplier == null)
            {
                TempData["error"] = "The supplier you are trying to edit does not exist!";
                return RedirectToAction(nameof(AllSuppliers));
            }

            return this.View(supplier);
        }

        [Authorize]
        [HttpPost("edit/{id}")]
        public IActionResult Edit(int id, EditSupplierModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var success = this.supplierService.Edit(id, model.Name, model.IsImporter);

            if (!success)
            {
                TempData["error"] = "The supplier you are trying to edit does not exist!";
            }
            else
            {
                this.logService.Create(User.Identity.Name, "Edit", "Supplier");
            }

            return RedirectToAction(nameof(AllSuppliers));
        }

        [Authorize]
        [Route("delete/{id}")]
        public IActionResult Delete( int id)
        {
            if (!this.supplierService.Exists(id))
            {
                TempData["error"] = "The supplier you are trying to delete does not exist!";
                return RedirectToAction(nameof(AllSuppliers));
            }

            return this.View(id);
        }

        [Authorize]
        [Route("destroy/{id}")]
        public IActionResult Destroy(int id)
        {
            var success = this.supplierService.Delete(id);

            if (!success)
            {
                TempData["error"] = "The supplier you are trying to delete does not exist!";
            }
            else
            {
                this.logService.Create(User.Identity.Name, "Delete", "Supplier");
            }

            return RedirectToAction(nameof(AllSuppliers));
        }
    }
}
