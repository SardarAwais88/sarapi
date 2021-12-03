
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using sarapi.Models;
using sarapi.Data;

namespace sarapi.Services
{
    public class AuthManager : IAuthManager
    {
        // create instance of usermaanger and use it in our class
        private readonly UserManager<User> _userManager;
        //   private readonly IConfiguration _configuration;
        private readonly IConfiguration _configuration;
           private User _user;

        // create a ctor
        // use injection
        public AuthManager(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
       // create token 
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
           var token = GenerateTokenOptions((SigningCredentials)signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(
                jwtSettings.GetSection("lifetime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Name, _user.UserName)
             };
            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Environment.GetEnvironmentVariable("KEY");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        // validate the user and create signing credientals
        public async Task<bool> ValidateUser(LoginDto loginDto)
        {
            _user = await _userManager.FindByNameAsync(loginDto.Email);
            var validPassword = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            return (_user != null && validPassword);
        }

      
    }
}
