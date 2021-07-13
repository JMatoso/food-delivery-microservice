using System.Collections.Generic;

namespace FoodDelivery.Web.Models
{
    public class VMProdExtras
    {
        public Models.Product Product { get; set; }
        public List<Extra> Extras { get; set; }
    }
}