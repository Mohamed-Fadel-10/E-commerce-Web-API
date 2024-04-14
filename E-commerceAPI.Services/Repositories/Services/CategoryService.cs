using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.Response;
using E_commerceAPI.Entities.Models;
using E_commerceAPI.Services.Data;
using E_commerceAPI.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Services.Repositories.Services
{
    public  class CategoryService:ICategoryService
    {
        private readonly ICurdRepository<Category> _curdRepo;
        private readonly Context _context;

        public CategoryService(ICurdRepository<Category> _curdRepo, Context context)
        {
            this._curdRepo = _curdRepo;
            _context = context;

        }
        public async Task<Category> AddCategoryAsync(CategoryDTO model)
        {
            if (model is not null)
            {
                var IsExist= await _curdRepo.GetByName(c=>c.Name==model.Name);
                if (IsExist is null)
                {
                    Category category = new Category();
                    category.Name = model.Name;
                    category.Description = model.Description;
                    await _curdRepo.Add(category);
                    return category;
                }
                return new Category();
            }
            return new Category();
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var Categories = await _curdRepo.GetAllInclude(c => c.Products);
            if (Categories.Count() > 0)
            {
                return Categories;
            }
            return new List<Category>();          
        }
        public async Task<Category> GetByIDAsync(int id)
        {
            if (id != 0)
            {
                var Category = await _curdRepo.GetByID(id);
                return Category;
            }       
            return new Category();
        }
        public async Task<Category> GetByNameAsync(string name)
        {
            if (name is not null)
            {
                var Category = await _curdRepo.GetByName(C=>C.Name==name);
                return Category;
            }
            return new Category();
        }
       
        public async Task<int> UpdateAsync(CategoryDTO model, int id)
        {            
                Category category = await _curdRepo.GetByID(id);
                if (category != null)
                {
                    category.ID = id; 
                    category.Name = model.Name;
                    category.Description = model.Description;
                    return await _curdRepo.Update(category, id);
                }
                return 0;
        }

        public async Task<int> DeleteAsync(int id)
        {
            Category category = await _curdRepo.GetByID(id);
            if (category != null)
            {
                int rows = await _curdRepo.Delete(id);
                if (rows > 0)
                    return 1;
            }
            return 0;
        }



    }
}
