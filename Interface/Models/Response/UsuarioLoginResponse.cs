using System.Text.Json.Serialization;

namespace Interface.Models.Response
{
    public class UsuarioLoginResponse
    {
        public bool Sucesso => Erros.Count == 0;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string AccessToken { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RefreshToken { get; set; }

        public List<string> Erros { get; set; } = new List<string>();

        public UsuarioLoginResponse() { }

        public UsuarioLoginResponse(bool sucesso, string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public void AdicionarErro(string erro) => Erros.Add(erro);

        public void AdicionarErros(IEnumerable<string> erros) => Erros.AddRange(erros);
    }

}
