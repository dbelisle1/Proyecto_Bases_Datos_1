using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Inmuebles.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public IActionResult Unauthorized(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
    }
}
