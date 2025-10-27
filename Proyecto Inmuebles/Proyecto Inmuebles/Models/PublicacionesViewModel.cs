namespace Proyecto_Inmuebles.Models
{
    public class PublicacionesViewModel : Publicaciones
    {
        public List<Publicaciones> PublicacionesList {  get; set; }
        public bool esComprador { get; set; } = false;
    }

    public class PublicacionesVerViewModel : Publicaciones
    {

    }
}
