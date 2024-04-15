using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using E_commerceAPI.Entities.DTOs.Register;
using Microsoft.AspNetCore.Identity;
using E_commerceAPI.Entities.DTOs.LogIn;
using E_commerceAPI.Services.Repositories.Interfaces;
using E_commerceAPI.Entities.DTOs.EmailSettingsDTOs;
using Microsoft.AspNetCore.Authorization;
using E_commerceAPI.Entities.DTOs.PasswordSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using E_commerceAPI.Entities.DTOs.Create;
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
        public async Task<IActionResult> Register([FromBody]RegisterDTO register)
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

        [HttpPost("LogOut")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            var Result = await _authService.LogoutAsync();
            if (Result.isDone == true)
            {
                return StatusCode(Result.StatusCode, Result.Message);
            }
            return StatusCode(Result.StatusCode, Result.Message);
        }


        [HttpPost("CreateRole")]
        [Authorize(Roles ="Admin",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateRoleAsync([FromBody]string Role)
        {
            if (ModelState.IsValid)
            {
                var Result = await _authService.CreateRoleAsync(Role);
                if(Result.isDone == true) {
                    return StatusCode(Result.StatusCode,Result.Message);
                }
                return StatusCode(Result.StatusCode, Result.Message);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("AddRoleToUser")]
        [Authorize(Roles ="Admin",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddUserToRoleAsync(UserRoleDTO model)
        {
            if (ModelState.IsValid)
            {
               var response= await _authService.AddUserToRoleAsync(model);
                if(response.isDone== true)
                {
                    return StatusCode(response.StatusCode,response.Message);
                }
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromForm] EmailRequestDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _mailService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body, dto.Attachments);
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("WelcomeEmail")]
        public async Task<IActionResult> SendWelcomeEmail([FromBody] WelcomeRequestDTO model)
        {
            var filepath = $"{Directory.GetCurrentDirectory()}\\Templets\\WelcomePage.html";
            var str = new StreamReader(filepath);
            var mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[username]", model.UserName).Replace("[email]", model.Email);
            await _mailService.SendEmailAsync(model.Email, "Welcome", mailText);
            return Ok("Mail Send Successfully");
        }

        [HttpPost("Change Password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePasswordAsync(ChangepasswordDTO model)
        {
            if (ModelState.IsValid)
            {
                var Response = await _authService.ChangePasswordAsync(model);
                if (Response.IsAuthenticated == true)
                {
                    return StatusCode(StatusCodes.Status200OK, Response.Message);
                }
                return StatusCode(StatusCodes.Status400BadRequest, Response.Message);
            }
            return BadRequest("Failed Process To Change Password");

        }


        [HttpDelete("RemoveRoleFromUser")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RemoveRoleFromUserAsync(UserRoleDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _authService.RemoveUserFromRoleAsync(model);
                if (response.isDone == true)
                {
                    return StatusCode(response.StatusCode, response.Message);
                }
                return StatusCode(response.StatusCode, response.Message);
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("DeleteRole/{roleId}")]
        [Authorize(Roles ="Admin", AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteRole(string roleId) 
        {
            if (ModelState.IsValid)
            {
                var Result = await _authService.DeleteRole(roleId);
                if (Result.isDone == true)
                {
                    return StatusCode(Result.StatusCode, Result.Message);
                }
                return StatusCode(Result.StatusCode, Result.Message);
            }
            return BadRequest();
        }

        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "Admin",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUserAsync (string UserId)
        {
            if (ModelState.IsValid)
            {
              var Result =  await _authService.DeleteUserAsync(UserId);
                if (Result.isDone == true)
                {
                    return StatusCode(Result.StatusCode, Result.Message);
                }
                return StatusCode(Result.StatusCode, Result.Message);
            }
            return BadRequest(ModelState);

        }






    }
}
