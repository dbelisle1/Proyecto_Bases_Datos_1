namespace Proyecto_Inmuebles.Queries
{
    public class AgentesQueries
    {
        /// <summary>
        /// :IdUsuario, :Codigo, :Nombres, :Apellidos, :Telefono, :Correo
        /// </summary>
        /// <returns></returns>
        public static string InsertAgenteQuery()
        {
            return @"INSERT INTO Agentes ( IdUsuario, Codigo, Nombres, Apellidos, Telefono, Correo) 
                        VALUES (:IdUsuario, :Codigo, :Nombres, :Apellidos, :Telefono, :Correo) ";
        }

        public static string SelectAgentesQuery()
        {
            return "SELECT * FROM Agentes";
        }

        public static string SelectAgenteFiltroQuery()
        {
            return "SELECT * FROM Agentes WHERE IdAgente = :IdAgente";
        }

        public static string InsertAgenteAdmQuery()
        {
            return "INSERT INTO Agentes ( IdUsuario, Codigo, Nombres, Apellidos, Telefono, Correo) " +
                   "VALUES ( :IdUsuario, :Codigo, :Nombres, :Apellidos, :Telefono, :Correo) " +
                   "RETURNING IdAgente INTO :IdSalida";
        }

        public static string UpdateAgenteQuery()
        {
            return "UPDATE Agentes SET IdUsuario = :IdUsuario, Codigo = :Codigo, Nombres = :Nombres, " +
                   "Apellidos = :Apellidos, Telefono = :Telefono, Correo = :Correo " +
                   "WHERE IdAgente = :IdAgente";
        }

        public static string DeleteAgenteQuery()
        {
            return "UPDATE Agentes SET Eliminado = 1, IdUsuario = NULL WHERE IdAgente = :IdAgente";
        }

    }
}
