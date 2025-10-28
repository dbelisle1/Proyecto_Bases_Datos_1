using System;
using System.Collections.Generic;
using Proyecto_Inmuebles.Models;

namespace Proyecto_Inmuebles.Parser
{
    public static class ReporteParser
    {
        // ---------- Helpers (Dictionary-based) ----------
        private static object? Get(Dictionary<string, object?> row, string key)
        {
            if (row.TryGetValue(key, out var v)) return v;

            // Try common case variants
            var u = key.ToUpperInvariant();
            if (row.TryGetValue(u, out v)) return v;

            var l = key.ToLowerInvariant();
            if (row.TryGetValue(l, out v)) return v;

            // Last resort: linear scan (case-insensitive)
            foreach (var kv in row)
            {
                if (string.Equals(kv.Key, key, StringComparison.OrdinalIgnoreCase))
                    return kv.Value;
            }
            return null;
        }

        private static string? ToStr(object? v)
        {
            if (v == null || v is DBNull) return null;
            return v switch
            {
                string s => s,
                DateTime dt => dt.ToString("yyyy-MM-dd"),
                _ => Convert.ToString(v)
            };
        }

        private static string Nn(string? s) => s ?? string.Empty;

        private static int ToInt(object? v)
        {
            if (v == null || v is DBNull) return 0;
            if (v is int i) return i;
            if (v is long l) return (int)l;
            if (v is decimal d) return (int)d;
            if (v is double db) return (int)db;
            if (v is float f) return (int)f;
            if (v is short sh) return sh;
            if (v is byte b) return b;
            if (v is string s && int.TryParse(s, out var p)) return p;
            try { return Convert.ToInt32(v); } catch { return 0; }
        }

        private static decimal ToDec(object? v)
        {
            if (v == null || v is DBNull) return 0m;
            if (v is decimal d) return d;
            if (v is double db) return (decimal)db;
            if (v is float f) return (decimal)f;
            if (v is long l) return l;
            if (v is int i) return i;
            if (v is string s && decimal.TryParse(s, out var p)) return p;
            try { return Convert.ToDecimal(v); } catch { return 0m; }
        }

        // ========== 1) Formas de pago usadas por un comprador ==========
        public static List<FormasPagoUsadasPorComprador> ParseFormasPagoUsadasPorComprador(List<Dictionary<string, object?>> data)
        {
            var list = new List<FormasPagoUsadasPorComprador>(data.Count);
            foreach (var row in data)
            {
                list.Add(new FormasPagoUsadasPorComprador
                {
                    Comprador = Nn(ToStr(Get(row, "COMPRADOR"))),
                    NombreFormaPago = ToStr(Get(row, "NOMBREFORMAPAGO")),
                    Veces = ToInt(Get(row, "VECES"))
                });
            }
            return list;
        }

        // ========== 2) Inmuebles por vendedor y tipo ==========
        public static List<InmueblesPorVendedorYTipo> ParseInmueblesPorVendedorYTipo(List<Dictionary<string, object?>> data)
        {
            var list = new List<InmueblesPorVendedorYTipo>(data.Count);
            foreach (var row in data)
            {
                list.Add(new InmueblesPorVendedorYTipo
                {
                    IdVendedor = ToInt(Get(row, "IDVENDEDOR")),
                    Vendedor = Nn(ToStr(Get(row, "VENDEDOR"))),
                    NombreTipo = Nn(ToStr(Get(row, "NOMBRETIPO"))),
                    Inmuebles = ToInt(Get(row, "INMUEBLES")),
                    TotalListado = ToDec(Get(row, "TOTAL_LISTADO")),
                    PrecioProm = ToDec(Get(row, "PRECIO_PROM"))
                });
            }
            return list;
        }

        // ========== 3) Top interés por inmueble ==========
        public static List<TopInteresPorInmueble> ParseTopInteresPorInmueble(List<Dictionary<string, object?>> data)
        {
            var list = new List<TopInteresPorInmueble>(data.Count);
            foreach (var row in data)
            {
                list.Add(new TopInteresPorInmueble
                {
                    IdInmueble = ToInt(Get(row, "IDINMUEBLE")),
                    Direccion = Nn(ToStr(Get(row, "DIRECCION"))),
                    Ofertas = ToInt(Get(row, "OFERTAS"))
                });
            }
            return list;
        }

        // ========== 4) Resumen por vendedor ==========
        public static List<ResumenPorVendedor> ParseResumenPorVendedor(List<Dictionary<string, object?>> data)
        {
            var list = new List<ResumenPorVendedor>(data.Count);
            foreach (var row in data)
            {
                list.Add(new ResumenPorVendedor
                {
                    IdVendedor = ToInt(Get(row, "IDVENDEDOR")),
                    Vendedor = Nn(ToStr(Get(row, "VENDEDOR"))),
                    InmueblesListados = ToInt(Get(row, "INMUEBLES_LISTADOS")),
                    InmueblesVendidos = ToInt(Get(row, "INMUEBLES_VENDIDOS")),
                    Ingresos = ToDec(Get(row, "INGRESOS"))
                });
            }
            return list;
        }

        // ========== 5) Ventas por agente y forma de pago ==========
        public static List<VentasPorAgenteFormaPago> ParseVentasPorAgenteFormaPago(List<Dictionary<string, object?>> data)
        {
            var list = new List<VentasPorAgenteFormaPago>(data.Count);
            foreach (var row in data)
            {
                list.Add(new VentasPorAgenteFormaPago
                {
                    Codigo = Nn(ToStr(Get(row, "CODIGO"))),
                    Agente = Nn(ToStr(Get(row, "AGENTE"))),
                    NombreFormaPago = ToStr(Get(row, "NOMBREFORMAPAGO")),
                    Ventas = ToInt(Get(row, "VENTAS")),
                    Ingresos = ToDec(Get(row, "INGRESOS")),
                    TicketProm = ToDec(Get(row, "TICKET_PROM"))
                });
            }
            return list;
        }

