using System.ComponentModel.DataAnnotations.Schema;

namespace Interface.Models.Response
{
    public class MedicoResponse
    {
        public string? Id { get; set; }
        public string? NomeCompleto { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Crm { get; set; }
        public string Cpf { get; set; }
        public long? IdEspecialidade { get; set; }
        public string? Email { get; set; }
        public ICollection<NovaDisponibilidadeModel> Disponibilidades { get; set; }
    }
}
