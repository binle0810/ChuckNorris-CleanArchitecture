using Application.Abstractions;
using Application.Auth.Commands.LoginUser;
using Domain.Abstractions;
using FluentAssertions;
using FluentValidation.TestHelper;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Unit_test.Auth
{
    public class LoginUserCommandHandlerTests
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly LoginUserCommandHandler _handler;
        private readonly LoginUserCommandValidator _validator;

        public LoginUserCommandHandlerTests()
        {
            _userService = Substitute.For<IUserService>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new LoginUserCommandHandler(_unitOfWork, _userService);
            _validator = new LoginUserCommandValidator();
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenLoginIsSuccessful()
        {
            // Arrange
            var request = new LoginUserCommand("test@example.com", "ValidPass123!");
            var expectedToken = "mocked-jwt-token";
            _userService.LoginUser(Arg.Any<LoginUserRequest>()).Returns(Task.FromResult(expectedToken));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Equals(expectedToken);
            await _userService.Received(1).LoginUser(Arg.Any<LoginUserRequest>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenLoginFails()
        {
            // Arrange
            var request = new LoginUserCommand("test@example.com", "WrongPass");
            _userService.LoginUser(Arg.Any<LoginUserRequest>()).ThrowsAsync(new UnauthorizedAccessException("The account does not exist"));

            // Act & Assert
            await FluentActions.Invoking(() => _handler.Handle(request, CancellationToken.None))
                .Should().ThrowAsync<UnauthorizedAccessException>();

            await _userService.Received(1).LoginUser(Arg.Any<LoginUserRequest>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public void Validate_ShouldHaveError_WhenEmailIsInvalid()
        {
            // Arrange
            var request = new LoginUserCommand("invalid-email", "ValidPass123!");

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("Email không hợp lệ.");
        }
    }
}
