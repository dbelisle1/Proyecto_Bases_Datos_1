using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;
using Proyecto_Inmuebles.Services;

namespace Proyecto_Inmuebles.Controllers
{
    public class ReportesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ReportesViewModel model = new ReportesViewModel();
            await llenarListas();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Reporte1Accion(ReportesViewModel model)
        {
            var viewModel = model ?? new ReportesViewModel();

            OracleDBConnection con = new OracleDBConnection();
            var data = await con.SelectAsync(
                ReportesQueries.FormasPagoCompradorQuery(),
                new[] { OracleDBConnection.In("IdComprador", viewModel.R1_IdComprador) }
            );

            var list = new List<FormasPagoUsadasPorComprador>();
            foreach (var row in ReporteParser.ParseFormasPagoUsadasPorComprador(data))
                list.Add(row);

            viewModel.Reporte1 = list;
            return View("Reporte1", viewModel);
        }

        // ---------------- R2) Inmuebles por vendedor y tipo (con tipo)
        [HttpPost]
        public async Task<IActionResult> Reporte2Accion(ReportesViewModel model)
        {
            var viewModel = model ?? new ReportesViewModel();

            OracleDBConnection con = new OracleDBConnection();
            var data = await con.SelectAsync(
                ReportesQueries.InmueblesVendendorTipoQuery(),
                new[]
                {
                    OracleDBConnection.In("IdVendedor",     viewModel.R2_IdVendedor),
                    OracleDBConnection.In("IdTipoInmueble", viewModel.R2_IdTipoInmueble)
                }
            );

            var list = new List<InmueblesPorVendedorYTipo>();
            foreach (var row in ReporteParser.ParseInmueblesPorVendedorYTipo(data))
                list.Add(row);

            viewModel.Reporte2 = list;
            return View("Reporte2", viewModel);
        }

        // ---------------- R3) Top interés por inmueble (OFERTAS.FECHAHORA)
        [HttpPost]
        public async Task<IActionResult> Reporte3Accion(ReportesViewModel model)
        {
            var viewModel = model ?? new ReportesViewModel();
            string? start = string.IsNullOrWhiteSpace(viewModel.R3_StartDateOfertas) ? null : viewModel.R3_StartDateOfertas;
            string? end = string.IsNullOrWhiteSpace(viewModel.R3_EndDateOfertas) ? null : viewModel.R3_EndDateOfertas;

            OracleDBConnection con = new OracleDBConnection();
            var data = await con.SelectAsync(
                ReportesQueries.TopInteresInmuebleQuery(),
                new[]
                {
                    OracleDBConnection.In("StartDate", start),
                    OracleDBConnection.In("EndDate",   end)
                }
            );

            var list = new List<TopInteresPorInmueble>();
            foreach (var row in ReporteParser.ParseTopInteresPorInmueble(data))
                list.Add(row);

            viewModel.Reporte3 = list;
            return View("Reporte3", viewModel);
        }

        // ---------------- R4) Resumen por vendedor
        [HttpPost]
        public async Task<IActionResult> Reporte4Accion(ReportesViewModel model)
        {
            var viewModel = model ?? new ReportesViewModel();

            OracleDBConnection con = new OracleDBConnection();
            var data = await con.SelectAsync(
                ReportesQueries.ResumenVendedorQuery(),
                new[]
                {
                    OracleDBConnection.In("IdVendedor", viewModel.R4_IdVendedor),
                }
            );

            var list = new List<ResumenPorVendedor>();
            foreach (var row in ReporteParser.ParseResumenPorVendedor(data))
                list.Add(row);

            viewModel.Reporte4 = list;
            return View("Reporte4", viewModel);
        }

