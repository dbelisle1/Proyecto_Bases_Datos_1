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

    }
}
