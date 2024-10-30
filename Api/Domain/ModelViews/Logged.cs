namespace DioVeiculos101.Domain.ModelViews;

public record Logged
{
    public string Email { get; set; } = default!;
    public string Profile { get; set; } = default!;
    public string Token { get; set; } = default!;
}