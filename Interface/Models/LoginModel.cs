using System.ComponentModel.DataAnnotations;

namespace Interface.Models
{
    public class LoginModel
    {
        public string TipoLogin { get; set; }

        [Display(Name = "Email ou CPF")]
        public string? EmailOuCpf { get; set; }
        public string? Email { get; set; }

        [Display(Name = "CRM")]
        public string? Crm { get; set; }

        [Required(ErrorMessage = "Senha deve ser preenchida")]
        public string Senha { get; set; }
    }
}
