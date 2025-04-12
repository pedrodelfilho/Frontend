using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Interface.Models
{
    public class DisponibilidadeMedicoModel
    {
        [Required]
        public DateTime Data { get; set; }

        [Required]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        public TimeSpan HoraFim { get; set; }
        public string Email {  get; set; }

        public List<DisponibilidadeMedicoListagemModel> Disponibilidades { get; set; } = new();
    }
    public class DisponibilidadeMedicoListagemModel
    {
        public string Id { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
    }
    public class NovaDisponibilidadeModel
    {
        public DateTime Data { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
    }


}
