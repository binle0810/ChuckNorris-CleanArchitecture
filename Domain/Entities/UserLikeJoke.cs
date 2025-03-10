using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserLikeJoke
    {
        public UserLikeJoke(string userId, string jokeId, DateTime likedAt)
        {
            UserId = userId;
            JokeId = jokeId;
            LikedAt = likedAt;
        }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public string JokeId { get; set; }
        public LikeJoke Joke { get; set; }

        public DateTime LikedAt { get; set; } // Thêm thông tin ngày like
    }
}
