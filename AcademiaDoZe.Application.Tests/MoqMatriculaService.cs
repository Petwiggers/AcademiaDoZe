using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Moq;
namespace AcademiaDoZe.Application.Tests
{
    public class MoqMatriculaServiceTests
    {
        private readonly Mock<IMatriculaService> _matriculaServiceMock;
        private readonly IMatriculaService _matriculaService;
        public MoqMatriculaServiceTests()
        {
            _matriculaServiceMock = new Mock<IMatriculaService>();
            _matriculaService = _matriculaServiceMock.Object;
        }

        private MatriculaDTO CriarMatriculaPadrao(int id = 1)
        {
            return new MatriculaDTO
            {
                Id = id,
                AlunoMatricula = new AlunoDTO
                {
                    Id = id,
                    Nome = "Aluno Teste",
                    Cpf = "12345678901",
                    DataNascimento = DateOnly.FromDateTime(DateTime.Now.AddYears(-30)),
                    Telefone = "11999999999",
                    Email = "aluno@teste.com",
                    Endereco = new LogradouroDTO { Id = 1, Cep = "12345678", Nome = "Rua Teste", Bairro = "Centro", Cidade = "São Paulo", Estado = "SP", Pais = "Brasil" },
                    Numero = "100",
                    Complemento = "Apto 101",
                    Senha = "Senha@123"
                },
                Objetivo = "Ganhar massa muscular",
                Plano = EAppMatriculaPlano.Trimestral,
                DataInicio = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                DataFim = DateOnly.FromDateTime(DateTime.Now.AddMonths(11))
            };
        }

        [Fact]
        public async Task CadastrarMatricula_ComDadosCorretos_DeveRetornarMatricula()
        { 
            var matriculaDto = CriarMatriculaPadrao();
            _matriculaServiceMock.Setup(s => s.AdicionarAsync(matriculaDto)).ReturnsAsync(matriculaDto);
            var resultado = await _matriculaService.AdicionarAsync(matriculaDto);
            Assert.NotNull(matriculaDto);
            Assert.Equal("Aluno Teste", matriculaDto.AlunoMatricula.Nome);
            _matriculaServiceMock.Verify(s => s.AdicionarAsync(matriculaDto), Times.Once);
        }

        [Fact]
        public async Task EditarMatricula_ComDadosCorretos_DeveRetornarMatriculaAtualizada()
        {
            var matriculaDto = CriarMatriculaPadrao();
            matriculaDto.Objetivo = "Perder peso";
            _matriculaServiceMock.Setup(s => s.AtualizarAsync(matriculaDto)).ReturnsAsync(matriculaDto);
            var resultado = await _matriculaService.AtualizarAsync(matriculaDto);
            Assert.NotNull(resultado);
            Assert.Equal("Perder peso", resultado.Objetivo);
            _matriculaServiceMock.Verify(s => s.AtualizarAsync(matriculaDto), Times.Once);
        }

        [Fact]
        public async Task ObterMatriculaPorId_DeveRetornarMatricula_QuandoExistir()
        {
            var matriculaId = 1;
            var matriculaDto = CriarMatriculaPadrao(matriculaId);
            _matriculaServiceMock.Setup(s => s.ObterPorIdAsync(matriculaId)).ReturnsAsync(matriculaDto);
            var result = await _matriculaService.ObterPorIdAsync(matriculaId);
            Assert.NotNull(result);
            Assert.Equal(matriculaId, result.Id);
            _matriculaServiceMock.Verify(s => s.ObterPorIdAsync(matriculaId), Times.Once);
        }

        [Fact]
        public async Task ObterMatriculaPorAlunoId_DeveRetornarMatricula_QuandoExistir()
        {
            var alunoId = 1;
            var matriculaDto = CriarMatriculaPadrao(alunoId);
            _matriculaServiceMock.Setup(s => s.ObterPorAlunoIdAsync(alunoId)).ReturnsAsync(matriculaDto);
            var result = await _matriculaService.ObterPorAlunoIdAsync(alunoId);
            Assert.NotNull(result);
            Assert.Equal(alunoId, result.AlunoMatricula.Id);
            _matriculaServiceMock.Verify(s => s.ObterPorAlunoIdAsync(alunoId), Times.Once);
        }

        [Fact]
        public async Task ObterMatriculasAtivas_DeveRetornarListaDeMatriculas()
        {
            var alunoId = 1;
            var matriculas = new List<MatriculaDTO>
            {
                CriarMatriculaPadrao(1),
                CriarMatriculaPadrao(2)
            };
            _matriculaServiceMock.Setup(s => s.ObterAtivasAsync(alunoId)).ReturnsAsync(matriculas);
            var result = await _matriculaService.ObterAtivasAsync(alunoId);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _matriculaServiceMock.Verify(s => s.ObterAtivasAsync(alunoId), Times.Once);
        }

        [Fact]
        public async Task ObterMatriculasVencendoEmDias_DeveRetornarListaDeMatriculas()
        {
            var dias = 5;
            var matriculas = new List<MatriculaDTO>
            {
                CriarMatriculaPadrao(1),
                CriarMatriculaPadrao(2)
            };
            _matriculaServiceMock.Setup(s => s.ObterVencendoEmDiasAsync(dias)).ReturnsAsync(matriculas);
            var result = await _matriculaService.ObterVencendoEmDiasAsync(dias);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _matriculaServiceMock.Verify(s => s.ObterVencendoEmDiasAsync(dias), Times.Once);
        }

        [Fact]
        public async Task ObterTodasMatriculas_DeveRetornarListaDeMatriculas()
        {
            var matriculas = new List<MatriculaDTO>
            {
                CriarMatriculaPadrao(1),
                CriarMatriculaPadrao(2),
                CriarMatriculaPadrao(3)
            };
            _matriculaServiceMock.Setup(s => s.ObterTodasAsync()).ReturnsAsync(matriculas);
            var result = await _matriculaService.ObterTodasAsync();
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            _matriculaServiceMock.Verify(s => s.ObterTodasAsync(), Times.Once);
        }

    }
}