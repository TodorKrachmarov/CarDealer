namespace CarDealer.App.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using CarDealer.App.Models;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Error()
        {
            return this.View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
