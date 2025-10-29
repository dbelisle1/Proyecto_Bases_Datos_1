using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;
using Proyecto_Inmuebles.Services;
using System.Reflection;
using System;

namespace Proyecto_Inmuebles.Controllers
{
    public class VentasController : Controller
    {
        public async Task<IActionResult> Index()
        {
            VentasViewModel model = new VentasViewModel();
            await llenarListas();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> FiltrarVentasAccion(VentasViewModel model)
        {
            var viewModel = model ?? new VentasViewModel();

            OracleDBConnection con = new OracleDBConnection();
            var data = await con.SelectAsync(
                VentasQueries.SelectVentasByIdAgenteQuery(),
                new[] { OracleDBConnection.In("IdAgente", viewModel.IdAgenteFiltro) }
            );

            var list = new List<VentaFiltro>();
            foreach (var row in ReporteParser.ParseVentaFiltro(data))
                list.Add(row);

            viewModel.VentasFiltroList = list;

            await llenarListas();
            return View("FiltrarVentas", viewModel);
        }

        public async Task<IActionResult> AceptarOfertaAccion(int IdOfertaAceptada, int IdFormaPago, int PrecioFinal, int PlazoDias)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(VentasQueries.InsertVentaAceptadaQueryQuery(),
              new[] {
                OracleDBConnection.In("IdOfertaAceptada", IdOfertaAceptada),
                OracleDBConnection.In("IdPrestamo", null),
                OracleDBConnection.In("IdFormaPago", IdFormaPago),
                OracleDBConnection.In("PrecioFinal", PrecioFinal),
                OracleDBConnection.In("PlazoDias", PlazoDias),
                    IdSalida });

            return RedirectToAction("Index", "Ventas");
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

            return true;
        }

    }
}
