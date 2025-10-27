namespace Proyecto_Inmuebles.Models
{
    public class VentasViewModel : Ventas
    {
        public List<Ventas> VentasList {  get; set; }
        public List<VentaFiltro> VentasFiltroList {  get; set; }

        public int IdAgenteFiltro { get; set; }
    }

    public class VentasVerViewModel : Ventas
    {

    }

    public class VentaFiltro
    {
        public int IdVenta { get; set; }
        public string? FechaCierre { get; set; }  
        public decimal PrecioFinal { get; set; }
        public int IdInmueble { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public int IdTipoInmueble { get; set; }
        public int IdVendedor { get; set; }
        public int IdComprador { get; set; }
    }
}
