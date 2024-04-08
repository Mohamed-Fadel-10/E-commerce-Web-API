using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.LogIn;
using E_commerceAPI.Entities.DTOs.Register;
using E_commerceAPI.Entities.Models;

namespace E_commerceAPI.Services.Repositories.Interfaces
{
    public interface IAuthService
    {
        Task<Authuntication> Register(RegisterDTO register);
        Task<string> AddRole(AddRole addRole);
        Task<Token> LogIn(LogInDTO logInDTO);
    }
}