using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.DTO
{
    [Table("Orders")]
    public class Order 
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid? ProductId { get; set; }

        [Required]
        public Guid? ClientId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string ProductName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductQuantity { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ProductPrice { get; set; }

        public Guid? ExtraId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? ExtraQuantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ExtraPrice { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        public string Longitude { get; set; }
        public string Latitude { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; }

        [DataType(DataType.Duration)]
        public DateTimeOffset Created { get; set; }
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