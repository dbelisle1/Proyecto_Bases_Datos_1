namespace Proyecto_Inmuebles.Models
{
    public class PublicacionesViewModel : Publicaciones
    {
    
        public List<PublicacionPorCondicion> PublicacionesFiltroList {  get; set; }
        public bool esComprador { get; set; } = false;
        public int IdTipoUsuario { get; set; } = 0;

        public int IdCondicionFiltro { get; set; } = 0;
        public int IdTipoInmuebleFiltro { get; set; } = 0;
        public int PrecioMinFiltro { get; set; } = 0;
        public int PrecioMaxFiltro { get; set; } = 0;
        public string? DireccionFiltro { get; set; } = string.Empty;


    }

    public class PublicacionesVerViewModel : Publicaciones
    {

    }


    public class PublicacionPorCondicion
    {
        public int IdPublicacion { get; set; }
        public string? FechaPublicacion { get; set; } 
        public int IdInmueble { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int IdTipoInmueble { get; set; }

        public int IdVendedor { get; set; }
        public string VendedorNombre { get; set; } = string.Empty;
        public string VendedorApellidos { get; set; } = string.Empty;

        public int IdAgente { get; set; }
        public string AgenteCodigo { get; set; } = string.Empty;
        public string AgenteNombre { get; set; } = string.Empty;
        public string AgenteApellidos { get; set; } = string.Empty;
    }
}
