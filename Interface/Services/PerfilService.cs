using Interface.Models.View;
using Interface.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Interface.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string ENDPOINT = "http://localhost:5153/api/v1/user";
        private readonly HttpClient _httpClient;

        public PerfilService(IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ENDPOINT)
            };
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<string> AlterarSenha(ChangePasswordViewModel alterarSenhaViewModel)
        {
            string json = JsonConvert.SerializeObject(alterarSenhaViewModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string url = ENDPOINT + "/trocarsenha";
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                throw new Exception("Token não encontrado. Faça login novamente.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(url, content);
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

            return response.StatusCode.ToString();
        }


    }
}
