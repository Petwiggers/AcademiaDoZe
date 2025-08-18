using Academia.Domain.Entities;
namespace AcademiaDoZe.Domain.Repositories
{
    public interface IMatriculaRepository : IRepository<Matricula>
    {
        // Métodos específicos do domínio

        Task<Matricula> ObterPorAluno(int alunoId);

        Task<IEnumerable<Matricula>> ObterAtivas();
        Task<IEnumerable<Matricula>> ObterVencendoEmDias(int dias);
    }
}