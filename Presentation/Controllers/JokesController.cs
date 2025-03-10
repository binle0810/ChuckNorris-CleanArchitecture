using Application.ChuckNorris.Commands.UserLikeJokee;
using Application.ChuckNorris.Commands.UserUnlikeJoke;
using Application.ChuckNorris.Queries.Getcategory;
using Application.ChuckNorris.Queries.GetRandomJoke;
using Application.ChuckNorris.Queries.GetRandomJokeByCategory;
using Application.ChuckNorris.Queries.GetRandomJokeBySearch;
using Application.ChuckNorris.Queries.GetUserHistoryLike;
using Domain.Entities;
using Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public sealed class JokesController : ApiController
    {
     
      
        [HttpGet("GetRandomJokeByCategory")]
        public async Task<IActionResult> GetRandomJokeByCategory(string category, CancellationToken cancellationToken)
        {
            var query = new GetRandomJokeByCategoryQuery(category);

            var jokes = await Sender.Send(query, cancellationToken);

            return Ok(jokes);
        }
        [HttpGet("GetRandomJoke")]
        public async Task<IActionResult> GetRandomJoke(CancellationToken cancellationToken)
        {
            var query = new GetRandomJokeQuery();

            var jokes = await Sender.Send(query, cancellationToken);

            return Ok(jokes);
        }
        [HttpGet("GetRandomJokeBySearch")]
        public async Task<IActionResult> GetRandomJokeBySearch( string search ,CancellationToken cancellationToken)
        {
            var query = new GetRandomJokeBySearchQuery(search);

            var jokes = await Sender.Send(query, cancellationToken);

            return Ok(jokes);
        }
        [Authorize]
        [HttpPost("Like")]
        public async Task<IActionResult> LikeJoke(string Jokeid, CancellationToken cancellationToken)
        {
            var userid = User.GetUserId();
     
            var command= new CreateLikeJokeCommand(userid,Jokeid);

            var jokes = await Sender.Send(command, cancellationToken);

            return Ok(jokes);
        }
        [Authorize]
        [HttpPost("UnLike")]
        public async Task<IActionResult> UnLikeJoke(string Jokeid, CancellationToken cancellationToken)
        {
            var userid = User.GetUserId();

            var command = new CreateUnlikeJokeCommand(userid, Jokeid);

            var jokes = await Sender.Send(command, cancellationToken);

            return Ok(jokes);
        }
        [Authorize]
        [HttpPost("LikeHistory")]
        public async Task<IActionResult> LikeHistoryJoke([FromQuery] QueryWithPage values ,CancellationToken cancellationToken)
        {
            var userid = User.GetUserId();

            var query = new GetUserHistoryLikeQuery(userid, values);

            var jokes = await Sender.Send(query, cancellationToken);

            return Ok(jokes);
        }
    }
}
