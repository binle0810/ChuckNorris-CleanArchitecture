using Application.Auth.Commands.CreateUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Commands.LoginUser
{
    public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>

    {
        public LoginUserCommandValidator()
        {
            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Email không hợp lệ."); ;
            RuleFor(c => c.Password)
                 .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                 .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự.");
        }
    }
}
