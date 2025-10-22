using Proyecto_Inmuebles.Models;

namespace Proyecto_Inmuebles.Parser
{
    public static class ModelParser
    {
       
        public static List<TipoUsuario> ParseTipoUsuario(List<Dictionary<string, object?>> data)
        {
            var list = new List<TipoUsuario>(data.Count);
            foreach (var row in data)
            {
                list.Add(new TipoUsuario
                {
                    IdTipoUsuario = ToInt(Get(row, "IdTipoUsuario")),
                    NombreTipo = Nn(ToStr(Get(row, "NombreTipo"))),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<TiposInmueble> ParseTiposInmueble(List<Dictionary<string, object?>> data)
        {
            var list = new List<TiposInmueble>(data.Count);
            foreach (var row in data)
            {
                list.Add(new TiposInmueble
                {
                    IdTipoInmueble = ToInt(Get(row, "IdTipoInmueble")),
                    NombreTipo = Nn(ToStr(Get(row, "NombreTipo"))),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<FormasPago> ParseFormasPago(List<Dictionary<string, object?>> data)
        {
            var list = new List<FormasPago>(data.Count);
            foreach (var row in data)
            {
                list.Add(new FormasPago
                {
                    IdFormaPago = ToInt(Get(row, "IdFormaPago")),
                    NombreFormaPago = Nn(ToStr(Get(row, "NombreFormaPago"))),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<EstadoOferta> ParseEstadoOferta(List<Dictionary<string, object?>> data)
        {
            var list = new List<EstadoOferta>(data.Count);
            foreach (var row in data)
            {
                list.Add(new EstadoOferta
                {
                    IdEstadoOferta = ToInt(Get(row, "IdEstadoOferta")),
                    NombreEstado = Nn(ToStr(Get(row, "NombreEstado"))),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<Condicion> ParseCondicion(List<Dictionary<string, object?>> data)
        {
            var list = new List<Condicion>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Condicion
                {
                    IdCondicion = ToInt(Get(row, "IdCondicion")),
                    NombreCondicion = Nn(ToStr(Get(row, "NombreCondicion"))),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        // ---------- 2) Users & profiles ----------
        public static List<Usuarios> ParseUsuarios(List<Dictionary<string, object?>> data)
        {
            var list = new List<Usuarios>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Usuarios
                {
                    IdUsuario = ToInt(Get(row, "IdUsuario")),
                    IdTipoUsuario = ToInt(Get(row, "IdTipoUsuario")),
                    NombreUsuario = Nn(ToStr(Get(row, "NombreUsuario"))),
                    Contrasena = Nn(ToStr(Get(row, "Contrasena"))),
                    Correo = ToStr(Get(row, "Correo")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<Compradores> ParseCompradores(List<Dictionary<string, object?>> data)
        {
            var list = new List<Compradores>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Compradores
                {
                    IdComprador = ToInt(Get(row, "IdComprador")),
                    IdUsuario = ToInt(Get(row, "IdUsuario")),
                    Nombres = Nn(ToStr(Get(row, "Nombres"))),
                    Apellidos = Nn(ToStr(Get(row, "Apellidos"))),
                    Direccion = ToStr(Get(row, "Direccion")),
                    Telefono = ToStr(Get(row, "Telefono")),
                    EstadoCivil = ToStr(Get(row, "EstadoCivil")),
                    Nacionalidad = ToStr(Get(row, "Nacionalidad")),
                    Edad = ToIntN(Get(row, "Edad")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<Vendedores> ParseVendedores(List<Dictionary<string, object?>> data)
        {
            var list = new List<Vendedores>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Vendedores
                {
                    IdVendedor = ToInt(Get(row, "IdVendedor")),
                    IdUsuario = ToInt(Get(row, "IdUsuario")),
                    Nombres = Nn(ToStr(Get(row, "Nombres"))),
                    Apellidos = Nn(ToStr(Get(row, "Apellidos"))),
                    Direccion = ToStr(Get(row, "Direccion")),
                    CantidadInmueblesVendidos = ToIntN(Get(row, "CantidadInmueblesVendidos")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<Agentes> ParseAgentes(List<Dictionary<string, object?>> data)
        {
            var list = new List<Agentes>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Agentes
                {
                    IdAgente = ToInt(Get(row, "IdAgente")),
                    IdUsuario = ToInt(Get(row, "IdUsuario")),
                    Codigo = Nn(ToStr(Get(row, "Codigo"))),
                    Nombres = Nn(ToStr(Get(row, "Nombres"))),
                    Apellidos = Nn(ToStr(Get(row, "Apellidos"))),
                    Telefono = ToStr(Get(row, "Telefono")),
                    Correo = ToStr(Get(row, "Correo")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        // ---------- 3) Properties ----------
        public static List<Inmuebles> ParseInmuebles(List<Dictionary<string, object?>> data)
        {
            var list = new List<Inmuebles>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Inmuebles
                {
                    IdInmueble = ToInt(Get(row, "IdInmueble")),
                    IdVendedor = ToInt(Get(row, "IdVendedor")),
                    IdTipoInmueble = ToInt(Get(row, "IdTipoInmueble")),
                    Direccion = Nn(ToStr(Get(row, "Direccion"))),
                    Precio = ToDec(Get(row, "Precio")),
                    Metraje = ToDecN(Get(row, "Metraje")),
                    AntiguedadAnos = ToIntN(Get(row, "AntiguedadAnos")),
                    Modelo = ToStr(Get(row, "Modelo")),
                    Material = ToStr(Get(row, "Material")),
                    Descripcion = ToStr(Get(row, "Descripcion")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<InmuebleCondicion> ParseInmuebleCondicion(List<Dictionary<string, object?>> data)
        {
            var list = new List<InmuebleCondicion>(data.Count);
            foreach (var row in data)
            {
                list.Add(new InmuebleCondicion
                {
                    IdInmueble = ToInt(Get(row, "IdInmueble")),
                    IdCondicion = ToInt(Get(row, "IdCondicion")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        // ---------- 4) Listings ----------
        public static List<Publicaciones> ParsePublicaciones(List<Dictionary<string, object?>> data)
        {
            var list = new List<Publicaciones>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Publicaciones
                {
                    IdPublicacion = ToInt(Get(row, "IdPublicacion")),
                    IdInmueble = ToInt(Get(row, "IdInmueble")),
                    IdAgente = ToInt(Get(row, "IdAgente")),
                    FechaPublicacion = ToDate(Get(row, "FechaPublicacion")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        // ---------- 5) Transactions ----------
        public static List<Ofertas> ParseOfertas(List<Dictionary<string, object?>> data)
        {
            var list = new List<Ofertas>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Ofertas
                {
                    IdOferta = ToInt(Get(row, "IdOferta")),
                    IdPublicacion = ToInt(Get(row, "IdPublicacion")),
                    IdComprador = ToInt(Get(row, "IdComprador")),
                    Monto = ToDec(Get(row, "Monto")),
                    IdFormaPago = ToInt(Get(row, "IdFormaPago")),
                    PlazoDias = ToIntN(Get(row, "PlazoDias")),
                    FechaHora = ToDate(Get(row, "FechaHora")),
                    IdEstadoOferta = ToInt(Get(row, "IdEstadoOferta")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<Contraofertas> ParseContraofertas(List<Dictionary<string, object?>> data)
        {
            var list = new List<Contraofertas>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Contraofertas
                {
                    IdContraoferta = ToInt(Get(row, "IdContraoferta")),
                    IdOferta = ToInt(Get(row, "IdOferta")),
                    Monto = ToDec(Get(row, "Monto")),
                    PlazoDias = ToIntN(Get(row, "PlazoDias")),
                    FechaHora = ToDate(Get(row, "FechaHora")),
                    IdEstadoOferta = ToInt(Get(row, "IdEstadoOferta")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<Prestamos> ParsePrestamos(List<Dictionary<string, object?>> data)
        {
            var list = new List<Prestamos>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Prestamos
                {
                    IdPrestamo = ToInt(Get(row, "IdPrestamo")),
                    IdComprador = ToInt(Get(row, "IdComprador")),
                    CodigoPrestamo = Nn(ToStr(Get(row, "CodigoPrestamo"))),
                    Descripcion = ToStr(Get(row, "Descripcion")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        public static List<Ventas> ParseVentas(List<Dictionary<string, object?>> data)
        {
            var list = new List<Ventas>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Ventas
                {
                    IdVenta = ToInt(Get(row, "IdVenta")),
                    IdOfertaAceptada = ToInt(Get(row, "IdOfertaAceptada")),
                    IdPrestamo = ToIntN(Get(row, "IdPrestamo")),
                    IdFormaPago = ToInt(Get(row, "IdFormaPago")),
                    FechaCierre = ToDate(Get(row, "FechaCierre")),
                    PrecioFinal = ToDec(Get(row, "PrecioFinal")),
                    PlazoDias = ToIntN(Get(row, "PlazoDias")),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        // ---------- 6) Notifications ----------
        public static List<Notificaciones> ParseNotificaciones(List<Dictionary<string, object?>> data)
        {
            var list = new List<Notificaciones>(data.Count);
            foreach (var row in data)
            {
                list.Add(new Notificaciones
                {
                    IdNotificacion = ToInt(Get(row, "IdNotificacion")),
                    IdOferta = ToIntN(Get(row, "IdOferta")),
                    IdContraoferta = ToIntN(Get(row, "IdContraoferta")),
                    FechaHora = ToDate(Get(row, "FechaHora")),
                    Descripcion = Nn(ToStr(Get(row, "Descripcion"))),
                    Eliminado = ToInt(Get(row, "Eliminado"))
                });
            }
            return list;
        }

        private static object? Get(Dictionary<string, object?> row, string key)
        {
            if (row.TryGetValue(key, out var v)) return v;
            var upper = key.ToUpperInvariant();
            if (row.TryGetValue(upper, out v)) return v;
            var lower = key.ToLowerInvariant();
            if (row.TryGetValue(lower, out v)) return v;
            return null;
        }

        private static int ToInt(object? v)
        {
            if (v == null || v is DBNull) return 0;
            if (v is int i) return i;
            if (v is long l) return checked((int)l);
            if (v is decimal dec) return (int)dec;
            if (v is double d) return (int)d;
            if (v is string s && int.TryParse(s, out var x)) return x;
            return Convert.ToInt32(v);
        }

        private static int? ToIntN(object? v)
        {
            if (v == null || v is DBNull) return null;
            return ToInt(v);
        }

        private static decimal ToDec(object? v)
        {
            if (v == null || v is DBNull) return 0m;
            if (v is decimal d) return d;
            if (v is double dd) return Convert.ToDecimal(dd);
            if (v is float ff) return Convert.ToDecimal(ff);
            if (v is int ii) return ii;
            if (v is long ll) return ll;
            if (v is string s && decimal.TryParse(s, out var x)) return x;
            return Convert.ToDecimal(v);
        }

        private static decimal? ToDecN(object? v)
        {
            if (v == null || v is DBNull) return null;
            return ToDec(v);
        }

        private static DateTime ToDate(object? v)
        {
            if (v == null || v is DBNull) return default;
            if (v is DateTime dt) return dt;
            return DateTime.Parse(v.ToString()!);
        }

        private static string? ToStr(object? v) => v is DBNull ? null : v?.ToString();

        private static string Nn(string? s) => s ?? string.Empty;
    }
}
