using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Interface.Models
{
    public class SignUpModel
    {
        [Required]
        public string Nome { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }
        [Required, Compare("Senha",ErrorMessage = "A senha e a Confirmação de senha precisam ser iguais")]
        public string ConfirmarSenha { get; set; }
    }
}
