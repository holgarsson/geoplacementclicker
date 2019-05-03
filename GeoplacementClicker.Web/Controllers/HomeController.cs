using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeoplacementClicker.Web.Models;
using GeoplacementClicker.Persistence;
using GeoplacementClicker.Web.Services;
using System.Net.Http;
using Newtonsoft.Json;
using GeoplacementClicker.Persistence.Entities;
using GeoplacementClicker.Web.Models.Home;

namespace GeoplacementClicker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoplacementClickerDbContext _dbContext;
        private readonly IListenerService _listenerService;
        private HttpClient _httpClient = new HttpClient();

        public HomeController(GeoplacementClickerDbContext dbContext, IListenerService listenerService)
        {
            _dbContext = dbContext;
            _listenerService = listenerService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel()
            {
                DataEntries = _dbContext.DataEntries.OrderByDescending(o => o.TimeStamp).Take(15).ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Location(int? id)
        {
            var viewModel = new LocationViewModel() {
                DataEntryId = id
            };

            DataEntry dataEntry = null;

            if (id == null)
                dataEntry = _dbContext.DataEntries.OrderByDescending(o => o.TimeStamp).FirstOrDefault();
            else
                dataEntry = _dbContext.DataEntries.FirstOrDefault(de => de.Id == id);

            if (dataEntry != null)
            {
                //Get locations from data property and set
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> StartListening()
        {
            Task.Run(() => {
                _listenerService.StartListening().Wait();
            }); 

            return Content($"Listening on websocket url: {await _listenerService.GetListeningUrl()}");
        }

        [HttpGet]
        public async Task<IActionResult> StopListening()
        {
            await _listenerService.StopListening();

            return Content("Web socket closed.");
        }

        public async Task<string> TestData()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://iotnet.teracom.dk/1/data/vnoRWgAAABFpb3RuZXQudGVyYWNvbS5ka6Q2SEzMoItjPNEWgJi-7hY=/BE7A115A.json?fbclid=IwAR1dnVrmZ9_XrREZzzFKrdVSC6xP_64_tEH2sBwgSy-KgWAEsPEuad7pZ0w");

            string jsonData = await response.Content.ReadAsStringAsync();

            var jsonDataDeserialized = JsonConvert.DeserializeObject<IEnumerable<DataEntry>>(jsonData);

            HashSet<Gateway> gateways = new HashSet<Gateway>();
            foreach (var entry in jsonDataDeserialized)
            {
                if (entry.Gateways == null)
                    continue;

                foreach (var gw in entry.Gateways)
                {
                    gateways.Add(gw);
                }
            }

            _dbContext.DataEntries.AddRange(jsonDataDeserialized);
            _dbContext.SaveChanges();

            return jsonData;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
