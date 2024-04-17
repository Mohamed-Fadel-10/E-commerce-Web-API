using E_commerceAPI.Entities.DTOs.Response;
using E_commerceAPI.Entities.Models;
using E_commerceAPI.Services.Data;
using E_commerceAPI.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Services.Repositories.Services
{
    public class CartService:ICartService
    {
        private readonly ICurdRepository<Cart> _curdRepo;
        private readonly IAuthService _authService;
        private readonly Context _context;
        public CartService(ICurdRepository<Cart> _curdRepo, IAuthService _authService,Context _context)
        {
            this._curdRepo = _curdRepo;         
            this._authService = _authService;
            this._context= _context;
        }
        public async Task<Response> GetCartProductsAsync()
        {       
           var CurrentUser = await _authService.GetCurrentUserAsync();
           if(CurrentUser is null)
           {
              return new Response() { Message= "UnAuthorized User ",StatusCode=401,isDone=false};
           }
           var UserCart = await _context.Products
               .Include(p => p.Category)
               .Join(_context.ProductsCarts, p => p.ID, pc => pc.ProductID, (p, pc) => new { Product = p, CartProducts = pc })
               .Join(_context.Carts, cp => cp.CartProducts.CartID, c => c.ID, (cp, c) => new { CartProducts = cp, Cart = c })
               .Where(c => c.Cart.UserID == CurrentUser.Id)
               .ToListAsync();          

           if(UserCart is not null)
           {
              var Sum = UserCart.Sum(c=>c.CartProducts.Product.Price);
              
              return new Response { Message = "Success Process",
                 isDone = true, 
                 StatusCode = 200,
                 Model = new {
                    Name = UserCart.Select(p => p.CartProducts.Product.Name),
                    Category= UserCart.Select(p => p.CartProducts.Product.Category.Name),
                    Description= UserCart.Select(p => p.CartProducts.Product.Description),
                    Image =UserCart.Select(p=>p.CartProducts.Product.Photo),
                    TotalPrice = Sum,
                    } 
             };
           }
              return new Response() { Message=" There is No Products in Cart , Cart is Empty",isDone=true,StatusCode=200,Model=null};       
        }

        public async Task<Response> RemoveCartProductAsync(int id)
        {
            var isProductExist= await _context.ProductsCarts.FirstOrDefaultAsync(p=>p.ProductID==id);
            if (isProductExist is  null)
            {
                return new Response() { Message = "This Product ID Not in this Cart", StatusCode = 404, isDone = false };
            }
            var CurrentUser = await _authService.GetCurrentUserAsync();
            if (CurrentUser is null)
            {
                return new Response() { Message = "UnAuthorized User ", StatusCode = 401, isDone = false };
            }
            var UserCart=_context.Carts.FirstOrDefault(c=>c.UserID==CurrentUser.Id);
            var UserProductsCart = await _context.ProductsCarts.Include(p => p.Product.Category).Where(p => p.ProductID == id&&p.CartID== UserCart.ID).FirstOrDefaultAsync();
            if (UserProductsCart is not null)
            {
                _context.ProductsCarts.Remove(UserProductsCart);
                int Rows = await _context.SaveChangesAsync();
                if (Rows == 1)
                {
                    return new Response() { Message = "Product Removed From Cart Successfully", isDone = true, StatusCode = 200};
                }
            }
            return new Response() { Message = "Cart not Include Products until Now", isDone = false, StatusCode = 400};
        }

    }
}
