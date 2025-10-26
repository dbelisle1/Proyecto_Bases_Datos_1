public class InmuebleCondicionQueries
{
    public static string InsertInmuebleCondicionesQuery()
    {
        return "INSERT INTO InmuebleCondicion (IdInmueble, IdCondicion) VALUES (:IdInmueble, :IdCondicion)";
    }


    public static string SelectInmuebleCondicionesByIdInmuebleQuery()
    {
        return "SELECT c.IdCondicion, c.NombreCondicion, ic.Eliminado FROM InmuebleCondicion ic JOIN CONDICION c ON ic.IdCondicion = c.IdCondicion WHERE ic.IdInmueble = :IdInmueble";
    }

    public static string DeleteInmuebleCondicionesByIdInmuebleQuery()
    {
        return "DELETE FROM InmuebleCondicion WHERE IdInmueble = :IdInmueble";
    }
}
