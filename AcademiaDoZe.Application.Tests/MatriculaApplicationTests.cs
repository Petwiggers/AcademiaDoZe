using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Enums;
using AcademiaDoZe.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
namespace AcademiaDoZe.Application.Tests
{
    public class MatriculaApplicationTests
    {
        // Configurações de conexão
        const string connectionString = "Server=localhost;Database=db_academia_do_ze;User Id=sa;Password=abcBolinhas12345;TrustServerCertificate=True;Encrypt=True;";
        const EAppDatabaseType databaseType = EAppDatabaseType.SqlServer;
        [Fact(Timeout = 60000)]
        public async Task MatriculaService_Integracao_Adicionar_Obter_Atualizar_Remover()
        {
            // Arrange: DI usando repositório real (Infra)
            // Configuração dos serviços usando a classe DependencyInjection
            var services = DependencyInjection.ConfigureServices(connectionString, databaseType);
            // Cria o provedor de serviços
            var provider = DependencyInjection.BuildServiceProvider(services);
            // Obtém os serviços necessários via injeção de dependência
            var matriculaService = provider.GetRequiredService<IMatriculaService>();
            var logradouroService = provider.GetRequiredService<ILogradouroService>();
            // Gera um CPF único para evitar conflito
            var _cpf = GerarCpfFake();
            // obter o logradouro
            var logradouro = await logradouroService.ObterPorIdAsync(5);
            Assert.NotNull(logradouro);
            Assert.Equal(5, logradouro!.Id);
            // cria um arquivo (para facilitar, copie uma foto para dentro do diretório com os fontes do teste)
            // caminho relativo da foto
            var caminhoFoto = Path.Combine("C:\\Users\\peter\\OneDrive\\Documentos\\Pet\\Sistemas De Informação\\Desenvolvimento de Sistemas\\AcademiaDoZe\\Foto_teste.png");
            //throw new Exception(caminhoFoto);
            ArquivoDTO foto = new();

            if (File.Exists(caminhoFoto)) { foto.Conteudo = await File.ReadAllBytesAsync(caminhoFoto); }

            else { foto.Conteudo = null; Assert.Fail("Foto de teste não encontrada."); }

            
            var dto = new MatriculaDTO
            {
                Id = 1,
                AlunoMatricula = new AlunoDTO
                {
                    Id = 2006,
                    Nome = "Aluno Teste",
                    Cpf = _cpf,
                    DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
                    Telefone = "11999999999",
                    Email = "Colaborador@teste.com",
                    Endereco = logradouro,
                    Numero = "100",
                    Complemento = "Apto 1",
                    Senha = "Senha@1",
                    Foto = foto
                },
                ObservacoesRestricoes = "Sem observações",
                Objetivo = "Ganhar massa muscular",
                Plano = EAppMatriculaPlano.Trimestral,
                DataInicio = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                DataFim = DateOnly.FromDateTime(DateTime.Now.AddMonths(11))
            };

            MatriculaDTO? criado = null;
            try
            {
                // Act - Adicionar
                criado = await matriculaService.AdicionarAsync(dto);
                // Assert - criação
                Assert.NotNull(criado);
                Assert.True(criado!.Id > 0);
                Assert.Equal(_cpf, criado.AlunoMatricula.Cpf);
                // Act - Obter por cpf
                var obtido = await matriculaService.ObterPorAlunoIdAsync(criado.AlunoMatricula.Id);
                // Assert - obter
                Assert.NotNull(obtido);
                Assert.Equal(criado.Id, obtido!.Id);
                Assert.Equal(criado.AlunoMatricula.Id, obtido.AlunoMatricula.Id);
                // Act - Atualizar
                var atualizar = new MatriculaDTO
                {
                    Id = criado.Id,
                    AlunoMatricula = new AlunoDTO
                    {
                        Nome = "Aluno Atualizado",
                        Cpf = _cpf,
                        DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)),
                        Telefone = "11999999999",
                        Email = "Colaborador@teste.com",
                        Endereco = logradouro,
                        Numero = "100",
                        Complemento = "Apto 1",
                        Senha = "Senha@1",
                        Foto = foto
                    },
                    ObservacoesRestricoes = "Sem observações",
                    Objetivo = "Objetivo Atualizado",
                    Plano = EAppMatriculaPlano.Semestral,
                    DataInicio = DateOnly.FromDateTime(DateTime.Now.AddMonths(-1)),
                    DataFim = DateOnly.FromDateTime(DateTime.Now.AddMonths(11))
                };
                
                var atualizado = await matriculaService.AtualizarAsync(atualizar);
                // Assert - atualizar
                Assert.NotNull(atualizado);
                Assert.Equal("Objetivo Atualizado", atualizado.Objetivo);
                Assert.Equal(EAppMatriculaPlano.Semestral, atualizado.Plano);
                // Act - Remover
                var removido = await matriculaService.RemoverAsync(criado.Id);
                Assert.True(removido);
                // Act - Conferir remoção
                await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                {
                    var aposRemocao = await matriculaService.ObterPorIdAsync(criado.Id);
                    Assert.Null(aposRemocao);
                });
                
                
            }
            finally
            {
                // Clean-up defensivo (se algo falhar antes do remove)
                if (criado is not null)
                {
                    try { await matriculaService.RemoverAsync(criado.Id); } catch { }
                }
            }
        }
        // Helper simples para gerar um CPF numérico de 11 dígitos (sem validação de dígito verificador)
        private static string GerarCpfFake()
        {
            var rnd = new Random();
            return string.Concat(Enumerable.Range(0, 11).Select(_ => rnd.Next(0, 10).ToString()));
        }
    }
}