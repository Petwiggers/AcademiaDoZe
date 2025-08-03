//Peterson Wiggers
namespace Academia.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Colaborador : Pessoa
{
    public DateOnly DataAdmissao { get; private set;}
    public EColaboradorTipo Tipo { get; private set;}
    public EColaboradorVinculo Vinculo { get; private set;}
        
    private Colaborador(string nomeCompleto,
    string cpf,
    DateOnly dataNascimento,
    string telefone,
    string email,
    Logradouro endereco,
    string numero,
    string complemento,
    string senha,
    Arquivo foto,
    DateOnly dataAdmissao,
    EColaboradorTipo tipo,
    EColaboradorVinculo vinculo)
    : base(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto)
    {
        DataAdmissao = dataAdmissao;
        Tipo = tipo;
        Vinculo = vinculo;
    }

    public static Colaborador Criar(string nomeCompleto,
        string cpf,
        DateOnly dataNascimento,
        string telefone,
        string email,
        Logradouro endereco,
        string numero,
        string complemento,
        string senha,
        Arquivo foto,
        DateOnly dataAdmissao,
        EColaboradorTipo tipo,
        EColaboradorVinculo vinculo)
    {
        // Validações e normalizações
        if (string.IsNullOrEmpty(nomeCompleto)) throw new DomainException("NOME_OBRIGATORIO");
        nomeCompleto = TextoNormalizadoService.LimparEspacos(nomeCompleto);

        if (string.IsNullOrEmpty(cpf)) throw new DomainException("CPF_OBRIGATORIO");
        cpf = TextoNormalizadoService.LimparEDigitos(cpf);
        if (cpf.Length != 11) throw new DomainException("CPF_DIGITOS");

        if (dataNascimento == default) throw new DomainException("DATA_NASCIMENTO_OBRIGATORIO");
        if (dataNascimento > DateOnly.FromDateTime(DateTime.Today.AddYears(-12))) throw new DomainException("DATA_NASCIMENTO_MINIMA_INVALIDA");

        if (string.IsNullOrEmpty(telefone)) throw new DomainException("TELEFONE_OBRIGATORIO");
        telefone = TextoNormalizadoService.LimparEDigitos(telefone);
        if (telefone.Length != 11) throw new DomainException("TELEFONE_DIGITOS");

        email = TextoNormalizadoService.LimparEspacos(email);
        if (TextoNormalizadoService.ValidarFormatoEmail(email)) throw new DomainException("EMAIL_FORMATO");

        if (string.IsNullOrEmpty(senha)) throw new DomainException("SENHA_OBRIGATORIO");
        senha = TextoNormalizadoService.LimparTodosEspacos(senha);
        if (TextoNormalizadoService.ValidarFormatoSenha(senha)) throw new DomainException("SENHA_FORMATO");

        // if (foto == null) throw new DomainException("FOTO_OBRIGATORIO");

        if (endereco == null) throw new DomainException("LOGRADOURO_OBRIGATORIO");

        if (string.IsNullOrEmpty(numero)) throw new DomainException("NUMERO_OBRIGATORIO");
        numero = TextoNormalizadoService.LimparEspacos(numero);

        complemento = TextoNormalizadoService.LimparEspacos(complemento);

        if (dataAdmissao == default) throw new DomainException("DATA_ADMISSAO_OBRIGATORIO");
        if (dataAdmissao > DateOnly.FromDateTime(DateTime.Today)) throw new DomainException("DATA_ADMISSAO_MAIOR_ATUAL");

        if (!Enum.IsDefined(tipo)) throw new DomainException("TIPO_COLABORADOR_INVALIDO");

        if (!Enum.IsDefined(vinculo)) throw new DomainException("VINCULO_COLABORADOR_INVALIDO");
        if (tipo == EColaboradorTipo.Administrador && vinculo == EColaboradorVinculo.CLT) throw new DomainException("ADMINISTRADOR_CLT_INVALIDO");

        return new Colaborador(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo);
    }
    /*
    Deve ser possível registrar os colaboradores com:

    nome completo, cpf, data de nascimento, telefone, e-mail, senha, /foto, 
    logradouro {cep, país, estado, cidade, bairro, nome logradouro}, 
    número e /complemento, data admissão, tipo {administrador, atendente, instrutor}, vínculo {clt, estágio}.
    */
}

