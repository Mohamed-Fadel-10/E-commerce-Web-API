using E_commerceAPI.Entities.DTOs;
using E_commerceAPI.Entities.DTOs.Create;
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
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IAuthService _authService;
        private readonly ICurdRepository<Order> _curdRepo;
        private Context _context;
        public OrderItemsService(IAuthService _authService, Context _context, ICurdRepository<Order> _curdRepo)
        {
            this._authService = _authService;
            this._context = _context;
            this._curdRepo = _curdRepo;
        }
        public async Task<Response> AddOrderAsync(OrderDTO model)
        {
            if (model is not null)
            {
                var currentUser = await _authService.GetCurrentUserAsync();
                if (currentUser is not null)
                {
                    var userCartItems = await _context.Products
                        .Include(p => p.Category)
                        .Join(_context.ProductsCarts, p => p.ID, pc => pc.ProductID, (p, pc) => new { Product = p, CartProducts = pc })
                        .Join(_context.Carts, cp => cp.CartProducts.CartID, c => c.ID, (cp, c) => new { CartProducts = cp, Cart = c })
                        .Where(c => c.Cart.UserID == currentUser.Id)
                        .ToListAsync();

                    if (userCartItems.Count > 0)
                    {
                        var Sum = userCartItems.Select(c => c.CartProducts.Product.Price).Sum();
                        Order order = new Order
                        {
                            UserID = currentUser.Id,
                            Address = model.Address,
                            UserName = model.FullName,
                            CreatedOn = DateTime.UtcNow.Date,
                            DeliveredOn = DateTime.UtcNow.Date.AddDays(7),
                            Phone = model.Phone,
                            TotalPrice= Sum,
                        };

                        await _context.Orders.AddAsync(order);
                        await _context.SaveChangesAsync();

                        var orderItems = userCartItems
                            .GroupBy(item => item.CartProducts.Product.ID)
                            .Select(group => new OrderItems
                            {
                                OrderId = order.ID,
                                ProductId = group.Key,
                                Quantity = group.Count(),
                                TotalPrice = group.Sum(item => item.CartProducts.Product.Price),   // Here I Fetch Total Price Per Item From Cart So i didn't Multiply * Quantity ^--^
                            });
                        foreach (var item in userCartItems) // Remove Items From Cart After Making an Order ^--^
                        {
                            _context.ProductsCarts.Remove(item.CartProducts.CartProducts);
                        }
                        await _context.OrderItems.AddRangeAsync(orderItems);
                        await _context.SaveChangesAsync();

                        return new Response { Message = "Order Added Successfully", isDone = true, StatusCode = 201, Model = 
                            new  { 
                            Message="Order Created Successfully",
                            OrderId=order.ID,
                            ProductId= orderItems.Select(o => o.ProductId),
                            CreatedOn = DateTime.Now.Date,
                            DeleiveredOn= DateTime.Now.Date.AddDays(7),
                            Quantity = orderItems.Select(o => o.Quantity),
                            TotalPrice = orderItems.Select(o => o.TotalPrice),
                            }
                        };
                    }
                    return new Response { Message = "There are No Products in Your Cart", isDone = false, StatusCode = 200 };
                }

                return new Response { Message = "Unauthorized", isDone = false, StatusCode = 401 };
            }

            return new Response { Message = "Invalid Model", isDone = false, StatusCode = 400 };
        }
        public async Task<Response> GetOrderAsync(int id)
        {
            if (id != 0)
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.ID == id);
                if (order is not null)
                {                 
                        var OrderDeatils = new OrderDetails
                        {
                            OrderID = order.ID,
                            UserName = order.UserName,
                            Phone = order.Phone,
                            CreatedOn = order.CreatedOn,
                            DeliveredOn = order.DeliveredOn,
                            Address = order.Address,
                            IsDone= order.IsDone,
                            TotalPrice=order.TotalPrice
                        };
                        return new Response { Message = "Order Details : ", isDone = true, StatusCode = 200, Model = OrderDeatils };                 
                }
                return new Response { Message = "Invalid Order ID ", isDone = false, StatusCode = 400 };
            }
            return new Response { Message = "Invalid ID ", isDone = false, StatusCode = 400 };
        }
        public async Task<Response> GetAllAsync()
        {
            var AllOrders = await _curdRepo.GetAll();
            if (AllOrders.Count() > 0)
            {
                return new Response { isDone = true, StatusCode = 200, Model = AllOrders.Select(o=>new OrderDetails
                {
                    OrderID=o.ID,
                    UserName=o.UserName,
                    Phone=o.Phone,
                    Address=o.Address,
                    CreatedOn=o.CreatedOn,
                    DeliveredOn=o.DeliveredOn,
                    IsDone=o.IsDone,
                    TotalPrice=o.TotalPrice,
                })
                   };
            }
            return new Response { isDone = false, StatusCode = 400 };
        }
        public async Task<Response> RemoveAsync(int id)
        {
            if(id != 0)
            {
                var order = await _curdRepo.GetByID(id);
                if(order is not null)
                {
                   int Row= await _curdRepo.Delete(id);
                    if (Row == 1)
                    {
                        return new Response { Message = "Order Deleted Successfully", isDone = true, StatusCode = 200 };
                    }
                    return new Response { Message = "Failed To Delete This Order", isDone = false, StatusCode = 400 };
                }
                return new Response { Message = $"There is No Order With ID = {id} ", isDone = false, StatusCode = 400 };
            }
            return new Response { Message = $"Invalid ID = {id} ", isDone = false, StatusCode = 400 };
        }
    }
}
