using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Account.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string LastName { get; set; }

        [Required]
        public string Role { get; set; }
    }
}