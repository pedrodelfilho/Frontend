using System.ComponentModel.DataAnnotations;

namespace Interface.Models.View
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "O campo Senha atual é obrigatório")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "O campo Nova senha é obrigatório")]
        public string NovaSenha { get; set; }


        [Required(ErrorMessage = "O campo Confirmar nova senha é obrigatório"), Compare("NovaSenha", ErrorMessage = "A senha e a Confirmação de senha precisam ser iguais")]
        public string ConfirmarNovaSenha { get; set; }

        public string? Email { get; set; }
    }
}
