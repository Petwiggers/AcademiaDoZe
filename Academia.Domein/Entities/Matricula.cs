//Peterson Wiggers
using AcademiaDoZe.Domain.ValueObjects;
namespace Academia.Domain.Entities;

using Academia.Domein.Services;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Matricula : Entity
{
    public Aluno AlunoMatricula { get; private set; }
    public EMatriculaPlano Plano { get; private set; }
    public DateOnly DataInicio { get; private set; }
    public DateOnly DataFim { get; private set; }
    public string Objetivo { get; private set; }
    public EMatriculaRestricoes RestricoesMedicas { get; private set; }
    public string ObservacoesRestricoes { get; private set; }
    public Arquivo LaudoMedico { get; private set; }

    private Matricula(Aluno alunoMatricula,
    EMatriculaPlano plano,
    DateOnly dataInicio,
    DateOnly dataFim,
    string objetivo,
    EMatriculaRestricoes restricoesMedicas,
    string observacoes,
    Arquivo laudoMedico)
    : base()
    {
        AlunoMatricula = alunoMatricula;
        Plano = plano;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Objetivo = objetivo;
        RestricoesMedicas = restricoesMedicas;
        LaudoMedico =  laudoMedico;
        ObservacoesRestricoes = observacoes;
    }

    public static Matricula Criar(Aluno alunoMatricula,
    EMatriculaPlano plano,
    DateOnly dataInicio,
    DateOnly dataFim,
    string objetivo,
    EMatriculaRestricoes restricoesMedicas,
    string observacoes,
    Arquivo laudoMedico)
    {
        if (alunoMatricula ==  null) throw new DomainException("ALUNO_OBRIGATORIO");
        if (string.IsNullOrWhiteSpace(objetivo)) throw new DomainException("OBJETIVO_OBRIGATORIO");
        objetivo = TextoNormalizadoService.LimparEspacos(objetivo);
        int idade = CalculoService.CalcularIdade(alunoMatricula.DataNascimento);
        Console.WriteLine($"{idade}");
        if (idade >= 12 & idade <= 16)
        {
            if(laudoMedico == null) throw new DomainException("LAUDO_OBRIGATORIO");
        }
        observacoes = TextoNormalizadoService.LimparEspacos(observacoes);

        return new Matricula(alunoMatricula, plano, dataInicio, dataFim, objetivo, restricoesMedicas, observacoes, laudoMedico);

        /*
         *aluno, *plano {mensal, trimestral, semestral ou anual}, *data de início, *data final, *objetivo, restrições 
         *{ex: diabetes, pressão alta, labirintite, alergias, *problemas respiratórios, uso de remédios contínuos, etc.}, 
         *observações sobre as restrições, laudo médico.*/

    }
}

