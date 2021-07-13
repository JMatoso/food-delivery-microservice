using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodDelivery.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Web.Services.Client;

namespace Controllers
{
    public class AuthController : Controller
    {
        private readonly IClientApp _client;
        public AuthController(IClientApp client)
        {
            _client = client;
        }

        public IActionResult Login(string returnUrl = "/")
        {
        	if(User.Identity.IsAuthenticated)
        	{
        		return RedirectToAction("Index", "Home");
        	}
            return View(new Login { ReturnUrl = returnUrl });
        } 

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if(ModelState.IsValid)
            {
                var status = await _client.LoginAsync(model, Routes.AuthBaseUrl);

                switch (status.Code)
                {
                    case 200:
                        var tokenReturned = (TokenReturned)status.Returned;

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, tokenReturned.Name),
                            new Claim(ClaimTypes.Name, tokenReturned.Id.ToString()),
                            new Claim(ClaimTypes.Hash, tokenReturned.Token),
                            new Claim(ClaimTypes.Role, tokenReturned.Role)
                        };

                        var identity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        User.AddIdentity(identity);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties 
                            { 
                                IsPersistent = false,
                                ExpiresUtc = tokenReturned.ExpireTime
                            }
                        );

                        return RedirectToAction("Index", "Home");

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
            
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
