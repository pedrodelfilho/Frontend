using Interface.Models;
using Interface.Services.Interfaces;

namespace Interface.Services
{
    public class ExameService : IExameService
    {
        public async Task<List<ConvenioModel>> BuscarConveniosAsync()
        {

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5153/api/v1/convenio/obtertodos");
            request.Headers.Add("Accept", "text/plain");
            request.Headers.Add("Authorization", "{{apiKey}}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());


            List<ConvenioModel> lista = new()
            {
                new(){Id = 1, Nome = "Convênio 1"},
                new(){Id = 2, Nome = "Convênio 2"},
                new(){Id = 3, Nome = "Convênio 3"},
                new(){Id = 4, Nome = "Convênio 4"}
            };
            return lista;
        }

        public PacienteModel BuscarPaciente()
        {
            return new()
            {
                Id = 0,
                Nome = "Fulano de Tal"
            };
        }
    }
}
