using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;
using System.Reflection;

namespace Proyecto_Inmuebles.Controllers
{
    public class TiposInmuebleController : Controller
    {
        public async Task<IActionResult> Index()
        {
            TiposInmuebleViewModel viewModel = new TiposInmuebleViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(TiposInmuebleQueries.SelectTiposQuery());

            List<TiposInmueble> tipos = new List<TiposInmueble>();
            foreach (TiposInmueble t in ModelParser.ParseTiposInmueble(data))
            {
                if (t.Eliminado == 0)
                {
                    tipos.Add(t);
                }
               
            }

            viewModel.tiposInmuebleList = tipos;


            return View(viewModel);
        }

        public async Task<IActionResult> VerTiposInmueble(int IdTipoInmueble)
        {

            TiposInmuebleVerViewModel viewModel = new TiposInmuebleVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(TiposInmuebleQueries.SelectTipoFiltroQuery(), new[] {OracleDBConnection.In("IdTipoInmueble", IdTipoInmueble) });

            if (data.Count > 0)
            {
                viewModel.IdTipoInmueble = IdTipoInmueble;
                viewModel.NombreTipo = data.FirstOrDefault()["NombreTipo"].ToString();
            }


            return View(viewModel);
        }

        public IActionResult CrearTiposInmueble()
        {
            TiposInmuebleVerViewModel viewModel = new TiposInmuebleVerViewModel();

            return View(viewModel);
        }

        public async Task<IActionResult> CrearTiposInmuebleAccion(TiposInmuebleVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(TiposInmuebleQueries.InsertTipoQuery(),
                new[] {OracleDBConnection.In("NombreTipo", model.NombreTipo),
               OracleDBConnection.In("IdTipoInmueble", model.IdTipoInmueble),
               IdSalida
                });


            return RedirectToAction("Index", "TiposInmueble");
        }
        public async Task<IActionResult> ModificarTiposInmueble(int IdTipoInmueble)
        {
            TiposInmuebleVerViewModel viewModel = new TiposInmuebleVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(TiposInmuebleQueries.SelectTipoFiltroQuery(), new[] { OracleDBConnection.In("IdTipoInmueble", IdTipoInmueble) });

            if (data.Count > 0)
            {
                viewModel.IdTipoInmueble = IdTipoInmueble;
                viewModel.NombreTipo = data.FirstOrDefault()["NombreTipo"].ToString();
            }


            return View(viewModel);
        }

        public async Task<IActionResult> ModificarTiposInmuebleAccion(TiposInmuebleVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(TiposInmuebleQueries.UpdateTipoQuery(), 
                new[] {OracleDBConnection.In("NombreTipo", model.NombreTipo),
               OracleDBConnection.In("IdTipoInmueble", model.IdTipoInmueble),
                } );


            return RedirectToAction("Index", "TiposInmueble");
        }

        public async Task<IActionResult> EliminarTiposInmueble(int IdTipoInmueble)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(TiposInmuebleQueries.DeleteTipoQuery(),
                new[] {
               OracleDBConnection.In("IdTipoInmueble", IdTipoInmueble),
                });

            return RedirectToAction("Index", "TiposInmueble");
        }

       
    }
}
