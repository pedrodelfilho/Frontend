using Interface.Models;

namespace Interface.Services.Interfaces
{
    public interface IExameService
    {
        Task<List<ConvenioModel>> BuscarConveniosAsync();
        PacienteModel BuscarPaciente();
    }
}