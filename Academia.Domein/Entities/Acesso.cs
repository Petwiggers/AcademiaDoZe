//Peterson Wiggers
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
namespace Academia.Domain.Entities;
public class Acesso : Entity
{
    public Pessoa AlunoColaborador { get; private set; }
    public DateTime DataHora { get; private set; }

    public EPessoaTipo tipo {  get; private set; }
    private Acesso(int id, EPessoaTipo tipo, Pessoa pessoa, DateTime dataHora):base(id)
    {
        Id = id;
        AlunoColaborador = pessoa;
        DataHora = dataHora;
    }
    public static Acesso Criar(int id, EPessoaTipo tipo, Pessoa pessoa, DateTime dataHora)
    {
        if (!Enum.IsDefined(tipo)) throw new DomainException("TIPO_OBRIGATORIO");
        if (pessoa == null) throw new DomainException("PESSOA_OBRIGATORIA");
        if (dataHora < DateTime.Now.AddMilliseconds(-15)) throw new DomainException("DATAHORA_INVALIDA");
        if (dataHora.TimeOfDay < new TimeSpan(6, 0, 0) || dataHora.TimeOfDay > new TimeSpan(22, 0, 0))
            throw new DomainException("DATAHORA_INTERVALO");
        return new Acesso(id, tipo, pessoa, dataHora);
    }
}
