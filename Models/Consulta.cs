using System.ComponentModel.DataAnnotations;

namespace ClinicaCaramelo.Models;

public class Consulta
{
    public int Id { get; set; }

    [Required, Display(Name = "Animal")]
    public int AnimalId { get; set; }
    public Animal? Animal { get; set; }

    [Required, Display(Name = "Veterinário")]
    public int VeterinarioId { get; set; }
    public Veterinario? Veterinario { get; set; }

    [Required, Display(Name = "Data e horário")]
    public DateTime DataHora { get; set; } = DateTime.Now.AddHours(1);

    [Required, StringLength(500)]
    [Display(Name = "Motivo da consulta")]
    public string Motivo { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Observações")]
    public string? Observacoes { get; set; }

    [Required, StringLength(20)]
    public string Status { get; set; } = "Agendada";
}
