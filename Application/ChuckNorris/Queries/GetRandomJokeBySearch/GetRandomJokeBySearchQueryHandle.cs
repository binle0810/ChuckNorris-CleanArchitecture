using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.ChuckNorris.Queries.GetRandomJoke;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Queries.GetRandomJokeBySearch
{
    internal sealed class GetRandomJokeBySearchQueryHandle : IQueryHandler<GetRandomJokeBySearchQuery, string>
    {
        private readonly IChuckNorrisService _chuckNorrisService;
      

        public GetRandomJokeBySearchQueryHandle(IChuckNorrisService chuckNorrisService)
        {
            _chuckNorrisService = chuckNorrisService;
            
        }

        public async Task<string> Handle(GetRandomJokeBySearchQuery request, CancellationToken cancellationToken)
        {
       
            return await _chuckNorrisService.GetRandomJokeBySearchAsync(request.search);
        }
    }
}
