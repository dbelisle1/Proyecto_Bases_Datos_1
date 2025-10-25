
namespace Proyecto_Inmuebles.Queries
{
    public class FormasPagoQueries
    {
        public static string SelectFormasPagoQuery()
        {
            return "SELECT * FROM FormasPago";
        }

        public static string SelectFormaPagoFiltroQuery()
        {
            return "SELECT * FROM FormasPago WHERE IdFormaPago = :IdFormaPago";
        }

        public static string InsertFormaPagoQuery()
        {
            return "INSERT INTO FormasPago (IdFormaPago, NombreFormaPago) VALUES (:IdFormaPago, :NombreFormaPago) RETURNING IdFormaPago INTO :IdSalida";
        }

        public static string UpdateFormaPagoQuery()
        {
            return "UPDATE FormasPago SET NombreFormaPago = :NombreFormaPago WHERE IdFormaPago = :IdFormaPago";
        }

        public static string DeleteFormaPagoQuery()
        {
            return "UPDATE FormasPago SET Eliminado = 1 WHERE IdFormaPago = :IdFormaPago";
        }
    }
}