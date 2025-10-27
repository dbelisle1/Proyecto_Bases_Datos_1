using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;
using Proyecto_Inmuebles.Services;

namespace Proyecto_Inmuebles.Controllers
{
    public class PublicacionesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            PublicacionesViewModel viewModel = new PublicacionesViewModel();

            OracleDBConnection con = new OracleDBConnection();

            List<Dictionary<string, object?>> data = null;

            var userType = HttpContext.Session.GetInt32(SessionKeys.UserType);

            //Agente
            if (userType == 1)
            {
               data = await con.SelectAsync(PublicacionesQueries.SelectPublicacionesIdAgenteQuery(),
                   new[] { OracleDBConnection.In("IdAgente", await GetIdAgente()) });

            }

            //Vendedor
            if (userType == 2)
            {
                data = await con.SelectAsync(PublicacionesQueries.SelectPublicacionesIdVendedorQuery(), 
                   new[] { OracleDBConnection.In("IdVendedor", await GetIdVendedor()) });

            }

            //Comprador
            if (userType == 3)
            {
              data = await con.SelectAsync(PublicacionesQueries.SelectPublicacionesQuery());


            }


            List<Publicaciones> PublicacionesList = new List<Publicaciones>();
            foreach (Publicaciones e in ModelParser.ParsePublicaciones(data))
            {
                if (e.Eliminado == 0)
                {
                    PublicacionesList.Add(e);
                }
            }

            viewModel.PublicacionesList = PublicacionesList;

            await llenarListas();
            return View(viewModel);
        }


        private async Task<int> GetIdVendedor()
        {
            if (HttpContext.Session.GetInt32(SessionKeys.UserType) == 99)
            {
                return 1; // Admin
            }

            OracleDBConnection con = new OracleDBConnection();

            int? IdUsuario = HttpContext.Session.GetInt32(SessionKeys.IdUsuario);
            int IdVendedor = -1;



            var data = await con.SelectAsync(VendedoresQueries.SelectVendedoresByIdUsuarioQuery(), new[] { OracleDBConnection.In("IdUsuario", IdUsuario) });
            if (data.Count > 0)
            {
                IdVendedor = int.Parse(data.FirstOrDefault()["IdVendedor"].ToString());
            }

            return IdVendedor;

        }

        private async Task<int> GetIdComprador()
        {
            if (HttpContext.Session.GetInt32(SessionKeys.UserType) == 99)
            {
                return 1; // Admin
            }

            OracleDBConnection con = new OracleDBConnection();

            int? IdUsuario = HttpContext.Session.GetInt32(SessionKeys.IdUsuario);
            int IdComprador = -1;



            var data = await con.SelectAsync(CompradoresQueries.SelectCompradoresByIdUsuarioQuery(), new[] { OracleDBConnection.In("IdUsuario", IdUsuario) });
            if (data.Count > 0)
            {
                IdComprador = int.Parse(data.FirstOrDefault()["IdComprador"].ToString());
            }

            return IdComprador;

        }

        private async Task<int> GetIdAgente()
        {
            if (HttpContext.Session.GetInt32(SessionKeys.UserType) == 99)
            {
                return 1; // Admin
            }

            OracleDBConnection con = new OracleDBConnection();

            int? IdUsuario = HttpContext.Session.GetInt32(SessionKeys.IdUsuario);
            int IdAgente = -1;



            var data = await con.SelectAsync(AgentesQueries.SelectAgentesByIdUsuarioQuery(), new[] { OracleDBConnection.In("IdUsuario", IdUsuario) });
            if (data.Count > 0)
            {
                IdAgente = int.Parse(data.FirstOrDefault()["IdAgente"].ToString());
            }

            return IdAgente;

        }

        [HttpPost]
        public async Task<IActionResult> FiltrarPublicacionesAccion(PublicacionesViewModel model)
        {
            var viewModel = model ?? new PublicacionesViewModel();

            OracleDBConnection con = new OracleDBConnection();
            var data = await con.SelectAsync(
                PublicacionesQueries.SelectPublicacionesByIdCondicion(),
                new[] { OracleDBConnection.In("IdCondicion", viewModel.IdCondicionFiltro) }
            );

            var list = new List<PublicacionPorCondicion>();
            foreach (var row in ReporteParser.ParsePublicacionesPorCondicion(data))
                list.Add(row);

            viewModel.PublicacionesFiltroList = list;

            await llenarListas();
            return View("FiltrarPublicaciones", viewModel);
        }

        private async Task<bool> llenarListas()
        {

            // vendedores
            List<SelectListItem> listaVendedores = await Utils.GetVendedores();

            ViewBag.listaVendedores = listaVendedores;

            // Compradores
            List<SelectListItem> listaCompradores = await Utils.GetCompradores();

            ViewBag.listaCompradores = listaCompradores;

            // Agentes
            List<SelectListItem> listaAgentes = await Utils.GetAgentes();

            ViewBag.listaAgentes = listaAgentes;

            // tiposInmueble
            List<SelectListItem> listaTipos = await Utils.GetTiposInmueble();

            ViewBag.listaTipos = listaTipos;

            // Inmuebles
            List<SelectListItem> listaInmuebles = await Utils.GetInmuebles();

            ViewBag.listaInmuebles = listaInmuebles;

            // Inmuebles
            List<SelectListItem> listaCondiciones = await Utils.GetCondiciones();

            ViewBag.listaCondiciones = listaCondiciones;

            return true;
        }


    }
}
