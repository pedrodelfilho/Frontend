using System.ComponentModel.DataAnnotations;

namespace Interface.Models.Response
{
    public class ConsultaResponse
    {
        public long Id { get; set; }
        public string IdUsuarioPaciente { get; set; }

        public string IdUsuarioMedico { get; set; }

        public long IdDisponibilidade { get; set; }

        public long IdStatus { get; set; }

    }
}
