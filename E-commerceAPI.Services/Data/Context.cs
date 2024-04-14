using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using E_commerceAPI.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace E_commerceAPI.Services.Data
{
    public class Context : IdentityDbContext<ApplicationUser>
    {
        public Context(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // ------------ OrderItems --------------------

            builder.Entity<OrderItems>()
                .HasOne(oi => oi.Order)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderItems>()
                .HasOne(oi => oi.Product)
                .WithMany(oi => oi.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            // ------------ Products_Carts --------------------
            builder.Entity<Products_Carts>()
                .HasOne(pc => pc.Product)
                .WithMany(pc => pc.Products_Carts)
                .HasForeignKey(pc => pc.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Products_Carts>()
               .HasOne(pc => pc.Cart)
               .WithMany(pc => pc.Products_Carts)
               .HasForeignKey(pc => pc.CartID)
               .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);





        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Products_Carts> ProductsCarts { get; set; }
        public DbSet<OrderItems> OrderItems  { get; set; }


    }
}
