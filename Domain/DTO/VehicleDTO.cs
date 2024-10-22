using DioVeiculos101.Domain.Interface;

namespace DioVeiculos101.Domain.DTO;

public class VehicleDTO : IValidatable
{
    public string Name { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public int Year { get; set; } = default!;
}