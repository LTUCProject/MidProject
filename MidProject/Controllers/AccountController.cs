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
        public AccountController(IAccountx context)
        {
            _accountServices = context;
        }
        //  [Authorize(Roles = "Admin")]

        [HttpPost("Register")]

        public async Task<ActionResult<AccountDto>> Register(RegisterdAccountDto registerdAccount)
        {
            var account = await _accountServices.Register(registerdAccount);
            return Ok(account);
        }

        //login 

        [HttpPost("Login")]
        public async Task<ActionResult<AccountDto>> Login(LoginDto loginDto)
        {
            var account = await _accountServices.AccountAuthentication(loginDto.UserName, loginDto.Password);
            if (account == null)
            {
                return Unauthorized();
            }
            return account;
        }
        [HttpPost("Logout")]
        public async Task<ActionResult<AccountDto>> LogOut(string username)
        {
            var account = await _accountServices.LogOut(username);
            return account;
        }
        
        [HttpGet("Profile")]
        public async Task<ActionResult<AccountDto>> Profile()
        {
            return await _accountServices.GetTokens(User);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("DeleteAccount")]

        public async Task<ActionResult<AccountDto>> DeleteAccount(string username)
        {
            var account = await _accountServices.DeleteAccount(username);
            return account;
        }
    }
}
