using DioVeiculos101.Domain.Enums;
using DioVeiculos101.Domain.Interface;
namespace DioVeiculos101.Domain.DTO;

public class AdminDTO : IValidatable
{
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public Profile Profile { get; set; } = default!;
}