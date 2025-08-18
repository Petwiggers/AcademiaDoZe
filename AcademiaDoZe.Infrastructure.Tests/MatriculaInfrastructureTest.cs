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

            Matricula matricula = Matricula.Criar(
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
            int id_aluno = 1; // ID do aluno para o qual a matrícula foi criada
            var repoMatricula = new MatriculaRepository(ConnectionString, DatabaseType);
            var matriculaExistente = await repoMatricula.ObterPorAluno(id_aluno);
            Assert.NotNull(matriculaExistente);

            // criar nova matrícula com os mesmos dados, editando o que quiser
            var matriculaAtualizada = Matricula.Criar(
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
        public async Task Aluno_ObterPorCpf_TrocarSenha()
        {
            var _cpf = "12345678900";
            Arquivo arquivo = Arquivo.Criar(new byte[] { 1, 2, 3 });
            var repoAlunoObterPorCpf = new AlunoRepository(ConnectionString, DatabaseType);
            var alunoExistente = await repoAlunoObterPorCpf.ObterPorCpf(_cpf);
            Assert.NotNull(alunoExistente);
            var novaSenha = "novaSenha123";
            var repoAlunoTrocarSenha = new AlunoRepository(ConnectionString, DatabaseType);

            var resultadoTrocaSenha = await repoAlunoTrocarSenha.TrocarSenha(alunoExistente.Id, novaSenha);
            Assert.True(resultadoTrocaSenha);

            var repoAlunoObterPorId = new AlunoRepository(ConnectionString, DatabaseType);
            var alunoAtualizado = await repoAlunoObterPorId.ObterPorId(alunoExistente.Id);
            Assert.NotNull(alunoAtualizado);
            Assert.Equal(novaSenha, alunoAtualizado.Senha);
        }
        [Fact]
        public async Task Aluno_ObterPorCpf_Remover_ObterPorId()
        {
            var _cpf = "12345678900";
            var repoAlunoObterPorCpf = new AlunoRepository(ConnectionString, DatabaseType);
            var alunoExistente = await repoAlunoObterPorCpf.ObterPorCpf(_cpf);
            Assert.NotNull(alunoExistente);

            // Remover
            var repoAlunoRemover = new AlunoRepository(ConnectionString, DatabaseType);
            var resultadoRemover = await repoAlunoRemover.Remover(alunoExistente.Id);
            Assert.True(resultadoRemover);

            var repoAlunoObterPorId = new AlunoRepository(ConnectionString, DatabaseType);
            var resultadoRemovido = await repoAlunoObterPorId.ObterPorId(alunoExistente.Id);
            Assert.Null(resultadoRemovido);
        }
        [Fact]
        public async Task Aluno_ObterTodos()
        {
            var repoAlunoRepository = new AlunoRepository(ConnectionString, DatabaseType);
            var resultado = await repoAlunoRepository.ObterTodos();
            Assert.NotNull(resultado);
        }
    }
}