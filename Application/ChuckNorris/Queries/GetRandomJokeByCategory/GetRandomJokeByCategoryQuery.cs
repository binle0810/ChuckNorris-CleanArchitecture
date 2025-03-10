using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Queries.GetRandomJokeByCategory
{
    public sealed record GetRandomJokeByCategoryQuery(string category) : IQuery<string>
    {
    }
}
