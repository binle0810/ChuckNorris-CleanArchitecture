using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.ChuckNorris.Queries.GetRandomJokeBySearch;
using Application.DTOs.Jokes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Queries.GetUserHistoryLike
{
    internal sealed class GetUserHistoryLikeQueryHandlerinternal : IQueryHandler<GetUserHistoryLikeQuery, List<JokeHistoryDto>>
    {
        private readonly IChuckNorrisService _chuckNorrisService;

        public GetUserHistoryLikeQueryHandlerinternal(IChuckNorrisService chuckNorrisService)
        {
            _chuckNorrisService = chuckNorrisService;
        }

        public async Task<List<JokeHistoryDto>> Handle(GetUserHistoryLikeQuery request, CancellationToken cancellationToken)
        {
            return await _chuckNorrisService.GetUserLikeJokeAsync(request.userid,request.values);
        }
    }
}