        // ---------------- R5) Ventas por agente y forma de pago (VENTAS.FECHACIERRE)
        [HttpPost]
        public async Task<IActionResult> Reporte5Accion(ReportesViewModel model)
        {
            var viewModel = model ?? new ReportesViewModel();
            string? start = string.IsNullOrWhiteSpace(viewModel.R5_StartDateVentas) ? null : viewModel.R5_StartDateVentas;
            string? end = string.IsNullOrWhiteSpace(viewModel.R5_EndDateVentas) ? null : viewModel.R5_EndDateVentas;

            OracleDBConnection con = new OracleDBConnection();
            var data = await con.SelectAsync(
                ReportesQueries.VentasAgenteFormaPagoQuery(),
                new[]
                {
                    OracleDBConnection.In("StartDate", start),
                    OracleDBConnection.In("EndDate",   end)
                }
            );

            var list = new List<VentasPorAgenteFormaPago>();
            foreach (var row in ReporteParser.ParseVentasPorAgenteFormaPago(data))
                list.Add(row);

            viewModel.Reporte5 = list;
            return View("Reporte5", viewModel);
        }

        // ---------------- R6) Tasa de aceptación de ofertas por agente (OFERTAS.FECHAHORA)
        [HttpPost]
        public async Task<IActionResult> Reporte6Accion(ReportesViewModel model)
        {
            var viewModel = model ?? new ReportesViewModel();
            string? start = string.IsNullOrWhiteSpace(viewModel.R6_StartDateOfertas) ? null : viewModel.R6_StartDateOfertas;
            string? end = string.IsNullOrWhiteSpace(viewModel.R6_EndDateOfertas) ? null : viewModel.R6_EndDateOfertas;

            OracleDBConnection con = new OracleDBConnection();
            var data = await con.SelectAsync(
                ReportesQueries.TasaAceptacionAgenteQuery(),
                new[]
                {
                    OracleDBConnection.In("StartDate", start),
                    OracleDBConnection.In("EndDate",   end)
                }
            );

            var list = new List<TasaAceptacionPorAgente>();
            foreach (var row in ReporteParser.ParseTasaAceptacionPorAgente(data))
                list.Add(row);

            viewModel.Reporte6 = list;
            return View("Reporte6", viewModel);
        }

        // ---------------- R7) Contraofertas: volumen y efectividad por agente (CONTRAOFERTAS.FECHAHORA)
        [HttpPost]
        public async Task<IActionResult> Reporte7Accion(ReportesViewModel model)
        {
            var viewModel = model ?? new ReportesViewModel();
            string? start = string.IsNullOrWhiteSpace(viewModel.R7_StartDateContraofertas) ? null : viewModel.R7_StartDateContraofertas;
            string? end = string.IsNullOrWhiteSpace(viewModel.R7_EndDateContraofertas) ? null : viewModel.R7_EndDateContraofertas;

            OracleDBConnection con = new OracleDBConnection();
            var data = await con.SelectAsync(
                ReportesQueries.ContraOfertaEfectividadQuery(),
                new[]
                {
                    OracleDBConnection.In("StartDate", start),
                    OracleDBConnection.In("EndDate",   end)
                }
            );

            var list = new List<ContraofertasPorAgente>();
            foreach (var row in ReporteParser.ParseContraofertasPorAgente(data))
                list.Add(row);

            viewModel.Reporte7 = list;
            return View("Reporte7", viewModel);
        }

        // ---------------- R8) Resultados por agente (All Time)
        [HttpPost]
        public async Task<IActionResult> Reporte8Accion(ReportesViewModel model)
        {
            var viewModel = model ?? new ReportesViewModel();

            OracleDBConnection con = new OracleDBConnection();
            var data = await con.SelectAsync(
                ReportesQueries.ResultadosAgenteQuery(),
                new[]
                {
                    OracleDBConnection.In("IdAgente",  viewModel.R8_IdAgente),
                }
            );

            var list = new List<ResultadosPorAgenteAllTime>();
            foreach (var row in ReporteParser.ParseResultadosPorAgenteAllTime(data))
                list.Add(row);

            viewModel.Reporte8 = list;
            return View("Reporte8", viewModel);
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

            return true;
        }

    }
}
