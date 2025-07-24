#Peterson Wiggers
using System.ComponentModel.DataAnnotations;
namespace Academia_Classes.Enums
{
    public enum EMatriculaPlano
    {
        [Display(Name = "Mensal")]
        mensal = 0,
        [Display(Name = "Trimestral")]
        trimestral = 1,
        [Display(Name = "Semestral")]
        semestral = 2,
        [Display(Name = "Anual")]
        anual = 3,
    }
}
