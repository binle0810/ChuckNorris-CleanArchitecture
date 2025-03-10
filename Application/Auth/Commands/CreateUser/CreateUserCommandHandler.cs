using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Webinars.Commands.CreateWebinar;
using Domain.Abstractions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.CreateUser
{
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, string>
    {
     //   private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public CreateUserCommandHandler( IUnitOfWork unitOfWork, IUserService userService)
        {
      //      _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var webinar = new CreateUserRequest(request.Username, request.Email, request.Password);

          var token= await _userService.RegisterUser(webinar);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return token;
        }
    }
}
