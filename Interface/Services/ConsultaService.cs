using Interface.Models;
using Interface.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Interface.Models.Response;
using Interface.Models.View;
using Interface.Models.Enum;
using System.Reflection;

namespace Interface.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly string ENDPOINT = "https://backendd-dahvaje9b9gjehea.brazilsouth-01.azurewebsites.net/api/v1/consulta";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        private readonly IAdministracaoService _administracaoService;
        private readonly IMedicoService _mediicoService;

        public ConsultaService(IHttpContextAccessor httpContextAccessor, IAdministracaoService administracaoService, IMedicoService mediicoService)
        {
            _httpClient = new()
            {
                BaseAddress = new(ENDPOINT)
            };
            _httpContextAccessor = httpContextAccessor;
            _administracaoService = administracaoService;
            _mediicoService = mediicoService;
        }

        public async Task<ConsultaResponse> AgendarConsulta(long idDisponibilidade, string emailMedico, string emailPaciente)
        {
            var model = new { EmailPaciente = emailPaciente, IdDisponibilidade = idDisponibilidade, EmailMedico = emailMedico, StatusConsulta = 0 };
            string json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ENDPOINT + "/cadastrar";
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            if (!response.IsSuccessStatusCode)
            {
                if (root.TryGetProperty("detail", out var detailElement))
                {
                    var detailMessage = detailElement.GetString();
                    throw new Exception(detailMessage);
                }
                else if (root.TryGetProperty("errors", out var errorsElement))
                {
                    foreach (var error in errorsElement.EnumerateObject())
                    {
                        foreach (var errorDetail in error.Value.EnumerateArray())
                        {
                            var errorMessage = errorDetail.GetString();
                            throw new Exception(errorMessage);
                        }
                        break;
                    }
                }
                else
                {
                    throw new Exception("Erro desconhecido.");
                }
            }
            if (root.TryGetProperty("data", out var dataElement))
            {
                var mailPaciente = new EmailModel { Assunto = "Solicitação de agendamento de consulta", EmailDestino1 = emailPaciente, Body = "Sua solicitação de agendamento de consulta foi realizado com sucesso. Aguarde até que o médico aprove a consulta. Vamos te informar por e-mail. Obrigado" };
                var mailMedico = new EmailModel { Assunto = "Solicitação de agendamento de consulta", EmailDestino1 = emailMedico, Body = "Uma nova consulta foi agendada e esta aguardando por aprovação." };
                await EnviarEmail(mailPaciente);
                await EnviarEmail(mailMedico);

                return new ConsultaResponse
                {
                    Id = dataElement.GetProperty("id").GetInt64(),
                    IdUsuarioPaciente = dataElement.GetProperty("idUsuarioPaciente").ToString(),
                    IdUsuarioMedico = dataElement.GetProperty("idUsuarioMedico").ToString(),
                    IdDisponibilidade = dataElement.GetProperty("idDisponibilidade").GetInt64(),
                    IdStatus = dataElement.GetProperty("idStatus").GetInt64(),
                };
            }
            else
                return null;
        }

        public async Task<ConsultaResponse> AprovarAgendamento(long id, StatusConsulta statusConsulta)
        {
            var usuarios = await _administracaoService.ObterTodosUsuarios();
            var model = new { IdConsulta = id, StatusConsulta = (byte)statusConsulta };
            string json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ENDPOINT + "/atualizarstatus";
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PutAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            if (!response.IsSuccessStatusCode)
            {
                if (root.TryGetProperty("detail", out var detailElement))
                {
                    var detailMessage = detailElement.GetString();
                    throw new Exception(detailMessage);
                }
                else if (root.TryGetProperty("errors", out var errorsElement))
                {
                    foreach (var error in errorsElement.EnumerateObject())
                    {
                        foreach (var errorDetail in error.Value.EnumerateArray())
                        {
                            var errorMessage = errorDetail.GetString();
                            throw new Exception(errorMessage);
                        }
                        break;
                    }
                }
                else
                {
                    throw new Exception("Erro desconhecido.");
                }
            }
            if (root.TryGetProperty("data", out var dataElement))
            {
                var emailPaciente = usuarios.Where(x => x.Id == dataElement.GetProperty("idUsuarioPaciente").ToString()).FirstOrDefault().Email;
                var emailMedico = usuarios.Where(x => x.Id == dataElement.GetProperty("idUsuarioMedico").ToString()).FirstOrDefault().Email;
                var mailPaciente = new EmailModel { Assunto = "Solicitação de agendamento de consulta", EmailDestino1 = emailPaciente, Body = "Sua solicitação de agendamento de consulta foi aprovada com sucesso. Obrigado!" };
                var mailMedico = new EmailModel { Assunto = "Solicitação de agendamento de consulta", EmailDestino1 = emailMedico, Body = "Uma nova consulta foi aprovada!" };
                await EnviarEmail(mailPaciente);
                await EnviarEmail(mailMedico);

                return new ConsultaResponse
                {
                    Id = dataElement.GetProperty("id").GetInt64(),
                    IdUsuarioPaciente = dataElement.GetProperty("idUsuarioPaciente").ToString(),
                    IdUsuarioMedico = dataElement.GetProperty("idUsuarioMedico").ToString(),
                    IdDisponibilidade = dataElement.GetProperty("idDisponibilidade").GetInt64(),
                    IdStatus = dataElement.GetProperty("idStatus").GetInt64(),
                };
            }
            else
                return null;
        }

        public async Task<ConsultaResponse> CancelarAgendamento(long id, StatusConsulta statusConsulta)
        {
            var usuarios = await _administracaoService.ObterTodosUsuarios();
            var model = new { IdConsulta = id, StatusConsulta = (byte)statusConsulta};
            string json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ENDPOINT + "/atualizarstatus";
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PutAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            if (!response.IsSuccessStatusCode)
            {
                if (root.TryGetProperty("detail", out var detailElement))
                {
                    var detailMessage = detailElement.GetString();
                    throw new Exception(detailMessage);
                }
                else if (root.TryGetProperty("errors", out var errorsElement))
                {
                    foreach (var error in errorsElement.EnumerateObject())
                    {
                        foreach (var errorDetail in error.Value.EnumerateArray())
                        {
                            var errorMessage = errorDetail.GetString();
                            throw new Exception(errorMessage);
                        }
                        break;
                    }
                }
                else
                {
                    throw new Exception("Erro desconhecido.");
                }
            }
            if (root.TryGetProperty("data", out var dataElement))
            {
                var emailPaciente = usuarios.Where(x => x.Id == dataElement.GetProperty("idUsuarioPaciente").ToString()).FirstOrDefault().Email;
                var emailMedico = usuarios.Where(x => x.Id == dataElement.GetProperty("idUsuarioMedico").ToString()).FirstOrDefault().Email;
                var mailPaciente = new EmailModel { Assunto = "Cancelamento de agendamento de consulta", EmailDestino1 = emailPaciente, Body = "Sua solicitação de agendamento de consulta foi cancelada. Obrigado!" };
                var mailMedico = new EmailModel { Assunto = "Cancelamento de agendamento de consulta", EmailDestino1 = emailMedico, Body = "Uma nova consulta foi cancelada!" };
                await EnviarEmail(mailPaciente);
                await EnviarEmail(mailMedico);

                return new ConsultaResponse
                {
                    Id = dataElement.GetProperty("id").GetInt64(),
                    IdUsuarioPaciente = dataElement.GetProperty("idUsuarioPaciente").ToString(),
                    IdUsuarioMedico = dataElement.GetProperty("idUsuarioMedico").ToString(),
                    IdDisponibilidade = dataElement.GetProperty("idDisponibilidade").GetInt64(),
                    IdStatus = dataElement.GetProperty("idStatus").GetInt64(),
                };
            }
            else
                return null;

        }

        public async Task EnviarEmail(EmailModel emailModel)
        {
            string json = JsonConvert.SerializeObject(emailModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ENDPOINT + "/email";
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            if (!response.IsSuccessStatusCode)
            {
                if (root.TryGetProperty("detail", out var detailElement))
                {
                    var detailMessage = detailElement.GetString();
                    throw new Exception(detailMessage);
                }
                else if (root.TryGetProperty("errors", out var errorsElement))
                {
                    foreach (var error in errorsElement.EnumerateObject())
                    {
                        foreach (var errorDetail in error.Value.EnumerateArray())
                        {
                            var errorMessage = errorDetail.GetString();
                            throw new Exception(errorMessage);
                        }
                        break;
                    }
                }
                else
                {
                    throw new Exception("Erro desconhecido.");
                }
            }
        }

        public async Task<AgendamentoViewModel> ObterAgendamentosPorId(long idConsulta)
        {
            string url = $"{ENDPOINT}/obterid?id={idConsulta}";
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;
            var usuarios = await _administracaoService.ObterTodosUsuarios();
            var especialidades = await _mediicoService.ObterEspecialidades();

            if (root.TryGetProperty("data", out var dataElement))
            {
                var idDisponibilidade = dataElement.GetProperty("idDisponibilidade").GetInt64();
                var disponibilidade = await _administracaoService.ObterDisponibilidadeId(idDisponibilidade);
                var idMedico = dataElement.GetProperty("idUsuarioMedico").ToString();
                var idEspecialidade = usuarios.Where(x => x.Id == idMedico).FirstOrDefault().IdEspecialidade;

                int idStatus = dataElement.GetProperty("idStatus").GetInt32();
                StatusConsulta status = (StatusConsulta)idStatus;
                string descricao = StatusConsultaHelper.GetEnumDescription(status);

                return new AgendamentoViewModel
                {
                    Id = dataElement.GetProperty("id").GetInt64(),
                    MedicoNome = usuarios.Where(x => x.Id == idMedico).FirstOrDefault().Nome,
                    Especialidade = especialidades.Where(x => x.Id == idEspecialidade).FirstOrDefault().DsEspecialidade,
                    Data = disponibilidade.Data,
                    HoraInicio = TimeSpan.Parse(disponibilidade.HoraInicio.ToString()),
                    HoraFim = TimeSpan.Parse(disponibilidade.HoraFim.ToString()),
                    Status = descricao
                };
            }

            return null;
        }

        public async Task<List<AgendamentoViewModel>> ObterAgendamentosPorUsuario(string email)
        {
            string url = $"{ENDPOINT}/obter?email={email}";
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;
            var usuarios = await _administracaoService.ObterTodosUsuarios();
            var especialidades = await _mediicoService.ObterEspecialidades();

            if (root.TryGetProperty("data", out var dataElement))
            {
                var result = new List<AgendamentoViewModel>();

                foreach (var item in dataElement.EnumerateArray())
                {
                    var idDisponibilidade = item.GetProperty("idDisponibilidade").GetInt64();
                    var disponibilidade = await _administracaoService.ObterDisponibilidadeId(idDisponibilidade);
                    var idMedico = item.GetProperty("idUsuarioMedico").ToString();
                    var idEspecialidade = usuarios.Where(x => x.Id == idMedico).FirstOrDefault().IdEspecialidade;

                    int idStatus = item.GetProperty("idStatus").GetInt32();
                    StatusConsulta status = (StatusConsulta)idStatus;
                    string descricao = StatusConsultaHelper.GetEnumDescription(status);
                    result.Add(new AgendamentoViewModel
                    {
                        Id = item.GetProperty("id").GetInt64(),
                        MedicoNome = usuarios.Where(x => x.Id == idMedico).FirstOrDefault().Nome,
                        Especialidade = especialidades.Where(x => x.Id == idEspecialidade).FirstOrDefault().DsEspecialidade,
                        Data = disponibilidade.Data,
                        HoraInicio = TimeSpan.Parse(disponibilidade.HoraInicio.ToString()),
                        HoraFim = TimeSpan.Parse(disponibilidade.HoraFim.ToString()),
                        Status = descricao
                    });
                }

                return result;
            }

            return null;
        }

    }
}
