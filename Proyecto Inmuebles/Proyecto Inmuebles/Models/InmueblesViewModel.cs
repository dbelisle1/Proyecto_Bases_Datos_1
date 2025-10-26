namespace Proyecto_Inmuebles.Models
{
    public class InmueblesViewModel : Inmuebles
    {
        public List<Inmuebles> InmueblesList {  get; set; }
    }

    public class InmueblesVerViewModel : Inmuebles
    {
        public int IdCondicion1 { get; set; }
        public int IdCondicion2 { get; set; }
        public int IdCondicion3 { get; set; }
    }
}
