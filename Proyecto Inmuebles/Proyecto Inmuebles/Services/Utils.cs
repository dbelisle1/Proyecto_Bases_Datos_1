using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using Proyecto_Inmuebles.DBConnection;
using Proyecto_Inmuebles.Models;
using Proyecto_Inmuebles.Parser;
using Proyecto_Inmuebles.Queries;
using System.Reflection;

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

        public static async Task<List<SelectListItem>> GetCompradores()
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(CompradoresQueries.SelectCompradoresQuery());


            List<SelectListItem> lVendendores = new List<SelectListItem>();


            foreach (Compradores t in ModelParser.ParseCompradores(data))
            {
                if (t.Eliminado == 0)
                {
                    lVendendores.Add(new SelectListItem { Text = t.Nombres, Value = t.IdComprador.ToString() });
                }

            }

            return lVendendores;

        }

        public static async Task<List<SelectListItem>> GetAgentes()
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(AgentesQueries.SelectAgentesQuery());


            List<SelectListItem> lVendendores = new List<SelectListItem>();


            foreach (Agentes t in ModelParser.ParseAgentes(data))
            {
                if (t.Eliminado == 0)
                {
                    lVendendores.Add(new SelectListItem { Text = t.Nombres, Value = t.IdAgente.ToString() });
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

        public static async Task<List<SelectListItem>> GetInmuebles()
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(InmueblesQueries.SelectInmueblesQuery());


            List<SelectListItem> lInmueble = new List<SelectListItem>();


            foreach (Inmuebles t in ModelParser.ParseInmuebles(data))
            {
                if (t.Eliminado == 0)
                {
                    lInmueble.Add(new SelectListItem { Text = t.Descripcion ?? "-", Value = t.IdInmueble.ToString() });
                }

            }

            return lInmueble;

        }

        public static async Task<List<SelectListItem>> GetFormasPago()
        {

            OracleDBConnection con = new OracleDBConnection();

            var data = await con.SelectAsync(FormasPagoQueries.SelectFormasPagoQuery());


            List<SelectListItem> lFormasPago = new List<SelectListItem>();


            foreach (FormasPago t in ModelParser.ParseFormasPago(data))
            {
                if (t.Eliminado == 0)
                {
                    lFormasPago.Add(new SelectListItem { Text = t.NombreFormaPago ?? "-", Value = t.IdFormaPago.ToString() });
                }

            }

            return lFormasPago;

        }

        public static async Task<bool> NotificarOferta(Notificaciones noti)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(NotificacionesQueries.InsertNotificacionOfertaQuery(),
              new[] {
                   OracleDBConnection.In("IdOferta", noti.IdOferta),
                   OracleDBConnection.In("Descripcion", noti.Descripcion),
                     IdSalida });
            return true;

        }

        public static async Task<bool> NotificarContraOferta(Notificaciones noti)
        {
            OracleDBConnection con = new OracleDBConnection();

            var IdSalida = OracleDBConnection.Out("IdSalida", OracleDbType.Int32);

            var (cantidadAfectados, salidas) = await con.InsertAsync(NotificacionesQueries.InsertNotificacionContraOfertaQuery(),
              new[] {
                   OracleDBConnection.In("IdContraoferta", noti.IdOferta),
                   OracleDBConnection.In("Descripcion", noti.Descripcion),
                     IdSalida });
            return true;
        }

    }
}
