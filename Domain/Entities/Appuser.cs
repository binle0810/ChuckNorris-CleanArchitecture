using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public sealed class AppUser : IdentityUser
    {
        public List<UserLikeJoke> LikedJokes { get; set; } = new List<UserLikeJoke>();
    }
}
