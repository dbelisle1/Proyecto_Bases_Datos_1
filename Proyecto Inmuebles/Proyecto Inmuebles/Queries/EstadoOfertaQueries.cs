namespace Proyecto_Inmuebles.Queries
{
    public class EstadoOfertaQueries
    {
        public static string SelectEstadosQuery()
        {
            return "SELECT * FROM EstadoOferta";
        }

        public static string SelectEstadoOfertaFiltroQuery()
        {
            return "SELECT * FROM EstadoOferta WHERE IdEstadoOferta = :IdEstadoOferta";
        }

        public static string InsertEstadoOfertaQuery()
        {
            return "INSERT INTO EstadoOferta (IdEstadoOferta, NombreEstado) VALUES (:IdEstadoOferta, :NombreEstado) RETURNING IdEstadoOferta INTO :IdSalida";
        }

        public static string UpdateEstadoOfertaQuery()
        {
            return "UPDATE EstadoOferta SET NombreEstado = :NombreEstado WHERE IdEstadoOferta = :IdEstadoOferta";
        }

        public static string DeleteEstadoOfertaQuery()
        {
            return "UPDATE EstadoOferta SET Eliminado = 1 WHERE IdEstadoOferta = :IdEstadoOferta";
        }
    }
}
