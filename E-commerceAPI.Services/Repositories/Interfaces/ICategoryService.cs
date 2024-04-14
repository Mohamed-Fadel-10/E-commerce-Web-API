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
    public  interface ICategoryService
    {
        Task<Category> AddCategoryAsync(CategoryDTO model);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIDAsync(int id);
        Task<Category> GetByNameAsync(string name);
        Task<int> UpdateAsync(CategoryDTO model, int id);

        Task<int> DeleteAsync(int id);
    }
}
