using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.PasswordSettings;
using E_commerceAPI.Entities.DTOs.Response;
using E_commerceAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Services.Repositories.Interfaces
{
    public interface  IAdminService
    {
        Task<Response> CreateRoleAsync(string addRole);
        Task<Response> DeleteRole(string RoleID);
        Task<Response> DeleteUserAsync(string userId);
        Task<Response> AddUserToRoleAsync(UserRoleDTO model);
        Task<Response> RemoveUserFromRoleAsync(UserRoleDTO model);
    }
}
