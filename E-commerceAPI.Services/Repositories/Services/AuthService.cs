using E_commerceAPI.Entities.DTOs.Register;
using E_commerceAPI.Entities.Models;
using E_commerceAPI.Services.Data;
using E_commerceAPI.Entities.DTOs;
using E_commerceAPI.Entities.DTOs.LogIn;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using E_commerceAPI.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using E_commerceAPI.Entities.Models.JWT_Token;
using System.Security.Cryptography;
using E_commerceAPI.Entities.DTOs.PasswordSettings;
using Microsoft.AspNetCore.Mvc;
using E_commerceAPI.Entities.DTOs.Response;
using E_commerceAPI.Entities.DTOs.Create;

namespace E_commerceAPI.Services.Repositories.Services
{

    public class AuthService : IAuthService
    {
        private readonly Context _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(Context _context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager
            , IConfiguration _configuration, IHttpContextAccessor _httpContextAccessor)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
            this._configuration = _configuration;
            this._httpContextAccessor = _httpContextAccessor;
        }

        public async Task<Authuntication> Register(RegisterDTO model)
        {
            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                return new Authuntication { Message = "This UserName is Already Placed in System try Again", IsAuthenticated = false };
            }
            if (await _userManager.FindByEmailAsync(model.Email) != null)
            {
                return new Authuntication { Message = "This Email Is Used Before", IsAuthenticated = false };
            }
            var User = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone
            };
            IdentityResult result = await _userManager.CreateAsync(User, model.ConfirmPassword);
            if (result.Succeeded)
            {
                List<string> Roles = new List<string>();
                foreach (var item in model.Roles)
                {
                    Roles.Add(item);
                }
                await _userManager.AddToRolesAsync(User, Roles);
                return new Authuntication
                {
                    Message = "The User Created Successfully",
                    IsAuthenticated = true,
                    UserName = User.UserName,
                    Email = User.Email,
                    Roles = Roles
                };
            }
            else
            {
                var ErrorMessage = string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));

                return new Authuntication { Message = ErrorMessage, IsAuthenticated = false };
            }
        }

        public async Task<Response> CreateRoleAsync(string Role)
        {
            if (await _roleManager.RoleExistsAsync(Role))
                return new Response { Message = "Role Already Created Before", StatusCode = 409,isDone= false };
            IdentityRole identityRole = new IdentityRole(Role);
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return new Response { Message = "Role Created Successfully", StatusCode = 201, isDone = true };
            }
            return new Response { Message = "Role Created Failed", StatusCode = 400 ,isDone = false };
        }


        public async Task<Token> LogIn(LogInDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                bool Found = await _userManager.CheckPasswordAsync(user, model.Password);
                if (Found)
                {

                    var roles = await _userManager.GetRolesAsync(user);
                    var userClaims = await _userManager.GetClaimsAsync(user);
                    var roleClaims = new List<Claim>();
                    roleClaims.AddRange(roles.Select(r => new Claim("roles", r)).ToList());

                    var claims = new[]
                    {
                         new Claim(ClaimTypes.Name, user.UserName),
                         new Claim(ClaimTypes.NameIdentifier, user.Id),
                         new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                         new Claim(JwtRegisteredClaimNames.Email, user.Email),
                         new Claim("uid", user.Id)
                     }
                    .Union(userClaims)
                    .Union(roleClaims);

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:issuer"],
                        audience: _configuration["JWT:audience"],
                        claims: claims,
                        signingCredentials: credentials,
                        expires: DateTime.Now.AddDays(7)
                 );

                    return new Token 
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        ExpireOn = token.ValidTo ,
                        Message="LogIn Successfully",
                        IsAuthenticated=true
                    };
                }
                else
                {
                    return new Token { token = string.Empty,
                        ExpireOn = DateTime.Now ,
                        Message = "User Not Found",
                        IsAuthenticated = false 
                    };
                }
            }
            return new Token 
            {   token = string.Empty ,
                Message = "User name Or Password Is Not Correct" ,
                IsAuthenticated=false
            };

        }
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            ClaimsPrincipal userIdClaim = _httpContextAccessor.HttpContext.User;

            return await _userManager.GetUserAsync(userIdClaim);
        }

        public async Task<Authuntication> ChangePasswordAsync(ChangepasswordDTO model)
        {
            try
            {
                var Autho = new Authuntication();
                var user = await GetCurrentUserAsync();
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
                if(role is null)
                {
                    return new Response { Message = "Role Not Found" ,StatusCode=404,isDone=false};
                }
                await _roleManager.DeleteAsync(role);
                return new Response { Message = "Role Deleted Successfully", StatusCode = 200 , isDone=true };
            }
            catch (Exception ex)
            {
                return new Response { Message = $"Cannot Delete Role {ex.Message} ", StatusCode = 400,isDone= false };
            }

        }
        public async Task<Response> DeleteUserAsync(string userId)
        {
            try
            {
              var user = await _userManager.FindByIdAsync(userId);
                if(user is null)
                {
                    return new Response { Message = "User Not Found", StatusCode = 404 , isDone = false };
                }
                await _userManager.DeleteAsync(user);
                return new Response { Message = "User Deleted Successfully", StatusCode = 200, isDone = true };

            }
            catch (Exception ex)
            {
                return new Response { Message = $"Cannot Delete this User {ex.Message} ", StatusCode = 400,isDone=false };
            }

        }
        public async Task<Response> AddUserToRoleAsync(UserRoleDTO model)
        {
           var user= await _userManager.FindByNameAsync(model.UserName);
            if (user is not null)
            {
                var IsinRole = await _userManager.IsInRoleAsync(user, model.RoleName);
                if (IsinRole == true)
                {
                    return new Response { Message="User is Already in this Role",StatusCode=409,isDone=false};
                }
                var role=  await _roleManager.FindByNameAsync(model.RoleName);
                if(role is null)
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
        public async Task<Response> LogoutAsync()
        {
            if (GetCurrentUserAsync() is null)
            {
                return new Response { Message = "UnAuthorized user ", StatusCode = 401 ,isDone=false};
            }
            await _signInManager.SignOutAsync();
            return new Response { Message = "User Logged Out Successfully", StatusCode = 200 ,isDone=true};
        }



    }
}
