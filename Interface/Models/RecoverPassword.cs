using System.ComponentModel.DataAnnotations;

namespace Interface.Models
{
    public class RecoverPassword
    {
        [Required]
        [StringLength(50, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova senha")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Token { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Id { get; set; }
    }
}
