using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Commands.UserUnlikeJoke
{
    public sealed record CreateUnlikeJokeRequest(string userid, string Jokeid);

}
