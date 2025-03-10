using Application.Webinars.Commands.CreateWebinar;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Commands.CreateUser
{
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Email không hợp lệ.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Tên người dùng không được để trống.")
                .MinimumLength(3).WithMessage("Tên người dùng phải có ít nhất 3 ký tự.")
                .MaximumLength(20).WithMessage("Tên người dùng không được vượt quá 20 ký tự.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống.")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự.")
                .Matches("[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất một chữ cái in hoa.")
                .Matches("[a-z]").WithMessage("Mật khẩu phải chứa ít nhất một chữ cái thường.")
                .Matches("[0-9]").WithMessage("Mật khẩu phải chứa ít nhất một chữ số.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Mật khẩu phải chứa ít nhất một ký tự đặc biệt.");
        }
    }
}
