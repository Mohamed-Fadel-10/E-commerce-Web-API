using E_commerceAPI.Entities.DTOs.LogIn;
using E_commerceAPI.Entities.DTOs.Register;
using E_commerceAPI.Services.Repositories.Interfaces;
using EMS_SYSTEM.DOMAIN.DTO.NewFolder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace E_commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        public AccountController(IAuthService _authService, IMailService _mailService)
        {
            this._authService = _authService;
            this._mailService = _mailService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Register(register);
                if (result.IsAuthenticated == true)
                {
                    var filePath = $"{Directory.GetCurrentDirectory()}\\Templets\\WelcomePage.html";
                    var str = new StreamReader(filePath);
                    var mailText = str.ReadToEnd();
                    str.Close();
                    mailText = mailText.Replace("[username]", register.UserName).Replace("[email]", register.Email);
                    await _mailService.SendEmailAsync(register.Email, "Registration Done Successfully", mailText);
                    return Ok(result);
                }
                else
                    return BadRequest(result);
            }
            else
                return BadRequest(ModelState);
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LogInDTO Model)
        {
            if (ModelState.IsValid)
            {
                var token = await _authService.LogIn(Model);
                if (token.IsAuthenticated == true)
                {
                    return StatusCode(StatusCodes.Status200OK, new { token.Message,token.Token, token.RefreshToken,token.RefreshTokenExpiration });
                }
                return BadRequest(new { token.Message });
            }
            return BadRequest(ModelState);

        }
        [HttpPost("RefreshToken")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshDTO model)
        {
            if (ModelState.IsValid)
            {
                var Response = await _authService.NewRefreshToken(model.Token);
                if (Response.IsAuthenticated == true)
                {
                    return Ok(new { Response });
                }
                return BadRequest(new { Response.Message });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("LogOut")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LogOut()
        {
            var Result = await _authService.LogoutAsync();
            if (Result.isDone == true)
            {
                return StatusCode(Result.StatusCode, Result.Message);
            }
            return StatusCode(Result.StatusCode, Result.Message);
        }
    }
}
