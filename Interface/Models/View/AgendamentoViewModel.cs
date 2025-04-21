namespace Interface.Models.View
{
    public class AgendamentoViewModel
    {
        public long Id { get; set; }
        public string MedicoNome { get; set; }
        public string Especialidade { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFim { get; set; }
        public string Status { get; set; }
    }
}
