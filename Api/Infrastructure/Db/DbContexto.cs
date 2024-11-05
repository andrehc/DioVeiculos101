using DioVeiculos101.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DioVeiculos101.Infrastructure.Db;

public class DbContexto : DbContext
{
    private readonly IConfiguration _appSettings;

    public DbContexto(DbContextOptions<DbContexto> options) : base(options) { }
    public DbSet<Admin> Admins { get; set; } = default!;
    public DbSet<Vehicle> Vehicles { get; set; } = default!;
}