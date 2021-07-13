using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace FoodDelivery.Web.Models.VMModels
{
    public class VMProduct
    {
        [Required]
        public Guid? CategoryId { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        [DataType(DataType.Duration)]
        public int? ReadyTime { get; set; }

        [Required]
        [Range(1, double.MaxValue)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        
        public bool Star { get; set; }
    }
}
