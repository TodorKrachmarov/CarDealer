namespace CarDealer.App.Controllers
{
    using CarDealer.App.Models.Log;
    using CarDealer.Services;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    public class LogController : Controller
    {
        private const int PageSize = 20;
        private readonly ILogService logService;

        public LogController(ILogService logService)
        {
            this.logService = logService;
        }

        [Route("logs/all?page={page?}&search={search?}")]
        public IActionResult All(string search, int page = 1)
        {
            var AllLogs = this.logService.All();

            if (search != null)
            {
                AllLogs = AllLogs.Where(l => l.Username.ToLower().Contains(search.ToLower()));
            }

            int totalPages = (int)Math.Ceiling(AllLogs.Count() / (double)PageSize);
            var logs = AllLogs.Skip((page - 1) * PageSize).Take(PageSize);


            return View(new LogPageListingModel
            {
                Logs = logs.ToList(),
                TotalPages = totalPages,
                CurrentPage = page
            });
        }
    }
}
