using System.ComponentModel.DataAnnotations;

namespace ClinicaCaramelo.Models;

public class Animal
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Nome { get; set; } = string.Empty;

    [Required, StringLength(40)]
    public string Especie { get; set; } = string.Empty;

    [StringLength(50)]
    public string? Raca { get; set; }

    [Range(0, 100)]
    public int? Idade { get; set; }

    [StringLength(10)]
    public string? Sexo { get; set; }

    [Required]
    [Display(Name = "Tutor")]
    public int TutorId { get; set; }
    public Tutor? Tutor { get; set; }

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}
