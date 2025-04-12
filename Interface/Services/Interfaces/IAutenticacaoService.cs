using Interface.Models;
using Interface.Models.Response;

namespace Interface.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<UsuarioLoginResponse> LoginAsync(LoginModel model);
        Task<Dictionary<string,string>> EsqueciASenha(EsqueciASenhaViewModel model);
        Task<bool> ConfirmarEmail(ConfirmarEmailModel model);
        Task<Dictionary<string, string>> CadastrarUsuarioAsync(SignUpModel model);
        Task<Dictionary<string, string>> RecoverPassword(RecoverPassword model);
    }
}