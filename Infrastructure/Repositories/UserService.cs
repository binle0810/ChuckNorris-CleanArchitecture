using Application.Abstractions;
using Application.Auth.Commands.CreateUser;
using Application.Auth.Commands.LoginUser;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _itokenservice;
        private readonly SignInManager<AppUser> _siginManager;

        public UserService(UserManager<AppUser> userManager, ITokenService itokenservice, SignInManager<AppUser> siginManager)
        {
            _userManager = userManager;
            _itokenservice = itokenservice;
            _siginManager = siginManager;
        }

        public async Task<string> RegisterUser(CreateUserRequest createUserRequest)
        {
            try
            {
                var appUser = new AppUser
                {
                    UserName = createUserRequest.Username,
                    Email = createUserRequest.Email
                };

                var createdUser = await _userManager.CreateAsync(appUser, createUserRequest.Password!);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)

                        return "Đăng ký thành công\nToken:\n" +await _itokenservice.CreateToken(appUser);
                    else
                    {
                        return string.Join("; ", roleResult.Errors.Select(e => e.Description));
                    }
                }
                else
                {
                    return string.Join("; ", createdUser.Errors.Select(e => e.Description));
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public async Task<string> LoginUser(LoginUserRequest loginUserRequest)
        {
          
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginUserRequest.Email);
            if (user == null) { 
                throw new UnauthorizedAccessException("The account does not exist"); }
            var result = await _siginManager.CheckPasswordSignInAsync(user, loginUserRequest.Password!, false) ;
            if (!result.Succeeded) throw new UnauthorizedAccessException("Wrong password");
            return "Đăng nhập thành công\nTOKEN:\n"+await _itokenservice.CreateToken(user);
            
        }

        
    }
}
