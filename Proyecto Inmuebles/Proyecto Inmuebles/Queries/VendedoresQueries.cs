namespace Proyecto_Inmuebles.Queries
{
    public class VendedoresQueries
    {
        /// <summary>
        ///  :IdUsuario, :Nombres, :Apellidos, :Direccion
        /// </summary>
        /// <returns></returns>
        public static string InsertVendedorQuery()
        {
            return @"INSERT INTO Vendedores (IdUsuario, Nombres, Apellidos, Direccion) 
                        VALUES ( :IdUsuario, :Nombres, :Apellidos, :Direccion)";
        }
    }
}
