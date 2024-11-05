using Microsoft.AspNetCore.Identity;
using MidProject.Models;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace MidProject.Repository.Services
{
    public class IdentityAccountService : IAccountx
    {
        private readonly UserManager<Account> _accountManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly JwtTokenService _jwtTokenService;
        private readonly MidprojectDbContext _context;
        private readonly MailjetEmailService _emailService;
        private readonly string _baseUrl;

        public IdentityAccountService
        (
            UserManager<Account> accountManager,
            SignInManager<Account> signInManager,
            JwtTokenService jwtTokenService,
            MidprojectDbContext context,
            MailjetEmailService emailService,
            IConfiguration configuration) // Inject IConfiguration instead
            {
            _accountManager = accountManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
            _emailService = emailService;
            _baseUrl = configuration["BaseUrl"]; // Assume you have a setting called BaseUrl
            }


        private static int? ran;
        private static string? email;

        public async Task SendPasswordResetEmailAsync(string email1)
        {
            var resetEmail = await _accountManager.FindByEmailAsync(email1);
            if (resetEmail != null)
            {
                ran = new Random().Next(1111, 9999);
                await _emailService.SendEmailAsync(resetEmail.Email, resetEmail.UserName, ran.ToString());
                email = email1;
            }

        }
        public bool ValidateCode(int code)
        {
            return ran.HasValue && code == ran; // Validate the code
        }
        public async Task<string> NewPassword(string newPassword, int c)

        {
            bool code = ValidateCode(c);
            if (code)
            {
                // Find the user by the stored email
                var user = await _accountManager.FindByEmailAsync(email);
                if (user != null)
                {
                    // Set the new password directly
                    user.PasswordHash = _accountManager.PasswordHasher.HashPassword(user, newPassword);
                    // Update the user in the database
                    var result = await _accountManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return "Password has been reset successfully.";
                    }
                    return "Failed to reset password.";
                }
            }
            return "User not found.";
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////
        public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordDTO model)
        {                                                        // To ChangePassword you must be Loged in 
                                                                 // Get the user by Id
            var user = await _accountManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "User not found."
                });
            }

            // Ensure the new password matches the confirmation
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "New password and confirmation do not match."
                });
            }

            // Change the user's password
            var result = await _accountManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                // Sign in the user again to refresh the security token
                await _signInManager.RefreshSignInAsync(user);
            }

            return result;
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


