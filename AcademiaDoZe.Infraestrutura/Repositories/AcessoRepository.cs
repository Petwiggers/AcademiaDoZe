using Academia.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Data;
using AcademiaDoZe.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademiaDoZe.Infraestrutura.Repositories
{
    public class AcessoRepository : BaseRepository<Acesso>, IAcessoRepository
    {
        public AcessoRepository(string connectionString, DatabaseType databaseType) : base(connectionString, databaseType) { }
        public override async Task<Acesso> Adicionar(Acesso entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = _databaseType == DatabaseType.SqlServer
                ?
                $"INSERT INTO {TableName} (pessoa_tipo, pessoa_id, data_hora) " +
                $"OUTPUT INSERTED.id_acesso " +
                $"VALUES (@Pessoa_tipo, @Pessoa_id, @Data_hora);"
                : $"INSERT INTO {TableName} pessoa_tipo, pessoa_id, data_hora) "
                + "VALUES (@Pessoa_tipo, @Pessoa_id, @Data_hora); "
                + "SELECT LAST_INSERT_ID();";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Pessoa_tipo", entity.tipo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Pessoa_id", entity.AlunoColaborador.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_hora", entity.DataHora, DbType.DateTime, _databaseType));
                var id = await command.ExecuteScalarAsync();
                if (id != null && id != DBNull.Value)
                {
                    // Define o ID usando reflection
                    var idProperty = typeof(Entity).GetProperty("Id");
                    idProperty?.SetValue(entity, Convert.ToInt32(id));
                }
                return entity;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao adicionar aluno: {ex.Message}", ex); }
        }

        public override async Task<Acesso> Atualizar(Acesso entity)
        {
            //Tem sentido ter um médtoto de atualizar o Acesso?
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"UPDATE {TableName} "
                + "SET pessoa_tipo = @Pessoa_tipo, "
                + "pessoa_id = @Pessoa_id, "
                + "data_hora = @Data_hora, "
                + "WHERE id_acesso = @Id";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Id", entity.Id, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@@Pessoa_tipo", entity.tipo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Pessoa_id", entity.AlunoColaborador.Id, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_hora", entity.DataHora, DbType.String, _databaseType));
                int rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"Nenhum aluno encontrado com o ID {entity.Id} para atualização.");
                }
                return entity;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao atualizar acesso: {ex.Message}", ex); }
        }
        public Task<IEnumerable<Acesso>> GetAcessosPorAlunoPeriodo(int? alunoId = null, DateOnly? inicio = null, DateOnly? fim = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Acesso>> GetAcessosPorColaboradorPeriodo(int? colaboradorId = null, DateOnly? inicio = null, DateOnly? fim = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Aluno>> GetAlunosSemAcessoNosUltimosDias(int dias)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<TimeOnly, int>> GetHorarioMaisProcuradoPorMes(int mes)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<int, TimeSpan>> GetPermanenciaMediaPorMes(int mes)
        {
            throw new NotImplementedException();
        }

        protected override async Task<Acesso> MapAsync(DbDataReader reader)
        {
            try
            {
                // Obtém o logradouro de forma assíncrona
                var pessoaId = Convert.ToInt32(reader["pessoa_id"]);
                var pessoaTipo = (EPessoaTipo)Convert.ToInt32(reader["tipo"]);
                Pessoa pessoa = null;
                if (pessoaTipo == EPessoaTipo.Aluno)
                {
                    var repository = new AlunoRepository(_connectionString, _databaseType);
                    pessoa = await repository.ObterPorId(pessoaId) ?? throw new InvalidOperationException($"Aluno com ID {pessoaId} não encontrado.");
                }
                else if (pessoaTipo == EPessoaTipo.Colaborador)
                {
                    var repository = new ColaboradorRepository(_connectionString, _databaseType);
                    pessoa = await repository.ObterPorId(pessoaId) ?? throw new InvalidOperationException($"Colaborador com ID {pessoaId} não encontrado.");
                }
                else
                {
                    throw new InvalidOperationException($"Tipo de pessoa inválido: {pessoaTipo}");
                }                
                // Cria o objeto aluno usando o método de fábrica
                var acesso = Acesso.Criar(pessoaTipo, pessoa, 
                    Convert.ToDateTime(reader["data_hora"])
                );
                // Define o ID usando reflection
                var idProperty = typeof(Entity).GetProperty("Id");
                idProperty?.SetValue(acesso, Convert.ToInt32(reader["id_aluno"]));
                return acesso;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao mapear dados do aluno: {ex.Message}", ex); }
        }
    }
}
