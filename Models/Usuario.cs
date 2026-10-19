using System.ComponentModel.DataAnnotations;

namespace ClinicaCaramelo.Models;

public class Usuario
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string Nome { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(120)] public string Email { get; set; } = string.Empty;
    [Required, StringLength(255)] public string SenhaHash { get; set; } = string.Empty;
}
