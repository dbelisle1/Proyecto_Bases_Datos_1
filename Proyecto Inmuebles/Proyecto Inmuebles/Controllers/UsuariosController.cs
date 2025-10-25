using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;

namespace Proyecto_Inmuebles.Controllers
{
    public class UsuariosController : Controller
    {
        public async Task<IActionResult> Index()
        {
            UsuariosViewModel viewModel = new UsuariosViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(UsuariosQueries.SelectUsuariosQuery());

            List<Usuarios> usuariosList = new List<Usuarios>();
            foreach (Usuarios e in ModelParser.ParseUsuarios(data))
            {
                if (e.Eliminado == 0)
                {
                    usuariosList.Add(e);
                }
            }

            viewModel.usuariosList = usuariosList;

            // tipos usuario
            List<SelectListItem> tiposUsuario = await GetTiposUsuario(validarElim: false);

            ViewBag.listaTiposUsuario = tiposUsuario;

            return View(viewModel);
        }

        public async Task<IActionResult> VerUsuario(int IdUsuario)
        {
            UsuariosVerViewModel viewModel = new UsuariosVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(UsuariosQueries.SelectUsuarioFiltroQuery(), new[] { OracleDBConnection.In("IdUsuario", IdUsuario) });
           
            if (data.Count > 0)
            {
                viewModel.IdUsuario = IdUsuario;
                viewModel.IdTipoUsuario = int.Parse(data.FirstOrDefault()["IdTipoUsuario"].ToString());
                viewModel.NombreUsuario = data.FirstOrDefault()["NombreUsuario"].ToString();
                viewModel.Contrasena = data.FirstOrDefault()["Contrasena"].ToString();
                viewModel.Correo = data.FirstOrDefault()["Correo"].ToString();
            }

            // tipos usuario
            List<SelectListItem> tiposUsuario = await GetTiposUsuario();

            ViewBag.listaTiposUsuario = tiposUsuario;

            return View(viewModel);
        }

        public async Task<IActionResult> CrearUsuario()
        {
            UsuariosVerViewModel viewModel = new UsuariosVerViewModel();

            // tipos usuario
            List<SelectListItem> tiposUsuario = await GetTiposUsuario();

            ViewBag.listaTiposUsuario = tiposUsuario;

            return View(viewModel);


        }

        public async Task<IActionResult> CrearUsuarioAccion(UsuariosVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdUsuario = OracleDBConnection.Out("IdUsuario", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(UsuariosQueries.InsertUsuarioQuery(),
              new[] { OracleDBConnection.In("IdTipoUsuario", model.IdTipoUsuario),
                OracleDBConnection.In("NombreUsuario", model.NombreUsuario),
                OracleDBConnection.In("Contrasena", model.Contrasena),
                OracleDBConnection.In("Correo", model.Contrasena),
                    IdUsuario });

            return RedirectToAction("Index", "Usuarios");
        }

        public async Task<IActionResult> ModificarUsuario(int IdUsuario)
        {
            UsuariosVerViewModel viewModel = new UsuariosVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(UsuariosQueries.SelectUsuarioFiltroQuery(), new[] { OracleDBConnection.In("IdUsuario", IdUsuario) });

            if (data.Count > 0)
            {
                viewModel.IdUsuario = IdUsuario;
                viewModel.IdTipoUsuario = int.Parse(data.FirstOrDefault()["IdTipoUsuario"].ToString());
                viewModel.NombreUsuario = data.FirstOrDefault()["NombreUsuario"].ToString();
                viewModel.Contrasena = data.FirstOrDefault()["Contrasena"].ToString();
                viewModel.Correo = data.FirstOrDefault()["Correo"].ToString();
            }

            // tipos usuario
            List<SelectListItem> tiposUsuario = await GetTiposUsuario();

            ViewBag.listaTiposUsuario = tiposUsuario;

            return View(viewModel);
        }

        public async Task<IActionResult> ModificarUsuarioAccion(UsuariosVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(UsuariosQueries.UpdateUsuarioQuery(),
                new[] { OracleDBConnection.In("NombreUsuario", model.NombreUsuario),
                                     OracleDBConnection.In("Contrasena", model.Contrasena),
                                     OracleDBConnection.In("Correo", model.Correo),
                                     OracleDBConnection.In("IdTipoUsuario", model.IdTipoUsuario),
                                     OracleDBConnection.In("IdUsuario", model.IdUsuario),
                });

            return RedirectToAction("Index", "Usuarios");
        }

        public async Task<IActionResult> EliminarUsuario(int IdUsuario)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(UsuariosQueries.DeleteUsuarioQuery(),
                new[] { OracleDBConnection.In("IdUsuario", IdUsuario) });

            return RedirectToAction("Index", "Usuarios");
        }


        private async Task<List<SelectListItem>> GetTiposUsuario(bool validarElim = true)
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(TipoUsuarioQueries.SelectTiposQuery());


            List<SelectListItem> TiposUsuario = new List<SelectListItem>();


            foreach (TipoUsuario t in ModelParser.ParseTipoUsuario(data))
            {
                if ( t.Eliminado == 0 || validarElim == false)
                {
                    TiposUsuario.Add(new SelectListItem { Text = t.NombreTipo, Value = t.IdTipoUsuario.ToString() });
                }

            }

            return TiposUsuario;

        }
    }
}
