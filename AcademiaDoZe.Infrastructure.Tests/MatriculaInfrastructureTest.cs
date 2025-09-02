//Peterson Wiggers
using Academia.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.ValueObjects;
using AcademiaDoZe.Infraestrutura.Repositories;
namespace AcademiaDoZe.Infrastructure.Tests
{
    public class MatriculaInfrastructureTest : TestBase
    {
        [Fact]
        public async Task AdicionarMatricula()
        {
            // com base em logradouroID, acessar logradourorepository e obter o logradouro
            var repoMatricula= new MatriculaRepository(ConnectionString, DatabaseType);
            
            var repoALuno = new AlunoRepository(ConnectionString, DatabaseType);
            Aluno? aluno = await repoALuno.ObterPorCpf("12345678900");

            Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });
            Assert.NotNull(aluno);

            Matricula matricula = Matricula.Criar(1,
                aluno, 
                EMatriculaPlano.anual,
                new DateOnly(2024, 01, 01),
                new DateOnly(2025, 01, 01),
                "Melhorar a saúde",
                EMatriculaRestricoes.diabetes,
                "Nenhuma restrição",
                arquivo
                );

            var matriculaInserida = await repoMatricula.Adicionar(matricula);
            Assert.NotNull(matriculaInserida);
            Assert.True(matriculaInserida.Id > 0);
        }

        [Fact]
        public async Task Matricula_ObterPorAluno_Atualizar()
        {
            int id_aluno = 1002; // ID do aluno para o qual a matrícula foi criada
            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
            var matriculaExistente = await repoMatricula.ObterPorAluno(id_aluno);
            Assert.NotNull(matriculaExistente);

            // criar nova matrícula com os mesmos dados, editando o que quiser
            var matriculaAtualizada = Matricula.Criar(1,
                matriculaExistente.AlunoMatricula,
                EMatriculaPlano.mensal,
                matriculaExistente.DataInicio.AddDays(30), // Exemplo de atualização
                matriculaExistente.DataFim.AddDays(30),
                "Melhorar a saúde e condicionamento físico",
                EMatriculaRestricoes.pressaoAlta,
                "Nenhuma restrição",
                matriculaExistente.LaudoMedico
            );

            // Usar reflexão para definir o ID
            var idProperty = typeof(Entity).GetProperty("Id");
            idProperty?.SetValue(matriculaAtualizada, matriculaExistente.Id);
            // Teste de Atualização
            var repoMatriculaAtualizar = new MatriculaRepository(ConnectionString, DatabaseType);
            var resultadoAtualizacao = await repoMatriculaAtualizar.Atualizar(matriculaAtualizada);
            Assert.NotNull(resultadoAtualizacao);
            Assert.Equal(EMatriculaPlano.mensal, resultadoAtualizacao.Plano);
        }

        [Fact]
        public async Task Obter_Matriculas_Ativas()
        {
            var repository = new MatriculaRepository(ConnectionString, DatabaseType);
            var matriculas = await repository.ObterAtivas();
            Assert.NotNull(matriculas);
            Assert.True(matriculas.Count() > 0);
        }

        [Fact]
        public async Task Matricula_ObterPorAluno_Remover_ObterPorId()
        {
            var respositoryAluno = new AlunoRepository(ConnectionString, DatabaseType);
            var aluno= await respositoryAluno.ObterPorCpf("12345678900");
            Assert.NotNull(aluno);

            var repoObterMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
            var matricula = await repoObterMatricula.ObterPorAluno(aluno.Id);
            Assert.NotNull(matricula);
            Assert.Equal(matricula.AlunoMatricula.Id, 1002);
            // Assert.True(false, matricula.AlunoMatricula.Id.ToString());

            var repoRemoverMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
            var resultadoRemover = await repoRemoverMatricula.Remover(matricula.Id);
            Assert.True(resultadoRemover);

            var repoVerificarSeMatriculaExiste = new MatriculaRepository(ConnectionString, DatabaseType);
            var resultadoVerificacao = await repoVerificarSeMatriculaExiste.ObterPorId(matricula.Id);
            Assert.Null(resultadoVerificacao);
        }

        [Fact]
        public async Task Matricula_Obter_Vencendo_Em_Dias()
        {
            var repository = new MatriculaRepository(ConnectionString, DatabaseType);
            var matriculas = await repository.ObterVencendoEmDias(10);
            Assert.NotNull(matriculas);

        }
    }
}