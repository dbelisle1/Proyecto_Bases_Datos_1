using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;
using Proyecto_Inmuebles.Services;

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
