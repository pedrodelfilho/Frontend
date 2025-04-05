using System.ComponentModel.DataAnnotations;

namespace Interface.Models
{
    public class AlterarSenhaViewModel
    {
        [Required, EmailAddress]        
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string SenhaAntiga { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NovaSenha { get; set; }
    }
}
