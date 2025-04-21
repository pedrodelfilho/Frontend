using Interface.Models;
using Interface.Models.Enum;
using Interface.Models.View;
using Interface.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Interface.Controllers
{
    [Authorize]
    public class ConsultaController : Controller
    {
        private readonly IMedicoService _medicoService;
        private readonly IAdministracaoService _administracaoService;
        private readonly IConsultaService _consultaService;

        public ConsultaController(IMedicoService medicoService, IAdministracaoService administracaoService, IConsultaService consultaService)
        {
            _medicoService = medicoService;
            _administracaoService = administracaoService;
            _consultaService = consultaService;
        }

        [HttpPost]
        public async Task<IActionResult> AgendarHorario(AgendamentoRequest request)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var consulta = await _consultaService.AgendarConsulta(request.IdDisponibilidade, request.EmailMedico, email);

                return Json(new
                {
                    success = true,
                    message = "Pré agendamento realizado com sucesso. Aguarde até que o médico confirme a consulta, você receberá um e-mail com a confirmação!",
                    redirectUrl = Url.Action("Agendamento")

                });


            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Ocorreu um erro ao tentar agendar a consulta.",
                    redirectUrl = Url.Action("Agendamento")
                });

            }
        }


        [HttpGet]
        public async Task<IActionResult> Agendamento()
        {
            var especialidades = await _medicoService.ObterEspecialidades();

            var model = new AgendamentoModel
            {
                Especialidades = especialidades.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.DsEspecialidade.ToString()
                }).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ObterMedicosPorEspecialidade(long idEspecialidade)
        {
            var medicos = await _medicoService.ObterMedicosPorEspecialidade(idEspecialidade);

            var result = medicos.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Email,
            });

            return Json(medicos);
        }

        [HttpGet]
        public async Task<IActionResult> ObterHorariosDisponiveis(string email)
        {
            var horarios = await _administracaoService.ObterDisponibilidadeAtual(email);

            return Json(horarios);
        }

        [HttpGet]
        public async Task<IActionResult> MeusAgendamentos()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var agendamentos = await _consultaService.ObterAgendamentosPorUsuario(email);

            var model = agendamentos;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarAgendamento(int id)
        {
            try
            {
                // Cancelar o agendamento
                await _consultaService.CancelarAgendamento(id, StatusConsulta.CanceladoPaciente);

                TempData["MensagemSuccess"] = "Agendamento cancelado com sucesso.";
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ocorreu um erro ao cancelar o agendamento.";
            }

            return RedirectToAction("MeusAgendamentos");
        }

    }
}
