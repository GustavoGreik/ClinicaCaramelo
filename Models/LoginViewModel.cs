using System.ComponentModel.DataAnnotations;

namespace ClinicaCaramelo.Models;

public class LoginViewModel
{
    [Required, EmailAddress, Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;
    [Required, DataType(DataType.Password), Display(Name = "Senha")]
    public string Senha { get; set; } = string.Empty;
}
