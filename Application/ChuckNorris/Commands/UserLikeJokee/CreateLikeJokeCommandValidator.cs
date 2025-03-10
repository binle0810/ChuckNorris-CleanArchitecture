using Application.Webinars.Commands.CreateWebinar;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Commands.UserLikeJokee
{
    public sealed class CreateLikeJokeCommandValidator : AbstractValidator<CreateLikeJokeCommand>
    {
        public CreateLikeJokeCommandValidator()
        {
            RuleFor(x => x.jokeid).NotEmpty().WithMessage("jokeid query is required.");

           
        }
    }
}
