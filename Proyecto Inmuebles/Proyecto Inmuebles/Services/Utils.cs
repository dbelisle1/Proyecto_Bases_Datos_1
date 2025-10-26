using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;

namespace Proyecto_Inmuebles.Services
{
    public static class Utils
    {
        public static async Task<List<SelectListItem>> GetCondiciones()
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(CondicionQueries.SelectCondicionesQuery());


            List<SelectListItem> lCondiciones = new List<SelectListItem>();


            foreach (Condicion t in ModelParser.ParseCondicion(data))
            {
                if (t.Eliminado == 0)
                {
                    lCondiciones.Add(new SelectListItem { Text = t.NombreCondicion, Value = t.IdCondicion.ToString() });
                }

            }

            return lCondiciones;

        }

        public static async Task<List<SelectListItem>> GetVendedores()
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(VendedoresQueries.SelectVendedoresQuery());


            List<SelectListItem> lVendendores = new List<SelectListItem>();


            foreach (Vendedores t in ModelParser.ParseVendedores(data))
            {
                if (t.Eliminado == 0)
                {
                    lVendendores.Add(new SelectListItem { Text = t.Nombres, Value = t.IdVendedor.ToString() });
                }

            }

            return lVendendores;

        }

        public static async Task<List<SelectListItem>> GetTiposInmueble()
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(TiposInmuebleQueries.SelectTiposQuery());


            List<SelectListItem> lTiposInmueble = new List<SelectListItem>();


            foreach (TiposInmueble t in ModelParser.ParseTiposInmueble(data))
            {
                if (t.Eliminado == 0)
                {
                    lTiposInmueble.Add(new SelectListItem { Text = t.NombreTipo, Value = t.IdTipoInmueble.ToString() });
                }

            }

            return lTiposInmueble;

        }

        


    }
}
