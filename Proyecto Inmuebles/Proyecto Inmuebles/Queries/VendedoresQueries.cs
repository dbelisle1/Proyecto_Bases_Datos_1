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

        public static string SelectVendedoresQuery()
        {
            return "SELECT * FROM Vendedores";
        }

        public static string SelectVendedorFiltroQuery()
        {
            return "SELECT * FROM Vendedores WHERE IdVendedor = :IdVendedor";
        }


        public static string InsertVendedorAdmQuery()
        {
            return @"INSERT INTO Vendedores (IdUsuario, Nombres, Apellidos, Direccion) 
                        VALUES ( :IdUsuario, :Nombres, :Apellidos, :Direccion) RETURNING IdVendedor INTO :IdSalida";
        }


        public static string UpdateVendedorQuery()
        {
            return "UPDATE Vendedores SET IdUsuario = :IdUsuario, Nombres = :Nombres, Apellidos = :Apellidos, " +
                   "Direccion = :Direccion, CantidadInmueblesVendidos = :CantidadInmueblesVendidos " +
                   "WHERE IdVendedor = :IdVendedor";
        }

        public static string DeleteVendedorQuery()
        {
            return "UPDATE Vendedores SET Eliminado = 1, IdUsuario = NULL WHERE IdVendedor = :IdVendedor";
        }
    }
}
