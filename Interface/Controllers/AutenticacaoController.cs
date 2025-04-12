using Interface.Models;
using Interface.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Interface.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly IAutenticacaoService _service;
        private readonly IMedicoService _mediicoService;

        public AutenticacaoController(IAutenticacaoService service, IMedicoService medicoService)
        {
            _service = service;
            _mediicoService = medicoService;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var tokenResponse = await _service.LoginAsync(model);

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenResponse.AccessToken);

                var identity = new ClaimsIdentity(token.Claims, "Interface");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Interface", principal);

                HttpContext.Response.Cookies.Append("AccessToken", tokenResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddMinutes(10)
                });


                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.MensagemErro = ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<Object> EsqueciaSenha(EsqueciASenhaViewModel esqueciaSenhaViewModel)
        {
            return await _service.EsqueciASenha(esqueciaSenhaViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> SignUp()
        {
            var especialidades = await _mediicoService.ObterEspecialidades(); // <- await aqui

            var model = new SignUpModel
            {
                Especialidades = especialidades.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.DsEspecialidade.ToString()
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _service.CadastrarUsuarioAsync(model);
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

        _service.RecoverPassword(model);

            return View("RegisterConfirmation", "Sua senha foi alterada com sucesso! Efetuar um novo login.");
        }
        [HttpGet]
        public async Task<IActionResult> ConfirmarEmail(ConfirmarEmailModel confirmarEmailModel)
        {
            var result = await _service.ConfirmarEmail(confirmarEmailModel);

            if (result)
                return View("RegisterConfirmation", "Seja bem vindo(a), obrigado pela confirmação do cadastro.");
            return View("RegisterConfirmation", "Opss... Ocorreu um erro durante a confirmação.");
        }

        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Interface");
            return RedirectToAction("Login", "Autenticacao");
        }
    }
}
