using System.Reflection;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Enums;
using DioVeiculos101.Domain.Interface;
using DioVeiculos101.Domain.Service;
using DioVeiculos101.Infrastructure.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Helpers;

namespace Tests.Services
{
    [TestClass]
    public class AdminServiceTest
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
        public void TestCreateAdmin()
        {
            var context = CreateTestContext();
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE Admins");
            var admin = new Admin();
            var passwordHasher = new PasswordHasher<Admin>();

            // admin.Id = 1;
            admin.Name = "User Test Ipsum";
            admin.Email = "user@test.com";
            admin.Password = passwordHasher.HashPassword(admin, "102030");
            admin.Profile = "admin";
            

            var AdminService = new AdminService(context);

            AdminService.New(admin);
            var adminResponse = AdminService.GetById(admin.Id);
            //AdminService.GetByEmail(admin.Email);
            //Assert.AreEqual("loremipsum@test.com", admin.Email);
            Assert.AreEqual(1, adminResponse.Id);
        }
    }

}