using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Interface.Models
{
    public class AgendamentoModel
    {
        [Display(Name = "Especialidade")]
        public long? IdEspecialidade { get; set; }
        public string? IdMedico { get; set; }
        public long IdDisponibilidade { get; set; }

        public List<SelectListItem> Especialidades { get; set; } = [];
        public List<SelectListItem> Medicos { get; set; } = new();
        public List<string> Disponibilidades { get; set; } = new();
    }
}