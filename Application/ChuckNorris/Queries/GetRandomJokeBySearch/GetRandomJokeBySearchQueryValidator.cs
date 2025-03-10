using Application.Webinars.Commands.CreateWebinar;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChuckNorris.Queries.GetRandomJokeBySearch
{
    public sealed class GetRandomJokeBySearchQueryValidator : AbstractValidator<GetRandomJokeBySearchQuery>
    {
        public GetRandomJokeBySearchQueryValidator()
        {
            RuleFor(x => x.search)
                  .NotEmpty().WithMessage("Search query is required.")
                  .Length(3, 120).WithMessage("Search query must be between 3 and 120 characters.");
        }
    }
}
