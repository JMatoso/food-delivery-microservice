using System;

namespace FoodDelivery.Web.Models
{
    public class TokenReturned
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string Role { get; set; }
        public string Token { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}