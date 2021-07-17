using System;
using System.Collections.Generic;
using FoodDelivery.Web.Models.VMModels;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Web.Models
{
    public class CommonModel
    {
        public List<Category> Categories { get; set; }

        public List<Product> Products { get; set; }
        public VMProduct Product { get; set; }
        public VMProdExtras ProductInfo { get; set; }

        public VMAddToCart AddToCart { get; set; }
        public List<Cart> ProductsOnCart { get; set; }
        public Cart Cart { get; set; }

        public List<Order> Orders { get; set; }
        public Order Order { get; set; }
        public string[] Status { get { return Enum.GetNames(typeof(OrderStatus)); }}
        
        public VMSendOrder SendOrder { get; set; }
        
        public IFormFile Image { get; set; }

        public const string ProductBaseUrl = "http://localhost:5002";

        public string GenWebId(Guid id) => id.ToString().Substring(0, 8);
        public string GetStatus(OrderStatus status)
        {
            switch(status)
            {
                case OrderStatus.Pendent:
                    return "muted";
                case OrderStatus.Accepted:
                    return "primary";
                case OrderStatus.Delivered:
                case OrderStatus.Ready:
                    return "success";
                case OrderStatus.Preparing:
                case OrderStatus.Lost:
                case OrderStatus.Delivering:
                    return "orange";
                case OrderStatus.Rejected:
                case OrderStatus.Canceled:
                    return "danger";
                default: return "muted";
            }
        }
    }
}
