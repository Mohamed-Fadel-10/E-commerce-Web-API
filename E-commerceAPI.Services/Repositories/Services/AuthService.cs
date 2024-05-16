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
                
                return new Authuntication
                {
                    Message = "The User Created Successfully",
                    IsAuthenticated = true,
                    UserName = User.UserName,
                    Email = User.Email,
                };
            }
            else
            {
                var ErrorMessage = string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));

                return new Authuntication { Message = ErrorMessage, IsAuthenticated = false };
            }
        }

        public async Task<Authuntication> LogIn(LogInDTO model)
        {
            var User = await _userManager.FindByNameAsync(model.UserName);
            if (User is not null)
            {
               var isFound = await _userManager.CheckPasswordAsync(User, model.Password);
                
                if (isFound)
                {
                    var Token = await CreateToken(User);
                    var Roles = await _userManager.GetRolesAsync(User);
                    var RefreshToken = "";
                    DateTime RefreshTokenExpireDate;

                    if (User.RefreshTokens!.Any(t => t.IsActive))
                    {
                        var ActiveRefreshToken = User.RefreshTokens.FirstOrDefault(t => t.IsActive);
                        RefreshToken = ActiveRefreshToken!.Token;
                        RefreshTokenExpireDate = ActiveRefreshToken.ExpiresOn;
                    }
                    else
                    {
                        var RefreshTokenObj = CreateRefreshToken();
                        RefreshToken = RefreshTokenObj.Token;
                        RefreshTokenExpireDate = RefreshTokenObj.ExpiresOn;
                        User.RefreshTokens.Add(RefreshTokenObj);
                        await _userManager.UpdateAsync(User);
                    }

                    return new Authuntication
                    {
                        IsAuthenticated = true,
                        UserName = User.UserName,
                        Email = User.Email,
                        Message = $"Welcome {User.UserName}",
                        Roles = Roles.ToList(),
                        Token = new JwtSecurityTokenHandler().WriteToken(Token),
                        RefreshToken = RefreshToken
                    };
                }
                return new Authuntication
                {
                    IsAuthenticated = false,
                    UserName = string.Empty,
                    Email = string.Empty,
                    Message = $"Invalid Password",
                    Roles = new List<string>(),
                    Token = string.Empty,
                };

            }
            return new Authuntication
            {
                IsAuthenticated = false,
                UserName = string.Empty,
                Email = string.Empty,
                Message = $"User name Or Password Is Not Correct",
                Roles = new List<string>(),
                Token = string.Empty,
            };
        }

        public async Task<Authuntication> NewRefreshToken(string token)
        {
            var authModel = new Authuntication();

            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "Invaild Token";

                return authModel;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                return new Authuntication
                {
                    IsAuthenticated = false,
                    UserName = string.Empty,
                    Email = string.Empty,
                    Message = "InActive Token",
                    Roles = new List<string>(),
                    Token = string.Empty,
                };
            }

            refreshToken.RevokeOn = DateTime.UtcNow;
            var newRefreshToken = CreateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            var Roles = await _userManager.GetRolesAsync(user);
            var jwtToken = await CreateToken(user);

            return new Authuntication
            {
                IsAuthenticated = true,
                UserName = user.UserName,
                Email = string.Empty,
                Message = "Active Token",
                Roles = Roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresOn
            };
        }
        private async Task<JwtSecurityToken> CreateToken(ApplicationUser User)
        {
            var claims = new List<Claim>
                      {
                     new Claim(ClaimTypes.Name, User.UserName),
                     new Claim(ClaimTypes.NameIdentifier, User.Id),
                     new Claim(JwtRegisteredClaimNames.Sub, User.UserName),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     new Claim(JwtRegisteredClaimNames.Email, User.Email!),
                     new Claim("uid", User.Id)
                     };
            var roles = await _userManager.GetRolesAsync(User);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            SecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            SigningCredentials signingCred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(
                issuer: _configuration["JWT:issuer"],
                audience: _configuration["JWT:audience"],
                claims: claims,
                signingCredentials: signingCred,
                expires: DateTime.Now.AddHours(5)
                );
            return Token;
        }
        private RefreshToken CreateRefreshToken()
        {
            var RandomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(RandomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow,
            };
        }
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            ClaimsPrincipal userIdClaim = _httpContextAccessor.HttpContext.User;

            return await _userManager.GetUserAsync(userIdClaim);
        }

        public async Task<Response> LogoutAsync()
        {
            var currentUser = await GetCurrentUserAsync();

            if (currentUser == null)
            {
                return new Response { Message = "Unauthorized user", StatusCode = 401, isDone = false };
            }

            await  _signInManager.SignOutAsync();
            return new Response { Message = "User logged out successfully", StatusCode = 200, isDone = true };
        }




    }
}
