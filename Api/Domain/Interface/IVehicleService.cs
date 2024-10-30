
using DioVeiculos101.Domain.Entity;

namespace DioVeiculos101.Domain.Interface;

public interface IVehicleService
{
    List<Vehicle> All(int? page, string? name = null, string? brand = null);
    Vehicle? GetId(int id);
    Vehicle Store(Vehicle vehicle);
    Vehicle Update(Vehicle vehicle);
    void Delete(Vehicle vehicle);
}