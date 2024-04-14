using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.Response;
using E_commerceAPI.Entities.Models;
using E_commerceAPI.Services.Repositories.Interfaces;
using E_commerceAPI.Services.Repositories.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace E_commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService _categoryService)
        {
            this._categoryService = _categoryService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _categoryService.GetAllAsync();
            var Data = response.Select(cat => new 
            {
                ID=cat.ID,
                Name = cat.Name,
                Description = cat.Description,
                Products = cat.Products.Select(p => p.Name).ToList()
            });
            return StatusCode(StatusCodes.Status200OK, Data);
        }



        [HttpGet("GetByID/{id}:int")]      
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            if (id != 0)
            {
              var response= await _categoryService.GetByIDAsync(id);
                if(response is not null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        ID = response.ID,
                        Name = response.Name,
                        Description = response.Description
                    });
                }
                return NotFound($"There is No Category with ID Equal {id}");
            
            }
            return BadRequest("Invalid ID");          
        }

        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            if (name is not null)
            {
                var response = await _categoryService.GetByNameAsync(name);
                if (response is not null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        ID = response.ID,
                        Name = response.Name,
                        Description = response.Description
                    });
                }
                return NotFound($"There is No Category with Name = {name}");

            }
            return BadRequest("Invalid Name");
        }       

        [HttpPost("AddCategory")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddCategoryAsync(CategoryDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _categoryService.AddCategoryAsync(model);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    ID = response.ID,
                    Name = response.Name,
                    Description = response.Description
                });
            }
            return BadRequest(ModelState);

        }

        [HttpPut("UpdateCategory")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateCategoryAsync(CategoryDTO model,int id)
        {
            if (ModelState.IsValid)
            {
                if (id != 0 && model is not null)
                {
                    var response = await _categoryService.UpdateAsync(model, id);
                    if (response == 1)
                    {
                        return StatusCode(StatusCodes.Status200OK, model);
                    }
                    else if(response==0)
                    return StatusCode(StatusCodes.Status404NotFound, "The Id is Not Found");
                }
            }
                return BadRequest(ModelState);

        }

        [HttpDelete("DeleteCategory")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
                if (id != 0 )
                {
                    var response = await _categoryService.DeleteAsync(id);
                    if (response == 1)
                    {
                        return StatusCode(StatusCodes.Status200OK,"Category Deleted Successfully");
                    }
                    else if (response == 0)
                        return StatusCode(StatusCodes.Status404NotFound, "The Id is Not Found");
                }
            return BadRequest(ModelState);
        }
        

    }
}
