using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;

namespace Proyecto_Inmuebles.Controllers
{
    public class AgentesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            AgentesViewModel viewModel = new AgentesViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(AgentesQueries.SelectAgentesQuery());

            List<Agentes> AgentesList = new List<Agentes>();
            foreach (Agentes e in ModelParser.ParseAgentes(data))
            {
                if (e.Eliminado == 0)
                {
                    AgentesList.Add(e);
                }
            }

            viewModel.AgentesList = AgentesList;

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);
        }

        public async Task<IActionResult> VerAgente(int IdAgente)
        {
            AgentesVerViewModel viewModel = new AgentesVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(AgentesQueries.SelectAgenteFiltroQuery(), new[] { OracleDBConnection.In("IdAgente", IdAgente) });
           
            if (data.Count > 0)
            {
                viewModel.IdAgente = IdAgente;
                viewModel.IdUsuario = int.Parse(data.FirstOrDefault()["IdUsuario"].ToString());
                viewModel.Codigo = data.FirstOrDefault()["Codigo"].ToString();
                viewModel.Nombres = data.FirstOrDefault()["Nombres"].ToString();
                viewModel.Apellidos = data.FirstOrDefault()["Apellidos"].ToString();
                viewModel.Telefono = data.FirstOrDefault()["Telefono"].ToString();
                viewModel.Correo = data.FirstOrDefault()["Correo"].ToString();

            }

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);
        }

        public async Task<IActionResult> CrearAgente()
        {
            AgentesVerViewModel viewModel = new AgentesVerViewModel();

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);


        }

        public async Task<IActionResult> CrearAgenteAccion(AgentesVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(AgentesQueries.InsertAgenteAdmQuery(),
              new[] { OracleDBConnection.In("Codigo", model.Codigo),
                OracleDBConnection.In("Nombres", model.Nombres),
                OracleDBConnection.In("Apellidos", model.Apellidos),
                OracleDBConnection.In("Telefono", model.Telefono),
                OracleDBConnection.In("Correo", model.Correo),
                OracleDBConnection.In("IdUsuario", model.IdUsuario),
                    IdSalida });

            return RedirectToAction("Index", "Agentes");
        }

        public async Task<IActionResult> ModificarAgente(int IdAgente)
        {
            AgentesVerViewModel viewModel = new AgentesVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(AgentesQueries.SelectAgenteFiltroQuery(), new[] { OracleDBConnection.In("IdAgente", IdAgente) });

            if (data.Count > 0)
            {
                viewModel.IdAgente = IdAgente;
                viewModel.IdUsuario = int.Parse(data.FirstOrDefault()["IdUsuario"].ToString());
                viewModel.Codigo = data.FirstOrDefault()["Codigo"].ToString();
                viewModel.Nombres = data.FirstOrDefault()["Nombres"].ToString();
                viewModel.Apellidos = data.FirstOrDefault()["Apellidos"].ToString();
                viewModel.Telefono = data.FirstOrDefault()["Telefono"].ToString();
                viewModel.Correo = data.FirstOrDefault()["Correo"].ToString();
            }

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);
        }

        public async Task<IActionResult> ModificarAgenteAccion(AgentesVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(AgentesQueries.UpdateAgenteQuery(),
                new[] {OracleDBConnection.In("Codigo", model.Codigo),
                OracleDBConnection.In("Nombres", model.Nombres),
                OracleDBConnection.In("Apellidos", model.Apellidos),
                OracleDBConnection.In("Telefono", model.Telefono),
                OracleDBConnection.In("Correo", model.Correo),
                OracleDBConnection.In("IdUsuario", model.IdUsuario),
                                     OracleDBConnection.In("IdAgente", model.IdAgente),
                });

            return RedirectToAction("Index", "Agentes");
        }

        public async Task<IActionResult> EliminarAgente(int IdAgente)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(AgentesQueries.DeleteAgenteQuery(),
                new[] { OracleDBConnection.In("IdAgente", IdAgente) });

            return RedirectToAction("Index", "Agentes");
        }


        private async Task<List<SelectListItem>> GetUsuarios(bool permitirAdmin = false)
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(UsuariosQueries.SelectUsuariosAgenteQuery());


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
