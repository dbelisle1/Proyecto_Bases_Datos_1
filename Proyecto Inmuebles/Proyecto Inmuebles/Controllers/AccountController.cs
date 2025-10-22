using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Inmuebles.Models;
using System.Security.Claims;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Queries;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Inmuebles.Parser;
using Oracle.ManagedDataAccess.Client;
using Microsoft.AspNetCore.Http;

namespace Proyecto_Inmuebles.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {



            List<SelectListItem> tiposUsuario = await GetTiposUsuario();

            ViewBag.listaTiposUsuario = tiposUsuario;
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel{ Username="admin", Password= "admin" });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            (bool validLogin, int IdUsuario, int userType) = await validateLogin(model.Username ?? "", model.Password ?? "");
            if (validLogin)
            {
                // Store in Session
                HttpContext.Session.SetInt32(SessionKeys.UserType, userType);
                HttpContext.Session.SetInt32(SessionKeys.IdUsuario, IdUsuario);

                // Sign in with cookie 
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Username ?? "user") };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("CredentialsError", new { returnUrl });
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult CredentialsError(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear(); // remove userType and anything else
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(int? userTypeRegister)
        {
            return View(new RegisterViewModel { IdTipoUsuario = userTypeRegister });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task <IActionResult> Register(RegisterViewModel model)
        {
            var ok = await submitUser(model);

            if (ok)
            {
                TempData["RegisterMessage"] = "User registered. Please log in.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Could not register user.");
            return View(model);
        }

        private async Task<(bool, int, int)> validateLogin(string username, string password)
        {
            bool validLogin = false;
            int IdUsuario = -1;
            int userType = -1;
            int eliminado = -1;

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(UsuariosQueries.SelectUserPassQuery(),
                new[] { OracleDBConnection.In("username", username),
                OracleDBConnection.In("password", password)
                });

            if (data.Count > 0)
            {
                eliminado = int.Parse(data.FirstOrDefault()["Eliminado"].ToString());
                if(eliminado == 0)
                {
                    validLogin = true;
                    IdUsuario = userType = int.Parse(data.FirstOrDefault()["IdUsuario"].ToString());
                    userType = int.Parse(data.FirstOrDefault()["IdTipoUsuario"].ToString());
                }
               
                
            }

            return (validLogin, IdUsuario, userType); 
        }

        private async Task<List<SelectListItem>> GetTiposUsuario()
        {

            OracleDBConnection con = new OracleDBConnection();

           var data = await con.SelectAsync(TipoUsuarioQueries.SelectTiposQuery());
       

            List<SelectListItem> TiposUsuario = new List<SelectListItem>();

            
            foreach (TipoUsuario t in ModelParser.ParseTipoUsuario(data))
            {
                if(t.Eliminado == 0)
                {
                    TiposUsuario.Add(new SelectListItem { Text = t.NombreTipo, Value = t.IdTipoUsuario.ToString() });
                }
               
            }

            return TiposUsuario;

    }

        private async Task<bool> submitUser(RegisterViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdUsuarioSalida = OracleDBConnection.Out("IdUsuario", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(UsuariosQueries.InsertUsuarioQuery(),
               new[] { OracleDBConnection.In("IdTipoUsuario", model.IdTipoUsuario),
                OracleDBConnection.In("NombreUsuario", model.NombreUsuario),
                OracleDBConnection.In("Contrasena", model.Contrasena),
                OracleDBConnection.In("Correo", model.Contrasena),
                    IdUsuarioSalida });


            int valorIdUsuarioSalida = int.Parse(salidas["IdUsuario"].ToString());
            if(model.IdTipoUsuario == 1) // Agente
            {
                (cantidadAfectados, salidas) = await con.InsertAsync(AgentesQueries.InsertAgenteQuery(),
                  new[] { OracleDBConnection.In("idUsuario", valorIdUsuarioSalida),
                OracleDBConnection.In("Codigo", model.Codigo),
                OracleDBConnection.In("Nombres", model.Nombres),
                OracleDBConnection.In("Apellidos", model.Apellidos),
                OracleDBConnection.In("Telefono", model.Telefono),
                OracleDBConnection.In("Correo", model.Correo),
                     });

                
            }
            else if (model.IdTipoUsuario == 2) // Vendedor
            {

                (cantidadAfectados, salidas) = await con.InsertAsync(VendedoresQueries.InsertVendedorQuery(),
                     new[] { OracleDBConnection.In("idUsuario", valorIdUsuarioSalida),
                OracleDBConnection.In("Nombres", model.Nombres),
                OracleDBConnection.In("Apellidos", model.Apellidos),
                OracleDBConnection.In("Direccion", model.Direccion)
                        });
            } 
            else if (model.IdTipoUsuario == 3) // Comprador
            {
                (cantidadAfectados, salidas) = await con.InsertAsync(CompradoresQueries.InsertCompradorQuery(),
                    new[] { OracleDBConnection.In("idUsuario", valorIdUsuarioSalida),
                OracleDBConnection.In("Nombres", model.Nombres),
                OracleDBConnection.In("Apellidos", model.Apellidos),
                OracleDBConnection.In("Direccion", model.Direccion),
                OracleDBConnection.In("Telefono", model.Telefono),
                OracleDBConnection.In("EstadoCivil", model.EstadoCivil),
                OracleDBConnection.In("Nacionalidad", model.Nacionalidad),
                OracleDBConnection.In("Edad", model.Edad),
                       });
            }


            return true;

        }
    }
}
