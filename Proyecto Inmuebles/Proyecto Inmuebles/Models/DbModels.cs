namespace Proyecto_Inmuebles.Models
{
    public   class TipoUsuario
    {
        public  int IdTipoUsuario { get; set; }
        public  string NombreTipo { get; set; } = null!;
        public  int Eliminado { get; set; }
    }

    public  class TiposInmueble
    {
        public  int IdTipoInmueble { get; set; }
        public  string NombreTipo { get; set; } = null!;
        public  int Eliminado { get; set; }
    }

    public  class FormasPago
    {
        public  int IdFormaPago { get; set; }
        public  string NombreFormaPago { get; set; } = null!;
        public  int Eliminado { get; set; }
    }

    public  class EstadoOferta
    {
        public  int IdEstadoOferta { get; set; }
        public  string NombreEstado { get; set; } = null!;
        public  int Eliminado { get; set; }
    }

    public  class Condicion
    {
        public  int IdCondicion { get; set; }
        public  string NombreCondicion { get; set; } = null!;
        public  int Eliminado { get; set; }
    }

    public  class Usuarios
    {
        public  int IdUsuario { get; set; }
        public  int IdTipoUsuario { get; set; }
        public  string NombreUsuario { get; set; } = null!;
        public  string Contrasena { get; set; } = null!;
        public  string? Correo { get; set; }
        public  int Eliminado { get; set; }
    }

    public  class Compradores
    {
        public  int IdComprador { get; set; }
        public  int IdUsuario { get; set; }
        public  string Nombres { get; set; } = null!;
        public  string Apellidos { get; set; } = null!;
        public  string? Direccion { get; set; }
        public  string? Telefono { get; set; }
        public  string? EstadoCivil { get; set; }
        public  string? Nacionalidad { get; set; }
        public  int? Edad { get; set; }
        public  int Eliminado { get; set; }
    }

    public  class Vendedores
    {
        public  int IdVendedor { get; set; }
        public  int IdUsuario { get; set; }
        public  string Nombres { get; set; } = null!;
        public  string Apellidos { get; set; } = null!;
        public  string? Direccion { get; set; }
        public  int? CantidadInmueblesVendidos { get; set; }
        public  int Eliminado { get; set; }
    }

    public  class Agentes
    {
        public  int IdAgente { get; set; }
        public  int IdUsuario { get; set; }
        public  string Codigo { get; set; } = null!;
        public  string Nombres { get; set; } = null!;
        public  string Apellidos { get; set; } = null!;
        public  string? Telefono { get; set; }
        public  string? Correo { get; set; }
        public  int Eliminado { get; set; }
    }

    // 3) Properties & conditions
    public  class Inmuebles
    {
        public  int IdInmueble { get; set; }
        public  int IdVendedor { get; set; }
        public  int IdTipoInmueble { get; set; }
        public  string Direccion { get; set; } = null!;
        public  decimal Precio { get; set; }
        public  decimal? Metraje { get; set; }
        public  int? AntiguedadAnos { get; set; }
        public  string? Modelo { get; set; }
        public  string? Material { get; set; }
        public  string? Descripcion { get; set; }
        public  int Eliminado { get; set; }
    }

    public  class InmuebleCondicion
    {
        public  int IdInmueble { get; set; }
        public  int IdCondicion { get; set; }
        public  int Eliminado { get; set; }
    }

    // 4) Listings
    public  class Publicaciones
    {
        public  int IdPublicacion { get; set; }
        public  int IdInmueble { get; set; }
        public  int IdAgente { get; set; }
        public  DateTime FechaPublicacion { get; set; }
        public  int Eliminado { get; set; }
    }

    // 5) Transactions
    public  class Ofertas
    {
        public  int IdOferta { get; set; }
        public  int IdPublicacion { get; set; }
        public  int IdComprador { get; set; }
        public  decimal Monto { get; set; }
        public  int IdFormaPago { get; set; }
        public  int? PlazoDias { get; set; }
        public  DateTime FechaHora { get; set; }
        public  int IdEstadoOferta { get; set; }
        public  int Eliminado { get; set; }
    }

    public  class Contraofertas
    {
        public  int IdContraoferta { get; set; }
        public  int IdOferta { get; set; }
        public  decimal Monto { get; set; }
        public  int? PlazoDias { get; set; }
        public  DateTime FechaHora { get; set; }
        public  int IdEstadoOferta { get; set; }
        public  int Eliminado { get; set; }
    }

    public  class Prestamos
    {
        public  int IdPrestamo { get; set; }
        public  int IdComprador { get; set; }
        public  string CodigoPrestamo { get; set; } = null!;
        public  string? Descripcion { get; set; }
        public  int Eliminado { get; set; }
    }

    public  class Ventas
    {
        public  int IdVenta { get; set; }
        public  int IdOfertaAceptada { get; set; }
        public  int? IdPrestamo { get; set; }
        public  int IdFormaPago { get; set; }
        public  DateTime FechaCierre { get; set; }
        public  decimal PrecioFinal { get; set; }
        public  int? PlazoDias { get; set; }
        public  int Eliminado { get; set; }
    }

    // 6) Notifications
    public  class Notificaciones
    {
        public  int IdNotificacion { get; set; }
        public  int? IdOferta { get; set; }
        public  int? IdContraoferta { get; set; }
        public  DateTime FechaHora { get; set; }
        public  string Descripcion { get; set; } = null!;
        public  int Eliminado { get; set; }
    }

    public class NotificacionesCompuesta : Notificaciones
    {
        public string Tipo { get; set; } = null!;

    }
}
