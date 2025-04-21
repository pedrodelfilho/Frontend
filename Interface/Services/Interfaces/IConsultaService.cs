using Interface.Models;
using Interface.Models.Enum;
using Interface.Models.Response;
using Interface.Models.View;

namespace Interface.Services.Interfaces
{
    public interface IConsultaService
    {
        Task<ConsultaResponse> AgendarConsulta(long idDisponibilidade, string emailMedico, string emailPaciente);
        Task<List<AgendamentoViewModel>> ObterAgendamentosPorUsuario(string email);
        Task<AgendamentoViewModel> ObterAgendamentosPorId(long idConsulta);
        Task<ConsultaResponse> CancelarAgendamento(long id, StatusConsulta statusConsulta);
        Task<ConsultaResponse> AprovarAgendamento(long id, StatusConsulta statusConsulta);
        Task EnviarEmail(EmailModel emailModel);
    }
}
