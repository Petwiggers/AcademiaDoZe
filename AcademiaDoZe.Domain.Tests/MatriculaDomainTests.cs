//Peterson Wigggers
using Academia.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Exceptions;
namespace AcademiaDoZe.Domain.Tests
{
    public class MatriculaDomainTests
    {
        private Logradouro GetValidLogradouro() => Logradouro.Criar("12345678", "Rua A", "Centro", "Cidade", "SP", "Brasil");
        private Aluno CriarAlunoValido()
        {
            var nome = "João da Silva"; var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)); var telefone = "11999999999";
            var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
            return Aluno.Criar(nome, cpf, dataNascimento, telefone, email, logradouro, numero, complemento, senha, foto);
        }

        private Aluno CriarAlunoValido12anos()
        {
            var nome = "João da Silva"; var cpf = "12345678901"; var dataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-12)); var telefone = "11999999999";
            var email = "joao@email.com"; var logradouro = GetValidLogradouro(); var numero = "123"; var complemento = "Apto 1"; var senha = "Senha@1"; var foto = GetValidArquivo();
            return Aluno.Criar(nome, cpf, dataNascimento, telefone, email, logradouro, numero, complemento, senha, foto);
        }

        private Arquivo GetValidArquivo() => Arquivo.Criar(new byte[1], ".jpg");

        [Fact]
        public void CriarMatricula_ComDadosValidos_DeveCriarObjeto()
        {
            var matricula = Matricula.Criar(CriarAlunoValido(),
            EMatriculaPlano.semestral,
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today.AddMonths(6)),
            "Meu objetivo é ficar grande",
            EMatriculaRestricoes.RemedioContinuo,
            "Observações",
            GetValidArquivo());

            Assert.NotNull(matricula);
        }

        [Fact]
        public void CriarMatricula_VerificaNormalizacoes()
        {
            var matricula = Matricula.Criar(CriarAlunoValido(),
            EMatriculaPlano.semestral,
            DateOnly.FromDateTime(DateTime.Today),
            DateOnly.FromDateTime(DateTime.Today.AddMonths(6)),
            "       Meu objetivo é ficar grande                ",
            EMatriculaRestricoes.RemedioContinuo,
            "       Observações    de restrição      ",
            GetValidArquivo());

            Assert.Equal("Meu objetivo é ficar grande", matricula.Objetivo);
            Assert.Equal("Observações de restrição", matricula.ObservacoesRestricoes);
        }

        [Fact]
        public void CriarMatricula_ObjetivoVazio_DeveGerarExcecao()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Matricula.Criar(CriarAlunoValido(),
                EMatriculaPlano.semestral,
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today.AddMonths(6)),
                "",
                EMatriculaRestricoes.RemedioContinuo,
                "Observações de restrição",
                GetValidArquivo()));

            Assert.Equal("OBJETIVO_OBRIGATORIO", ex.Message);
        }

        [Fact]
        public void CriarMatricula_AlunoNull_DeveGerarExcecao()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Matricula.Criar(null,
                EMatriculaPlano.semestral,
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today.AddMonths(6)),
                "Meu objetivo é ficar grande",
                EMatriculaRestricoes.RemedioContinuo,
                "Observações de restrição",
                GetValidArquivo()));

            Assert.Equal("ALUNO_OBRIGATORIO", ex.Message);
        }

        [Fact]
        public void CriarMatricula_Aluno12AnosSemLaudo_DeveGerarExcecao()
        {
            var ex = Assert.Throws<DomainException>(() =>
                Matricula.Criar(CriarAlunoValido12anos(),
                EMatriculaPlano.semestral,
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today.AddMonths(6)),
                "Meu objetivo é ficar grande",
                EMatriculaRestricoes.RemedioContinuo,
                "Observações de restrição",
                null));

            Assert.Equal("LAUDO_OBRIGATORIO", ex.Message);
        }
    }
}
