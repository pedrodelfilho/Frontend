using Interface.Models;

namespace Interface.Services.Interfaces
{
    public interface IAdministracaoService
    {
        Task<List<GerenciarUsuariosModel>> PreencherGridView();
        Task DesativarUsuario(string email);
        Task AtivarUsuario(string email);
        Task AlterarRoleUsuario(string email, string role);
        Task RemoverDisponibilidade(long idDisponibilidade);
        Task<List<DisponibilidadeMedicoListagemModel>> ObterDisponibilidadeAtual(string email);
        Task<DisponibilidadeMedicoModel> AdicionarDisponibilidade(DisponibilidadeMedicoModel model);
    }
}
