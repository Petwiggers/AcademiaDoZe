//Peterson Wiggers
using Academia.Domain.Entities;
using AcademiaDoZe.Domain.Enums;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Infrastructure.Data;
using AcademiaDoZe.Infrastructure.Repositories;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

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
        public async Task<IEnumerable<Acesso>> GetAcessosPorAlunoPeriodo(int? alunoId = null, DateOnly? inicio = null, DateOnly? fim = null)
        {
            try
            {
                if(inicio == null || inicio == default) throw new InvalidOperationException("Data de início não pode ser nula ou padrão.");
                if(fim == null || fim == default) throw new InvalidOperationException("Data de fim não pode ser nula ou padrão.");
                if(alunoId == null) throw new InvalidOperationException("Aluno ID está como null");
                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT * FROM {TableName} WHERE data_hora>=@Data_inicio and data_hora<DATEADD(day, 1, @Data_fim) and id_aluno=@Id_aluno;";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Data_inicio", inicio?.ToDateTime(new TimeOnly(0, 0)), DbType.DateTime, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_inicio", fim?.ToDateTime(new TimeOnly(0, 0)), DbType.DateTime, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Id_aluno", alunoId, DbType.Int32, _databaseType));
                await using var reader = await command.ExecuteReaderAsync();
                var entities = new List<Acesso>();
                while (await reader.ReadAsync())
                {
                    entities.Add(await MapAsync(reader));
                }
                return entities;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao obter acessos por aluno: {ex.Message}", ex); }
        }

        public async Task<IEnumerable<Acesso>> GetAcessosPorColaboradorPeriodo(int? colaboradorId = null, DateOnly? inicio = null, DateOnly? fim = null)
        {
            try
            {
                if (inicio == null || inicio == default) throw new InvalidOperationException("Data de início não pode ser nula ou padrão.");
                if (fim == null || fim == default) throw new InvalidOperationException("Data de fim não pode ser nula ou padrão.");
                if (colaboradorId == null) throw new InvalidOperationException("Colaborador ID está como null");
                await using var connection = await GetOpenConnectionAsync();
                string query = $"SELECT * FROM {TableName} WHERE data_hora>=@Data_inicio and data_hora<DATEADD(day, 1, @Data_fim) and id_colaborador=@Id_colaborador;";
                await using var command = DbProvider.CreateCommand(query, connection);
                command.Parameters.Add(DbProvider.CreateParameter("@Data_inicio", inicio?.ToDateTime(new TimeOnly(0, 0)), DbType.DateTime, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Data_inicio", fim?.ToDateTime(new TimeOnly(0, 0)), DbType.DateTime, _databaseType));
                command.Parameters.Add(DbProvider.CreateParameter("@Id_colaborador", colaboradorId, DbType.Int32, _databaseType));
                await using var reader = await command.ExecuteReaderAsync();
                var entities = new List<Acesso>();
                while (await reader.ReadAsync())
                {
                    entities.Add(await MapAsync(reader));
                }
                return entities;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao obter acessos por aluno: {ex.Message}", ex); }
        }

        public async Task<IEnumerable<Aluno>> GetAlunosSemAcessoNosUltimosDias(int dias)
        {
            if (dias <= 0) throw new InvalidOperationException("O número de dias deve ser maior que zero." + nameof(dias));
            await using var connection = await GetOpenConnectionAsync();
            string query = $"select * from tb_aluno al"
            + $"join {TableName} a on pessoa_id = id_aluno"
            + $"where pessoa_tipo = {(int)EPessoaTipo.Aluno} and CAST(data_hora AS date) >= CAST(DATEADD(DAY, @Dias, GETDATE()) AS date);";
            await using var commando = DbProvider.CreateCommand(query, connection);
            commando.Parameters.Add(DbProvider.CreateParameter("@Dias", -dias, DbType.Int32, _databaseType));
            await using var reader = await commando.ExecuteReaderAsync();
            var entities = new List<Aluno>();
            var repositoryAluno = new AlunoRepository(_connectionString, _databaseType);
            while (await reader.ReadAsync())
            {
                entities.Add(await repositoryAluno.MapAsync(reader));
            }
            return entities;
        }

        public async Task<Dictionary<TimeOnly, int>> GetHorarioMaisProcuradoPorMes(int mes)
        {
            try
            {

                if (mes <= 0) throw new InvalidOperationException("O número de dias deve ser maior que zero" + nameof(mes));
                await using var connection = await GetOpenConnectionAsync();
                //Falta filtrar por mês
                string query = $"SELECT TOP 1 " +
                                $"CAST(data_acesso AS DATE) AS data, " +
                                $"COUNT(*) AS total_acessos FROM {TableName}"
                            + $"WHERE MONTH(data_acesso) = @Mes"
                            + $"GROUP BY DATEPART(HOUR, GETDATE()) AS hora"
                            + $"ORDER BY total_acessos DESC";
                await using var commando = DbProvider.CreateCommand(query, connection);
                commando.Parameters.Add(DbProvider.CreateParameter("@Mes", mes, DbType.Int32, _databaseType));
                await using var reader = await commando.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var dateTime = Convert.ToDateTime(reader["hora"]);
                    var resultado = new Dictionary<TimeOnly, int>();
                    resultado.Add(TimeOnly.FromDateTime(dateTime), Convert.ToInt32(reader["totoal_acessos"]));
                    return resultado;
                }
                return null;
            }
            catch (DbException ex) { throw new InvalidOperationException($"Erro ao obter horário mais procurado por mês: {ex.Message}", ex); }
        }

        public Task<Dictionary<int, TimeSpan>> GetPermanenciaMediaPorMes(int mes)
        {
            throw new NotImplementedException();
        }

        public override async Task<Acesso> MapAsync(DbDataReader reader)
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
