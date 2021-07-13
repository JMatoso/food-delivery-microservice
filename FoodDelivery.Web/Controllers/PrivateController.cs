using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDelivery.Web.Models;
using FoodDelivery.Web.Models.VMModels;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Web.Helpers;
using FoodDelivery.Web.Services.Client;

namespace FoodDelivery.Web.Controllers
{
    public class PrivateController: Controller
    {
        private readonly IClientApp _client;
        private CommonModel _common { get; set; }
        
        public PrivateController(IClientApp client)
        {
            _client = client;
        }

        public IActionResult Category()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Category(VMCategory model)
        {
            if(ModelState.IsValid)
            {
                var status = await _client.PostAsync<VMCategory>(model, Routes.CategoryBaseUrl, "application/json", "");

                status.Returned = status.Returned == null ? $"Something went wrong. Error Code: #{status.Code}" : status.Returned.ToString();

                switch(status.Code)
                {
                    case 200: 
                        ViewBag.Success = "Category has been added.";
                        break;
                    case 400:
                        ViewBag.Warning = $"Service unavailable, try again later. \n Error Message: {status.Returned.ToString()}";
                        break;
                    default:
                        ViewBag.Error = $"Something went wrong. Error Code: #{status.Code} \n Error Message: {status.Returned.ToString()}";
                        break;
                }
            }

            return View();
        }

        public async Task<IActionResult> Product()
        {
            var categories = await _client.GetAsync<List<Category>>(Routes.CategoryBaseUrl, "application/json", "");
            
            return View(
                _common = new()
                {
                    Product = new(),
                    Categories = (List<Category>)categories.Returned
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Product(CommonModel model)
        {
            if(ModelState.IsValid)
            {
            	model.Product.Image = await _client.Upload(model.Image);
                var status = await _client.PostAsync<VMProduct>(model.Product, Routes.ProductBaseUrl, "application/json", "");

                status.Returned = status.Returned == null ? "400" : status.Returned.ToString();

                switch(status.Code)
                {
                    case 200: 
                        ViewBag.Success = "Product has been added.";
                        break;
                    case 400:
                        ViewBag.Warning = $"Service unavailable, try again later. \n Error Message: {status.Returned.ToString()}";
                        break;
                    default:
                        ViewBag.Error = $"Something went wrong. Error Code: #{status.Code} \n Error Message: {status.Returned.ToString()}";
                        break;
                }
            }

            var categories = await _client.GetAsync<List<Category>>(Routes.CategoryBaseUrl, "application/json", "");
            
            return View(
                _common = new()
                {
                    Product = new(),
                    Categories = (List<Category>)categories.Returned
                }
            );
        }
    }
}
