using System.ComponentModel.DataAnnotations;

namespace Product.Models
{
    public class VMCategory
    {
        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}