namespace Proyecto_Inmuebles.Queries
{
    public class InmueblesQueries
    {
        public static string SelectInmueblesQuery()
        {
            return "SELECT * FROM Inmuebles";
        }

        public static string SelectInmueblesByVendedorQuery()
        {
            return "SELECT * FROM Inmuebles WHERE IdVendedor = :IdVendedor";
        }

        public static string SelectInmueblesFiltroQuery()
        {
            return "SELECT * FROM Inmuebles WHERE IdInmueble = :IdInmueble";
        }

        public static string InsertInmuebleQuery()
        {
            return "INSERT INTO Inmuebles ( IdVendedor, IdTipoInmueble, Direccion, Precio, Metraje, AntiguedadAnos, Modelo, Material, Descripcion) " +
                   "VALUES ( :IdVendedor, :IdTipoInmueble, :Direccion, :Precio, :Metraje, :AntiguedadAnos, :Modelo, :Material, :Descripcion) " +
                   "RETURNING IdInmueble INTO :IdSalida";
        }

        public static string UpdateInmuebleQuery()
        {
            return "UPDATE Inmuebles SET IdVendedor = :IdVendedor, IdTipoInmueble = :IdTipoInmueble, Direccion = :Direccion, " +
                   "Precio = :Precio, Metraje = :Metraje, AntiguedadAnos = :AntiguedadAnos, Modelo = :Modelo, Material = :Material, Descripcion = :Descripcion " +
                   "WHERE IdInmueble = :IdInmueble";
        }

        public static string DeleteInmuebleQuery()
        {
            return "UPDATE Inmuebles SET Eliminado = 1 WHERE IdInmueble = :IdInmueble";
        }
    }
}
