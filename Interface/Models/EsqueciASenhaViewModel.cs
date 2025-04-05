using System.ComponentModel.DataAnnotations;

namespace Interface.Models
{
    public class EsqueciASenhaViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} é inválido")]
        public string Email { get; set; }
    }
}
