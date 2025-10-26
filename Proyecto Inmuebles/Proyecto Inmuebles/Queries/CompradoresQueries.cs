namespace Proyecto_Inmuebles.Queries
{
    public class CompradoresQueries
    {
        /// <summary>
        /// :IdUsuario, :Nombres, :Apellidos, :Direccion, :Telefono, :EstadoCivil, :Nacionalidad, :Edad
        /// </summary>
        /// <returns></returns>
        public static string InsertCompradorQuery()
        {
            return @"INSERT INTO Compradores  ( IdUsuario, Nombres, Apellidos, Direccion, Telefono, EstadoCivil, Nacionalidad, Edad) 
                        VALUES ( :IdUsuario, :Nombres, :Apellidos, :Direccion, :Telefono, :EstadoCivil, :Nacionalidad, :Edad)";
        }

        public static string SelectCompradoresQuery()
        {
            return "SELECT * FROM Compradores";
        }

        public static string SelectCompradoresFiltroQuery()
        {
            return "SELECT * FROM Compradores WHERE IdComprador = :IdComprador";
        }

        public static string InsertCompradoresQuery()
        {
            return "INSERT INTO Compradores (IdUsuario, Nombres, Apellidos, Direccion, Telefono, EstadoCivil, Nacionalidad, Edad) " +
                   "VALUES (:IdUsuario, :Nombres, :Apellidos, :Direccion, :Telefono, :EstadoCivil, :Nacionalidad, :Edad) " +
                   "RETURNING IdComprador INTO :IdSalida";
        }

        public static string UpdateCompradoresQuery()
        {
            return "UPDATE Compradores SET IdUsuario = :IdUsuario, Nombres = :Nombres, Apellidos = :Apellidos, Direccion = :Direccion, " +
                   "Telefono = :Telefono, EstadoCivil = :EstadoCivil, Nacionalidad = :Nacionalidad, Edad = :Edad " +
                   "WHERE IdComprador = :IdComprador";
        }

        public static string DeleteCompradoresQuery()
        {
            return "UPDATE Compradores SET Eliminado = 1, IdUsuario = NULL WHERE IdComprador = :IdComprador";
        }
    }
}
