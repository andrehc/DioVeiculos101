using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Interface;
using Microsoft.AspNetCore.Identity;

namespace Tests.Mocks
{
    public class AdminMockService : IAdminService
    {
        private static readonly PasswordHasher<Admin> passwordHasher = new PasswordHasher<Admin>();
        //admin.Password = passwordHasher.HashPassword(admin, "102030");
        private static List<Admin> admins = new List<Admin>()
        {
            new Admin
            {
                Id = 1,
                Email = "dollor@mail.com",
                Name = "Dollor Asimmet",
                Password = GenerateHashedPassword("102030"),
                Profile = "admin"
            },
            new Admin
            {
                Id = 2,
                Email = "sanctum@mail.com",
                Name = "Sanctum Famias",
                Password = GenerateHashedPassword("102030"),
                Profile = "editor"
            }
        };

        private static string GenerateHashedPassword(string plainPassword)
        {
            var tempAdmin = new Admin(); // Cria um admin temporário
            return passwordHasher.HashPassword(tempAdmin, plainPassword);
        }

        public List<Admin>? All(int? page, string? username = null, string? profile = null)
        {
            return admins;
        }

        public void Delete(Admin admin)
        {
            admins.Remove(admin);
        }

        public Admin? GetByEmail(string email)
        {
            return admins.Find(x => x.Email == email);
        }

        public Admin? GetById(int id)
        {
            return admins.Find(x => x.Id == id);
        }

        public Admin? Login(LoginDTO loginDto)
        {
            return admins.Find(x => x.Email == loginDto.Email && x.Password == loginDto.Password);
        }

        public Admin New(Admin admin)
        {

            admin.Id = admins.Count() + 1;
            admins.Add(admin);
            return admin;
        }

        public Admin Update(Admin admin)
        {
            admins.Add(admin);
            return admin;
        }
    }
}