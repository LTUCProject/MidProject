using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MidProject.Models;
using MidProject.Models.Dto.Request;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;
using MidProject.Repository.Services;

namespace MidProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountx _accountServices;
        private readonly MailjetEmailService _emailService;
        private readonly UserManager<Account> _identityUserManager;

        public AccountController(IAccountx accountServices, MailjetEmailService emailService, UserManager<Account> identityUserManager)
        {
            _accountServices = accountServices;
            _emailService = emailService;
            _identityUserManager = identityUserManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AccountRegisterdResponseDto>> Register(RegisterdAccountDto registerdAccount)
        {
            try
            {
                // Validate roles
                var validRoles = new List<string> { "Admin", "Client", "Owner", "Servicer" };
                foreach (var role in registerdAccount.Roles)
                {
                    if (!validRoles.Contains(role))
                    {
                        return BadRequest($"Invalid role: {role}. Allowed roles are: Admin, Client, Owner, Servicer.");
                    }
                }

                // Register account
                var account = await _accountServices.Register(registerdAccount);
                if (account == null)
                {
                    return BadRequest("Registration failed");
                }

                // Send registration email
                string emailBody = "<h1>Welcome to EV Management System</h1><p>Thank you for registering! You can now log in and start using the platform.</p>";
                bool emailSent = await _emailService.SendEmailAsync(registerdAccount.Email, registerdAccount.UserName, emailBody);

                if (!emailSent)
                {
                    // Optionally log the email sending failure
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Registration succeeded, but email sending failed." });
                }

                // Prepare and return response
                var responseDto = new AccountRegisterdResponseDto
                {
                    Id = account.Id,
                    UserName = account.UserName,
                    Roles = account.Roles
                };

                return CreatedAtAction(nameof(Profile), new { id = responseDto.Id }, responseDto);
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logging service
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during registration.", details = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AccountRegisterdResponseDto>> Login(LoginDto loginDto)
        {
            try
            {
                var account = await _accountServices.AccountAuthentication(loginDto.UserName, loginDto.Password);
                if (account == null)
                {
                    return Unauthorized("Invalid username or password");
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logging service
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during login.", details = ex.Message });
            }
        }

        [HttpPost("Logout")]
        public async Task<ActionResult<AccountRegisterdResponseDto>> LogOut(string username)
        {
            try
            {
                var account = await _accountServices.LogOut(username);
                return Ok(new { message = "Logout successful.", account });
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logging service
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during logout.", details = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<ActionResult<AccountRegisterdResponseDto>> Profile()
        {
            try
            {
                var profile = await _accountServices.GetTokens(User);
                if (profile == null)
                {
                    return Unauthorized("User not found or not authenticated");
                }
                return Ok(profile);
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logging service
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the profile.", details = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(string username)
        {
            try
            {
                var account = await _accountServices.DeleteAccount(username);
                if (account == null)
                {
                    return NotFound("Account not found");
                }
                return Ok(new { message = "Account deleted successfully.", account });
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logging service
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the account.", details = ex.Message });
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string email)
        {
            try
            {
                await _accountServices.SendPasswordResetEmailAsync(email);
                return Ok("Verification code sent to email.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ValidateCode")]
        public async Task<IActionResult> ValidateCode(int code)
        {
            try
            {
                var isValid = _accountServices.ValidateCode(code);
                if (!isValid)
                {
                    return BadRequest("Invalid or expired code.");
                }
                return Ok("Code is valid.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("NewPassword")]
        public async Task<IActionResult> NewPassword(string newPassword, int code)
        {
            try
            {
                var result = await _accountServices.NewPassword(newPassword, code);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _identityUserManager.GetUserId(User); // Get the currently logged-in user's ID
            var result = await _accountServices.ChangePasswordAsync(userId, changePasswordDTO);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok("Password changed successfully.");
        }
    }
}