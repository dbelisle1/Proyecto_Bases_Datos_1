using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;

namespace Proyecto_Inmuebles.Controllers
{
    public class VendedoresController : Controller
    {
        public async Task<IActionResult> Index()
        {
            VendedoresViewModel viewModel = new VendedoresViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(VendedoresQueries.SelectVendedoresQuery());

            List<Vendedores> VendedoresList = new List<Vendedores>();
            foreach (Vendedores e in ModelParser.ParseVendedores(data))
            {
                if (e.Eliminado == 0)
                {
                    VendedoresList.Add(e);
                }
            }

            viewModel.VendedoresList = VendedoresList;

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);
        }

        public async Task<IActionResult> VerVendedor(int IdVendedor)
        {
            VendedoresVerViewModel viewModel = new VendedoresVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(VendedoresQueries.SelectVendedorFiltroQuery(), new[] { OracleDBConnection.In("IdVendedor", IdVendedor) });
           
            if (data.Count > 0)
            {
                viewModel.IdVendedor = IdVendedor;
                viewModel.IdUsuario = int.Parse(data.FirstOrDefault()["IdUsuario"].ToString());
                viewModel.Nombres = data.FirstOrDefault()["Nombres"].ToString();
                viewModel.Apellidos = data.FirstOrDefault()["Apellidos"].ToString();
                viewModel.Direccion = data.FirstOrDefault()["Direccion"].ToString();
            }

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);
        }

        public async Task<IActionResult> CrearVendedor()
        {
            VendedoresVerViewModel viewModel = new VendedoresVerViewModel();

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);


        }

        public async Task<IActionResult> CrearVendedorAccion(VendedoresVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(VendedoresQueries.InsertVendedorAdmQuery(),
              new[] {
                OracleDBConnection.In("Nombres", model.Nombres),
                OracleDBConnection.In("Apellidos", model.Apellidos),
                OracleDBConnection.In("Direccion", model.Direccion),
                OracleDBConnection.In("CantidadInmueblesVendidos", model.CantidadInmueblesVendidos),
                OracleDBConnection.In("IdUsuario", model.IdUsuario),
                    IdSalida });

            return RedirectToAction("Index", "Vendedores");
        }

        public async Task<IActionResult> ModificarVendedor(int IdVendedor)
        {
            VendedoresVerViewModel viewModel = new VendedoresVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(VendedoresQueries.SelectVendedorFiltroQuery(), new[] { OracleDBConnection.In("IdVendedor", IdVendedor) });

            if (data.Count > 0)
            {
                viewModel.IdVendedor = IdVendedor;
                viewModel.IdUsuario = int.Parse(data.FirstOrDefault()["IdUsuario"].ToString());
                viewModel.Nombres = data.FirstOrDefault()["Nombres"].ToString();
                viewModel.Apellidos = data.FirstOrDefault()["Apellidos"].ToString();
                viewModel.Direccion = data.FirstOrDefault()["Direccion"].ToString();
            }

            // Usuarios
            List<SelectListItem> listaUsuarios = await GetUsuarios();

            ViewBag.listaUsuarios = listaUsuarios;

            return View(viewModel);
        }

        public async Task<IActionResult> ModificarVendedorAccion(VendedoresVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(VendedoresQueries.UpdateVendedorQuery(),
                new[] {
                 OracleDBConnection.In("Nombres", model.Nombres),
                OracleDBConnection.In("Apellidos", model.Apellidos),
                OracleDBConnection.In("Direccion", model.Direccion),
                OracleDBConnection.In("CantidadInmueblesVendidos", model.CantidadInmueblesVendidos),
                OracleDBConnection.In("IdUsuario", model.IdUsuario),
                OracleDBConnection.In("IdVendedor", model.IdVendedor),
                });

            return RedirectToAction("Index", "Vendedores");
        }

        public async Task<IActionResult> EliminarVendedor(int IdVendedor)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(VendedoresQueries.DeleteVendedorQuery(),
                new[] { OracleDBConnection.In("IdVendedor", IdVendedor) });

            return RedirectToAction("Index", "Vendedores");
        }


        private async Task<List<SelectListItem>> GetUsuarios(bool permitirAdmin = false)
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(UsuariosQueries.SelectUsuariosVendedorQuery());


            List<SelectListItem> listaUsuarios = new List<SelectListItem>();


            foreach (Usuarios t in ModelParser.ParseUsuarios(data))
            {
                if ( t.Eliminado == 0 || permitirAdmin == true)
                {
                    listaUsuarios.Add(new SelectListItem { Text = t.NombreUsuario, Value = t.IdUsuario.ToString() });
                }

            }

            return listaUsuarios;

        }
    }
}
