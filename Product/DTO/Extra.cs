using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Product.DTO
{
    [Table("Extras")]
    public class Extra
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid? ProductId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Duration)]
        public int? ReadyTime { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; } 

        [Required]
        [Range(0, int.MaxValue)]
        public int MaxQuantityPerOrder { get; set; } 

        public bool IsDisabled { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset Created { get; set; }
    }
}