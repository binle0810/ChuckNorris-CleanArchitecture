using Application.ChuckNorris.Commands.UserLikeJokee;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Commands.UserUnlikeJoke
{
    public sealed class CreateUnlikeJokeCommandValidator : AbstractValidator<CreateUnlikeJokeCommand>
    {
        public CreateUnlikeJokeCommandValidator()
        {
            RuleFor(x => x.jokeid).NotEmpty().WithMessage("jokeid query is required.");


        }
    }
}
