using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodDelivery.Web.Models;
using FoodDelivery.Web.Models.VMModels;
using FoodDelivery.Web.Helpers;
using FoodDelivery.Web.Services.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Controllers
{
    public class ShoppingController : Controller
    {
        private readonly IClientApp _client;
        private CommonModel _common { get; set; }
        private string token { get; set; }
        private Guid userId { get; set; }

        public ShoppingController(IClientApp client)
        {
            _client = client;
        }

        #region Plate
        [HttpGet]
        public async Task<IActionResult> Plate(Guid productId)
        {
            if(Guid.Empty != productId)
            {
                var status = await _client.GetAsync<VMProdExtras>(Routes.ProductBaseUrl + $"/i/{productId}");
                var categories = await _client.GetAsync<List<Category>>(Routes.CategoryBaseUrl);

                switch(status.Code)
                {
                    case 200:
                        var prod = (VMProdExtras)status.Returned;
                        var prodCat = await _client.GetAsync<List<FoodDelivery.Web.Models.Product>>(Routes.ProductBaseUrl + $"/category/{prod.Product.CategoryId}");

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
        public async Task<IActionResult> Plate(CommonModel model, Guid productId)
        {
            await GetData(productId);
            GetCredentials();

            if(_common == null)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                if(User.Identity.IsAuthenticated)
                {
                    var status = await _client.GetAsync<VMProdExtras>(Routes.ProductBaseUrl + $"/i/{model.AddToCart.ProductId}", token);
                    var status2 = model.AddToCart.ExtraId == Guid.Empty ? null : await _client.GetAsync<FoodDelivery.Web.Models.Extra>(Routes.ExtraBaseUrl + $"/i/{model.AddToCart.ExtraId}", token);

                    switch (status.Code)
                    {
                        case 200:

                            var prod = (VMProdExtras)status.Returned;
                            int qntProd = model.AddToCart.ProductQuantity;

                            decimal totalPrice = qntProd * prod.Product.Price;

                            Extra extra = null;
                            int? extraQnt = model.AddToCart.ExtraQuantity;

                            if(status2.Returned != null && status2.Code == 200)
                            {
                                extra = (Extra)status2.Returned;
                                totalPrice += (decimal)extraQnt * extra.Price;
                            }

                            VMCart cart = new()
                            {
                                ProductId = prod.Product.Id,
                                ClientId = userId,
                                ProductName = prod.Product.Name,
                                ProductQuantity = qntProd,
                                ProductPrice = prod.Product.Price,
                                Image = prod.Product.Image,
                                ExtraId = extra == null ? Guid.Empty : extra.Id,
                                ExtraQuantity = extraQnt,
                                ExtraPrice = extra == null ? 0 : extra.Price,
                                TotalPrice = totalPrice
                            };

                            var status3 = await _client.PostAsync<VMCart>(cart, Routes.ShoppingBaseUrl, token);
                            status3.Returned = status3.Returned == null ? "Something went wrong." : status3.Returned.ToString();

                            if(status3.Code == 200)
                            {
                                ViewBag.Success = $"Product '{prod.Product.Name}' has been added to your cart succesfully.";
                            }
                            else
                            {
                                ViewBag.Warning = status3.Returned.ToString() + $" Error Code: {status3.Code}";
                            }

                            return View(_common);
                        case 400:
                            status.Returned = status.Returned == null ? "400" : status.Returned.ToString();
                            ViewBag.Warning = $"Service unavailable, try again later. \n Error Message: {status.Returned.ToString()}";
                            break;
                        case 401:
                            ViewBag.Warning = string.IsNullOrEmpty(status.Returned.ToString()) ? "Wrong credentials." : status.Returned.ToString();
                            break;
                        case 404:
                            return NotFound();
                        default:
                            ViewBag.Error = $"Something went wrong. Error Code: #{status.Code} \n Error Message: {status.Returned.ToString()}";
                            return View(_common);
                    }
                }

                ViewBag.Error = "Login first.";
                return View(_common);
            }

            ViewBag.Error = "Fill all required fields.";
            return View(_common);
        }
        #endregion
        
        #region Cart
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Cart()
        {
            GetCredentials();
            var status = await _client.GetAsync<List<Cart>>(Routes.ShoppingBaseUrl + $"/{userId}", token);
            return View((List<Cart>)status.Returned);
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public async Task<IActionResult> RemoveFromCart(Guid cartId)
        {
            if(cartId != Guid.Empty)
            {
                GetCredentials();
                var status = await _client.DeleteAsync(Routes.ShoppingBaseUrl + $"/{cartId}", token);

                switch(status.Code)
                {
                    case 200: 
                        ViewBag.Success = "Product has been removed from cart succesfully.";
                        break;
                    case 401: return RedirectToAction("Logout", "Auth");
                    case 404: return NotFound();
                    default: 
                        status.Returned = status.Returned == null ? "Something went wrong. " : status.Returned.ToString();
                        ViewBag.Error = status.Returned == null ? $"Something went wrong. Error Code: #{status.Code}." : $"Something went wrong. Error Code: #{status.Code} \n Error Message: {status.Returned.ToString()}";
                        break;
                }

                var status2 = await _client.GetAsync<List<Cart>>(Routes.ShoppingBaseUrl + $"/{userId}", token);
            }

            return RedirectToAction(nameof(Cart));
        }
        #endregion

        #region Ordering
        public async Task<IActionResult> Orders()
        {
            GetCredentials();
            var status = await _client.GetAsync<List<Order>>(Routes.OrderBaseUrl + $"/{userId}", token);
            _common = new()
            {
                Orders = ((List<Order>)status.Returned)
            };

            return View(_common);
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public async Task<IActionResult> Ordering(Guid orderId)
        {
            if(orderId != Guid.Empty)
            {
                GetCredentials();
                var status = await _client.GetAsync<Order>(Routes.OrderBaseUrl + $"/o/{orderId}", token);

                switch(status.Code)
                {
                    case 200: 
                        _common = new()
                        {
                            Order = (Order)status.Returned
                        };
                        break;
                    case 401: return RedirectToAction("Logout", "Auth");
                    case 404: return NotFound();
                    default: 
                        status.Returned = status.Returned == null ? "Something went wrong. " : status.Returned.ToString();
                        ViewBag.Error = status.Returned == null ? $"Something went wrong. Error Code: #{status.Code}." : $"Something went wrong. Error Code: #{status.Code} \n Error Message: {status.Returned.ToString()}";
                        break;
                }

                return View(_common);
            }

            return RedirectToAction(nameof(Orders));
        }
        #endregion

        #region Checkout
        [Authorize(Roles = "Client")]
        [HttpGet]
        public async Task<IActionResult> Checkout(Guid cartId)
        {
            if(cartId != Guid.Empty)
            {
                GetCredentials();
                var status = await _client.GetAsync<Cart>(Routes.ShoppingBaseUrl + $"/cart/{cartId}", token);

                switch(status.Code)
                {
                    case 200: 
                        _common = new()
                        {
                            SendOrder = new(),
                            Cart = (Cart)status.Returned
                        };
                        break;
                    case 401: return RedirectToAction("Logout", "Auth");
                    case 404: return NotFound();
                    default: 
                        status.Returned = status.Returned == null ? "Something went wrong. " : status.Returned.ToString();
                        ViewBag.Error = status.Returned == null ? $"Something went wrong. Error Code: #{status.Code}." : $"Something went wrong. Error Code: #{status.Code} \n Error Message: {status.Returned.ToString()}";
                        break;
                }

                return View(_common);
            }

            return RedirectToAction(nameof(Cart));
        }

        public async Task<IActionResult> CheckedOut(CommonModel model, Guid cartId)
        {
            if(ModelState.IsValid)
            {
                GetCredentials();
                var status = await _client.GetAsync<Cart>(Routes.ShoppingBaseUrl + $"/cart/{cartId}", token);

                if(status.Code == 200)
                {
                    var cart = (Cart)status.Returned;
                    VMOrder order = new()
                    {
                        ProductId = cart.ProductId,
                        ClientId = cart.ClientId,
                        ProductName = cart.ProductName,
                        ProductQuantity = cart.ProductQuantity,
                        Image = cart.Image,
                        ProductPrice = cart.ProductPrice,
                        ExtraId = cart.ExtraId,
                        ExtraQuantity = cart.ExtraQuantity,
                        TotalPrice = cart.TotalPrice,
                        Longitude = model.SendOrder.Longitude,
                        Latitude = model.SendOrder.Latitude,
                        DeliveryAddress = model.SendOrder.DeliveryAddress,
                        PaymentType = model.SendOrder.PaymentType
                    };

                    var status2 = await _client.PostAsync<VMOrder>(order, Routes.OrderBaseUrl, token);

                    if(status2.Code == 200)
                    {
                        ViewBag.Success = "Your order has been shipped you can track its status now in 'Orders' menu.";

                        var status3 = await _client.DeleteAsync(Routes.ShoppingBaseUrl + $"/{cartId}", token);

                        if(status3.Code != 200)
                        {
                            ViewBag.Success += $"\nError removing product from cart. Error Code: {status3.Code}";
                        }

                        //await RemoveFromCart(cartId);
                    }
                    else
                    {
                        ViewBag.Error = status2.Returned = status2.Returned == null ? $"Something went wrong. Error Code: #{status2.Code}." : status2.Returned.ToString() + $" Error Code: {status2.Code}.";
                    }

                    return View();
                }

                return RedirectToAction($"Checkout?cartId={cartId}");
            }

            return RedirectToAction(nameof(Cart));
        }
        #endregion

        #region ...
        private async Task<CommonModel> GetData(Guid productId)
        {
            GetCredentials();
            var status4 = await _client.GetAsync<VMProdExtras>(Routes.ProductBaseUrl + $"/i/{productId}", token);
            var categories = await _client.GetAsync<List<Category>>(Routes.CategoryBaseUrl, token);

            if(status4.Code == 200)
            {
                var prod = (VMProdExtras)status4.Returned;
                var prodCat = await _client.GetAsync<List<FoodDelivery.Web.Models.Product>>(Routes.ProductBaseUrl + $"/category/{prod.Product.CategoryId}", token);

                _common = new()
                {
                    AddToCart = new(),
                    Categories = (List<Category>)categories.Returned,
                    ProductInfo = prod,
                    Products = (List<FoodDelivery.Web.Models.Product>)prodCat.Returned
                };
            }

            return _common;
        }

        private void GetCredentials()
        {
            if(User.Identity.IsAuthenticated)
            {
                token = Functions.GetClaim(User.Identity as ClaimsIdentity, ClaimTypes.Hash);
                userId = Guid.Parse(User.Identity.Name);
            }
        }
        #endregion
    }
}