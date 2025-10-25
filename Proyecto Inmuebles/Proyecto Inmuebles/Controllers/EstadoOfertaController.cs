using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;

namespace Proyecto_Inmuebles.Controllers
{
    public class EstadoOfertaController : Controller
    {
        public async Task<IActionResult> Index()
        {
            EstadoOfertaViewModel viewModel = new EstadoOfertaViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(EstadoOfertaQueries.SelectEstadosQuery());

            List<EstadoOferta> estadoOfertas = new List<EstadoOferta>();
            foreach (EstadoOferta e in ModelParser.ParseEstadoOferta(data))
            {
                if (e.Eliminado == 0)
                {
                    estadoOfertas.Add(e);
                }
            }

            viewModel.estadoOfertaList = estadoOfertas;

            return View(viewModel);
        }

        public async Task<IActionResult> VerEstadoOferta(int IdEstadoOferta)
        {
            EstadoOfertaVerViewModel viewModel = new EstadoOfertaVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(EstadoOfertaQueries.SelectEstadoOfertaFiltroQuery(), new[] { OracleDBConnection.In("IdEstadoOferta", IdEstadoOferta) });

            if (data.Count > 0)
            {
                viewModel.IdEstadoOferta = IdEstadoOferta;
                viewModel.NombreEstado = data.FirstOrDefault()["NombreEstado"].ToString();
            }

            return View(viewModel);
        }

        public IActionResult CrearEstadoOferta()
        {
            EstadoOfertaVerViewModel viewModel = new EstadoOfertaVerViewModel();

            return View(viewModel);
        }

        public async Task<IActionResult> CrearEstadoOfertaAccion(EstadoOfertaVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(EstadoOfertaQueries.InsertEstadoOfertaQuery(),
                new[] { OracleDBConnection.In("NombreEstado", model.NombreEstado),
                 OracleDBConnection.In("IdEstadoOferta", model.IdEstadoOferta),
                IdSalida
                });

            return RedirectToAction("Index", "EstadoOferta");
        }

        public async Task<IActionResult> ModificarEstadoOferta(int IdEstadoOferta)
        {
            EstadoOfertaVerViewModel viewModel = new EstadoOfertaVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(EstadoOfertaQueries.SelectEstadoOfertaFiltroQuery(), new[] { OracleDBConnection.In("IdEstadoOferta", IdEstadoOferta) });

            if (data.Count > 0)
            {
                viewModel.IdEstadoOferta = IdEstadoOferta;
                viewModel.NombreEstado = data.FirstOrDefault()["NombreEstado"].ToString();
            }

            return View(viewModel);
        }

        public async Task<IActionResult> ModificarEstadoOfertaAccion(EstadoOfertaVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(EstadoOfertaQueries.UpdateEstadoOfertaQuery(),
                new[] { OracleDBConnection.In("NombreEstado", model.NombreEstado),
                                     OracleDBConnection.In("IdEstadoOferta", model.IdEstadoOferta),
                });

            return RedirectToAction("Index", "EstadoOferta");
        }

        public async Task<IActionResult> EliminarEstadoOferta(int IdEstadoOferta)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(EstadoOfertaQueries.DeleteEstadoOfertaQuery(),
                new[] { OracleDBConnection.In("IdEstadoOferta", IdEstadoOferta) });

            return RedirectToAction("Index", "EstadoOferta");
        }
    }
}
