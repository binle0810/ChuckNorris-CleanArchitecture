using Application.Abstractions;
using Application.Abstractions.Messaging;
using Application.Auth.Commands.CreateUser;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands.LoginUser
{
    internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public LoginUserCommandHandler(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var login = new LoginUserRequest( request.Email, request.Password);

            var token = await _userService.LoginUser(login);
          


            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return token;
        }
    }
}
