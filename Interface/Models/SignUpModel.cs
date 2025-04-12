using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Interface.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [RegularExpression(@"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$", ErrorMessage = "CPF inválido")]
        public string Cpf { get; set; }

        public string? Crm { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"), Compare("Senha",ErrorMessage = "A senha e a Confirmação de senha precisam ser iguais")]
        public string ConfirmarSenha { get; set; }
        public bool SouMedico { get; set; }

        [Display(Name = "Especialidade")]
        public long? IdEspecialidade { get; set; }
        public List<SelectListItem> Especialidades { get; set; } = [];
    }
}
