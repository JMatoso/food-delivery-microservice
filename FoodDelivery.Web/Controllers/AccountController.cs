using System.Threading.Tasks;
using FoodDelivery.Web.Models;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Web.Services.Client;
using FoodDelivery.Web.Helpers;

namespace Controllers
{
    public class AccountController : Controller
    {
        private readonly IClientApp _client;
        public AccountController(IClientApp client)
        {
            _client = client;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User model)
        {
            if(ModelState.IsValid)
            {
            	model.Role = "Client";
                var status = await _client.PostAsync<User>(model, Routes.AccountBaseUrl);

                status.Returned = status.Returned == null ? $"Something went wrong. Error Code: #{status.Code}" : status.Returned.ToString();
                switch(status.Code)
                {
                    case 200:
                        return CreatedAtAction("Login", "Auth", new Login 
                            { 
                                Email = model.Email, 
                                Password = model.Password, 
                                ReturnUrl = "/account/profile" 
                            }
                        );
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

        public IActionResult Profile()
        {
            return View();
        }
    }
}
