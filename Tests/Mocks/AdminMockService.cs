using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Interface;

namespace Tests.Mocks
{
    public class AdminMockService : IAdminService
    {
        private static List<Admin> admins = new List<Admin>()
        {
            new Admin() {
                Id = 1,
                Email = "dollor@mail.com",
                Name = "Dollor Asimmet",
                Password = "102030",
                Profile = "admin"
            },
            new Admin() {
                Id = 2,
                Email = "sanctum@mail.com",
                Name = "Sanctum Famias",
                Password = "102030",
                Profile = "editor"
            }
        };

        public List<Admin>? All(int? page, string? username = null, string? profile = null)
        {
            return admins;
        }

        public void Delete(Admin admin)
        {
            admin = admins.Find(x => x.Id == admin.Id);
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
            admin = admins.Find(a => a.Id == admin.Id);
            admins.Add(admin);
            return admin;
        }
    }
}