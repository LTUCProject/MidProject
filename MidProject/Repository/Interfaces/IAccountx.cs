using Microsoft.AspNetCore.Identity;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using System.Security.Claims;

namespace MidProject.Repository.Interfaces
{
    public interface IAccountx
    {
        public Task<AccountRegisterdResponseDto> Register(RegisterdAccountDto registerdAccountDto);
        public Task<AccountLoginDto> AccountAuthentication(string username, string password);
        public Task<AccountRegisterdResponseDto> LogOut(string username);
        public Task<AccountRegisterdResponseDto> GetTokens(ClaimsPrincipal claimsPrincipal);
        public Task<AccountRegisterdResponseDto> DeleteAccount(string username);
        Task SendPasswordResetEmailAsync(string email);
        bool ValidateCode(int code);
        Task<string> NewPassword(string newPassword, int c);
        public Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDTO model);
    }
}