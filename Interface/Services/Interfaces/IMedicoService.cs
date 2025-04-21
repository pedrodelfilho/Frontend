using Interface.Models;
using Interface.Models.Response;

namespace Interface.Services.Interfaces
{
    public interface IMedicoService
    {
        Task<List<EspecialidadeModel>> ObterEspecialidades();
        Task<List<MedicoResponse>> ObterMedicosPorEspecialidade(long idEspecialidade);
    }
}
