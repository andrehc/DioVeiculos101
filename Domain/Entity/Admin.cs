using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DioVeiculos101.Domain.Entity;

public class Admin
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set;  } = default!;

    [Required]
    [StringLength(100)]
    public string Username { get; set;  } = default!;

    [Required]
    [StringLength(100)]
    public string Email { get; set;  } = default!;

    [StringLength(20)]
    public string Password { get; set;  } = default!;

    [StringLength(20)]
    public string Profile  { get; set;  } = default!;
}