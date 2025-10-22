namespace Proyecto_Inmuebles.Queries
{
    public class TipoUsuarioQueries
    {
        public static string SelectTiposQuery()
        {
            return "SELECT * FROM TipoUsuario";
        }

        public static string SelectTipoFiltroQuery()
        {
            return "SELECT * FROM TipoUsuario WHERE IdTipoUsuario = :IdTipoUsuario";
        }

        public static string UpdateTipoQuery()
        {
            return "UPDATE TipoUsuario SET NombreTipo = :NombreTipo WHERE IdTipoUsuario = :IdTipoUsuario";
        }

        public static string DeleteTipoQuery()
        {
            return "UPDATE TipoUsuario SET Eliminado = 1 WHERE IdTipoUsuario = :IdTipoUsuario";
        }
    }
}
