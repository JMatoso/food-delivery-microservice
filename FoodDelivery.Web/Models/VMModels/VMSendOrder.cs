using System;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Web.Models.VMModels
{
    public class VMSendOrder
    {
        public Guid CartId { get; set; }
        public string Longitude { get; set; }

        public string Latitude { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }
    }
}