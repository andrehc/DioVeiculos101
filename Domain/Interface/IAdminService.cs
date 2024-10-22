using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Entity;

namespace DioVeiculos101.Domain.Interface;

public interface IAdminService
{
    Admin? Login(LoginDTO loginDto);
    List<Admin>? All(int? page, string? username = null, string? profile = null);
    Admin? GetById(int id);
    Admin New(Admin admin);
    Admin Update(Admin admin); // id
    void Delete(Admin admin); // id
}