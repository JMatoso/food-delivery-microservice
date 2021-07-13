using System;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Web.Models.VMModels
{
    public class VMAddToCart
    {
        [Required]
        public Guid? ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductQuantity { get; set; }

        public Guid? ExtraId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? ExtraQuantity { get; set; }
    }
}