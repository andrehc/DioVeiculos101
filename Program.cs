using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Interface;
using DioVeiculos101.Domain.ModelViews;
using DioVeiculos101.Domain.Service;
using DioVeiculos101.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DioVeiculos101.Domain.Entity;
using System.ComponentModel.DataAnnotations;
using DioVeiculos101.Domain.Enums;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Admins
app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
{
    if (adminService.Login(loginDTO) != null)
        return Results.Ok(loginDTO);

    return Results.Unauthorized();
}).WithTags("Admins");

app.MapPost("/admins", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
{
    var validatorUser = new ValidatorDTO().ValidateDto(dto: adminDTO);

    if (validatorUser.Messages.Count > 0)
        return Results.BadRequest(validatorUser);

    var user = new Admin
    {
        Email = adminDTO.Email,
        Name = adminDTO.Name,
        Password = adminDTO.Password,
        Profile = adminDTO.Profile.ToString() ?? Profile.editor.ToString()
    };

    adminService.New(user);

    return Results.Created($"/admin/{user.Id}", user);
}
).WithTags("Admins");

app.MapGet("/admins", ([FromQuery] int? page, IAdminService adminService) =>
{
    var admins = adminService.All(page);

    return Results.Ok(admins);
}).WithTags("Admins");

app.MapGet("/admin/{Id}", ([FromRoute] int Id, IAdminService adminService) =>
{
    var admin = adminService.GetById(Id);

    if (admin == null)
        return Results.NotFound();


    return Results.Ok(admin);
}).WithTags("Admins");

app.MapPut("/admin/{Id}", ([FromRoute] int Id, AdminDTO adminDTO, IAdminService adminService) =>
{
    var validatorUser = new ValidatorDTO().ValidateDto(dto: adminDTO);

    if (validatorUser.Messages.Count > 0)
        return Results.BadRequest(validatorUser);

    var admin = adminService.GetById(Id);

    if (admin == null)
        return Results.NotFound();

    admin.Email = adminDTO.Email;
    admin.Name = adminDTO.Name;
    admin.Password = adminDTO.Password;
    admin.Profile = adminDTO.Profile.ToString();

    adminService.Update(admin);

    return Results.Ok(admin);

}).WithTags("Admins");

app.MapDelete("/admin/{id}", ([FromRoute] int Id, IAdminService adminService) =>
{
    var admin = adminService.GetById(Id);

    if (admin == null)
        return Results.NotFound();

    adminService.Delete(admin);

    return Results.NoContent();
}).WithTags("Admins");

#endregion
#region Validator Rules


#endregion

#region Vehicles
app.MapPost("/vehicles", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{
    var validatorVehicle = new ValidatorDTO().ValidateDto(dto: vehicleDTO);

    if (validatorVehicle.Messages.Count > 0)
        return Results.BadRequest(validatorVehicle);

    var vehicle = new Vehicle
    {
        Name = vehicleDTO.Name,
        Brand = vehicleDTO.Brand,
        Year = vehicleDTO.Year,
    };
    vehicleService.Store(vehicle);

    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
}).WithTags("Vehicles");

app.MapGet("/vehicles", ([FromQuery] int? page, IVehicleService vehicleService) =>
{
    var vehicles = vehicleService.All(page);
    return Results.Ok(vehicles);
}).WithTags("Vehicles");

app.MapGet("/vehicles/{Id}", ([FromRoute] int Id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetId(Id);
    if (vehicle == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(vehicle);
}).WithTags("Vehicles");
app.MapPut("/vehicles/{Id}", ([FromRoute] int Id, VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{
    var validatorVehicle = new ValidatorDTO().ValidateDto(dto: vehicleDTO);

    if (validatorVehicle.Messages.Count > 0)
        return Results.BadRequest(validatorVehicle);
    var vehicle = vehicleService.GetId(Id);

    if (vehicle == null)
        return Results.NotFound();

    if (validatorVehicle.Messages.Count > 0)
        return Results.BadRequest(validatorVehicle);

    vehicle.Name = vehicleDTO.Name;
    vehicle.Brand = vehicleDTO.Brand;
    vehicle.Year = vehicleDTO.Year;

    vehicleService.Update(vehicle);

    return Results.Ok(vehicle);
}).WithTags("Vehicles");

app.MapDelete("/vehicles/{Id}", ([FromRoute] int Id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetId(Id);
    if (vehicle == null) return Results.NotFound();

    vehicleService.Delete(vehicle);

    return Results.NoContent();
}).WithTags("Vehicles");
#endregion

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
