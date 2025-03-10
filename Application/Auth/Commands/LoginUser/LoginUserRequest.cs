using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Commands.LoginUser
{
    public sealed record LoginUserRequest(string Email, string Password)
    {
    }
}
