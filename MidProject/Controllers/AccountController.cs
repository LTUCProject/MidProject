using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MidProject.Models.Dto.Request2;
using MidProject.Models.Dto.Response;
using MidProject.Repository.Interfaces;

namespace MidProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountx _accountServices;

        public AccountController(IAccountx accountServices)
        {
            _accountServices = accountServices;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AccountDto>> Register(RegisterdAccountDto registerdAccount)
        {
            try
            {
                var account = await _accountServices.Register(registerdAccount);
                if (account == null)
                {
                    return BadRequest("Registration failed");
                }
                return CreatedAtAction(nameof(Profile), new { id = account.Id }, account);
            }
            catch (Exception ex)
            {
                // Log the exception if you have a logging service
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred during registration.", details = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AccountDto>> Login(LoginDto loginDto)
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
        public async Task<ActionResult<AccountDto>> LogOut(string username)
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
        public async Task<ActionResult<AccountDto>> Profile()
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

        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteAccount")]
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
    }
}