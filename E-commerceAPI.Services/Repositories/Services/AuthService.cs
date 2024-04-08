using E_commerceAPI.Entities.DTOs.Register;
using E_commerceAPI.Entities.Models;
using E_commerceAPI.Services.Data;
using E_commerceAPI.Entities.DTOs.Create;
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

        public AuthService(Context _context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, IConfiguration _configuration, IHttpContextAccessor _httpContextAccessor)
        {
            this._context = _context;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
            this._configuration = _configuration;
            this._httpContextAccessor = _httpContextAccessor;
        }
        public async Task<Authuntication> Register(RegisterDTO register)
        {
            if (await _userManager.FindByNameAsync(register.UserName) != null)
            {
                return new Authuntication { Message = "This UserName is Already Placed in System try Again", IsAuthenticated = false };
            }
            if (await _userManager.FindByEmailAsync(register.Email) != null)
            {
                return new Authuntication { Message = "This Email Is Used Before", IsAuthenticated = false };
            }
            var User = new ApplicationUser
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.UserName,
                Email = register.Email,
                PhoneNumber = register.Phone
            };
            IdentityResult result = await _userManager.CreateAsync(User, register.ConfirmPassword);
            if (result.Succeeded)
            {
                List<string> Roles = new List<string>();
                foreach (var item in register.Roles)
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
                string ErrorMessage = string.Empty;
                foreach (var item in result.Errors)
                {
                    ErrorMessage += $"{result.Errors} , ";
                }
                return new Authuntication { Message = ErrorMessage, IsAuthenticated = false };
            }
        }
        public async Task<string> AddRole(AddRole addRole)
        {
            if (await _roleManager.RoleExistsAsync(addRole.Role))
                return "Role is Existed Before";
            IdentityRole identityRole = new IdentityRole(addRole.Role);
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return "Role Created Successfully";
            }
            return "Cannot Add this Role";
        }
        public async Task<Token> LogIn(LogInDTO Model)
        {
            var user = await _userManager.FindByNameAsync(Model.UserName);
            if (user != null)
            {
                bool Found = await _userManager.CheckPasswordAsync(user, Model.Password);
                if (Found)
                {

                    var roles = await _userManager.GetRolesAsync(user);
                    var Claims = new List<Claim>();
                    Claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                    Claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    foreach (var item in roles)
                    {
                        new Claim(ClaimTypes.Role, item);
                    }
                    SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                    SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    JwtSecurityToken token = new JwtSecurityToken(
                        issuer: _configuration["JWT:issuer"],
                        audience: _configuration["JWT:audience"],
                        claims: Claims,
                        signingCredentials: credentials,
                        expires: DateTime.Now.AddDays(7)
                 );

                    return new Token
                    { token = new JwtSecurityTokenHandler().WriteToken(token), ExpireOn = token.ValidTo ,Message="LogIn Successfully",IsAuthenticated=true};
                }
                else
                {
                    return new Token { token = string.Empty, ExpireOn = DateTime.Now , Message = "User Not Found", IsAuthenticated = false };
                }
            }
            return new Token { token = string.Empty , Message = "User name Or Password Is Not Correct" ,IsAuthenticated=false};

        }
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = _httpContextAccessor.HttpContext.User;
            return await _userManager.GetUserAsync(currentUser);
        }
        public async Task<string> LogoutAsync()
        {
            if (GetCurrentUserAsync() == null)
            {
                return "User Not Found";
            }
            await _signInManager.SignOutAsync();
            return "User Logged Out Successfully";
        }


    }
}
