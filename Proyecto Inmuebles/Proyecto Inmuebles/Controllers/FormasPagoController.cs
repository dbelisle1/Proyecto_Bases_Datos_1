using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;

namespace Proyecto_Inmuebles.Controllers
{
    public class FormasPagoController : Controller
    {
        public async Task<IActionResult> Index()
        {
            FormasPagoViewModel viewModel = new FormasPagoViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(FormasPagoQueries.SelectFormasPagoQuery());

            List<FormasPago> formasPago = new List<FormasPago>();
            foreach (FormasPago f in ModelParser.ParseFormasPago(data))
            {
                if (f.Eliminado == 0)
                {
                    formasPago.Add(f);
                }
            }

            viewModel.formasPagoList = formasPago;

            return View(viewModel);
        }

        public async Task<IActionResult> VerFormasPago(int IdFormaPago)
        {
            FormasPagoVerViewModel viewModel = new FormasPagoVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(FormasPagoQueries.SelectFormaPagoFiltroQuery(), new[] { OracleDBConnection.In("IdFormaPago", IdFormaPago) });

            if (data.Count > 0)
            {
                viewModel.IdFormaPago = IdFormaPago;
                viewModel.NombreFormaPago = data.FirstOrDefault()["NombreFormaPago"].ToString();
            }

            return View(viewModel);
        }

        public IActionResult CrearFormasPago()
        {
            FormasPagoVerViewModel viewModel = new FormasPagoVerViewModel();

            return View(viewModel);
        }

        public async Task<IActionResult> CrearFormasPagoAccion(FormasPagoVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(FormasPagoQueries.InsertFormaPagoQuery(),
                new[] { OracleDBConnection.In("NombreFormaPago", model.NombreFormaPago),
                 OracleDBConnection.In("IdFormaPago", model.IdFormaPago),
                IdSalida
                });

            return RedirectToAction("Index", "FormasPago");
        }

        public async Task<IActionResult> ModificarFormasPago(int IdFormaPago)
        {
            FormasPagoVerViewModel viewModel = new FormasPagoVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(FormasPagoQueries.SelectFormaPagoFiltroQuery(), new[] { OracleDBConnection.In("IdFormaPago", IdFormaPago) });

            if (data.Count > 0)
            {
                viewModel.IdFormaPago = IdFormaPago;
                viewModel.NombreFormaPago = data.FirstOrDefault()["NombreFormaPago"].ToString();
            }

            return View(viewModel);
        }

        public async Task<IActionResult> ModificarFormasPagoAccion(FormasPagoVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(FormasPagoQueries.UpdateFormaPagoQuery(),
                new[] { OracleDBConnection.In("NombreFormaPago", model.NombreFormaPago),
                                     OracleDBConnection.In("IdFormaPago", model.IdFormaPago),
                });

            return RedirectToAction("Index", "FormasPago");
        }

        public async Task<IActionResult> EliminarFormasPago(int IdFormaPago)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(FormasPagoQueries.DeleteFormaPagoQuery(),
                new[] { OracleDBConnection.In("IdFormaPago", IdFormaPago) });

            return RedirectToAction("Index", "FormasPago");
        }
    }
}
