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
        Task<Authuntication> Register(RegisterDTO model);
        Task<Response> CreateRoleAsync(string addRole);
        Task<Token> LogIn(LogInDTO model);
        Task<Response> LogoutAsync();
        Task<Authuntication> ChangePasswordAsync(ChangepasswordDTO model);
        Task<Response> DeleteRole(string RoleID);
        Task<Response> DeleteUserAsync(string userId);
        Task<Response> AddUserToRoleAsync(UserRoleDTO model);
        Task<Response> RemoveUserFromRoleAsync(UserRoleDTO model);
    }

}