namespace Proyecto_Inmuebles.Models
{
    public class OfertasViewModel : Ofertas
    {
        public List<OfertaDetalle> OfertasList {  get; set; }
        public bool esComprador { get; set; } = false;
        public int IdCondicionFiltro { get; set; } = 0;
    }

    public class OfertasVerViewModel : Ofertas
    {

    }


    public class OfertaDetalle {
        public int IdOferta { get; set; }
        public string? FechaHora { get; set; }    
        public decimal Monto { get; set; }
        public int PlazoDias { get; set; }
        public string NombreFormaPago { get; set; } = string.Empty;
        public string NombreEstado { get; set; } = string.Empty;

        public int IdPublicacion { get; set; }
        public int IdInmueble { get; set; }//
        public string Direccion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int IdTipoInmueble { get; set; }//
    }
}
