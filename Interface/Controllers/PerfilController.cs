using Microsoft.AspNetCore.Mvc;
using Interface.Models;
using Interface.Services.Interfaces;
using Interface.Models.View;
using System.Security.Claims;

namespace Interface.Controllers
{
    public class PerfilController : Controller
    {
        private readonly IPerfilService service;

        public PerfilController(IPerfilService service)
        {
            this.service = service;
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    model.Email = User.FindFirst(ClaimTypes.Email)?.Value;

                    await service.AlterarSenha(model);
                    ViewBag.MensagemSuccess = "Senha alterada com sucesso";
                }
                catch (Exception ex)
                {
                    ViewBag.MensagemErro = ex.Message;
                }
            }
            return View(model); 
        }  
    }
}
