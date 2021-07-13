using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace FoodDelivery.Web.Helpers
{
    public static class Functions
    {
        public static string GetClaim(ClaimsIdentity identity, string type)
        {
            IEnumerable<Claim> claims = identity.Claims;
            return claims.Where(x => x.Type == type).FirstOrDefault().Value;
        }
    }
}