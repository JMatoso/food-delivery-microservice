using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDelivery.Web.Models;
using FoodDelivery.Web.Models.VMModels;
using FoodDelivery.Web.Services.Client;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    public class ShoppingController : Controller
    {
        private readonly IClientApp _client;
        private CommonModel _common { get; set; }
        public ShoppingController(IClientApp client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Plate(Guid productId)
        {
            if(Guid.Empty != productId)
            {
                var status = await _client.GetAsync<VMProdExtras>(Routes.ProductBaseUrl + $"/i/{productId}", "application/json", "");
                var categories = await _client.GetAsync<List<Category>>(Routes.CategoryBaseUrl, "application/json", "");

                switch(status.Code)
                {
                    case 200:
                        var prod = (VMProdExtras)status.Returned;
                        var prodCat = await _client.GetAsync<List<FoodDelivery.Web.Models.Product>>(Routes.ProductBaseUrl + $"category/{prod.Product.CategoryId}", "application/json", "");

                        _common = new()
                        {
                            AddToCart = new(),
                            Categories = (List<Category>)categories.Returned,
                            ProductInfo = prod,
                            Products = (List<FoodDelivery.Web.Models.Product>)prodCat.Returned
                        };
                        break;
                    case 404: return NotFound();
                    default: return RedirectToAction("Error", "Home");
                }

                return View(_common);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Plate(VMAddToCart model)
        {
            if(ModelState.IsValid)
            {
                if(User.Identity.IsAuthenticated)
                {
                    var status = await _client.GetAsync<FoodDelivery.Web.Models.Product>(Routes.ProductBaseUrl + $"/i/{model.ProductId}", "application/json", "");

                    switch (status.Code)
                    {
                        case 200:
                            
                        case 400:
                            status.Returned = status.Returned == null ? "400" : status.Returned.ToString();
                            ViewBag.Warning = $"Service unavailable, try again later. \n Error Message: {status.Returned.ToString()}";
                            break;
                        case 401:
                            ViewBag.Warning = string.IsNullOrEmpty(status.Returned.ToString()) ? "Wrong credentials." : status.Returned.ToString();
                            break;
                        default:
                            ViewBag.Error = $"Something went wrong. Error Code: #{status.Code} \n Error Message: {status.Returned.ToString()}";
                            break;
                    }
                }
            }
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Orders()
        {
            return View();
        }

        public IActionResult Ordering()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}