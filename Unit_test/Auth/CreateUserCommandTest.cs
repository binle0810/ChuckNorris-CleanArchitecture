using Application.Abstractions;
using Application.Auth.Commands.CreateUser;
using Domain.Abstractions;
using Infrastructure.Repositories;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Unit_test.Auth
{
    public class CreateUserCommandTest
    {
        private static readonly CreateUserCommand _createUserCommand = new("user3", "user3@gmail.com", "User3@123");
        private readonly CreateUserCommandHandler _handler;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public CreateUserCommandTest()
        {
        
            this._unitOfWork = Substitute.For<IUnitOfWork>();
            this._userService = Substitute.For<IUserService>();
            this._handler = new CreateUserCommandHandler(_unitOfWork,_userService);
        }
        [Fact]
        public async Task Handle_ShouldReturnToken_WhenUserIsRegisteredSuccessfully()
        {
            // Arrange
    
            var expectedToken = "mocked_token";

            _userService.RegisterUser(Arg.Any<CreateUserRequest>()).Returns(Task.FromResult(expectedToken));

            // Act
            var result = await _handler.Handle(_createUserCommand, CancellationToken.None);

            // Assert
            Assert.Equal(expectedToken, result);
            await _userService.Received(1).RegisterUser(Arg.Any<CreateUserRequest>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }
        [Fact]
        public void Add_TwoNumbers_ReturnsSum()
        {
            // Arrange
            int a = 2, b = 3;

            // Act
            int result = a + b;

            // Assert
            Assert.Equal(5, result);
        }
    }
}
