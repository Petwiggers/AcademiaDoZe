//Peterson Wigggers
using Academia.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Domain.Tests
{
    public class AcessoDomainTests
    {
        private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1]);
        private Logradouro GetValidLogradouro() => Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");

        private DateTime DateTimeInvalido() => DateTime.Today + new TimeSpan(23,0,0);
        private Aluno CriarAlunoValido()
        {
            var nome = "João da Silva"; var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
            var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
            return Aluno.Criar(nome, cpf, dataNascimento, telefone, email, logradouro, numero, complemento, senha, foto);
        }

        [Fact]
        public void Cria_Acesso_Valido()
        {
            var acesso = Acesso.Criar(
                EPessoaTipo.Aluno,
                CriarAlunoValido(),
                DateTime.Now);

            Assert.NotNull(acesso);
        }

        [Fact]
        public void CriaAcess_PessoaNull_DeveGerarExcecao()
        {
            var ex = Assert.Throws<DomainException>(() => Acesso.Criar(
            EPessoaTipo.Aluno,
            null,
            DateTime.Now)); 

            Assert.Equal("PESSOA_OBRIGATORIA", ex.Message);
        }

        [Fact]
        public void CriaAcess_DataHoraInvalida_DeveGerarExcecao()
        {
            var ex = Assert.Throws<DomainException>(() => Acesso.Criar(
            EPessoaTipo.Aluno,
            CriarAlunoValido(),
            DateTimeInvalido()));

            Assert.Equal("DATAHORA_INTERVALO", ex.Message);
        }
    }
}
