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
    }
}
