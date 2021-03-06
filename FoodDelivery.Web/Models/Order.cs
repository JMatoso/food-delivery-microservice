using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Web.Models
{
    public class Order : Cart
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; }

        public bool IsDisabled { get; set; }
    }
    
    public enum PaymentType
    {
        TPA,
        Wallet,
        Reference
    }

    public enum OrderStatus
    {
        Pendent,
        Accepted,
        Preparing,
        Ready,
        Delivering,
        Delivered,
        Rejected,
        Canceled,
        Lost
    }
}