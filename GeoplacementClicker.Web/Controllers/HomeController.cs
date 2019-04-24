using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoplacementClicker.Web.Models;
using GeoplacementClicker.Persistence;

namespace GeoplacementClicker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoplacementClickerDbContext _dbContext;
        public HomeController(GeoplacementClickerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            _dbContext.DataEntries.Add(new Persistence.Entities.DataEntry()
            {
                Data = "Some data"
            });

            await _dbContext.SaveChangesAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
