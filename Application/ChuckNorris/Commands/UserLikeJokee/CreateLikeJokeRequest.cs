using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Commands.UserLikeJokee
{
    public sealed record CreateLikeJokeRequest(string userid, string Jokeid);
}
