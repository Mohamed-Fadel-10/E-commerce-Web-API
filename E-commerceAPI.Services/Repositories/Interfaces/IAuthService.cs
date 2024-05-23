using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.LogIn;
using E_commerceAPI.Entities.DTOs.PasswordSettings;
using E_commerceAPI.Entities.DTOs.Register;
using E_commerceAPI.Entities.DTOs.Response;
using E_commerceAPI.Entities.Models;
using E_commerceAPI.Entities.Models.JWT_Token;

namespace E_commerceAPI.Services.Repositories.Interfaces
{
    public interface IAuthService
    {
        public Task<Authuntication> Register(RegisterDTO model);
        public Task<Authuntication> LogIn(LogInDTO model);
        public Task<Authuntication> NewRefreshToken(string token);
        public Task<Authuntication> ChangePasswordAsync(ChangepasswordDTO model);

        public Task<Response> LogoutAsync();
        public Task<ApplicationUser> GetCurrentUserAsync();
    }

}