using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Entity;

namespace DioVeiculos101.Domain.Interface;

public interface IAdminService
{
    Admin? Login(LoginDTO loginDto);
    List<Admin>? GetAdmin();
    // Admin Create(AdminDTO adminDto);
    // Admin Edit(AdminDTO adminDto); // id
    // Admin Delete(AdminDTO adminDto); // id
}