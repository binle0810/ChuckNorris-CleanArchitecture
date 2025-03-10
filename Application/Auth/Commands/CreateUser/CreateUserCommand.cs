using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Commands.CreateUser
{
    public sealed record CreateUserCommand(string Username,string Email,string Password) : ICommand<string>;
   
}
