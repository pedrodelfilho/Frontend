using Microsoft.AspNetCore.Mvc.Rendering;

namespace Interface.Models
{
    public class SolicitacaoModel
    {
        public List<BreadCrumpModel> BreadCrumps { get; set; }
        public string DataSolicitacao { get; internal set; }
        public string Nome { get; internal set; }
        public List<SelectListItem> Convenios { get; internal set; }
    }
}