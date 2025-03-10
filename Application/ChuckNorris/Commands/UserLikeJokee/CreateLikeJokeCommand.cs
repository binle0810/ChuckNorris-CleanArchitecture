using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.ChuckNorris.Commands.UserLikeJokee
{
    public sealed record CreateLikeJokeCommand(string userid, string jokeid) : ICommand<object>;
}
