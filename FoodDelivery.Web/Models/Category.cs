using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Web.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public bool IsDisabled { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset Created { get; set; }
    }
}