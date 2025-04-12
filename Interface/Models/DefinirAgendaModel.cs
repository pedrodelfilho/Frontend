namespace Interface.Models
{
    public class DefinirAgendaModel
    {
        public NovaDisponibilidadeModel NovaDisponibilidade { get; set; }
        public List<DisponibilidadeMedicoListagemModel> Disponibilidades { get; set; } = new();
    }

}
