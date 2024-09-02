using Microsoft.AspNetCore.Identity;
using MidProject.Models;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using System.Security.Claims;

namespace MidProject.Repository.Services
{
    public class IdentityAccountService : IAccountx
    {
        private readonly UserManager<Account> _accountManager;
        private readonly SignInManager<Account> _signInManager;

        private JwtTokenService _jwtTokenService;
        public IdentityAccountService(UserManager<Account> Manager, SignInManager<Account> signInManager, JwtTokenService jwtTokenService)
        {
            _accountManager = Manager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AccountDto> Register(RegisterdAccountDto registerdAccountDto)
        {
            var account = new Account()
            {
                UserName = registerdAccountDto.UserName,
                Email = registerdAccountDto.Email,

            };
            var result = await _accountManager.CreateAsync(account, registerdAccountDto.Password);
            var x = registerdAccountDto.Roles;
            if (result.Succeeded)
            {
                await _accountManager.AddToRolesAsync(account, registerdAccountDto.Roles);
                return new AccountDto()
                {
                    Id = account.Id,
                    UserName = account.UserName,
                    Token = await _jwtTokenService.GenerateToken(account, System.TimeSpan.FromMinutes(7)),
                    Roles = await _accountManager.GetRolesAsync(account)
                };
            }

            return null;
        }

        public async Task<AccountDto> AccountAuthentication(string username, string password)
        {
            var account = await _accountManager.FindByNameAsync(username);
            bool passValidation = await _accountManager.CheckPasswordAsync(account, password);
            if (passValidation)
            {
                return new AccountDto()
                {
                    Id = account.Id,
                    UserName = account.UserName,
                    Token = await _jwtTokenService.GenerateToken(account, System.TimeSpan.FromMinutes(7)),
                    Roles = await _accountManager.GetRolesAsync(account)
                };
            }
            return null;
        }
        
       

        public async Task<AccountDto> LogOut(string username)
        {
            var account = await _accountManager.FindByNameAsync(username);
            if (account == null)
            {
                throw new Exception("Account not found.");
            }

            await _signInManager.SignOutAsync();

            var result = new AccountDto()
            {
                Id = account.Id,
                UserName = account.UserName
            };

            return result;
        }

        public async Task<AccountDto> DeleteAccount(string username)
        {
            var account = await _accountManager.FindByNameAsync(username);
            if (account == null)
            {
                throw new Exception("Account not found.");
            }

            await _accountManager.DeleteAsync(account);

            var result = new AccountDto()
            {
                Id = account.Id,
                UserName = account.UserName
            };

            return result;
        }

        public async Task<AccountDto> GetTokens(ClaimsPrincipal claimsPrincipal)
        {
            var newToken = await _accountManager.GetUserAsync(claimsPrincipal);

            if (newToken == null)
            {
                throw new InvalidOperationException("Token is not exist");
            }
            return new AccountDto()
            {
                Id = newToken.Id,
                UserName = newToken.UserName,
                Token = await _jwtTokenService.GenerateToken(newToken, System.TimeSpan.FromMinutes(5))
            };
        }
    }
}
