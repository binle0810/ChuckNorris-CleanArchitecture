using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LikeJoke
    {
        public string Id { get; set; }
        public int Like { get; set; }=0;
        public List<UserLikeJoke> LikedByUsers { get; set; } = new List<UserLikeJoke>();
    }

}
