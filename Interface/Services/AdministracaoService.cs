using Interface.Models;
using Interface.Models.Enum;
using Interface.Models.View;
using Interface.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Interface.Services
{
    public class AdministracaoService : IAdministracaoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string ENDPOINT = "https://backendd-dahvaje9b9gjehea.brazilsouth-01.azurewebsites.net/api/v1";
        private readonly string RESOURCE_USER = "/user";
        private readonly string RESOURCE_MEDICO = "/medico";
        private readonly string RESOURCE_CONSULTA = "/consulta";
        private readonly HttpClient _httpClient;

        public AdministracaoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new()
            {
                BaseAddress = new(ENDPOINT)
            };
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DisponibilidadeMedicoModel> AdicionarDisponibilidade(DisponibilidadeMedicoModel model)
        {
            string json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ENDPOINT + RESOURCE_MEDICO + "/adicionardisponibilidade";
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
                return new DisponibilidadeMedicoModel
                {
                    Data = dataElement.GetProperty("data").GetDateTime(),
                    HoraInicio = TimeSpan.Parse(dataElement.GetProperty("horaInicio").ToString()),
                    HoraFim = TimeSpan.Parse(dataElement.GetProperty("horaFim").ToString())
                };
            }

            return null;
        }

        public async Task AlterarRoleUsuario(string email, string role)
        {
            string url = ENDPOINT + RESOURCE_USER + "/alterarfuncaousuario";

            string json = JsonSerializer.Serialize(new { email, role });
            StringContent content = new(json, Encoding.UTF8, "application/json");

            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                using var document = JsonDocument.Parse(responseBody);
                var root = document.RootElement;

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
            await PreencherGridView();
        }

        public async Task AtivarUsuario(string email)
        {
            string url = ENDPOINT + RESOURCE_USER + "/ativarusuario";

            string json = JsonSerializer.Serialize(new { email });
            StringContent content = new(json, Encoding.UTF8, "application/json");

            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                using var document = JsonDocument.Parse(responseBody);
                var root = document.RootElement;

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
            await PreencherGridView();
        }

        public async Task DesativarUsuario(string email)
        {
            string url = ENDPOINT + RESOURCE_USER + "/desativarusuario";

            string json = JsonSerializer.Serialize(new { email });
            StringContent content = new(json, Encoding.UTF8, "application/json");

            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                using var document = JsonDocument.Parse(responseBody);
                var root = document.RootElement;

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
            await PreencherGridView();
        }

        public async Task<List<DisponibilidadeMedicoListagemModel>> ObterDisponibilidadeAtual(string email)
        {
            string url = $"{ENDPOINT + RESOURCE_MEDICO}/obterdisponibilidade?email={email}";
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

            if (root.TryGetProperty("data", out var dataElement))
            {
                var result = new List<DisponibilidadeMedicoListagemModel>();

                foreach (var item in dataElement.EnumerateArray())
                {
                    result.Add(new DisponibilidadeMedicoListagemModel
                    {
                        Id = item.GetProperty("id").ToString(),
                        Data = item.GetProperty("data").GetDateTime(),
                        HoraInicio = TimeSpan.Parse(item.GetProperty("horaInicio").ToString()),
                        HoraFim = TimeSpan.Parse(item.GetProperty("horaFim").ToString())
                    });
                }

                return result;
            }

            return null;
        }

        public async Task RemoverDisponibilidade(long idDisponibilidade)
        {
            string url = $"{ENDPOINT}{RESOURCE_MEDICO}/removerdisponibilidade?idDisponibilidade={idDisponibilidade}";
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync(url);
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

        public async Task<List<GerenciarUsuariosModel>> PreencherGridView()
        {
            return await ObterTodosUsuarios();
        }

        public async Task<List<GerenciarUsuariosModel>> ObterTodosUsuarios()
        {
            string url = ENDPOINT + RESOURCE_USER + "/obterusuarios";

            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(url);
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
                else if (root.TryGetProperty("errors", out var detailElement2))
                {
                    foreach (var property in detailElement2.EnumerateObject())
                    {
                        foreach (var error in property.Value.EnumerateArray())
                        {
                            var detailMessage = error.GetString();
                            throw new Exception(detailMessage);
                        }
                        break;
                    }
                }
                throw new Exception("Erro ao autenticar.");
            }

            var listaUsuarios = new List<GerenciarUsuariosModel>();

            if (root.TryGetProperty("data", out var dataElement))
            {
                foreach (var userElement in dataElement.EnumerateArray())
                {
                    var elemento = userElement.GetRawText();
                    var usuario = JsonSerializer.Deserialize<GerenciarUsuariosModel>(elemento, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (usuario != null)
                        listaUsuarios.Add(usuario);
                }
            }

            return listaUsuarios ?? new List<GerenciarUsuariosModel>();
        }

        public async Task<DisponibilidadeMedicoListagemModel> ObterDisponibilidadeId(long idDisponibilidade)
        {
            string url = $"{ENDPOINT + RESOURCE_MEDICO}/obterdisponibilidadeid?idDisponibilidade={idDisponibilidade}";
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

            if (root.TryGetProperty("data", out var dataElement))
            {

                return new DisponibilidadeMedicoListagemModel
                {
                    Id = dataElement.GetProperty("id").ToString(),
                    Data = dataElement.GetProperty("data").GetDateTime(),
                    HoraInicio = TimeSpan.Parse(dataElement.GetProperty("horaInicio").ToString()),
                    HoraFim = TimeSpan.Parse(dataElement.GetProperty("horaFim").ToString())
                };
            }

            return null;
        }

        public async Task<List<AgendamentoAprovacaoViewModel>> ObterAgendamentoAprovacao(string email)
        {
            string url = $"{ENDPOINT + RESOURCE_CONSULTA}/obterconsultapendente?email={email}";
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

            if (!response.IsSuccessStatusCode)
            {
                if (root.TryGetProperty("detail", out var detailElement))
                {
                    var detailMessage = detailElement.GetString();
                    throw new Exception(detailMessage);
                }
                else if (root.TryGetProperty("errors", out var detailElement2))
                {
                    foreach (var property in detailElement2.EnumerateObject())
                    {
                        foreach (var error in property.Value.EnumerateArray())
                        {
                            var detailMessage = error.GetString();
                            throw new Exception(detailMessage);
                        }
                        break;
                    }
                }
                throw new Exception("Erro ao consultar.");
            }

            var agendamentos = new List<AgendamentoAprovacaoViewModel>();

            if (root.TryGetProperty("data", out var dataElement))
            {
                foreach (var userElement in dataElement.EnumerateArray())
                {
                    var elemento = userElement.GetRawText();
                    var agendamento = JsonSerializer.Deserialize<AgendamentoAprovacaoViewModel>(elemento, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (agendamento != null)
                    {
                        agendamento.StatusDescricao = StatusConsultaHelper.GetEnumDescription((StatusConsulta)Convert.ToByte(agendamento.StatusDescricao));
                        agendamentos.Add(agendamento);
                    }
                }

                return agendamentos;
            }

            return null;
        }
    }
}
