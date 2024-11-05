using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Interface;

namespace Tests.Mocks
{
    public class VehicleMockService : IVehicleService
    {
        public List<Vehicle> All(int? page, string? name = null, string? brand = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public Vehicle? GetId(int id)
        {
            throw new NotImplementedException();
        }

        public Vehicle Store(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }

        public Vehicle Update(Vehicle vehicle)
        {
            throw new NotImplementedException();
        }
    }
}