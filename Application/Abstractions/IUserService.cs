using Application.Auth.Commands.CreateUser;
using Application.Auth.Commands.LoginUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions
{
    public interface IUserService
    {
        Task<string> RegisterUser(CreateUserRequest createUserRequest);
        Task<string> LoginUser(LoginUserRequest loginUserRequest);

    }
}
