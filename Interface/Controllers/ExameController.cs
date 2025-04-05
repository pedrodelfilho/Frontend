using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Interface.Models;
using Interface.Services.Interfaces;

namespace Interface.Controllers
{
    public class ExameController : Controller
    {
        private readonly IExameService service;

        public ExameController(IExameService service)
        {
            this.service = service;
        }
        public IActionResult Aprovacao()
        {
            BreadCrumpModel breadCrumpModel1 = new() { Nome = "Home", Ancora = true, Ativo = false };
            BreadCrumpModel breadCrumpModel2 = new() { Nome = "Exame", Ancora = false, Ativo = false };
            BreadCrumpModel breadCrumpModel3 = new() { Nome = "Aprovação de Exames", Ancora = false, Ativo = true };
            List<BreadCrumpModel> breadCrumpModels = new()
            {
                    breadCrumpModel1,
                    breadCrumpModel2,
                    breadCrumpModel3
                };
            AprovacaoModel model = new()
            {
                BreadCrumps = breadCrumpModels
            };
            return View(model);
        }
        public IActionResult Agendamento()
        {
            BreadCrumpModel breadCrumpModel1 = new() { Nome = "Home", Ancora = true, Ativo = false };
            BreadCrumpModel breadCrumpModel2 = new() { Nome = "Exame", Ancora = false, Ativo = false };
            BreadCrumpModel breadCrumpModel3 = new() { Nome = "Agendamento de Exames", Ancora = false, Ativo = true };
            List<BreadCrumpModel> breadCrumpModels = new()
            {
                    breadCrumpModel1,
                    breadCrumpModel2,
                    breadCrumpModel3
                };
            AgendamentoModel model = new()
            {
                BreadCrumps = breadCrumpModels
            };
            return View(model);
        }
        public async Task<IActionResult> Solicitacao()
        {
            BreadCrumpModel breadCrumpModel1 = new() { Nome = "Home", Ancora = true, Ativo = false };
            BreadCrumpModel breadCrumpModel2 = new() { Nome = "Exame", Ancora = false, Ativo = false };
            BreadCrumpModel breadCrumpModel3 = new() { Nome = "Solicitação de Exame", Ancora = false, Ativo = true };
            List<BreadCrumpModel> breadCrumpModels = new()
            {
                breadCrumpModel1,
                breadCrumpModel2,
                breadCrumpModel3
            };
            var paciente = service.BuscarPaciente();
            List<ConvenioModel> listaConvenios = await service.BuscarConveniosAsync();
            listaConvenios.Insert(0, new() { Id = 0, Nome = "[Selecione...]" });
            var convenios = listaConvenios.Select(item => new SelectListItem(item.Nome, item.Id.ToString(), item.Id == 0)).ToList();
            SolicitacaoModel model = new()
            {
                BreadCrumps = breadCrumpModels,
                DataSolicitacao = DateTime.Now.ToString("dd/MM/yyyy - HH:mm"),
                Nome = "Fulando de Tal",
                Convenios = convenios
            };
            return View(model);
        }
    }
}
