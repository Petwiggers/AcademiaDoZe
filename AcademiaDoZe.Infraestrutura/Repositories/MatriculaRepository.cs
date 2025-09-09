//Peterson Wigggers
using Academia.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Data;
using AcademiaDoZe.Infrastructure.Repositories;
using System.Data;
using System.Data.Common;
using AcademiaDoZe.Domain.ValueObjects;

namespace AcademiaDoZe.Infraestrutura.Repositories
{
    public class MatriculaRepository : BaseRepository<Matricula>, IMatriculaRepository
    {
        public MatriculaRepository (string connectionString, DatabaseType databaseType) : base (connectionString, databaseType) { }
        public override async Task<Matricula> Adicionar(Matricula entity)
        {
            try
            {
                Matricula matricula = await ObterPorAluno(entity.AlunoMatricula.Id);

                if (matricula != null && matricula.DataFim > DateOnly.FromDateTime(DateTime.Today)) throw new InvalidOperationException("ALUNO_JA_POSSUI_MATRICULA");

                await using var connection = await GetOpenConnectionAsync();
                string query = _databaseType == DatabaseType.SqlServer
                ?
                $"INSERT INTO {TableName} (aluno_id, plano, data_inicio, data_fim, objetivo, restricao_medica, obs_restricao) " +
                $"OUTPUT INSERTED.id_matricula " +
                $"VALUES (@Aluno_id, @Plano, @Data_inicio, @Data_fim, @Objetivo, @Restricao_medica, @Obs_restricao);"
                : $"INSERT INTO {TableName} (aluno_id,  plano, data_inicio, data_fim, objetivo, restricao_medica, obs_restricao) "
                + "VALUES (@Aluno_id, @Plano, @Data_inicio, @Data_fim, @Objetivo, @Restricao_medica, @Obs_restricao); "
                + "SELECT LAST_INSERT_ID();";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Aluno_id", entity.AlunoMatricula.Id, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", (int)entity.Plano, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_inicio", entity.DataInicio, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_fim", entity.DataFim, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Restricao_medica", (int)entity.RestricoesMedicas, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Obs_restricao", entity.ObservacoesRestricoes, DbType.String, _databaseType));
                var id = await command.ExecuteScalarAsync();
                if (id != null && id != DBNull.Value)
                {
                    // Define o ID usando reflection
                    var idProperty = typeof(Entity).GetProperty("Id");
                    idProperty?.SetValue(entity, Convert.ToInt32(id));
                }
                return entity;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao adicionar matricula: {ex.Message}", ex); }
        }

        public override async Task<Matricula> Atualizar(Matricula entity)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"UPDATE {TableName} "
                + "SET plano = @Plano,"
                + "data_inicio = @Data_inicio, "
                + "data_fim = @Data_fim, "
                + "objetivo = @Objetivo, "
                + "restricao_medica = @Restricao_medica, "
                + "obs_restricao= @Obs_restricao "
                + $"WHERE {IdTableName} = @Aluno_id";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Aluno_id", entity.AlunoMatricula.Id, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Plano", (int)entity.Plano, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_inicio", entity.DataInicio, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_fim", entity.DataFim, DbType.Date, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Objetivo", entity.Objetivo, DbType.String, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Restricao_medica", (int)entity.RestricoesMedicas, DbType.Int32, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Obs_restricao", entity.ObservacoesRestricoes, DbType.String, _databaseType));

                int rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new InvalidOperationException($"Nenhuma matricula encontrado com o ID {entity.Id} para atualização.");
                }
                return entity;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao atualizar matricula com ID {entity.Id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Matricula>> ObterAtivas(int idAluno = 0)
        {
            try
            {

                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT * FROM {TableName} WHERE data_fim >= {(_databaseType == DatabaseType.SqlServer ? "GETDATE()" :
                "CURRENT_DATE()")} {(idAluno > 0 ? "AND aluno_id = @id" : "")} ";
                await using var command = DbProvider.CreateCommand(query, connection);
                await using var reader = await command.ExecuteReaderAsync();
                var matriculas = new List<Matricula>();
                while (reader.Read())
                {
                    matriculas.Add(await MapAsync(reader));
                }
                return matriculas;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao obter matriculas ativas: {ex.Message}", ex);
            }
        }

        public async Task<Matricula> ObterPorAluno(int alunoId)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"select * from {TableName} where aluno_id = @Aluno_ID;";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Aluno_ID", alunoId, DbType.Int32, _databaseType));
                await using var reader = await command.ExecuteReaderAsync();
                 
                while (reader.Read())
                {
                    var matricula = await MapAsync(reader);
                    return matricula;
                }

                return null;
                
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao obter matricula por aluno com ID {alunoId}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias)
        {
            try
            {
                await using var connection = await GetOpenConnectionAsync();
                string query = $"select * from tb_matricula "
                + "where data_fim <= CAST(DATEADD(DAY, @Dias, GETDATE()) AS date)"
                + "AND data_fim >= CAST(GETDATE() as date);";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Dias", dias, DbType.Int32, _databaseType));
                await using var reader = await command.ExecuteReaderAsync();
                var matriculas = new List<Matricula>();
                while (reader.Read())
                {
                    matriculas.Add(await MapAsync(reader));
                }
                return matriculas;
            }
            catch (DbException ex)
            {
                throw new InvalidOperationException($"Erro ao obter matriculas ativas: {ex.Message}", ex);
            }
        }
        public override async Task<Matricula> MapAsync(DbDataReader reader)
        {
            try
            {
                var alunoId = Convert.ToInt32(reader["aluno_id"]);
                var alunoRepository = new AlunoRepository(_connectionString, _databaseType);
                var aluno = await alunoRepository.ObterPorId(alunoId) ?? throw new InvalidOperationException($"Aluno com ID {alunoId} não encontrado.");

                var matricula = Matricula.Criar(
                    id: Convert.ToInt32(reader["id_matricula"]),
                    alunoMatricula: aluno,
                    plano: (EMatriculaPlano)Convert.ToInt32(reader["plano"]),
                    dataInicio: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_inicio"])),
                    dataFim: DateOnly.FromDateTime(Convert.ToDateTime(reader["data_fim"])),
                    objetivo: reader["objetivo"].ToString()!,
                    restricoesMedicas: (EMatriculaRestricoes)Convert.ToInt32(reader["restricao_medica"]),
                    observacoes: reader["obs_restricao"]?.ToString(),
                    laudoMedico: reader["laudo_medico"] is DBNull ? null : Arquivo.Criar((byte[])reader["laudo_medico"])
                );
                return matricula;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao mapear dados da matricula: {ex.Message}", ex); }
            throw new NotImplementedException();
        }
    }
}
