namespace Interface.Models.View
{
    public class AgendamentoAprovacaoViewModel
    {
        public long Id { get; set; }
        public string PacienteNome { get; set; }
        public string MedicoNome { get; set; }
        public string Especialidade { get; set; }
        public string Data { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFim { get; set; }
        public string StatusDescricao { get; set; }
    }
}