        // ========== 6) Tasa de aceptación por agente ==========
        public static List<TasaAceptacionPorAgente> ParseTasaAceptacionPorAgente(List<Dictionary<string, object?>> data)
        {
            var list = new List<TasaAceptacionPorAgente>(data.Count);
            foreach (var row in data)
            {
                list.Add(new TasaAceptacionPorAgente
                {
                    Codigo = Nn(ToStr(Get(row, "CODIGO"))),
                    Aceptadas = ToInt(Get(row, "ACEPTADAS")),
                    Total = ToInt(Get(row, "TOTAL")),
                    PctAceptacion = ToDec(Get(row, "PCT_ACEPTACION"))
                });
            }
            return list;
        }

        // ========== 7) Contraofertas: volumen y efectividad por agente ==========
        public static List<ContraofertasPorAgente> ParseContraofertasPorAgente(List<Dictionary<string, object?>> data)
        {
            var list = new List<ContraofertasPorAgente>(data.Count);
            foreach (var row in data)
            {
                list.Add(new ContraofertasPorAgente
                {
                    Codigo = Nn(ToStr(Get(row, "CODIGO"))),
                    Contraofertas = ToInt(Get(row, "CONTRAOFERTAS")),
                    Aceptadas = ToInt(Get(row, "ACEPTADAS")),
                    PctAceptadas = ToDec(Get(row, "PCT_ACEPTADAS"))
                });
            }
            return list;
        }

        // ========== 8) Resultados por agente (All Time) ==========
        public static List<ResultadosPorAgenteAllTime> ParseResultadosPorAgenteAllTime(List<Dictionary<string, object?>> data)
        {
            var list = new List<ResultadosPorAgenteAllTime>(data.Count);
            foreach (var row in data)
            {
                list.Add(new ResultadosPorAgenteAllTime
                {
                    Codigo = Nn(ToStr(Get(row, "CODIGO"))),
                    Publicaciones = ToInt(Get(row, "PUBLICACIONES")),
                    Ventas = ToInt(Get(row, "VENTAS")),
                    Ingresos = ToDec(Get(row, "INGRESOS"))
                });
            }
            return list;
        }


        public static List<VentaFiltro> ParseVentaFiltro(List<Dictionary<string, object?>> data)
        {
            var list = new List<VentaFiltro>(data.Count);
            foreach (var row in data)
            {
                list.Add(new VentaFiltro
                {
                    IdVenta = ToInt(Get(row, "IdVenta")),
                    FechaCierre = ToStr(Get(row, "FechaCierre")),
                    PrecioFinal = ToDec(Get(row, "PrecioFinal")),
                    IdInmueble = ToInt(Get(row, "IdInmueble")),
                    Direccion = Nn(ToStr(Get(row, "Direccion"))),
                    IdTipoInmueble = ToInt(Get(row, "IdTipoInmueble")),
                    IdVendedor = ToInt(Get(row, "IdVendedor")),
                    IdComprador = ToInt(Get(row, "IdComprador"))
                });
            }
            return list;
        }

        public static List<PublicacionPorCondicion> ParsePublicacionesPorCondicion(List<Dictionary<string, object?>> data)
        {
            var list = new List<PublicacionPorCondicion>(data.Count);
            foreach (var row in data)
            {
                list.Add(new PublicacionPorCondicion
                {
                    IdPublicacion = ToInt(Get(row, "IdPublicacion")),
                    FechaPublicacion = ToStr(Get(row, "FechaPublicacion")),
                    IdInmueble = ToInt(Get(row, "IdInmueble")),
                    Direccion = Nn(ToStr(Get(row, "Direccion"))),
                    Precio = ToDec(Get(row, "Precio")),
                    IdTipoInmueble = ToInt(Get(row, "IdTipoInmueble")),
                    IdVendedor = ToInt(Get(row, "IdVendedor")),
                    VendedorNombre = Nn(ToStr(Get(row, "VendedorNombre"))),
                    VendedorApellidos = Nn(ToStr(Get(row, "VendedorApellidos"))),
                    IdAgente = ToInt(Get(row, "IdAgente")),
                    AgenteCodigo = Nn(ToStr(Get(row, "AgenteCodigo"))),
                    AgenteNombre = Nn(ToStr(Get(row, "AgenteNombre"))),
                    AgenteApellidos = Nn(ToStr(Get(row, "AgenteApellidos")))
                });
            }
            return list;
        }

        public static List<OfertaDetalle> ParseOfertaDetalle(List<Dictionary<string, object?>> data)
        {
            var list = new List<OfertaDetalle>(data.Count);
            foreach (var row in data)
            {
                list.Add(new OfertaDetalle
                {
                    IdOferta = ToInt(Get(row, "IdOferta")),
                    FechaHora = ToStr(Get(row, "FechaHora")),
                    Monto = ToDec(Get(row, "Monto")),
                    PlazoDias = ToInt(Get(row, "PlazoDias")),
                    NombreFormaPago = Nn(ToStr(Get(row, "NombreFormaPago"))),
                    NombreEstado = Nn(ToStr(Get(row, "NombreEstado"))),

                    IdPublicacion = ToInt(Get(row, "IdPublicacion")),
                    IdInmueble = ToInt(Get(row, "IdInmueble")),
                    Direccion = Nn(ToStr(Get(row, "Direccion"))),
                    Precio = ToDec(Get(row, "Precio")),
                    IdTipoInmueble = ToInt(Get(row, "IdTipoInmueble"))
                });
            }
            return list;
        }
    }
}
