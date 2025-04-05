using System.ComponentModel.DataAnnotations;

namespace Interface.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email deve ser preenchido!"), EmailAddress(ErrorMessage = "Preencha com um email válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Senha deve ser preenchida")]
        public string Senha { get; set; }
    }
}
