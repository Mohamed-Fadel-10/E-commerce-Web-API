using E_commerceAPI.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
       private readonly ICartService _cartService;
        public CartController(ICartService _cartService)
        {
            this._cartService = _cartService;
            
        }
        [HttpGet("GetCart")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCartAsync()
        {
            var Response= await _cartService.GetCartProductsAsync();
            if(Response.isDone=true && Response.Model is not null)
            {
                return StatusCode(Response.StatusCode, Response.Model);          
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("RemoveProductFromCart")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RemoveCartProductAsync(int ProductID)
        {
            if (ProductID != 0)
            {
              var Response = await _cartService.RemoveCartProductAsync(ProductID);
                if (Response.isDone == true)
                {
                    return StatusCode(Response.StatusCode, Response.Message);
                }
                return StatusCode(Response.StatusCode, Response.Message);
            }
            return BadRequest(ModelState);
        }
    }
}

