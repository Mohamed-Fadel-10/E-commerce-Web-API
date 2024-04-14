using E_commerceAPI.Entities.DTOs.Create;
using E_commerceAPI.Entities.DTOs.Response;
using E_commerceAPI.Entities.Models;
using E_commerceAPI.Services.Data;
using E_commerceAPI.Services.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Services.Repositories.Services
{
    public class ProductService:IProductService
    {
        private readonly ICurdRepository<Product> _curdRepo;
        private readonly Context _context;
        private readonly IAuthService _authService;
        public ProductService(ICurdRepository<Product> _curdRepo,Context _context, IAuthService _authService)
        {
            this._curdRepo = _curdRepo;
            this._context = _context;
            this._authService= _authService;
        }
        public async Task<Product> AddProductAsync(ProductDTO model)
        {
            if(model is not null)
            {
                using var stream = new MemoryStream();
                await model.Photo.CopyToAsync(stream);
                Product product = new Product();
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryID = model.CategoryID;
                product.Photo = stream.ToArray();
                await _curdRepo.Add(product);
                return product;
            }
            return new Product();
        }


        public async Task<Response> AddProductToCartAsync(int ProductID)
        {
            if (ProductID != 0)
            {
                Cart cart = new Cart();

                Products_Carts products_Carts = new Products_Carts();

                var IsProductExist = await _curdRepo.IsExist(p => p.ID == ProductID);
                if (IsProductExist is null)
                    return new Response { Message = "This Product Not Found in System", isDone = false, StatusCode = 404, Model = null };

                var CurrentUser = await _authService.GetCurrentUserAsync();
                if (CurrentUser is not null)
                {
                    var isCartExistToUser = await _context.Carts.FirstOrDefaultAsync(c => c.UserID == CurrentUser.Id);
                    if (isCartExistToUser is null)
                    {
                        cart.CreatedOn = DateTime.Now.Date;
                        cart.UserID = CurrentUser.Id;
                        await _context.Carts.AddAsync(cart);
                        int Rows = await _context.SaveChangesAsync();
                        if (Rows == 1)
                        {
                            products_Carts.CartID = cart.ID;
                            products_Carts.ProductID = ProductID;
                            await _context.ProductsCarts.AddAsync(products_Carts);
                            await _context.SaveChangesAsync();
                            return new Response { Message = "Product Added To Cart Successfully", isDone = true, StatusCode = 201,
                                Model = new
                                {
                                    ID = products_Carts.Product.ID,
                                    Name = products_Carts.Product.Name,
                                    Price = products_Carts.Product.Price,
                                    Image = products_Carts.Product.Photo,
                                    Description = products_Carts.Product.Description,

                                }
                            };
                        }
                    }
                    else
                    {
                        products_Carts.CartID = isCartExistToUser.ID;
                        products_Carts.ProductID = ProductID;
                        await _context.ProductsCarts.AddAsync(products_Carts);
                        await _context.SaveChangesAsync();
                        return new Response { Message = "Product Added To Cart Successfully", isDone = true, StatusCode = 201, Model = new {
                            ID=products_Carts.Product.ID,
                            Name=products_Carts.Product.Name,
                            Price=products_Carts.Product.Price,
                            Image=products_Carts.Product.Photo,
                            Description=products_Carts.Product.Description,

                        } };
                    }
                    return new Response { Message = "Field To Add To Cart", isDone = false, StatusCode = 400, Model = null };                   
                }
                return new Response { Message = "UnAuthorized , Log In First Then Add Product To Cart ", isDone = false, StatusCode = 401, Model = null };
            }
            return new Response { Message = "Invalid Product ID ", isDone = false, StatusCode = 400, Model = null };
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var Products= await _curdRepo.GetAll();
            if (Products.Count() > 0)
            {
                return Products;
            }
            return new List<Product>();
        }

        public async Task<Product> GetByIDAsync(int id)
        {
            if (id != 0)
            {
                var Product = await _curdRepo.GetByID(id);
                    return Product;             
            }
            return new Product();
        }
        public async Task<Product> GetByNameAsync(string name)
        {
            if (name is not null)
            {
                var Product = await _curdRepo.GetByName(P=>P.Name==name);
                    return Product;
            }
            return new Product();
        }
        public async Task<int> UpdateAsync(ProductDTO model, int id)
        {
            if (id != 0)
            {
                var product = await _curdRepo.GetByID(id);
                if (product is not null)
                {
                    using var stream = new MemoryStream();
                    await model.Photo.CopyToAsync(stream);
                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.CategoryID = model.CategoryID;
                    product.Photo = stream.ToArray();
                    await _curdRepo.Update(product, id);
                    return 1;
                }
                return 0;
            }
            return 0;   
        }
        public async Task<int> DeleteAsync(int id)
        {
            if (id != 0)
            {
                var product = await _curdRepo.GetByID(id);
                if (product is not null)
                {
                    await _curdRepo.Delete(id);
                    return 1;
                }
                return 0;
            }
            return 0;
        }


       

    }
}
