using System.ComponentModel.DataAnnotations;

namespace ClinicaCaramelo.Models;

public class Veterinario
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Crmv { get; set; } = string.Empty;

    [Required, StringLength(60)]
    public string Especialidade { get; set; } = string.Empty;

    [EmailAddress, StringLength(120)]
    public string? Email { get; set; }

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}
