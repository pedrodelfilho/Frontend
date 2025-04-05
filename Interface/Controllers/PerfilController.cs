using Microsoft.AspNetCore.Mvc;
using Interface.Models;
using Interface.Services.Interfaces;

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
        public IActionResult TrocarSenha()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TrocarSenha(AlterarSenhaViewModel model)
        {
            if (ModelState.IsValid)
                service.AlterarSenha(model);
            return View(model); 
        }  
    }
}
