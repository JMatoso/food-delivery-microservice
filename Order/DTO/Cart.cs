using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Order.DTO
{
    [Table("Cart")]
    public class Cart
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

        [DataType(DataType.Duration)]
        public DateTimeOffset Created { get; set; }
    }
}
