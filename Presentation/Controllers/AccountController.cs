using Application.Auth.Commands.CreateUser;
using Application.Auth.Commands.LoginUser;
using Application.Webinars.Commands.CreateWebinar;
using Domain.Entities;
using Mapster;
using MediatR;
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
    public sealed class AccountController : ApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequest createUserRequest,
    CancellationToken cancellationToken)
        {
            var command = createUserRequest.Adapt<CreateUserCommand>();

            var userRegister = await Sender.Send(command, cancellationToken);

            return Ok(userRegister);
        }
        // user1@gmail.com
        // User1@123
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest loginUserRequest, CancellationToken cancellationToken)
        {
            var command = loginUserRequest.Adapt<LoginUserCommand>();

            var userLogin = await Sender.Send(command, cancellationToken);

            return Ok(userLogin);
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            

            return Ok("logout successful");
        }
    }
}
