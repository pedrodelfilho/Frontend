namespace Interface.Models
{
    public class GerenciarUsuariosModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public long? IdEspecialidade { get; set; }
        public bool Bloqueado { get; set; }
    }
}
