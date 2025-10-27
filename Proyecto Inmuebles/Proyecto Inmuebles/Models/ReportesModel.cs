namespace Proyecto_Inmuebles.Models
{
    public class FormasPagoUsadasPorComprador
    {
        public string Comprador { get; set; } = string.Empty;
        public string? NombreFormaPago { get; set; }
        public int Veces { get; set; }
    }

    public class InmueblesPorVendedorYTipo
    {
        public int IdVendedor { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public string NombreTipo { get; set; } = string.Empty;
        public int Inmuebles { get; set; }
        public decimal TotalListado { get; set; }
        public decimal PrecioProm { get; set; }
    }

    public class TopInteresPorInmueble
    {
        public int IdInmueble { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public int Ofertas { get; set; }
    }

    public class ResumenPorVendedor
    {
        public int IdVendedor { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public int InmueblesListados { get; set; }
        public int InmueblesVendidos { get; set; }
        public decimal Ingresos { get; set; }
    }

    public class VentasPorAgenteFormaPago
    {
        public string Codigo { get; set; } = string.Empty;
        public string Agente { get; set; } = string.Empty;
        public string? NombreFormaPago { get; set; }
        public int Ventas { get; set; }
        public decimal Ingresos { get; set; }
        public decimal TicketProm { get; set; }
    }

    public class TasaAceptacionPorAgente
    {
        public string Codigo { get; set; } = string.Empty;
        public int Aceptadas { get; set; }
        public int Total { get; set; }
        public decimal PctAceptacion { get; set; }
    }

    public class ContraofertasPorAgente
    {
        public string Codigo { get; set; } = string.Empty;
        public int Contraofertas { get; set; }
        public int Aceptadas { get; set; }
        public decimal PctAceptadas { get; set; }
    }

    public class ResultadosPorAgenteAllTime
    {
        public string Codigo { get; set; } = string.Empty;
        public int Publicaciones { get; set; }
        public int Ventas { get; set; }
        public decimal Ingresos { get; set; }
    }
}
