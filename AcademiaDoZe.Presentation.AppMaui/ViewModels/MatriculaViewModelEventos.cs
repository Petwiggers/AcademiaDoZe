using AcademiaDoZe.Application.Enums;
using System.Windows.Input;

namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    [QueryProperty(nameof(MatriculaId), "Id")]
    public partial class MatriculaViewModel : BaseViewModel
    {
        public ICommand SelecionarDataInicialCommand { get; }

        private void CalcularDataFinal()
        {
            switch (Matricula.Plano)
            {
                case EAppMatriculaPlano.Mensal:
                    Matricula.DataFim = Matricula.DataInicio.AddMonths(1);
                    break;
                case EAppMatriculaPlano.Trimestral:
                    Matricula.DataFim = Matricula.DataInicio.AddMonths(3);
                    break;
                case EAppMatriculaPlano.Semestral:
                    Matricula.DataFim = Matricula.DataInicio.AddMonths(6);
                    break;
                case EAppMatriculaPlano.Anual:
                    Matricula.DataFim = Matricula.DataInicio.AddMonths(12);
                    break;
                default:
                    break;
            }
        }
    }
}