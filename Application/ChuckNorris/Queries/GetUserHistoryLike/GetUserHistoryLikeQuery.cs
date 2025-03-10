using Application.Abstractions.Messaging;
using Application.DTOs.Jokes;
using Domain.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Queries.GetUserHistoryLike
{
    public sealed record GetUserHistoryLikeQuery(string userid, QueryWithPage values) : IQuery<List<JokeHistoryDto>>;
    
}
