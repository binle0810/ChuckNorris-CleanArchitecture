using Application.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authentication
{
    public sealed class TokenService:ITokenService
    {
   
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtOptions _jwtOptions;
       
        public TokenService( UserManager<AppUser> userManager, JwtOptions jwtOptions)
        {
            
            _userManager = userManager;
            _jwtOptions = jwtOptions;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        }

        public async Task<string> CreateToken(AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                  new Claim(ClaimTypes.Email, user.Email), // Thêm claim email
                  new Claim(ClaimTypes.GivenName, user.UserName), // Thêm claim given_name
                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) // Thêm claim sub
            };

            // Thêm tất cả các role vào claims
            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
