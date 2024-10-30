using DioVeiculos101.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DioVeiculos101.Infrastructure.Db;

public class DbContexto : DbContext
{
    private readonly IConfiguration _appSettings;
    public DbContexto(IConfiguration appSettings)
    {
        _appSettings = appSettings;
    }
    public DbSet<Admin> Admins { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<Admin>().HasData(
    //         new Admin
    //         {
    //             Id = 1,
    //             Username = "John Doe",
    //             Email = "john@teste.com",
    //             Password = "102030",
    //             Profile = "admin"
    //         },
    //         new Admin
    //         {
    //             Id = 2,
    //             Username = "Marc Goddart",
    //             Email = "marc@teste.com",
    //             Password = "102030",
    //             Profile = "n1"
    //         }
    //     );
    // }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _appSettings.GetConnectionString("DefaultConnection")?.ToString();
        optionsBuilder.UseSqlServer(connectionString);
    }
}