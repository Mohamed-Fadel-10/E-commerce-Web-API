using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.Response;
using E_commerceAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Services.Repositories.Interfaces
{
    public interface IProductService
    {
        Task<Product> AddProductAsync(ProductDTO model);
        Task<Response> AddProductToCartAsync(int ProductID);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIDAsync(int id);
        Task<Product> GetByNameAsync(string name);
        Task<int> UpdateAsync(ProductDTO model, int id);
        Task<int> DeleteAsync(int id);

    }
}
