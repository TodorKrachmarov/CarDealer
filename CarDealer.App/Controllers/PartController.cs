namespace CarDealer.App.Controllers
{
    using Models.Part;
    using CarDealer.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using CarDealer.Services.Models;
    using Microsoft.AspNetCore.Authorization;

    public class PartController : Controller
    {
        private readonly IPartService partService;
        private readonly ISupplierService supplierService;
        private readonly ILogService logService;

        public PartController(IPartService partService, ISupplierService supplierService, ILogService logService)
        {
            this.partService = partService;
            this.supplierService = supplierService;
            this.logService = logService;
        }

        [Route("parts/all")]
        public IActionResult AllParts()
        {
            var parts = this.partService.AllParts();

            return View(new AllPartsModel
            {
                Parts = parts
            });
        }

        [Authorize]
        [Route("parts/add")]
        public IActionResult Create()
        {
            var model = new AddPartModel
            {
                Suppliers = this.supplierService.AllSuppliers().ToList()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost("parts/add")]
        public IActionResult Create(AddPartModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Suppliers = this.supplierService.AllSuppliers().ToList();
                return View(model);
            }

            var quantity = (model.Quantity == null ? 1 : int.Parse(model.Quantity.ToString()));

            var success = this.partService.Create(model.Name, model.Price, model.SupplierId, quantity);

            if (success)
            {
                this.logService.Create(User.Identity.Name, "Create", "Part");
                return RedirectToAction(nameof(AllParts));
            }
            else
            {
                ModelState.AddModelError(nameof(AddPartModel.SupplierId), "Invalid Supplier!");
                model.Suppliers = this.supplierService.AllSuppliers().ToList();
                return View(model);
            }
        }

        [Authorize]
        [Route("parts/delete/{id}")]
        public IActionResult Delete(int id) => View(id);

        [Authorize]
        [Route("parts/destroy/{id}")]
        public IActionResult Destroy(int id)
        {
            var success = this.partService.Delete(id);

            if (!success)
            {
                TempData["error"] = "The part you are trying to delete does not exist!";

            }
            else
            {
                this.logService.Create(User.Identity.Name, "Delete", "Part");
            }

            return RedirectToAction(nameof(AllParts));
        }

        [Authorize]
        [Route("parts/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var model = this.partService.FindToEdit(id);

            if (model == null)
            {
                TempData["error"] = "The part you are trying to edit does not exist!";
                return RedirectToAction(nameof(AllParts));
            }

            return View(model);
        }

        [Authorize]
        [HttpPost("parts/edit/{id}")]
        public IActionResult Edit(int id, EditPartModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var quantity = (model.Quantity == null ? 1 : int.Parse(model.Quantity.ToString()));

            var success = this.partService.Edit(id, model.Price, quantity);

            if (!success)
            {
                TempData["error"] = "The part you are trying to edit does not exist!";
            }
            else
            {
                this.logService.Create(User.Identity.Name, "Edit", "Part");
            }

            return RedirectToAction(nameof(AllParts));
        }
    }
}
