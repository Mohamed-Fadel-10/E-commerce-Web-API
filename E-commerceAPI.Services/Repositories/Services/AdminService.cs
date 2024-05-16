using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.PasswordSettings;
using E_commerceAPI.Entities.DTOs.Response;
using E_commerceAPI.Entities.Models;
using E_commerceAPI.Services.Data;
using E_commerceAPI.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Services.Repositories.Services
{
    public class AdminService:IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;

        public AdminService( UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager
            , IConfiguration _configuration, IAuthService _authService)
        {
            this._userManager = _userManager;
            this._roleManager = _roleManager;
            this._configuration = _configuration;
            this._authService = _authService;
        }
        public async Task<Response> CreateRoleAsync(string Role)
        {
            if (await _roleManager.RoleExistsAsync(Role))
                return new Response { Message = "Role Already Created Before", StatusCode = 409, isDone = false };
            IdentityRole identityRole = new IdentityRole(Role);
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return new Response { Message = "Role Created Successfully", StatusCode = 201, isDone = true };
            }
            return new Response { Message = "Role Created Failed", StatusCode = 400, isDone = false };
        }


        public async Task<Authuntication> ChangePasswordAsync(ChangepasswordDTO model)
        {
            try
            {
                var Autho = new Authuntication();
                var user = await _authService.GetCurrentUserAsync();
                if (user is null)
                {
                    return new Authuntication { Message = "User Not Found" };
                }
                if (!await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                {
                    return new Authuntication { Message = "Invalid Password" };
                }
                IdentityResult result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    Autho.Message = "Password Changed Successfully";
                    Autho.IsAuthenticated = true;
                    Autho.Email = user.Email;
                    Autho.UserName = user.UserName;
                }
                else
                {
                    Autho.Message = "Password Changed Successfully";
                    Autho.IsAuthenticated = false;
                    Autho.Email = user.Email;
                    Autho.UserName = user.UserName;
                }
                return Autho;
            }
            catch (Exception ex)
            {
                return new Authuntication { Message = $" Failed To Change Password , {ex.Message}" };
            }

        }

        public async Task<Response> DeleteRole(string RoleID)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(RoleID);
                if (role is null)
                {
                    return new Response { Message = "Role Not Found", StatusCode = 404, isDone = false };
                }
                await _roleManager.DeleteAsync(role);
                return new Response { Message = "Role Deleted Successfully", StatusCode = 200, isDone = true };
            }
            catch (Exception ex)
            {
                return new Response { Message = $"Cannot Delete Role {ex.Message} ", StatusCode = 400, isDone = false };
            }

        }
        public async Task<Response> DeleteUserAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user is null)
                {
                    return new Response { Message = "User Not Found", StatusCode = 404, isDone = false };
                }
                await _userManager.DeleteAsync(user);
                return new Response { Message = "User Deleted Successfully", StatusCode = 200, isDone = true };

            }
            catch (Exception ex)
            {
                return new Response { Message = $"Cannot Delete this User {ex.Message} ", StatusCode = 400, isDone = false };
            }

        }
        public async Task<Response> AddUserToRoleAsync(UserRoleDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is not null)
            {
                var IsinRole = await _userManager.IsInRoleAsync(user, model.RoleName);
                if (IsinRole == true)
                {
                    return new Response { Message = "User is Already in this Role", StatusCode = 409, isDone = false };
                }
                var role = await _roleManager.FindByNameAsync(model.RoleName);
                if (role is null)
                {
                    return new Response { Message = "this Role Not Found", StatusCode = 404, isDone = false };
                }
                await _userManager.AddToRoleAsync(user, model.RoleName);
                return new Response { Message = $"{model.RoleName} Role Added To User Successfully ", StatusCode = 200, isDone = true };
            }
            return new Response { Message = $"{model.RoleName} User Name Not Found ", StatusCode = 200, isDone = false };

        }
        public async Task<Response> RemoveUserFromRoleAsync(UserRoleDTO model)
        {

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is not null)
            {
                var IsinRole = await _userManager.IsInRoleAsync(user, model.RoleName);
                if (IsinRole is false)
                {
                    return new Response { Message = "User Not in this Role", StatusCode = 404, isDone = false };
                }
                await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                return new Response { Message = $"{model.RoleName} Role Deleted From User Successfully ", StatusCode = 200, isDone = true };
            }
            return new Response { Message = $"{model.RoleName} User Name Not Found", StatusCode = 404, isDone = false };

        }
    }
}
