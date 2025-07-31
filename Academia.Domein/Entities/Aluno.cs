//Peterson Wiggers
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Academia.Domain.Entities;
public class Aluno : Pessoa
{
    private Aluno(string nomeCompleto,
    string cpf,
    DateOnly dataNascimento,
    string telefone,
    string email,
    Logradouro endereco,
    string numero,
    string complemento,
    string senha,
    Arquivo foto)
    : base(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto)
    {
    }

    public static Aluno Criar(string nomeCompleto,
    string cpf,
    DateOnly dataNascimento,
    string telefone,
    string email,
    Logradouro endereco,
    string numero,
    string complemento,
    string senha,
    Arquivo foto)
    {
        if (string.IsNullOrWhiteSpace(nomeCompleto)) throw new DomainException("NOME_OBRIGATORIO");
        nomeCompleto = TextoNormalizadoService.LimparEspacos(nomeCompleto);
        nomeCompleto = TextoNormalizadoService.ParaMaiusculo(nomeCompleto);
        if (string.IsNullOrWhiteSpace(cpf)) throw new DomainException("CPF_OBRIGATORIO");
        cpf = TextoNormalizadoService.LimparEDigitos(cpf);
        if (dataNascimento == default) throw new DomainException("DATA_NASCIMENTO_OBRIGATORIO");
        if (string.IsNullOrWhiteSpace(telefone)) throw new DomainException("TELEFONE_OBRIGATORIO");
        telefone = TextoNormalizadoService.LimparEDigitos(telefone);
        if (string.IsNullOrWhiteSpace(senha)) throw new DomainException("SENHA_OBRIGATORIO");
        senha = TextoNormalizadoService.LimparTodosEspacos(senha);
        senha = TextoNormalizadoService.ParaMaiusculo(senha);
        if (endereco == null) throw new DomainException("LOGRADOURO_OBRIGATORIO");
        if (string.IsNullOrWhiteSpace(numero)) throw new DomainException("NUMERO_OBRIGATORIO");
        numero = TextoNormalizadoService.LimparTodosEspacos(numero);

        return new Aluno(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto);
    }


    /*Cadastro de alunos: *nome completo, *cpf, *data de nascimento, *telefone, e-mail, *senha, foto, *logradouro {cep,
    pais, estado,*número, complemento*/
}

