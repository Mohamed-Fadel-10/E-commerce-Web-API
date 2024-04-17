using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderItemsService _orderService;
        public OrderController(IOrderItemsService _orderService)
        {
            this._orderService = _orderService;
        }
        [HttpGet("GetAll")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllAsync()
        {
            var Response = await _orderService.GetAllAsync();
            if (Response.isDone == true && Response.Model is not null)
            {
                return StatusCode(Response.StatusCode, Response.Model);
            }
            return StatusCode(Response.StatusCode, Response.Message);
        }

        [HttpGet("GetOrder")]
        [Authorize(Roles = "Admin , User", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            if (ModelState.IsValid)
            {
                var Response = await _orderService.GetOrderAsync(id);
                if (Response.isDone == true)
                {
                    return StatusCode(Response.StatusCode, Response.Model);
                }
                return StatusCode(Response.StatusCode, Response.Message);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("MakeOrder")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> AddOrderAsync(OrderDTO model)
        {
            if (ModelState.IsValid)
            {
                var Response = await _orderService.AddOrderAsync(model);
                if (Response.isDone == true&&Response.Model is not null)
                {
                    return StatusCode(Response.StatusCode, Response.Model);
                }
                return StatusCode(Response.StatusCode,Response.Message);
           }
            return BadRequest(ModelState);
        }

        [HttpDelete("DeleteOrder")]
        [Authorize(Roles ="Admin",AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            if (ModelState.IsValid)
            {
                var Response = await _orderService.RemoveAsync(id);
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
