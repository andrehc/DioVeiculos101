using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Interface;
using DioVeiculos101.Infrastructure.Db;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace DioVeiculos101.Domain.Service;

public class VehicleService : IVehicleService
{
    private readonly DbContexto _dbContext;

    public VehicleService(DbContexto dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Vehicle> All(int? page = 1, string? name = null, string? brand = null)
    {
        int itemsPerPage = 10;
        var data = _dbContext.Vehicles.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            data = data.Where(v => EF.Functions.Like(v.Name.ToLower(), $"%{name}%"));
        }

        if (!string.IsNullOrEmpty(brand))
        {
            data = data.Where(v => EF.Functions.Like(v.Brand.ToLower(), $"%{brand}%"));
        }
        if(page != null)
        {
            data = data.Skip(((int)page - 1) * itemsPerPage).Take(itemsPerPage);
        }

        return data.ToList();
    }

    public void Delete(Vehicle vehicle)
    {
        _dbContext.Vehicles.Remove(vehicle);
        _dbContext.SaveChanges();
    }

    public Vehicle? GetId(int id)
    {
        return _dbContext.Vehicles.Where(v => v.Id == id).FirstOrDefault();
    }

    public Vehicle Store(Vehicle vehicle)
    {
        _dbContext.Vehicles.Add(vehicle);
        _dbContext.SaveChanges();
        return vehicle;
    }

    public Vehicle Update(Vehicle vehicle)
    {
        _dbContext.Vehicles.Update(vehicle);
        _dbContext.SaveChanges();
        return vehicle;
    }
}