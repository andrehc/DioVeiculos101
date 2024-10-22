using DioVeiculos101.Domain.DTO;
using DioVeiculos101.Domain.Entity;
using DioVeiculos101.Domain.Interface;
using DioVeiculos101.Infrastructure.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DioVeiculos101.Domain.Service;

public class AdminService : IAdminService
{
    private readonly DbContexto _dbContext;
    private readonly PasswordHasher<Admin> _passwordHasher;
    public AdminService(DbContexto context)
    {
        _dbContext = context;
        _passwordHasher = new PasswordHasher<Admin>();
    }

    public Admin? Login(LoginDTO loginDto)
    {
        var admin = _dbContext.Admins.Where(a => a.Email == loginDto.Email && a.Password == loginDto.Password).FirstOrDefault();

        return admin;

    }
    public List<Admin>? All(int? page = 1, string? name = null, string? profile = null)
    {
        int itemsPerPage = 10;
        var data = _dbContext.Admins.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            data = data.Where(v => EF.Functions.Like(v.Name.ToLower(), $"%{name}%"));
        }

        if (!string.IsNullOrEmpty(profile))
        {
            data = data.Where(v => EF.Functions.Like(v.Profile.ToLower(), $"%{profile}%"));
        }
        if (page != null)
        {
            data = data.Skip(((int)page - 1) * itemsPerPage).Take(itemsPerPage);
        }

        return data.ToList();
    }

    public Admin? GetById(int id)
    {
        return _dbContext.Admins.Where(v => v.Id == id).FirstOrDefault();
    }
    public Admin New(Admin admin)
    {
        admin.Password = _passwordHasher.HashPassword(admin, admin.Password);
        _dbContext.Admins.Add(admin);
        _dbContext.SaveChanges();
        return admin;
    }

    public Admin Update(Admin admin)
    {
        _dbContext.Admins.Update(admin);
        _dbContext.SaveChanges();
        return admin;
    }

    public void Delete(Admin admin)
    {
        _dbContext.Admins.Remove(admin);
        _dbContext.SaveChanges();
    }

}