using System.Reflection;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Enums;
using DioVeiculos101.Domain.Interface;
using DioVeiculos101.Domain.Service;
using DioVeiculos101.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Services
{
    [TestClass]
    public class AdminServiceTest
    {
        [TestMethod]
        public void TestCreateAdmin()
        {
            var admin = new Admin();

            // admin.Id = 1;
            admin.Name = "Lorem Ipsum";
            admin.Email = "loremipsum@test.com";
            admin.Password = "102030";
            admin.Profile = "admin";

            var context = CreateContext();
            var AdminService = new AdminService(context);

            AdminService.New(admin);
            var adminResponse = AdminService.GetById(1);
            //AdminService.GetByEmail(admin.Email);
            //Assert.AreEqual("loremipsum@test.com", admin.Email);
            Assert.AreEqual(1, adminResponse.Id);
        }
        [TestMethod]
        public void TestGetByEmailOrId()
        {
            var admin = new Admin();

            admin.Name = "Lorem Ipsum";
            admin.Email = "loremipsum@test.com";
            admin.Password = "102030";
            admin.Profile = "admin";

            var context = CreateContext();
            var adminService = new AdminService(context);

            adminService.New(admin);
            var retrievedAdmin = adminService.GetByEmail(admin.Email);

            Assert.AreEqual("loremipsum@test.com", retrievedAdmin.Email);
        }

        private DbContexto CreateContext()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            var connectionString = config.GetConnectionString("TestsConnection");

            var options = new DbContextOptionsBuilder<DbContexto>()
                .UseSqlServer(connectionString)
                .Options;

            var context = new DbContexto(options);
            context.Database.EnsureCreated();  // Garante que o banco seja criado

            return context;
        }
    }

}