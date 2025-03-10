using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public static class ClaimsExtension
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
