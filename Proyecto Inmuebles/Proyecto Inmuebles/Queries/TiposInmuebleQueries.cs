namespace Proyecto_Inmuebles.Queries
{
    public class TiposInmuebleQueries
    {
        public static string SelectTiposQuery()
        {
            return "SELECT * FROM TiposInmueble";
        }

        public static string SelectTipoFiltroQuery()
        {
            return "SELECT * FROM TiposInmueble WHERE IdTipoInmueble = :IdTipoInmueble";
        }

        public static string InsertTipoQuery()
        {
            return "INSERT INTO TiposInmueble (IdTipoInmueble, NombreTipo) VALUES (:IdTipoInmueble, :NombreTipo) RETURNING IdTipoInmueble INTO :IdSalida";
        }

        public static string UpdateTipoQuery()
        {
            return "UPDATE TiposInmueble SET NombreTipo = :NombreTipo WHERE IdTipoInmueble = :IdTipoInmueble";
        }

        public static string DeleteTipoQuery()
        {
            return "UPDATE TiposInmueble SET Eliminado = 1 WHERE IdTipoInmueble = :IdTipoInmueble";
        }
    }
}
