using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodDelivery.Web.Models;
using FoodDelivery.Web.Services.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using FoodDelivery.Web.Helpers;

namespace FoodDelivery.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClientApp _client;
        private CommonModel _common { get; set; }

        public HomeController(
            ILogger<HomeController> logger,
            IClientApp client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _client.GetAsync<List<Category>>(Routes.CategoryBaseUrl);
            var products = await _client.GetAsync<List<Models.Product>>(Routes.ProductBaseUrl);
            
            return View(
                _common = new()
                {
                    Categories = (List<Category>)categories.Returned,
                    Products = (List<Models.Product>)products.Returned
                }
            );
        }

        public async Task<IActionResult> Menu()
        {
            var categories = await _client.GetAsync<List<Category>>(Routes.CategoryBaseUrl);
            var products = await _client.GetAsync<List<Models.Product>>(Routes.ProductBaseUrl);
            
            return View(
                _common = new()
                {
                    Categories = (List<Category>)categories.Returned,
                    Products = (List<Models.Product>)products.Returned
                }
            );
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
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
