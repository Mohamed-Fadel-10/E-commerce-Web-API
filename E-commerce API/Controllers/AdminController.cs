using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.EmailSettingsDTOs;
using E_commerceAPI.Entities.DTOs.PasswordSettings;
using E_commerceAPI.Services.Repositories.Interfaces;
using E_commerceAPI.Services.Repositories.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IMailService _mailService;
        public AdminController(IAdminService _adminService, IMailService _mailService)
        {
            this._adminService = _adminService;
            this._mailService = _mailService;
        }
        [HttpPost("CreateRole")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateRoleAsync([FromBody] string Role)
        {
            if (ModelState.IsValid)
            {
                var Result = await _adminService.CreateRoleAsync(Role);
                if (Result.isDone == true)
                {
                    return StatusCode(Result.StatusCode, Result.Message);
                }
                return StatusCode(Result.StatusCode, Result.Message);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("AddRoleToUser")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddUserToRoleAsync(UserRoleDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _adminService.AddUserToRoleAsync(model);
                if (response.isDone == true)
                {
                    return StatusCode(response.StatusCode, response.Message);
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

       


        [HttpDelete("RemoveRoleFromUser")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RemoveRoleFromUserAsync(UserRoleDTO model)
        {
            if (ModelState.IsValid)
            {
                var Response = await _adminService.RemoveUserFromRoleAsync(model);
                if (Response.isDone == true)
                {
                    return StatusCode(Response.StatusCode, Response.Message);
                }
                return StatusCode(Response.StatusCode, Response.Message);
            }
            return BadRequest(ModelState);
        }


        [HttpDelete("DeleteRole/{roleId}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            if (ModelState.IsValid)
            {
                var Response = await _adminService.DeleteRole(roleId);
                if (Response.isDone == true)
                {
                    return StatusCode(Response.StatusCode, Response.Message);
                }
                return StatusCode(Response.StatusCode, Response.Message);
            }
            return BadRequest();
        }

        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUserAsync(string UserId)
        {
            if (ModelState.IsValid)
            {
                var Result = await _adminService.DeleteUserAsync(UserId);
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
