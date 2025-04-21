using Interface.Models;
using Interface.Models.Response;
using Interface.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Interface.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly string ENDPOINT = "http://localhost:5153/api/v1/medico";
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MedicoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new()
            {
                BaseAddress = new(ENDPOINT)
            };
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<EspecialidadeModel>> ObterEspecialidades()
        {
            string url = ENDPOINT + "/obterespecialidades";
            var response = await _httpClient.GetAsync(url);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return null;

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            var listaEspecialidades = new List<EspecialidadeModel>();

            if (root.TryGetProperty("data", out var dataElement))
            {
                foreach (var userElement in dataElement.EnumerateArray())
                {
                    var elemento = userElement.GetRawText();
                    var usuario = JsonSerializer.Deserialize<EspecialidadeModel>(elemento, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    if (usuario != null)
                        listaEspecialidades.Add(usuario);
                }
            }

            return listaEspecialidades ?? [];
        }

        public async Task<List<MedicoResponse>> ObterMedicosPorEspecialidade(long idEspecialidade)
        {
            string url = $"{ENDPOINT}/obtermedicoespecialidade?idEspecialidade={idEspecialidade}";
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
                var result = new List<MedicoResponse>();

                foreach (var item in dataElement.EnumerateArray())
                {
                    result.Add(new MedicoResponse
                    {
                        Id = item.GetProperty("id").GetString(),
                        NomeCompleto = item.GetProperty("nomeCompleto").GetString(),
                        Email = item.GetProperty("userName").GetString(),
                    });
                }

                return result;
            }

            return null;
        }

    }
}
