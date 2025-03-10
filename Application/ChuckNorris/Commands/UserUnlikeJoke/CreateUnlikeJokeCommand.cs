using Application.Abstractions.Messaging;
using Application.DTOs.Jokes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Commands.UserUnlikeJoke
{
    public sealed record CreateUnlikeJokeCommand(string userid ,string jokeid) : ICommand<JokeDto>;
  
}
