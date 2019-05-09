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
using Microsoft.Extensions.Configuration;
using System.Text;

namespace GeoplacementClicker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeoplacementClickerDbContext _dbContext;
        private readonly IListenerService _listenerService;
        private HttpClient _httpClient = new HttpClient();

        public HomeController(GeoplacementClickerDbContext dbContext,
            IListenerService listenerService,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _listenerService = listenerService;

        }

        public async Task<IActionResult> Index()
        {
            List<DataEntry> dataEntries = _dbContext.DataEntries.OrderByDescending(o => o.TimeStamp).Take(15).ToList();

            SetLongitudeLatitude(dataEntries);

            var viewModel = new HomeViewModel()
            {
                DataEntries = dataEntries
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Location(int? id)
        {
            var viewModel = new LocationViewModel();

            DataEntry dataEntry = null;

            if (id == null)
                dataEntry = _dbContext.DataEntries.OrderByDescending(o => o.TimeStamp).FirstOrDefault();
            else
                dataEntry = _dbContext.DataEntries.FirstOrDefault(de => de.Id == id);

            if (dataEntry != null)
            {
                SetLongitudeLatitude(ref dataEntry);
            }

            if (dataEntry.Latitude.HasValue && dataEntry.Longitude.HasValue)
            {
                viewModel.Latitude = dataEntry.Latitude.Value;
                viewModel.Longitude = dataEntry.Longitude.Value;
                viewModel.EUI = dataEntry.EUI;
                viewModel.DataEntryId = dataEntry.Id;
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dataEntry = _dbContext.DataEntries.FirstOrDefault(de => de.Id == id);

            if (dataEntry != null)
            {
                SetLongitudeLatitude(ref dataEntry);
            }

            var viewModel = new DetailsViewModel()
            {
                DataEntry = dataEntry
            };

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

        private void SetLongitudeLatitude(ref DataEntry dataEntry)
        {
            var rawData = FromHexString(dataEntry.Data);
            string[] data = rawData.Split(',');
            if (data.Length != 3)
                return;

            dataEntry.IsSOS = data[0].Contains("S");
            dataEntry.Latitude = decimal.Parse(data[1]);
            dataEntry.Longitude = decimal.Parse(data[2]);
        }

        private void SetLongitudeLatitude(List<DataEntry> dataEntries)
        {
            foreach(var dataEntry in dataEntries)
            {
                var rawData = FromHexString(dataEntry.Data);
                string[] data = rawData.Split(',');
                if (data.Length != 3)
                    return;

                dataEntry.IsSOS = data[0].Contains("S");
                dataEntry.Latitude = decimal.Parse(data[1]);
                dataEntry.Longitude = decimal.Parse(data[2]);
            }
        }

        string FromHexString(string hexString)
        {
            if (hexString == null || (hexString.Length & 1) == 1)
            {
                return string.Empty;
            }
            var sb = new StringBuilder();
            for (var i = 0; i < hexString.Length; i += 2)
            {
                var hexChar = hexString.Substring(i, 2);
                sb.Append((char)Convert.ToByte(hexChar, 16));
            }
            return sb.ToString();
        }
    }
}
