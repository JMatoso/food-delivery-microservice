using System.ComponentModel.DataAnnotations.Schema;

namespace Order.DTO
{
    [Table("Orders")]
    public class Order : Cart
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string DeliveryAddress { get; set; }
        public PaymentType PaymentType { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
    
    public enum PaymentType
    {
        TPA,
        Wallet,
        Reference
    }

    public enum OrderStatus
    {
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