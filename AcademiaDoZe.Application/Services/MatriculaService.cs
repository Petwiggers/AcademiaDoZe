//Peterson Wiggers
using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using AcademiaDoZe.Domain.Repositories;


namespace AcademiaDoZe.Application.Services
{
    public class MatriculaService : IMatriculaService
    {

        private readonly Func<IMatriculaRepository> _repoFactory;
        public MatriculaService(Func<IMatriculaRepository> repoFactory)
        {
            _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
        }
        public async Task<MatriculaDTO> AdicionarAsync(MatriculaDTO matriculaDto)
        {
            var matricula = await _repoFactory().ObterPorAluno(matriculaDto.AlunoMatricula.Id);
            if (matricula == null)
            {
                var matriculaDomain = matriculaDto.ToEntity();
                await _repoFactory().Adicionar(matriculaDomain);
                return matriculaDomain.ToDto();
            }
            throw new InvalidOperationException($"Já existe uma matricula ativa para o aluno {matriculaDto.AlunoMatricula.Nome} ID: {matriculaDto.AlunoMatricula.Id}");
        }

        public async Task<MatriculaDTO> AtualizarAsync(MatriculaDTO matriculaDto)
        {
            var matriculas = await _repoFactory().ObterAtivas(matriculaDto.AlunoMatricula.Id);
            if (matriculas.Any())
            {
                var matricula = matriculaDto.ToEntity();
                await _repoFactory().Atualizar(matricula);
                return matricula.ToDto();
            }
            throw new InvalidOperationException($"Não existe uma matricula ativa para o aluno {matriculaDto.AlunoMatricula.Nome}");
        }

        public async Task<IEnumerable<MatriculaDTO>> ObterAtivasAsync(int alunoId = 0)
        {
            var matriculas = await _repoFactory().ObterAtivas(alunoId);
            if (!matriculas.Any()){ throw new InvalidOperationException($"O Aluno não possui nenhuma matrícula !"); }
            return matriculas.Select(m => m.ToDto());
        }

        public async Task<MatriculaDTO> ObterPorAlunoIdAsync(int alunoId)
        {
            var matricula = await _repoFactory().ObterPorAluno(alunoId);
            if(matricula == null) { throw new KeyNotFoundException($"Matrícula para o Aluno ID {alunoId} não encontrada."); }
            return matricula.ToDto();
        }
        public async Task<MatriculaDTO> ObterPorAlunoCpfAsync(string cpf)
        {
            var matricula = await _repoFactory().ObterPorAlunoCpf(cpf);
            if (matricula == null) { throw new KeyNotFoundException($"Matrícula para o Aluno CPf {cpf} não encontrada."); }
            return matricula.ToDto();
        }

        public async Task<MatriculaDTO> ObterPorIdAsync(int id)
        {
            var matricula = await _repoFactory().ObterPorId(id);
            if (matricula == null) { throw new KeyNotFoundException($"Mátricula com o {id} não encontrada."); }
            return matricula.ToDto();
        }

        public async Task<IEnumerable<MatriculaDTO>> ObterTodasAsync()
        {
            var matriculas = await _repoFactory().ObterTodos();
            if (!matriculas.Any()) { throw new KeyNotFoundException($"Nenhuma mátricula cadastrada !"); }
            return matriculas.Select(m => m.ToDto());
        }

        public async Task<IEnumerable<MatriculaDTO>> ObterVencendoEmDiasAsync(int dias)
        {
            var matriculas =  await _repoFactory().ObterVencendoEmDias(dias);
            if (!matriculas.Any()) { throw new InvalidOperationException($"Nenhuma matrícula vencendo em {dias} dias."); }
            return matriculas.Select(m => m.ToDto());
        }

        public async Task<bool> RemoverAsync(int id)
        {
            bool resultado = await _repoFactory().Remover(id);
            if (!resultado) { throw new KeyNotFoundException($"Matrícula ID {id} não encontrada para remoção."); }
            return resultado;
        }
    }
}