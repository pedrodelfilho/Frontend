using Interface.Models;
using Interface.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interface.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly IAutenticacaoService service;

        public AutenticacaoController(IAutenticacaoService service)
        {
            this.service = service;
        }
        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await service.LoginAsync(model);

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ViewBag.MensagemErro = ex.Message;
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<Object> EsqueciaSenha(EsqueciASenhaViewModel esqueciaSenhaViewModel)
        {
            return await service.EsqueciASenha(esqueciaSenhaViewModel);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await service.CadastrarUsuarioAsync(model);
                    return View("RegisterConfirmation", $"Confirmação de cadastro enviado para o e-mail {model.Email}.");
                }
                catch (Exception ex)
                {
                    ViewBag.MensagemErro = ex.Message;
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult RecoverPassword()
        {
            HttpContext.Session.SetString("SessionToken", Request.Query["Token"]);
            HttpContext.Session.SetString("SessionId", Request.Query["userid"]);

            return View();
        }

        [HttpPost]
        public IActionResult RecoverPassword(RecoverPassword model)
        {

            model.Token = HttpContext.Session.GetString("SessionToken");
            model.Id = HttpContext.Session.GetString("SessionId");

            service.RecoverPassword(model);

            return View("RegisterConfirmation", "Sua senha foi alterada com sucesso! Efetuar um novo login.");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmarEmail(ConfirmarEmailModel confirmarEmailModel)
        {
            var result = await service.ConfirmarEmail(confirmarEmailModel);

            if (result)
                return View("RegisterConfirmation", "Seja bem vindo(a), obrigado pela confirmação do cadastro.");
            return View("RegisterConfirmation", "Opss... Ocorreu um erro durante a confirmação.");
        }

        public IActionResult AccessDenied()
        {
            return Redirect("https://google.com");
        }
    }
}
