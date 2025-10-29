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
    public class OfertasController : Controller
    {
        public async Task<IActionResult> Index()
        {

            OfertasViewModel viewModel = new OfertasViewModel();

            OracleDBConnection con = new OracleDBConnection();

            List<Dictionary<string, object?>> data = null;

            var userType = HttpContext.Session.GetInt32(SessionKeys.UserType);

            //Agente
            if (userType == 1)
            {
                data = await con.SelectAsync(OfertasQueries.SelectOfertasByIdAgente(),
                   new[] { OracleDBConnection.In("IdAgente", await GetIdAgente()) });

            }

            //Comprador
            if (userType == 3)
            {
                data = await con.SelectAsync(OfertasQueries.SelectOfertasByIdComprador(),
                     new[] { OracleDBConnection.In("IdComprador", await GetIdComprador()) });


            }


            List<OfertaDetalle> OfertasList = ReporteParser.ParseOfertaDetalle(data);

            viewModel.OfertasList = OfertasList;

            await llenarListas();
            return View(viewModel);
        }

        public async Task<IActionResult> CrearOferta(int IdPublicacion)
        {
            OfertasVerViewModel model = new OfertasVerViewModel();
            model.IdPublicacion = IdPublicacion;
            model.FechaHora = DateTime.Now;
            model.IdComprador = await GetIdComprador();
            await llenarListas();
            return View("CrearOferta", model);
        }


        public async Task<IActionResult> CrearOfertaAccion(OfertasVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(OfertasQueries.InsertOfertaQuery(),
              new[] {
                OracleDBConnection.In("IdPublicacion", model.IdPublicacion),
                OracleDBConnection.In("IdComprador", model.IdComprador),
                OracleDBConnection.In("Monto", model.Monto),
                OracleDBConnection.In("IdFormaPago", model.IdFormaPago),
                OracleDBConnection.In("PlazoDias", model.PlazoDias),
                OracleDBConnection.In("FechaHora", model.FechaHora),
                    IdSalida });

            int IdOfertaResult = int.Parse(salidas["IdSalida"].ToString() ?? "-1");

            await Utils.NotificarOferta(new Notificaciones { IdOferta = IdOfertaResult, Descripcion = $"Se oferto por publicacion con id {model.IdPublicacion}" });

            return RedirectToAction("Index", "Ofertas");
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

            // FormasPAgo
            List<SelectListItem> listaFormasPago = await Utils.GetFormasPago();

            ViewBag.listaFormasPago = listaFormasPago;

            return true;
        }


    }
}
