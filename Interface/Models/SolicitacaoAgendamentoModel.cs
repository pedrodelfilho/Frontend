namespace Interface.Models
{
    public class SolicitacaoAgendamentoModel
    {
        public string IdUsuarioPaciente { get; set; }
        public string IdUsuarioMedico { get; set; }
        public long IdDisponibilidade { get; set; }
        public DateTime Data { get; set; }
    }
}
