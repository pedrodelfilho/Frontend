using Interface.Models;

namespace Interface.Services.Interfaces
{
    public interface IMedicoService
    {
        Task<List<EspecialidadeModel>> ObterEspecialidades();
    }
}
