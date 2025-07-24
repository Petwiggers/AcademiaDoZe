#Peterson Wiggers
namespace Academia_Classes
{
    public class Aluno : Pessoa
    {
        public Aluno(string nomeCompleto,
        string cpf,
        DateOnly dataNascimento,
        string telefone,
        string email,
        Logradouro endereco,
        string numero,
        string complemento,
        string senha,
        Arquivo foto
        : base(nomeCompleto, cpf, dataNascimento, telefone, email, endereco, numero, complemento, senha, foto)
        {
        }
    }
}
