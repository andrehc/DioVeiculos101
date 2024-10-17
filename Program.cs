using System.Diagnostics.CodeAnalysis;
using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Interface;
using DioVeiculos101.Domain.Service;
using DioVeiculos101.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
{
    if(adminService.Login(loginDTO) != null)
    {
        return Results.Ok(loginDTO);
    }
    return Results.Unauthorized();
});

app.Run();
