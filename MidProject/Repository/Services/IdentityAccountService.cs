using Microsoft.AspNetCore.Identity;
using MidProject.Models;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;

namespace MidProject.Repository.Services
{
    public class IdentityAccountService : IAccountx
    {
        private readonly UserManager<Account> _accountManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly MidprojectDbContext _context;
        public IdentityAccountService(
            UserManager<Account> accountManager,
            SignInManager<Account> signInManager,
            JwtTokenService jwtTokenService,
            MidprojectDbContext context)
        {
            _accountManager = accountManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }
        public async Task<AccountRegisterdResponseDto> Register(RegisterdAccountDto registerdAccountDto)
        {
            // Validate roles
            var validRoles = new List<string> { "Admin", "Client", "Owner", "Servicer" };
            foreach (var role in registerdAccountDto.Roles)
            {
                if (!validRoles.Contains(role))
                {
                    throw new ArgumentException($"Invalid role: {role}. Allowed roles are: Admin, Client, Owner, Servicer.");
                }
            }
            var account = new Account
            {
                UserName = registerdAccountDto.UserName,
                Email = registerdAccountDto.Email,
            };
            var result = await _accountManager.CreateAsync(account, registerdAccountDto.Password);
            if (result.Succeeded)
            {
                await _accountManager.AddToRolesAsync(account, registerdAccountDto.Roles);
                foreach (var role in registerdAccountDto.Roles)
                {
                    switch (role)
                    {
                        case "Admin":
                            var admin = new Admin
                            {
                                AccountId = account.Id,
                                Name = account.UserName,
                                Email = account.Email,
                                // Populate additional Admin fields if needed
                            };
                            _context.Admins.Add(admin);
                            break;
                        case "Client":
                            var client = new Client
                            {
                                AccountId = account.Id,
                                Name = account.UserName,
                                Email = account.Email,
                                // Populate additional Client fields if needed
                            };
                            _context.Clients.Add(client);
                            break;
                        case "Owner":
                        case "Servicer":
                            var provider = new Provider
                            {
                                AccountId = account.Id,
                                Name = account.UserName,
                                Email = account.Email,
                                Type = role,
                            };
                            _context.Providers.Add(provider);
                            break;
                    }
                }
                await _context.SaveChangesAsync();
                return new AccountRegisterdResponseDto
                {
                    Id = account.Id,
                    UserName = account.UserName,
                    Roles = await _accountManager.GetRolesAsync(account)
                };
            }
            throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        public async Task<AccountLoginDto> AccountAuthentication(string username, string password)
        {
            var account = await _accountManager.FindByNameAsync(username);
            if (account == null || !await _accountManager.CheckPasswordAsync(account, password))
            {
                throw new Exception("Invalid username or password.");
            }
            return new AccountLoginDto
            {
                Id = account.Id,
                UserName = account.UserName,
                Token = await _jwtTokenService.GenerateToken(account, TimeSpan.FromMinutes(90)),
                Roles = await _accountManager.GetRolesAsync(account)
            };
        }
        public async Task<AccountRegisterdResponseDto> LogOut(string username)
        {
            var account = await _accountManager.FindByNameAsync(username);
            if (account == null)
            {
                throw new Exception("Account not found.");
            }
            await _signInManager.SignOutAsync();
            return new AccountRegisterdResponseDto
            {
                Id = account.Id,
                UserName = account.UserName,
                Roles = await _accountManager.GetRolesAsync(account)
            };
        }
        public async Task<AccountRegisterdResponseDto> DeleteAccount(string username)
        {
            var account = await _accountManager.FindByNameAsync(username);
            if (account == null)
            {
                throw new Exception("Account not found.");
            }
            await _accountManager.DeleteAsync(account);
            return new AccountRegisterdResponseDto
            {
                Id = account.Id,
                UserName = account.UserName,
                Roles = await _accountManager.GetRolesAsync(account)

            };
        }
        public async Task<AccountRegisterdResponseDto> GetTokens(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _accountManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            return new AccountRegisterdResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = await _accountManager.GetRolesAsync(user)

            };
        }

        
    }
}


