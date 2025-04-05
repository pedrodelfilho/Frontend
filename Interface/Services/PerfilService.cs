using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Interface.Models;
using Interface.Services.Interfaces;

namespace Interface.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly string ENDPOINT = "http://localhost:5153/api/v1/user";
        private readonly HttpClient _httpClient;

        public PerfilService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(ENDPOINT)
            };
        }
        public async Task<string> AlterarSenha(AlterarSenhaViewModel alterarSenhaViewModel)
        {
            try
            {
                string json = JsonConvert.SerializeObject(alterarSenhaViewModel);
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                ByteArrayContent content = new ByteArrayContent(buffer);

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string url = ENDPOINT + "/trocarsenha";
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    return response.StatusCode.ToString();
                }

                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
    }
}
