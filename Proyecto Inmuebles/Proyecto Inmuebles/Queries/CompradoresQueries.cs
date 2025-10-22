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
    }
}
