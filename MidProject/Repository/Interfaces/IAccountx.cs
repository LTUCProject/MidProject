using Microsoft.AspNetCore.Identity;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using System.Security.Claims;

namespace MidProject.Repository.Interfaces
{
    public interface IAccountx
    {
        public Task<AccountDto> Register(RegisterdAccountDto registerdAccountDto);
        public Task<AccountDto> AccountAuthentication(string username, string password);
        public Task<AccountDto> LogOut(string username);
        public Task<AccountDto> GetTokens(ClaimsPrincipal claimsPrincipal);
        public Task<AccountDto> DeleteAccount(string username);
    }
}