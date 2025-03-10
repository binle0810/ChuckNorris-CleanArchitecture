using Application.Abstractions;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Commands.UserLikeJokee
{
    internal sealed class CreateLikeJokeCommandHandler : ICommandHandler<CreateLikeJokeCommand, object>
    {
        private readonly IChuckNorrisService _chuckNorrisService;
        private readonly IUnitOfWork _unitOfWork;
        

        public CreateLikeJokeCommandHandler(IChuckNorrisService chuckNorrisService, IUnitOfWork unitOfWork)
        {
            _chuckNorrisService = chuckNorrisService;
            _unitOfWork = unitOfWork;
        }

        public async Task<object> Handle(CreateLikeJokeCommand request, CancellationToken cancellationToken)
        {
            /////////////////////////////////////
            var userlikejoke = new UserLikeJoke(request.userid, request.jokeid,DateTime.Now);

          var jokelike=  await _chuckNorrisService.CreateLikeJokeAsync(userlikejoke);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
         
            return new
            {
                jokeid= jokelike.Id,
                UserId= userlikejoke.UserId,
                LikeCount=jokelike.Like
            };
        }
    }
}
