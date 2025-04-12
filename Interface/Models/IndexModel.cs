using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Interface.Models
{
    public class IndexModel : PageModel
    {
        public string TituloPagina { get; set; } = "Bem-vindo à Health&Med";
        public string Introducao { get; set; } = "A Health&Med é uma startup inovadora no setor de saúde que está transformando a maneira como você cuida da sua saúde.";

        public List<string> Funcionalidades { get; set; } = new()
        {
            "Agendamento online de consultas",
            "Consultas médicas por videoconferência",
            "Acesso prático e rápido ao seu histórico médico"
        };

        public void OnGet()
        {
        }
    }

}
