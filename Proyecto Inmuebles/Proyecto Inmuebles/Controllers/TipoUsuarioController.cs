using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;
using System.Reflection;

namespace Proyecto_Inmuebles.Controllers
{
    public class TipoUsuarioController : Controller
    {
        public async Task<IActionResult> Index()
        {
            TipoUsuarioViewModel viewModel = new TipoUsuarioViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(TipoUsuarioQueries.SelectTiposQuery());

            List<TipoUsuario> tipos = new List<TipoUsuario>();
            foreach (TipoUsuario t in ModelParser.ParseTipoUsuario(data))
            {
                if (t.Eliminado == 0)
                {
                    tipos.Add(t);
                }
               
            }

            viewModel.tiposUsuario = tipos;


            return View(viewModel);
        }

        public async Task<IActionResult> VerTipoUsuario(int IdTipoUsuario)
        {

            TipoUsuarioVerViewModel viewModel = new TipoUsuarioVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(TipoUsuarioQueries.SelectTipoFiltroQuery(), new[] {OracleDBConnection.In("IdTipoUsuario", IdTipoUsuario) });

            if (data.Count > 0)
            {
                viewModel.IdTipoUsuario = IdTipoUsuario;
                viewModel.NombreTipo = data.FirstOrDefault()["NombreTipo"].ToString();
            }


            return View(viewModel);
        }
        public async Task<IActionResult> ModificarTipoUsuario(int IdTipoUsuario)
        {
            TipoUsuarioVerViewModel viewModel = new TipoUsuarioVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(TipoUsuarioQueries.SelectTipoFiltroQuery(), new[] { OracleDBConnection.In("IdTipoUsuario", IdTipoUsuario) });

            if (data.Count > 0)
            {
                viewModel.IdTipoUsuario = IdTipoUsuario;
                viewModel.NombreTipo = data.FirstOrDefault()["NombreTipo"].ToString();
            }


            return View(viewModel);
        }

        public async Task<IActionResult> ModificarTipoUsuarioAccion(TipoUsuarioVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(TipoUsuarioQueries.UpdateTipoQuery(), 
                new[] {OracleDBConnection.In("NombreTipo", model.NombreTipo),
               OracleDBConnection.In("IdTipoUsuario", model.IdTipoUsuario),
                } );


            return RedirectToAction("Index", "TipoUsuario");
        }

        public async Task<IActionResult> EliminarTipoUsuario(int IdTipoUsuario)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(TipoUsuarioQueries.DeleteTipoQuery(),
                new[] {
               OracleDBConnection.In("IdTipoUsuario", IdTipoUsuario),
                });

            return RedirectToAction("Index", "TipoUsuario");
        }

       
    }
}
