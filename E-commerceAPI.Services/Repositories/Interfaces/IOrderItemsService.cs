using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Services.Repositories.Interfaces
{
    public interface IOrderItemsService
    {
        Task<Response> AddOrderAsync(OrderDTO model);
        Task<Response> GetOrderAsync(int id);
        Task<Response> GetAllAsync();
        Task<Response> RemoveAsync(int id);

    }
}
