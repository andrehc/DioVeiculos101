using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Interface;
using DioVeiculos101.Domain.ModelViews;
using DioVeiculos101.Domain.Service;
using DioVeiculos101.Infrastructure.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var key = builder.Configuration.GetSection("Jwt").ToString() ?? "chave-api-jwt";

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };

});

builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token: ",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});

builder.Services.AddDbContext<DbContexto>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

#region Home
app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
#endregion

#region Admins
app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
{
    if (string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
    {
        return Results.BadRequest("Email e senha são obrigatórios.");
    }

    var admin = adminService.GetByEmail(loginDTO.Email);

    if (admin == null)
    {
        return Results.NotFound();
    }

    var passwordHasher = new PasswordHasher<Admin>();
    var verificationResult = passwordHasher.VerifyHashedPassword(admin, admin.Password, loginDTO.Password);

    if (verificationResult == PasswordVerificationResult.Success)
    {
        string token = FillJwtToken(admin);
        return Results.Ok(new Logged
        {
            Email = admin.Email,
            Profile = admin.Profile,
            Token = token
        });
    }

    return Results.Unauthorized();
}).AllowAnonymous().WithTags("Admins");

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
).RequireAuthorization().WithTags("Admins");

app.MapGet("/admins", ([FromQuery] int? page, IAdminService adminService) =>
{
    var admins = adminService.All(page);

    return Results.Ok(admins);
}).RequireAuthorization().WithTags("Admins");

app.MapGet("/admin/{Id}", ([FromRoute] int Id, IAdminService adminService) =>
{
    var admin = adminService.GetById(Id);

    if (admin == null)
        return Results.NotFound();


    return Results.Ok(admin);
}).RequireAuthorization().WithTags("Admins");

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

}).RequireAuthorization().WithTags("Admins");

app.MapDelete("/admin/{id}", ([FromRoute] int Id, IAdminService adminService) =>
{
    var admin = adminService.GetById(Id);

    if (admin == null)
        return Results.NotFound();

    adminService.Delete(admin);

    return Results.NoContent();
}).RequireAuthorization().WithTags("Admins");

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
}).RequireAuthorization().WithTags("Vehicles");

app.MapGet("/vehicles", ([FromQuery] int? page, IVehicleService vehicleService) =>
{
    var vehicles = vehicleService.All(page);
    return Results.Ok(vehicles);
}).RequireAuthorization().WithTags("Vehicles");

app.MapGet("/vehicles/{Id}", ([FromRoute] int Id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetId(Id);
    if (vehicle == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(vehicle);
}).RequireAuthorization().WithTags("Vehicles");
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
}).RequireAuthorization().WithTags("Vehicles");

app.MapDelete("/vehicles/{Id}", ([FromRoute] int Id, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetId(Id);
    if (vehicle == null) return Results.NotFound();

    vehicleService.Delete(vehicle);

    return Results.NoContent();
}).RequireAuthorization().WithTags("Vehicles");
#endregion

string FillJwtToken(Admin admin)
{
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    var claims = new List<Claim>()
    {
        new Claim("Email", admin.Email),
        new Claim("Profile", admin.Profile)
    };
    var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
