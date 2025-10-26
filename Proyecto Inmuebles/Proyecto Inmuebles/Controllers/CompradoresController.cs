using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;

namespace Proyecto_Inmuebles.Controllers
{
    public class CompradoresController : Controller
    {
        public async Task<IActionResult> Index()
        {
            CompradoresViewModel viewModel = new CompradoresViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(CompradoresQueries.SelectCompradoresQuery());

            List<Compradores> CompradoresList = new List<Compradores>();
            foreach (Compradores e in ModelParser.ParseCompradores(data))
            {
                if (e.Eliminado == 0)
                {
                    CompradoresList.Add(e);
                }
            }

            viewModel.CompradoresList = CompradoresList;

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);
        }

        public async Task<IActionResult> VerComprador(int IdComprador)
        {
            CompradoresVerViewModel viewModel = new CompradoresVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(CompradoresQueries.SelectCompradoresFiltroQuery(), new[] { OracleDBConnection.In("IdComprador", IdComprador) });
           
            if (data.Count > 0)
            {
                viewModel.IdComprador = IdComprador;
                viewModel.IdUsuario = int.Parse(data.FirstOrDefault()["IdUsuario"].ToString());
                viewModel.Nombres = data.FirstOrDefault()["Nombres"].ToString();
                viewModel.Apellidos = data.FirstOrDefault()["Apellidos"].ToString();
                viewModel.Direccion = data.FirstOrDefault()["Direccion"].ToString();
                viewModel.Telefono = data.FirstOrDefault()["Telefono"].ToString();
                viewModel.EstadoCivil = data.FirstOrDefault()["EstadoCivil"].ToString();
                viewModel.Nacionalidad = data.FirstOrDefault()["Nacionalidad"].ToString();
                viewModel.Edad = int.Parse(data.FirstOrDefault()["Edad"].ToString());

            }

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);
        }

        public async Task<IActionResult> CrearComprador()
        {
            CompradoresVerViewModel viewModel = new CompradoresVerViewModel();

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);


        }

        public async Task<IActionResult> CrearCompradorAccion(CompradoresVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(CompradoresQueries.InsertCompradoresQuery(),
              new[] {
                OracleDBConnection.In("Nombres", model.Nombres),
                OracleDBConnection.In("Apellidos", model.Apellidos),
                OracleDBConnection.In("Direccion", model.Direccion),
                OracleDBConnection.In("Telefono", model.Telefono),
                OracleDBConnection.In("EstadoCivil", model.EstadoCivil),
                OracleDBConnection.In("Nacionalidad", model.Nacionalidad),
                OracleDBConnection.In("Edad", model.Edad),
                OracleDBConnection.In("IdUsuario", model.IdUsuario),
                    IdSalida });

            return RedirectToAction("Index", "Compradores");
        }

        public async Task<IActionResult> ModificarComprador(int IdComprador)
        {
            CompradoresVerViewModel viewModel = new CompradoresVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(CompradoresQueries.SelectCompradoresFiltroQuery(), new[] { OracleDBConnection.In("IdComprador", IdComprador) });

            if (data.Count > 0)
            {
                viewModel.IdComprador = IdComprador;
                viewModel.IdUsuario = int.Parse(data.FirstOrDefault()["IdUsuario"].ToString());
                viewModel.Nombres = data.FirstOrDefault()["Nombres"].ToString();
                viewModel.Apellidos = data.FirstOrDefault()["Apellidos"].ToString();
                viewModel.Direccion = data.FirstOrDefault()["Direccion"].ToString();
                viewModel.Telefono = data.FirstOrDefault()["Telefono"].ToString();
                viewModel.EstadoCivil = data.FirstOrDefault()["EstadoCivil"].ToString();
                viewModel.Nacionalidad = data.FirstOrDefault()["Nacionalidad"].ToString();
                viewModel.Edad = int.Parse(data.FirstOrDefault()["Edad"].ToString());
            }

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);
        }

        public async Task<IActionResult> ModificarCompradorAccion(CompradoresVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(CompradoresQueries.UpdateCompradoresQuery(),
                new[] {
                 OracleDBConnection.In("Nombres", model.Nombres),
                OracleDBConnection.In("Apellidos", model.Apellidos),
                OracleDBConnection.In("Direccion", model.Direccion),
                OracleDBConnection.In("Telefono", model.Telefono),
                OracleDBConnection.In("EstadoCivil", model.EstadoCivil),
                OracleDBConnection.In("Nacionalidad", model.Nacionalidad),
                OracleDBConnection.In("Edad", model.Edad),
                                     OracleDBConnection.In("IdComprador", model.IdComprador),
                                     OracleDBConnection.In("IdUsuario", model.IdUsuario),
                });

            return RedirectToAction("Index", "Compradores");
        }

        public async Task<IActionResult> EliminarComprador(int IdComprador)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(CompradoresQueries.DeleteCompradoresQuery(),
                new[] { OracleDBConnection.In("IdComprador", IdComprador) });

            return RedirectToAction("Index", "Compradores");
        }


        private async Task<List<SelectListItem>> GetUsuarios(bool permitirAdmin = false)
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(UsuariosQueries.SelectUsuariosComrpradorQuery());


            List<SelectListItem> listaUsuarios = new List<SelectListItem>();


            foreach (Usuarios t in ModelParser.ParseUsuarios(data))
            {
                if ( t.Eliminado == 0 || permitirAdmin == true)
                {
                    listaUsuarios.Add(new SelectListItem { Text = t.NombreUsuario, Value = t.IdUsuario.ToString() });
                }

            }

            return listaUsuarios;

        }
    }
}
