using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MidProject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MidProject.Repository.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<Account> _signInManager;

        public JwtTokenService(IConfiguration configuration, SignInManager<Account> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public static TokenValidationParameters ValidatToken(IConfiguration configuration)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSecurityKey(configuration),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }

        public static SecurityKey GetSecurityKey(IConfiguration configuration)
        {
            var secretKey = configuration["JWT:SecretKey"];
            if (secretKey == null)
            {
                throw new InvalidOperationException("jwt secret key is not exist");
            }

            var secretBytes = Encoding.UTF8.GetBytes(secretKey);

            return new SymmetricSecurityKey(secretBytes);
        }


        public async Task<string> GenerateToken(Account user, TimeSpan expiryDate)
        {
            var userPrincliple = await _signInManager.CreateUserPrincipalAsync(user);
            if (userPrincliple == null)
            {
                return null;
            }

            var signInKey = GetSecurityKey(_configuration);

            var token = new JwtSecurityToken
                (
                expires: DateTime.UtcNow + expiryDate,
                signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256),
                claims: userPrincliple.Claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
