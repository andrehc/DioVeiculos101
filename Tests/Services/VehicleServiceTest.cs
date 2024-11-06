using System.Reflection;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Service;
using DioVeiculos101.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tests.Services
{
    [TestClass]
    public class VehicleServiceTest
    {
        private DbContexto CreateTestContext()
        {
            var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.GetFullPath(Path.Combine(assemblyPath ?? "", "..", "..", ".."));

            var builder = new ConfigurationBuilder()
                .SetBasePath(path ?? Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            return new DbContexto(configuration);
        }

        [TestMethod]
        public void TestCreateVehicle()
        {
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");
            var car = new Vehicle();

            car.Name = "New Car test";
            car.Brand = "Brand test";
            car.Year = 2024;
            
            var vehicleService = new VehicleService(context);

            vehicleService.Store(car);
            var vehicleResponse = vehicleService.GetId(car.Id);

            Assert.AreEqual(car.Id, vehicleResponse.Id);
        }

        [TestMethod]
        public void TestUpdateVehicle()
        {
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");
            var car = new Vehicle();

            car.Name = "Car before Update";
            car.Brand = "Brand before Update";
            car.Year = 2024;

            var service = new VehicleService(context);

            service.Store(car);

            var vehicleUpdate = service.GetId(car.Id);

            vehicleUpdate.Name = "Car after Update";
            vehicleUpdate.Brand = "Brand after Update";
            vehicleUpdate.Year = 2025;

            service.Update(vehicleUpdate);

            var response = service.GetId(vehicleUpdate.Id);
            Assert.AreEqual(car.Id, response.Id);
        }
    }

}