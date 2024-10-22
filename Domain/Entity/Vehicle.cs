using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DioVeiculos101.Domain.Entity;

public class Vehicle
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(30)]
    public string Brand { get; set; } = default!;

    public int Year { get; set; }
}