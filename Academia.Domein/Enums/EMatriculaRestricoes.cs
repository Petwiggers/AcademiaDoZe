//Peterson_Wiggers
namespace AcademiaDoZe.Domain.Enums;
using System.ComponentModel.DataAnnotations;
[Flags]
public enum EMatriculaRestricoes
{
    [Display(Name = "Diabetes")]
    Diabetes = 1,
    [Display(Name = "Pressão Alta")]
    PressaoAlta = 2,
    [Display(Name = "Labirintite")]
    Labirintite = 4,
    [Display(Name = "Alergias")]
    Alergias = 8,
    [Display(Name = "Problemas Respiratórios")]
    ProblemasRespiratorios = 16,
    [Display(Name = "Remédio Contínuo")]
    RemedioContinuo = 32
}


