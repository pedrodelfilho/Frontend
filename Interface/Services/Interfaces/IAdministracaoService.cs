using Interface.Models;
using Interface.Models.View;

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
        Task<DisponibilidadeMedicoListagemModel> ObterDisponibilidadeId(long idDisponibilidade);
        Task<DisponibilidadeMedicoModel> AdicionarDisponibilidade(DisponibilidadeMedicoModel model);
        Task<List<GerenciarUsuariosModel>> ObterTodosUsuarios();
        Task<List<AgendamentoAprovacaoViewModel>> ObterAgendamentoAprovacao(string email);
    }
}
