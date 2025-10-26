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
    public class InmueblesController : Controller
    {
        public async Task<IActionResult> Index()
        {
            InmueblesViewModel viewModel = new InmueblesViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(InmueblesQueries.SelectInmueblesQuery());

            List<Inmuebles> InmueblesList = new List<Inmuebles>();
            foreach (Inmuebles e in ModelParser.ParseInmuebles(data))
            {
                if (e.Eliminado == 0)
                {
                    InmueblesList.Add(e);
                }
            }

            viewModel.InmueblesList = InmueblesList;

            await llenarListas();

            return View("Index", viewModel);
        }

        public async Task<IActionResult> IndexVendedor()
        {
            InmueblesViewModel viewModel = new InmueblesViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(InmueblesQueries.SelectInmueblesByVendedorQuery(), new[] { OracleDBConnection.In("IdVendedor", await GetIdVendedor()) });

            List<Inmuebles> InmueblesList = new List<Inmuebles>();
            foreach (Inmuebles e in ModelParser.ParseInmuebles(data))
            {
                if (e.Eliminado == 0)
                {
                    InmueblesList.Add(e);
                }
            }

            viewModel.InmueblesList = InmueblesList;

            await llenarListas();

            return View("Index", viewModel);
        }

        public async Task<IActionResult> VerInmueble(int IdInmueble)
        {
            InmueblesVerViewModel viewModel = new InmueblesVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(InmueblesQueries.SelectInmueblesFiltroQuery(), new[] { OracleDBConnection.In("IdInmueble", IdInmueble) });


            if (data.Count > 0)
            {
                viewModel.IdInmueble = IdInmueble;
                viewModel.IdVendedor = int.Parse(data.FirstOrDefault()["IdVendedor"].ToString());
                viewModel.IdTipoInmueble = int.Parse(data.FirstOrDefault()["IdTipoInmueble"].ToString());
                viewModel.Direccion = data.FirstOrDefault()["Direccion"].ToString();
                viewModel.Precio = decimal.Parse(data.FirstOrDefault()["Precio"].ToString());
                viewModel.Metraje = int.Parse(data.FirstOrDefault()["Metraje"].ToString());
                viewModel.AntiguedadAnos = int.Parse(data.FirstOrDefault()["AntiguedadAnos"].ToString());
                viewModel.Modelo = data.FirstOrDefault()["Modelo"].ToString();
                viewModel.Material = data.FirstOrDefault()["Material"].ToString();
                viewModel.Descripcion = data.FirstOrDefault()["Descripcion"].ToString();
                

            }

            // Obtener Condiciones

            data = await con.SelectAsync(InmuebleCondicionQueries.SelectInmuebleCondicionesByIdInmuebleQuery(), new[] { OracleDBConnection.In("IdInmueble", IdInmueble) });

            List<Condicion> InmueblesList = new List<Condicion>();
            int count = 1;
            foreach (Condicion c in ModelParser.ParseCondicion(data))
            {
                if(count == 1)
                {
                    viewModel.IdCondicion1 = c.IdCondicion;
                    count++;
                    continue;
                }

                if (count == 2)
                {
                    viewModel.IdCondicion2 = c.IdCondicion;
                    count++;
                    continue;
                }
                if (count == 3)
                {
                    viewModel.IdCondicion3 = c.IdCondicion;
                    count++;
                    continue;
                }
            }

            await llenarListas();

            return View(viewModel);
        }

        public async Task<IActionResult> CrearInmueble()
        {
            InmueblesVerViewModel viewModel = new InmueblesVerViewModel();


            await llenarListas();
            return View(viewModel);


        }

        public async Task<IActionResult> CrearInmuebleAccion(InmueblesVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(InmueblesQueries.InsertInmuebleQuery(),
              new[] {
                OracleDBConnection.In("IdVendedor", await GetIdVendedor()),
                OracleDBConnection.In("IdTipoInmueble", model.IdTipoInmueble),
                OracleDBConnection.In("Direccion", model.Direccion),
                OracleDBConnection.In("Precio", model.Precio),
                OracleDBConnection.In("Metraje", model.Metraje),
                OracleDBConnection.In("AntiguedadAnos", model.AntiguedadAnos),
                OracleDBConnection.In("Modelo", model.Modelo),
                OracleDBConnection.In("Material", model.Material),
                OracleDBConnection.In("Descripcion", model.Descripcion),
                    IdSalida });

            var IdInmuebleLocal = IdSalida.Value;

            // Insertar condiciones

            (cantidadAfectados, salidas) = await con.InsertAsync(InmuebleCondicionQueries.InsertInmuebleCondicionesQuery(),
              new[] {
                OracleDBConnection.In("IdInmueble", IdInmuebleLocal),
                OracleDBConnection.In("IdCondicion", model.IdCondicion1),
                     });

            (cantidadAfectados, salidas) = await con.InsertAsync(InmuebleCondicionQueries.InsertInmuebleCondicionesQuery(),
              new[] {
                OracleDBConnection.In("IdInmueble", IdInmuebleLocal),
                OracleDBConnection.In("IdCondicion", model.IdCondicion2),
                     });

            (cantidadAfectados, salidas) = await con.InsertAsync(InmuebleCondicionQueries.InsertInmuebleCondicionesQuery(),
             new[] {
                OracleDBConnection.In("IdInmueble", IdInmuebleLocal),
                OracleDBConnection.In("IdCondicion", model.IdCondicion3),
                    });

            return RedirectToAction("Index", "Inmuebles");
        }

        public async Task<IActionResult> ModificarInmueble(int IdInmueble)
        {
            InmueblesVerViewModel viewModel = new InmueblesVerViewModel();

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(InmueblesQueries.SelectInmueblesFiltroQuery(), new[] { OracleDBConnection.In("IdInmueble", IdInmueble) });


            if (data.Count > 0)
            {
                viewModel.IdInmueble = IdInmueble;
                viewModel.IdVendedor = int.Parse(data.FirstOrDefault()["IdVendedor"].ToString());
                viewModel.IdTipoInmueble = int.Parse(data.FirstOrDefault()["IdTipoInmueble"].ToString());
                viewModel.Direccion = data.FirstOrDefault()["Direccion"].ToString();
                viewModel.Precio = decimal.Parse(data.FirstOrDefault()["Precio"].ToString());
                viewModel.Metraje = int.Parse(data.FirstOrDefault()["Metraje"].ToString());
                viewModel.AntiguedadAnos = int.Parse(data.FirstOrDefault()["AntiguedadAnos"].ToString());
                viewModel.Modelo = data.FirstOrDefault()["Modelo"].ToString();
                viewModel.Material = data.FirstOrDefault()["Material"].ToString();
                viewModel.Descripcion = data.FirstOrDefault()["Descripcion"].ToString();


            }

            // Obtener Condiciones

            data = await con.SelectAsync(InmuebleCondicionQueries.SelectInmuebleCondicionesByIdInmuebleQuery(), new[] { OracleDBConnection.In("IdInmueble", IdInmueble) });

            List<Condicion> InmueblesList = new List<Condicion>();
            int count = 1;
            foreach (Condicion c in ModelParser.ParseCondicion(data))
            {
                if (c.Eliminado == 1)
                {
                    continue;
                }

                if (count == 1)
                {
                    viewModel.IdCondicion1 = c.IdCondicion;
                    count++;
                    continue;
                }

                if (count == 2)
                {
                    viewModel.IdCondicion2 = c.IdCondicion;
                    count++;
                    continue;
                }
                if (count == 3)
                {
                    viewModel.IdCondicion3 = c.IdCondicion;
                    count++;
                    continue;
                }
            }

            await llenarListas();

            return View(viewModel);
        }

        public async Task<IActionResult> ModificarInmuebleAccion(InmueblesVerViewModel model)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var cantidadAfectadosUpdate = await con.UpdateAsync(InmueblesQueries.UpdateInmuebleQuery(),
              new[] {
                OracleDBConnection.In("IdInmueble", model.IdInmueble),
                OracleDBConnection.In("IdVendedor", await GetIdVendedor()),
                OracleDBConnection.In("IdTipoInmueble", model.IdTipoInmueble),
                OracleDBConnection.In("Direccion", model.Direccion),
                OracleDBConnection.In("Precio", model.Precio),
                OracleDBConnection.In("Metraje", model.Metraje),
                OracleDBConnection.In("AntiguedadAnos", model.AntiguedadAnos),
                OracleDBConnection.In("Modelo", model.Modelo),
                OracleDBConnection.In("Material", model.Material),
                OracleDBConnection.In("Descripcion", model.Descripcion),
                    IdSalida });

            var IdInmuebleLocal = model.IdInmueble;


            // Quitar condiciones anteriores 

            int dataDelete = await con.UpdateAsync(InmuebleCondicionQueries.DeleteInmuebleCondicionesByIdInmuebleQuery(),
                new[] { OracleDBConnection.In("IdInmueble", IdInmuebleLocal),
                });


            // Insertar condiciones

            var (cantidadAfectados, salidas) = await con.InsertAsync(InmuebleCondicionQueries.InsertInmuebleCondicionesQuery(),
              new[] {
                OracleDBConnection.In("IdInmueble", IdInmuebleLocal),
                OracleDBConnection.In("IdCondicion", model.IdCondicion1),
                     });

            (cantidadAfectados, salidas) = await con.InsertAsync(InmuebleCondicionQueries.InsertInmuebleCondicionesQuery(),
              new[] {
                OracleDBConnection.In("IdInmueble", IdInmuebleLocal),
                OracleDBConnection.In("IdCondicion", model.IdCondicion2),
                     });

            (cantidadAfectados, salidas) = await con.InsertAsync(InmuebleCondicionQueries.InsertInmuebleCondicionesQuery(),
             new[] {
                OracleDBConnection.In("IdInmueble", IdInmuebleLocal),
                OracleDBConnection.In("IdCondicion", model.IdCondicion3),
                    });

            return RedirectToAction("Index", "Inmuebles");
        }

        public async Task<IActionResult> EliminarInmueble(int IdInmueble)
        {
            OracleDBConnection con = new OracleDBConnection();

            int data = await con.UpdateAsync(InmueblesQueries.DeleteInmuebleQuery(),
                new[] { OracleDBConnection.In("IdInmueble", IdInmueble) });

            return RedirectToAction("Index", "Inmuebles");
        }





        private async Task<int> GetIdVendedor()
        {
            if(HttpContext.Session.GetInt32(SessionKeys.UserType) == 99)
            {
                return 1; // Admin
            }

            OracleDBConnection con = new OracleDBConnection();

            int? IdUsuario = HttpContext.Session.GetInt32(SessionKeys.IdUsuario);
            int IdVendedor = -1;

            

            var data = await con.SelectAsync(VendedoresQueries.SelectVendedoresByIdUsuarioQuery(), new[] { OracleDBConnection.In("IdUsuario", IdUsuario) });
            if(data.Count > 0)
            {
                IdVendedor = int.Parse(data.FirstOrDefault()["IdVendedor"].ToString());
            }

            return IdVendedor;

        }

        private async Task<bool> llenarListas()
        {

            // vendedores
            List<SelectListItem> listaVendedores = await Utils.GetVendedores();

            ViewBag.listaVendedores = listaVendedores;

            // tipos
            List<SelectListItem> listaTipos = await Utils.GetTiposInmueble();

            ViewBag.listaTipos = listaTipos;

            // condiciones
            List<SelectListItem> listaCondiciones = await Utils.GetCondiciones();

            ViewBag.listaCondiciones = listaCondiciones;
            return true;
        }

    }
}
