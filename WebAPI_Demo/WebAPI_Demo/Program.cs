using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAPI_Demo.Data;
using WebAPI_Demo.Models;
using WebAPI_Demo.Services.IServices;
using WebAPI_Demo.Services;
using WebAPI_Demo.Extensions;
using AutoMapper;
using WebAPI_Demo;
using WebAPI_Demo.Rediscache;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#region Setup Auto Mapper
// Tạo 1 class MappingConfig
IMapper mapper = MappingConfig.RegisterMap().CreateMapper();

builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

builder.Services.Configure<JWTOption>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


builder.Services.AddControllers();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddRedisCacheService();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Xác thực và ủy quyền vào API 
builder.AddAppAuthentication();
builder.Services.AddAuthorization();


builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
