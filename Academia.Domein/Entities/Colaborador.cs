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
        if (string.IsNullOrWhiteSpace(nomeCompleto)) throw new DomainException("NOME_OBRIGATORIO");
        nomeCompleto = TextoNormalizadoService.LimparEspacos(nomeCompleto);
        nomeCompleto = TextoNormalizadoService.ParaMaiusculo(nomeCompleto);
        if (string.IsNullOrWhiteSpace(cpf)) throw new DomainException("CPF_OBRIGATORIO");
        cpf = TextoNormalizadoService.LimparEDigitos(cpf);
        if (dataNascimento ==  default) throw new DomainException("DATA_NASCIMENTO_OBRIGATORIO");
        if (string.IsNullOrWhiteSpace(telefone)) throw new DomainException("TELEFONE_OBRIGATORIO");
        telefone = TextoNormalizadoService.LimparEDigitos(telefone);
        if (string.IsNullOrWhiteSpace(email)) throw new DomainException("EMAIL_OBRIGATORIO");
        email = TextoNormalizadoService.LimparEspacos(email);
        email = TextoNormalizadoService.ParaMaiusculo(email);
        if (string.IsNullOrWhiteSpace(senha)) throw new DomainException("SENHA_OBRIGATORIO");
        senha = TextoNormalizadoService.LimparTodosEspacos(senha);
        senha = TextoNormalizadoService.ParaMaiusculo(senha);
        if (endereco == null) throw new DomainException("LOGRADOURO_OBRIGATORIO");
        if (string.IsNullOrWhiteSpace(numero)) throw new DomainException("NUMERO_OBRIGATORIO");
        numero = TextoNormalizadoService.LimparTodosEspacos(numero);

        return new Colaborador(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto, dataAdmissao, tipo, vinculo);
    }
    /*
    Deve ser possível registrar os colaboradores com:

    nome completo, cpf, data de nascimento, telefone, e-mail, senha, /foto, 
    logradouro {cep, país, estado, cidade, bairro, nome logradouro}, 
    número e /complemento, data admissão, tipo {administrador, atendente, instrutor}, vínculo {clt, estágio}.
    */
}

