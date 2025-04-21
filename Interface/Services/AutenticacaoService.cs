using Interface.Models;
using Interface.Models.Response;
using Interface.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using JsonSerializer = System.Text.Json.JsonSerializer;

internal class AutenticacaoService : IAutenticacaoService
{
    private readonly string ENDPOINT = "http://localhost:5153/api/v1/user";
    private readonly HttpClient _httpClient;

    public AutenticacaoService()
    {
        _httpClient = new()
        {
            BaseAddress = new(ENDPOINT)
        };
    }
    public async Task<UsuarioResponse> LoginAsync(LoginModel model)
    {
        model.Email = model.EmailOuCpf;

        if (model.TipoLogin == "paciente" && !IsEmail(model.EmailOuCpf ?? string.Empty))
            model.Email = await ObterUsuarioAsync(model.EmailOuCpf ?? string.Empty);
        else if (model.TipoLogin == "medico")
            model.Email = await ObterUsuarioAsync(model.Crm ?? string.Empty);

        string contentJson = JsonSerializer.Serialize(model);

        var request = new HttpRequestMessage(HttpMethod.Post, ENDPOINT + "/login");
        request.Headers.Add("Accept", "text/plain");

        request.Content = new StringContent(contentJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
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
            else
            {
                throw new Exception("Erro desconhecido ao autenticar.");
            }
        }
        else
        {
            var tokenResponse = JsonSerializer.Deserialize<UsuarioResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tokenResponse == null || !tokenResponse.Sucesso)
                throw new Exception("Erro ao fazer login");
            return tokenResponse;
        }
    }
    private async Task<string?> ObterUsuarioAsync(string cpfOuCrm)
    {
        var response = await _httpClient.GetAsync($"{ENDPOINT}/obterusuario?crmOuCpf={cpfOuCrm}");
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return null;

        using var document = JsonDocument.Parse(responseBody);
        var root = document.RootElement;

        if (root.TryGetProperty("data", out var dataElement) &&
            dataElement.TryGetProperty("email", out var emailElement))
        {
            return emailElement.GetString();
        }

        return null;
    }
    public async Task<bool> ConfirmarEmail(ConfirmarEmailModel model)
    {
        string url = ENDPOINT + "/confirmaremail";

        string json = JsonConvert.SerializeObject(model);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
            return true;
        return false;
    }
    public async Task<Dictionary<string, string>> EsqueciASenha(EsqueciASenhaViewModel model)
    {
        string json = JsonConvert.SerializeObject(model);
        StringContent content = new(json, Encoding.UTF8, "application/json");

        MediaTypeHeaderValue mediaTypeHeaderValue = new("application/json");
        content.Headers.ContentType = mediaTypeHeaderValue;

        string url = ENDPOINT + "/esquecisenha";
        HttpResponseMessage response = await _httpClient.PostAsync(url, content);


        if (response.IsSuccessStatusCode)
        {
            Dictionary<string, string> resultado = new()
            {
                ["title"] = "Sucesso",
                ["message"] = $"Link para recuperação de senha enviado para o e-mail {model.Email}"
            };
            return resultado;
        }
        else
        {
            var responseBody = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            if (root.TryGetProperty("detail", out var detailElement))
            {
                var detailMessage = detailElement.GetString();
                throw new Exception(detailMessage);
            }
            else
            {
                throw new Exception("Erro desconhecido ao autenticar.");
            }
        }
    }
    public async Task<Dictionary<string, string>> CadastrarUsuarioAsync(SignUpModel model)
    {
        string json = JsonConvert.SerializeObject(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        string url = ENDPOINT + "/cadastro";
        HttpResponseMessage response = await _httpClient.PostAsync(url, content);

        Dictionary<string, string> resultado = new();

        if (response.IsSuccessStatusCode)
        {
            resultado["title"] = "Sucesso";
            resultado["message"] = "Usuário cadastrado com sucesso!";
        }
        else
        {
            var responseBody = await response.Content.ReadAsStringAsync();

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
            else
            {
                throw new Exception("Erro desconhecido ao autenticar.");
            }
        }

        return resultado;
    }
    public async Task<Dictionary<string, string>> RecoverPassword(RecoverPassword model)
    {
        try
        {
            string json = JsonConvert.SerializeObject(model);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            string url = ENDPOINT + "/resetarsenha";
            HttpResponseMessage response = await _httpClient.PutAsync(url, content);

            Dictionary<string, string> objeto = new();
            string titulo = "";
            string mensagem = "";


            if (response.IsSuccessStatusCode)
            {
                titulo = "Sucesso";
                mensagem = "Usuário cadastrado com Sucesso!";
            }
            else
            {
                titulo = "Erro";
                mensagem = response.RequestMessage == null ? "" : response.RequestMessage.ToString();
            }


            return objeto;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public static bool IsEmail(string valor)
    {
        return Regex.IsMatch(valor ?? "", @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}