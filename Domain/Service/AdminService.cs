using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Interface;
using DioVeiculos101.Infrastructure.Db;

namespace DioVeiculos101.Domain.Service;

public class AdminService : IAdminService
{
    private readonly DbContexto _context;
    public AdminService(DbContexto contexto)
    {
        _context = contexto;
    }
    public List<Admin>? GetAdmin()
    {
        return [];
    }

    public Admin? Login(LoginDTO loginDto)
    {
        var admin = _context.Admins.Where(a => a.Email == loginDto.Email && a.Password == loginDto.Password).FirstOrDefault();

        return admin;
        
    }
}
/*
    TODO: CRIAR UMA INTERFACE, IMPLEMENTAR OS METODOS DE LOGIN:
    TODO: REALIZAR LOGIN (RETORNAR UM FIRST DO BANCO)
    TODO: CADASTRAR USUARIOS ADMIN
    TODO: EDITAR USUARIOS ADMIN
    TODO: DELETAR (SOFT DELETE) USUARIOS ADMIN
*/