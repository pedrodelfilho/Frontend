using Interface.Models;
using Interface.Services.Interfaces;
using System.Text.Json;

namespace Interface.Services
{
    public class MedicoService : IMedicoService
    {
        private readonly string ENDPOINT = "http://localhost:5153/api/v1/medico";
        private readonly HttpClient _httpClient;

        public MedicoService()
        {
            _httpClient = new()
            {
                BaseAddress = new(ENDPOINT)
            };
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
    }
}
