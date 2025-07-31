//Peterson Wiggers
namespace AcademiaDoZe.Domain.Enums;
using System.ComponentModel.DataAnnotations;
public enum EColaboradorTipo
{
    [Display(Name = "Administrador")]
    Administrador = 0,
    [Display(Name = "Atendente")]
    Atendente = 1,
    [Display(Name = "Instrutor")]
    Instrutor = 2
}

