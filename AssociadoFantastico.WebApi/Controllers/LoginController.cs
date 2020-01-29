using AssociadoFantastico.WebApi.Authentication.Services;
using AssociadoFantastico.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssociadoFantastico.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<AuthInfoViewModel> Login(AuthInfoViewModel usuario) =>
            _loginService.Login(usuario.CPF, usuario.Matricula);
    }
}
