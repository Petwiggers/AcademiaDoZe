//Peterson Wiggers
using AcademiaDoZe.Domain.ValueObjects;
namespace Academia.Domain.Entities;

public abstract class Pessoa : Entity
{
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public DateOnly DataNascimento { get; private set; }
    public string Telefone { get; private set; }
    public string Email { get; private set; }
    public Logradouro Endereco { get; private set; }
    public string Numero { get; private set; }
    public string Complemento { get; private set; }
    public string Senha { get; private set; }
    public Arquivo Foto { get; private set; }

    protected Pessoa(int id, 
        string nome,
        string cpf,
        DateOnly dataNascimento,
        string telefone,
        string email,
        Logradouro endereco,
        string numero,
        string complemento,
        string senha,
    Arquivo foto) : base()
    {
        Id = id;
        Nome = nome;
        Cpf = cpf;
        DataNascimento = dataNascimento;
        Telefone = telefone;
        Email = email;
        Endereco = endereco;
        Numero = numero;
        Complemento = complemento;
        Senha = senha;
        Foto = foto;
    }
}

