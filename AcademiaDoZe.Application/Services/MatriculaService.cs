using AcademiaDoZe.Application.DTOs;
using AcademiaDoZe.Application.Interfaces;
using AcademiaDoZe.Application.Mappings;
using AcademiaDoZe.Domain.Repositories;
using AcademiaDoZe.Application.Security;

namespace AcademiaDoZe.Application.Services
{
    public class MatriculaService : IMatriculaService
    {
        private readonly Func<IMatriculaService> _repoFactory;
        public MatriculaService(Func<IMatriculaService> repoFactory)
        {
            _repoFactory = repoFactory ?? throw new ArgumentNullException(nameof(repoFactory));
        }

        public Task<MatriculaDTO> AdicionarAsync(MatriculaDTO matriculaDto)
        {
            throw new NotImplementedException();
        }

        public Task<MatriculaDTO> AtualizarAsync(MatriculaDTO matriculaDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MatriculaDTO>> ObterAtivasAsync(int alunoId = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MatriculaDTO>> ObterPorAlunoIdAsync(int alunoId)
        {
            throw new NotImplementedException();
        }

        public Task<MatriculaDTO> ObterPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MatriculaDTO>> ObterTodasAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MatriculaDTO>> ObterVencendoEmDiasAsync(int dias)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoverAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}