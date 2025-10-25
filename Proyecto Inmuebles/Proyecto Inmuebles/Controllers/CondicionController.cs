using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;

namespace Proyecto_Inmuebles.Controllers
{
    public class CondicionController : Controller
    {
        public async Task<IActionResult> Index()
        {
            CondicionViewModel viewModel = new CondicionViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(CondicionQueries.SelectCondicionesQuery());

            List<Condicion> condiciones = new List<Condicion>();
            foreach (Condicion c in ModelParser.ParseCondicion(data))
            {
                if (c.Eliminado == 0)
                {
                    condiciones.Add(c);
                }
            }

            viewModel.condicionList = condiciones;

            return View(viewModel);
        }

        public async Task<IActionResult> VerCondicion(int IdCondicion)
        {
            CondicionVerViewModel viewModel = new CondicionVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(CondicionQueries.SelectCondicionFiltroQuery(), new[] { OracleDBConnection.In("IdCondicion", IdCondicion) });

            if (data.Count > 0)
            {
                viewModel.IdCondicion = IdCondicion;
                viewModel.NombreCondicion = data.FirstOrDefault()["NombreCondicion"].ToString();
            }

            return View(viewModel);
        }

        public IActionResult CrearCondicion()
        {
            CondicionVerViewModel viewModel = new CondicionVerViewModel();

            return View(viewModel);
        }

        public async Task<IActionResult> CrearCondicionAccion(CondicionVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(CondicionQueries.InsertCondicionQuery(),
                new[] { OracleDBConnection.In("NombreCondicion", model.NombreCondicion),
                 OracleDBConnection.In("IdCondicion", model.IdCondicion),
                IdSalida
                });

            return RedirectToAction("Index", "Condicion");
        }

        public async Task<IActionResult> ModificarCondicion(int IdCondicion)
        {
            CondicionVerViewModel viewModel = new CondicionVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(CondicionQueries.SelectCondicionFiltroQuery(), new[] { OracleDBConnection.In("IdCondicion", IdCondicion) });

            if (data.Count > 0)
            {
                viewModel.IdCondicion = IdCondicion;
                viewModel.NombreCondicion = data.FirstOrDefault()["NombreCondicion"].ToString();
            }

            return View(viewModel);
        }

        public async Task<IActionResult> ModificarCondicionAccion(CondicionVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(CondicionQueries.UpdateCondicionQuery(),
                new[] { OracleDBConnection.In("NombreCondicion", model.NombreCondicion),
                                     OracleDBConnection.In("IdCondicion", model.IdCondicion),
                });

            return RedirectToAction("Index", "Condicion");
        }

        public async Task<IActionResult> EliminarCondicion(int IdCondicion)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(CondicionQueries.DeleteCondicionQuery(),
                new[] { OracleDBConnection.In("IdCondicion", IdCondicion) });

            return RedirectToAction("Index", "Condicion");
        }
    }
}
