public class CondicionQueries
{
    public static string SelectCondicionesQuery()
    {
        return "SELECT * FROM Condicion";
    }

    public static string SelectCondicionFiltroQuery()
    {
        return "SELECT * FROM Condicion WHERE IdCondicion = :IdCondicion";
    }

    public static string InsertCondicionQuery()
    {
        return "INSERT INTO Condicion (IdCondicion, NombreCondicion) VALUES (:IdCondicion, :NombreCondicion) RETURNING IdCondicion INTO :IdSalida";
    }

    public static string UpdateCondicionQuery()
    {
        return "UPDATE Condicion SET NombreCondicion = :NombreCondicion WHERE IdCondicion = :IdCondicion";
    }

    public static string DeleteCondicionQuery()
    {
        return "UPDATE Condicion SET Eliminado = 1 WHERE IdCondicion = :IdCondicion";
    }
}
