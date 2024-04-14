using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.Models;
using E_commerceAPI.Services.Repositories.Interfaces;
using E_commerceAPI.Services.Repositories.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Xml.Linq;

namespace E_commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService  _productService)
        {
            this._productService = _productService;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var Response = await _productService.GetAllAsync();
            var Products=Response.Select(prod=>new
            {
                prod.ID,
                prod.Name,
                prod.Description,
                prod.Price,
                prod.CategoryID,
                prod.Photo,
                
            });
            return StatusCode(StatusCodes.Status200OK, Products);

        }
        [HttpGet("GetByID:int")]
        public async Task<IActionResult> GetByIDAsync(int id)
        {
            if (id != 0)
            {
               var Response = await _productService.GetByIDAsync(id);
               if(Response is not null)
               {
                 return StatusCode(StatusCodes.Status200OK, new
                 {
                  ID = Response.ID,
                  Name = Response.Name,
                  Description = Response.Description,
                  Price = Response.Price,
                  CategoryID = Response.CategoryID,
                  Photo = Response.Photo,
                 });
               }
                return NotFound($"There is No Product With ID = {id}");
            }
            return BadRequest(ModelState);

        }
        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            if (name is not null)
            {
                var Response = await _productService.GetByNameAsync(name);
                if (Response is not null)
                {
                    return StatusCode(StatusCodes.Status200OK, new
                    {
                        ID = Response.ID,
                        Name = Response.Name,
                        Description = Response.Description,
                        Price = Response.Price,
                        CategoryID = Response.CategoryID,
                        Photo = Response.Photo,
                    });
                }
                return NotFound($"There is No Product With ID = {name}");
            }
            return BadRequest(ModelState);

        }
       
        [HttpPost("AddProduct")]
        [Authorize(Roles ="Admin",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddProductAsync([FromForm]ProductDTO model)
        {
            if (ModelState.IsValid)
            {
               var Response= await _productService.AddProductAsync(model);
                return StatusCode(StatusCodes.Status201Created, new
                {
                  ID= Response.ID,
                  Name=Response.Name,
                  Description=Response.Description,
                  Price=Response.Price,
                  CategoryID=Response.CategoryID,
                  Photo=Response.Photo,
                });
            }
            return BadRequest(ModelState);
        }
        [HttpPost("AddProductToCart")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddProductToCartAsync(int ProductID)
        {
            if (ModelState.IsValid)
            {
                var Response = await _productService.AddProductToCartAsync(ProductID);
                if (Response.isDone == true)
                {
                    return StatusCode(Response.StatusCode, Response.Model);
                }
                return StatusCode(Response.StatusCode, Response.Message);
            }
            return BadRequest(ModelState);

        }
        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateProductAsync([FromForm]ProductDTO model,int id)
        {
            if (ModelState.IsValid)
            {
                var Response = await _productService.UpdateAsync(model, id);
                if (Response == 1)
                {
                    return StatusCode(StatusCodes.Status200OK, "Product data Updated Successfully");
                }
                return NotFound($"There is No Product With ID = {id}");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("DeleteProduct")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            if (ModelState.IsValid)
            {
                var Response = await _productService.DeleteAsync(id);
                if (Response == 1)
                {
                    return StatusCode(StatusCodes.Status200OK, "Product Deleted Successfully");
                }
                return NotFound($"There is No Product With ID = {id}");
            }
            return BadRequest(ModelState);
        }
    }
}
