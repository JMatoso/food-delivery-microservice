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
        
        public IFormFile Image { get; set; }

        public string ProductBaseUrl = "http://localhost:5002";

        public string GenWebId(Guid id) => id.ToString().Substring(0, 8);
    }
}
