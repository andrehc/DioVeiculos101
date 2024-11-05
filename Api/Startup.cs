using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DioVeiculos101;
using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Enums;
using DioVeiculos101.Domain.Interface;
using DioVeiculos101.Domain.ModelViews;
using DioVeiculos101.Domain.Service;
using DioVeiculos101.Infrastructure.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        key = Configuration.GetSection("Jwt").ToString() ?? "";
    }

    private string key = "";
    public IConfiguration Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
        var key = Configuration.GetSection("Jwt").ToString() ?? "chave-api-jwt";

        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddEndpointsApiExplorer();
        services.AddAuthentication(option =>
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

        services.AddAuthorization();
        services.AddSwaggerGen(options =>
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

        var isTestEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing";

        if (isTestEnvironment)
        {
            services.AddDbContext<DbContexto>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("TestsConnection")));
        }
        else
        {
            services.AddDbContext<DbContexto>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            string FillJwtToken(Admin admin)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>()
                {
                    new Claim("Email", admin.Email),
                    new Claim("Profile", admin.Profile),
                    new Claim(ClaimTypes.Role, admin.Profile)
                };
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            #region Home
            endpoints.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
            #endregion

            #region Admins
            endpoints.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
            {
                if (string.IsNullOrEmpty(loginDTO.Email) || string.IsNullOrEmpty(loginDTO.Password))
                    return Results.BadRequest("Email e senha são obrigatórios.");

                var admin = adminService.GetByEmail(loginDTO.Email);

                if (admin == null) return Results.NotFound();

                var passwordHasher = new PasswordHasher<Admin>();
                var verificationResult = passwordHasher.VerifyHashedPassword(
                    admin,
                    admin.Password,
                    loginDTO.Password
                );

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
            })
            .AllowAnonymous()
            .WithTags("Admins");

            endpoints.MapPost("/admins", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
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
            )
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
            .WithTags("Admins");

            endpoints.MapGet("/admins", ([FromQuery] int? page, IAdminService adminService) =>
            {
                var admins = adminService.All(page);

                return Results.Ok(admins);
            })
            .RequireAuthorization()
            .WithTags("Admins");

            endpoints.MapGet("/admin/{Id}", ([FromRoute] int Id, IAdminService adminService) =>
            {
                var admin = adminService.GetById(Id);

                if (admin == null)
                    return Results.NotFound();


                return Results.Ok(admin);
            })
            .RequireAuthorization()
            .WithTags("Admins");

            endpoints.MapPut("/admin/{Id}", ([FromRoute] int Id, AdminDTO adminDTO, IAdminService adminService) =>
            {
                var validatorUser = new ValidatorDTO().ValidateDto(dto: adminDTO);

                if (validatorUser.Messages.Count > 0)
                    return Results.BadRequest(validatorUser);

                var admin = adminService.GetById(Id);

                if (admin == null) return Results.NotFound();

                var passwordHasher = new PasswordHasher<Admin>();
                admin.Email = adminDTO.Email;
                admin.Name = adminDTO.Name;
                admin.Password = adminDTO.Password = passwordHasher.HashPassword(admin, adminDTO.Password);
                admin.Profile = adminDTO.Profile.ToString();

                adminService.Update(admin);

                return Results.Ok(admin);

            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
            .WithTags("Admins");

            endpoints.MapDelete("/admin/{id}", ([FromRoute] int Id, IAdminService adminService) =>
            {
                var admin = adminService.GetById(Id);

                if (admin == null)
                    return Results.NotFound();

                adminService.Delete(admin);

                return Results.NoContent();
            })
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Admin" })
            .WithTags("Admins");

            #endregion
            #region Validator Rules


            #endregion

            #region Vehicles
            endpoints.MapPost("/vehicles", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
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
            })
            .RequireAuthorization()
            .WithTags("Vehicles");

            endpoints.MapGet("/vehicles", ([FromQuery] int? page, IVehicleService vehicleService) =>
            {
                var vehicles = vehicleService.All(page);
                return Results.Ok(vehicles);
            })
            .RequireAuthorization()
            .WithTags("Vehicles");

            endpoints.MapGet("/vehicles/{Id}", ([FromRoute] int Id, IVehicleService vehicleService) =>
            {
                var vehicle = vehicleService.GetId(Id);

                if (vehicle == null) return Results.NotFound();

                return Results.Ok(vehicle);
            })
            .RequireAuthorization()
            .WithTags("Vehicles");

            endpoints.MapPut("/vehicles/{Id}", ([FromRoute] int Id, VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
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

            endpoints.MapDelete("/vehicles/{Id}", ([FromRoute] int Id, IVehicleService vehicleService) =>
            {
                var vehicle = vehicleService.GetId(Id);

                if (vehicle == null) return Results.NotFound();

                vehicleService.Delete(vehicle);

                return Results.NoContent();
            })
            .RequireAuthorization()
            .WithTags("Vehicles");
            #endregion
        });
    }
}