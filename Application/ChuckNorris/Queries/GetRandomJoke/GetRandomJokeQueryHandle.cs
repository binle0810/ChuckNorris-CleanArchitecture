using Application.Abstractions;
using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Queries.GetRandomJoke
{
    internal sealed class GetRandomJokeQueryHandle : IQueryHandler<GetRandomJokeQuery, string>
    {
        private readonly IChuckNorrisService _chuckNorrisService;

        public GetRandomJokeQueryHandle(IChuckNorrisService chuckNorrisService)
        {
            _chuckNorrisService = chuckNorrisService;
        }

        public async Task<string> Handle(GetRandomJokeQuery request, CancellationToken cancellationToken)
        {
            return await _chuckNorrisService.GetRandomJokeAsync();
        }
    }
}
