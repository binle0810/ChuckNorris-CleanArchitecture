using Application.Abstractions.Messaging;
using Application.Abstractions;
using Application.ChuckNorris.Commands.UserLikeJokee;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.Jokes;

namespace Application.ChuckNorris.Commands.UserUnlikeJoke
{
    internal sealed class CreateUnlikeJokeCommandHandler : ICommandHandler<CreateUnlikeJokeCommand, JokeDto>
    {
        private readonly IChuckNorrisService _chuckNorrisService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUnlikeJokeCommandHandler(IChuckNorrisService chuckNorrisService, IUnitOfWork unitOfWork)
        {
            _chuckNorrisService = chuckNorrisService;
            _unitOfWork = unitOfWork;
        }

        public async Task<JokeDto> Handle(CreateUnlikeJokeCommand request, CancellationToken cancellationToken)
        {

     

            var jokelike = await _chuckNorrisService.DeleteLikeJokeAsync(request.userid,request.jokeid);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return jokelike;
        }
    }
}
