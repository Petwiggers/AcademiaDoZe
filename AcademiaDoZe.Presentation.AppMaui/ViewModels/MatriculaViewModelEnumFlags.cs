using Academia.Domain.Entities;
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Services;
using CommunityToolkit.Mvvm.Input;
using ZstdSharp.Unsafe;
namespace AcademiaDoZe.Presentation.AppMaui.ViewModels
{
    [QueryProperty(nameof(MatriculaId), "Id")]
    public partial class MatriculaViewModel : BaseViewModel
    {
        
        private EAppMatriculaRestricoes _tiposRestricoesSelecionadas;
        public EAppMatriculaRestricoes TiposRestricoes
        {
            get => _tiposRestricoesSelecionadas;
            set => SetProperty(ref _tiposRestricoesSelecionadas, value);
        }

        // Lista de opções disponíveis para o CheckBox
        public List<TipoMatriculaOption> OpcoesTipo { get; } = new();

        public void InicializaTipoRestricoes()
        {
            // Preenche as opções
            var valores = Enum.GetValues(typeof(EAppMatriculaRestricoes))
                                .Cast<EAppMatriculaRestricoes>();

            foreach (var valor in valores)
            {
                OpcoesTipo.Add(new TipoMatriculaOption
                {
                    Valor = valor,
                    Nome = valor.GetDisplayName(),
                    IsSelecionado = false
                });
            }
        }

        

    }
    // Classe auxiliar para binding
    public class TipoMatriculaOption
    {
        public EAppMatriculaRestricoes Valor { get; set; }
        public string Nome { get; set; }
        public bool IsSelecionado { get; set; }
    }
}