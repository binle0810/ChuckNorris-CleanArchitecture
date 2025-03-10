using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.ChuckNorris.Queries.Getcategory;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Queries.GetRandomJokeByCategory
{
    internal sealed class GetRandomJokeByCategoryQueryHandle : IQueryHandler<GetRandomJokeByCategoryQuery, string>
    {
        private readonly IChuckNorrisService _chuckNorrisService;

        public GetRandomJokeByCategoryQueryHandle(IChuckNorrisService chuckNorrisService)
        {
            _chuckNorrisService = chuckNorrisService;
        }

        public async Task<string> Handle(GetRandomJokeByCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _chuckNorrisService.GetRandomJokeByCategoryAsync(request.category);
        }
    }
}
