using Interface.Models;

namespace Interface.Services.Interfaces
{
    public interface IPerfilService
    {
        Task<string> AlterarSenha(AlterarSenhaViewModel alterarSenhaViewModel);
    }
}
