using Interface.Models;
using Interface.Models.Enum;
using Interface.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Interface.Controllers
{
    public class AdministracaoController : Controller
    {
        private readonly IAdministracaoService _administracaoService;
        private readonly IConsultaService _consultaService;

        public AdministracaoController(IAdministracaoService administracaoService, IConsultaService consultaService)
        {
            _administracaoService = administracaoService;
            _consultaService = consultaService;
        }

        public async Task<IActionResult> GerenciarUsuarios()
        {
            var usuarios = await _administracaoService.PreencherGridView();
            return View(usuarios);
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> AlterarRole(string email, string novaRole)
        {
            if (string.IsNullOrEmpty(novaRole))
            {
                ModelState.AddModelError("novaRole", "A seleção de uma nova role é obrigatória.");
                return View();
            }
            await _administracaoService.AlterarRoleUsuario(email, novaRole);
            return RedirectToAction(nameof(GerenciarUsuarios));
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> BloquearUsuario(string email)
        {
            await _administracaoService.DesativarUsuario(email);
            return RedirectToAction(nameof(GerenciarUsuarios));
        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> DesbloquearUsuario(string email)
        {
            await _administracaoService.AtivarUsuario(email);
            return RedirectToAction(nameof(GerenciarUsuarios));
        }


        [Authorize(Roles = "Medico")]
        [HttpGet]
        public async Task<IActionResult> DefinirAgenda(bool success = false)
        {

            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                var disponibilidades = await _administracaoService.ObterDisponibilidadeAtual(email ?? string.Empty);

                var model = new DefinirAgendaModel
                {
                    NovaDisponibilidade = new NovaDisponibilidadeModel(),
                    Disponibilidades = disponibilidades
                };

                if(success)
                    ViewBag.MensagemSuccess = "Operação realizada com sucesso!";

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View();
            }

        }


        [Authorize(Roles = "Medico")]
        [HttpPost]
        public async Task<IActionResult> DefinirAgenda(DefinirAgendaModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;

                var novaDisponibilidade = new DisponibilidadeMedicoModel
                {
                    Data = model.NovaDisponibilidade.Data,
                    HoraInicio = model.NovaDisponibilidade.HoraInicio,
                    HoraFim = model.NovaDisponibilidade.HoraFim,
                    Email = email ?? string.Empty,
                };

                if (novaDisponibilidade.Data < DateTime.Now.Date)
                    throw new ArgumentException("Data inválida!");
                if (novaDisponibilidade.HoraInicio >= novaDisponibilidade.HoraFim)
                    throw new ArgumentException("A hora de inicio precisa ser anterior a hora fim!");
                if ((novaDisponibilidade.HoraFim - novaDisponibilidade.HoraInicio).TotalMinutes < 30)
                    throw new ArgumentException("O intervalo minimo é de 30 minutos!");
                if (novaDisponibilidade.Data.Date.Add(novaDisponibilidade.HoraInicio) < DateTime.Now.AddMinutes(30))
                    throw new ArgumentException($"A nova disponibilidade deve ser com hora inicio superior a {DateTime.Now.AddMinutes(30)}");

                await _administracaoService.AdicionarDisponibilidade(novaDisponibilidade);
                
                return RedirectToAction(nameof(DefinirAgenda), new { success = true });
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                model.Disponibilidades = await _administracaoService.ObterDisponibilidadeAtual(User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty);
                return View(model);
            }
        }

        [Authorize(Roles = "Medico")]
        [HttpPost]
        public async Task<IActionResult> ExcluirDisponibilidade(long idDisponibilidade)
        {
            try
            {
                await _administracaoService.RemoverDisponibilidade(idDisponibilidade);
                return RedirectToAction(nameof(DefinirAgenda), new { success = true });
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                var disponibilidades = await _administracaoService.ObterDisponibilidadeAtual(User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty);

                var model = new DefinirAgendaModel
                {
                    NovaDisponibilidade = new NovaDisponibilidadeModel(),
                    Disponibilidades = disponibilidades
                };

                return View(model);
            }
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public async Task<IActionResult> AprovarAgendamento()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var agendamentos = await _administracaoService.ObterAgendamentoAprovacao(email);

            var model = agendamentos;

            return View(model);
        }

        [Authorize(Roles = "Medico")]
        [HttpPost]
        public async Task<IActionResult> Aprovar(long id)
        {
            try
            {
                await _consultaService.AprovarAgendamento(id, StatusConsulta.Autorizado);
                TempData["MensagemSuccess"] = "Agendamento aprovado com sucesso!";
                return RedirectToAction("AprovarAgendamento");

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("AprovarAgendamento");
            }
        }

        [Authorize(Roles = "Medico")]
        [HttpPost]
        public async Task<IActionResult> Negar(long id)
        {
            try
            {
                await _consultaService.CancelarAgendamento(id, StatusConsulta.CanceladoMedico);
                TempData["MensagemSuccess"] = "Agendamento cancelado com sucesso!";
                return RedirectToAction("AprovarAgendamento");
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = ex.Message;
                return RedirectToAction("AprovarAgendamento");
            }
        }
    }
}
