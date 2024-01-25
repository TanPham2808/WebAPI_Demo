using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using WebAPI_Demo.Data;
using WebAPI_Demo.Models;
using WebAPI_Demo.Services.IServices;
using WebAPI_Demo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<JWTOption>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


builder.Services.AddControllers();

builder.Services.AddScoped<IAuthService, AuthService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
