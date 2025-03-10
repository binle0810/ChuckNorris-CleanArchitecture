using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Jokes
{
    public class JokeDto
    {
        public string Id { get; set; }
        public int Like { get; set; } = 0;
    }
    public class JokeHistoryDto
    {
        public string Id { get; set; }
        public int Like { get; set; } 

        public DateTime likeat { get; set; }
    }
}
