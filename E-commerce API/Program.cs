using E_commerceAPI.Entities.Models;
using E_commerceAPI.Services.Data;
using E_commerceAPI.Services.Repositories.Interfaces;
using E_commerceAPI.Services.Repositories.Services;
using E_commerceAPI.Services.Repositories.Services.EmailSetting;
using Microsoft.AspNetCore.Identity;

//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defualtConnection"));

});
builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<IMailService, MailingService>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("EmailConfiguration"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
