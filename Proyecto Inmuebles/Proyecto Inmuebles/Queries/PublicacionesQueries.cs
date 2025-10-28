namespace Proyecto_Inmuebles.Queries
{
    public class PublicacionesQueries
    {
        public static string SelectPublicacionesQuery()
        {
            return @"SELECT p.*
                    FROM Publicaciones p
                      WHERE p.Eliminado = 0
                    ORDER BY p.FechaPublicacion DESC";
        }
        public static string SelectPublicacionesIdVendedorQuery()
        {
            return @"SELECT p.*
                    FROM Publicaciones p
                    JOIN Inmuebles i ON i.IdInmueble = p.IdInmueble
                    WHERE i.IdVendedor = :IdVendedor
                      AND p.Eliminado = 0
                      AND i.Eliminado = 0
                    ORDER BY p.FechaPublicacion DESC";
        }

        public static string SelectPublicacionesIdAgenteQuery()
        {
            return @"SELECT p.*
                    FROM Publicaciones p
                    WHERE p.IdAgente = :IdAgente
                      AND p.Eliminado = 0
                    ORDER BY p.FechaPublicacion DESC";
        }

        public static string SelectPublicacionesByIdCondicion()
        {
            return @"
                SELECT
                      p.IdPublicacion, p.FechaPublicacion,
                      i.IdInmueble, i.Direccion, i.Precio, i.IdTipoInmueble,
                      v.IdVendedor, v.Nombres AS VendedorNombre, v.Apellidos AS VendedorApellidos,
                      a.IdAgente, a.Codigo AS AgenteCodigo, a.Nombres AS AgenteNombre, a.Apellidos AS AgenteApellidos
                    FROM Publicaciones p
                    JOIN Inmuebles i  ON i.IdInmueble = p.IdInmueble
                    JOIN InmuebleCondicion ic ON ic.IdInmueble = i.IdInmueble
                    JOIN Vendedores v  ON v.IdVendedor = i.IdVendedor
                    JOIN Agentes a     ON a.IdAgente = p.IdAgente
                    WHERE ic.IdCondicion = :IdCondicion
                      AND p.Eliminado = 0 AND i.Eliminado = 0 AND ic.Eliminado = 0
                      AND v.Eliminado = 0 AND a.Eliminado = 0
                        AND ( :IdTipoInmueble = 0 OR i.IdTipoInmueble = :IdTipoInmueble )
                          AND ( :PrecioMin = 0 OR i.Precio >= :PrecioMin )
                          AND ( :PrecioMax = 0 OR i.Precio <= :PrecioMax )
                          AND ( :Direccion IS NULL OR UPPER(i.Direccion) LIKE '%' || UPPER(:Direccion) || '%' )
                    ORDER BY p.FechaPublicacion DESC";
        }

       


        public static string InsertPublicacionQuery()
        {
            return @"INSERT INTO Publicaciones (IdInmueble, IdAgente, FechaPublicacion) 
                        VALUES ( :IdInmueble, :IdAgente, :FechaPublicacion) RETURNING IdPublicacion INTO :IdSalida";
        }
    }
}
