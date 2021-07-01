using System.Collections.Generic;
using Product.DTO;

namespace Product.Models
{
    public class VMProdExtras
    {
        public DTO.Product Product { get; set; }
        public List<Extra> Extras { get; set; }
    }
}