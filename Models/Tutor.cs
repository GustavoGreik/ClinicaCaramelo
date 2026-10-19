using System.ComponentModel.DataAnnotations;

namespace ClinicaCaramelo.Models;

public class Tutor
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    [Display(Name = "Nome completo")]
    public string Nome { get; set; } = string.Empty;

    [Required, StringLength(14)]
    public string Cpf { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required, Phone, StringLength(20)]
    public string Telefone { get; set; } = string.Empty;

    [StringLength(180)]
    public string? Endereco { get; set; }

    public ICollection<Animal> Animais { get; set; } = new List<Animal>();
}
