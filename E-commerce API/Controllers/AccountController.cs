using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using E_commerceAPI.Entities.DTOs.Register;
using Microsoft.AspNetCore.Identity;
using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.LogIn;
using E_commerceAPI.Services.Repositories.Interfaces;
using E_commerceAPI.Entities.DTOs;
namespace E_commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMailService _mailService;
        public AccountController(IAuthService _authService, IMailService _mailService )
        {
            this._authService = _authService;
            this._mailService= _mailService;
        }
        [HttpPost("Register")]
   
        public async Task<IActionResult> Register(RegisterDTO register)
        {
        if (ModelState.IsValid)
        {
          var result = await _authService.Register(register);
                if (result.IsAuthenticated == true)
                    return Ok(result);
                else                
                    return BadRequest(result);           
        }
        else
           return BadRequest(ModelState);
    }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody]LogInDTO Model)
        {
            if (ModelState.IsValid)
            {
              var token = await _authService.LogIn(Model);
                if (token.IsAuthenticated == true)
                {
                    return StatusCode(StatusCodes.Status200OK ,new { token.token , token.ExpireOn });
                }
                return BadRequest(new {token.Message});
            }
            return BadRequest(ModelState);

        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(AddRole addRole)
        {
            if (ModelState.IsValid)
            {
                var Result = await _authService.AddRole(addRole);
                return Ok(Result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> EmailTest([FromForm]EmailRequestDTO dto)
        {
             await _mailService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body, dto.Attachments);

            return Ok();
        }
        [HttpPost("Welcome")]
        public async Task<IActionResult> SendWelcomeEmail([FromBody]WelcomeRequestDTO dto)
        {
            var filepath = $"{Directory.GetCurrentDirectory()}\\Templets\\WelcomePage.html";
            var str = new StreamReader(filepath);
            var mailText=str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[username]", dto.UserName).Replace("[email]", dto.Email);
            await _mailService.SendEmailAsync(dto.Email, "Welcome", mailText);
            return Ok();

        }




    }
}
