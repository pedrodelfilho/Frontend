using Interface.Models;
using Interface.Models.View;

namespace Interface.Services.Interfaces
{
    public interface IPerfilService
    {
        Task<string> AlterarSenha(ChangePasswordViewModel alterarSenhaViewModel);
    }
}
