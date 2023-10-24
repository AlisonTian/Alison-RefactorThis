using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RefactorThis.Dtos;
using RefactorThis.Services;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService, ILogger<LoginController> logger)
        {
            _loginService = loginService;
            _logger = logger;
        }

        [HttpPost]
        //Hint: "username":"test", "password":"1234" 
        public async Task<IActionResult> Login(UserLoginDto login)
        {
            var user = await _loginService.AuthenticateWithUsernamePassword(login.Username, login.Password);
            if (user == null)
            {
                return BadRequest("You can't log in");
            }

            var isTokenVaild = _loginService.IsVaildToken(user);
            TokenDto tokenDto = new TokenDto();
            if (isTokenVaild)
            {
                tokenDto.APIToken = user.APIToken;
                tokenDto.Message = "You are logged in and can use the API";
            }
            else
            {
                try
                {
                    var newToken = await _loginService.GenerateToken(user);
                    tokenDto.APIToken = newToken;
                    tokenDto.Message = "Your api token has expired, but it has been refresh.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred during generate token for user (name:{user.Name})");
                    return BadRequest(ex);
                }
               
            }
            return Ok(tokenDto);

        }
    }
}