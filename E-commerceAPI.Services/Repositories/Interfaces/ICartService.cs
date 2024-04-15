using E_commerceAPI.Entities.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Services.Repositories.Interfaces
{
    public interface ICartService
    {
        Task<Response> GetCartProductsAsync();
        Task<Response> RemoveCartProductAsync(int id);
    }
}
