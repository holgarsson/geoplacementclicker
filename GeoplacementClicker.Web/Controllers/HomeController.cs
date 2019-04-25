using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoplacementClicker.Web.Models;
using GeoplacementClicker.Persistence;
using GeoplacementClicker.Web.Services;

namespace GeoplacementClicker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoplacementClickerDbContext _dbContext;
        private readonly IListenerService _listenerService;
        public HomeController(GeoplacementClickerDbContext dbContext, IListenerService listenerService)
        {
            _dbContext = dbContext;
            _listenerService = listenerService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> StartListening()
        {
            await _listenerService.StartListening();

            return Content($"Listening on websocket url: {await _listenerService.GetListeningUrl()}");
        }


        [HttpGet]
        public async Task<IActionResult> StopListening()
        {
            await _listenerService.StopListening();

            return Content("Web socket closed.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
