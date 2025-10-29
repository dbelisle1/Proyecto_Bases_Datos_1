namespace Proyecto_Inmuebles.Queries
{
    public class OfertasQueries
    {

        public static string SelectOfertasByIdComprador()
        {
            return @"
                SELECT
                  o.IdOferta,
                  o.FechaHora,
                  o.Monto,
                  o.PlazoDias,
                  fp.NombreFormaPago,
                   fp.IdFormaPago,
                  eo.NombreEstado,
                  p.IdPublicacion,
                  i.IdInmueble,
                  i.Direccion,
                  i.Precio,
                  i.IdTipoInmueble
                FROM Ofertas o
                JOIN FormasPago    fp ON fp.IdFormaPago = o.IdFormaPago
                JOIN EstadoOferta  eo ON eo.IdEstadoOferta = o.IdEstadoOferta
                JOIN Publicaciones p  ON p.IdPublicacion = o.IdPublicacion
                JOIN Inmuebles     i  ON i.IdInmueble = p.IdInmueble
                WHERE o.IdComprador = :IdComprador
                  AND o.Eliminado = 0
                  AND fp.Eliminado = 0
                  AND eo.Eliminado = 0
                  AND p.Eliminado  = 0
                  AND i.Eliminado  = 0
                ORDER BY o.FechaHora DESC";
        }

        public static string SelectOfertasByIdAgente()
        {
            return @"
                SELECT
                  o.IdOferta,
                  o.FechaHora,
                  o.Monto,
                  o.PlazoDias,
                  fp.NombreFormaPago,
                   fp.IdFormaPago,
                  eo.NombreEstado,
                  p.IdPublicacion,
                  i.IdInmueble,
                  i.Direccion,
                  i.Precio,
                  i.IdTipoInmueble
                FROM Ofertas o
                JOIN FormasPago    fp ON fp.IdFormaPago = o.IdFormaPago
                JOIN EstadoOferta  eo ON eo.IdEstadoOferta = o.IdEstadoOferta
                JOIN Publicaciones p  ON p.IdPublicacion = o.IdPublicacion
                JOIN Inmuebles     i  ON i.IdInmueble = p.IdInmueble
                WHERE p.IdAgente   = :IdAgente
                  AND o.Eliminado  = 0
                  AND fp.Eliminado = 0
                  AND eo.Eliminado = 0
                  AND p.Eliminado  = 0
                  AND i.Eliminado  = 0
                ORDER BY o.FechaHora DESC";
        }
        public static string InsertOfertaQuery()
        {
            return @"
               INSERT INTO Ofertas (
                  IdPublicacion, IdComprador, Monto, IdFormaPago, PlazoDias, FechaHora, IdEstadoOferta, Eliminado
                )
                VALUES (
                  :IdPublicacion,
                  :IdComprador,
                  :Monto,
                  :IdFormaPago,
                  :PlazoDias,
                    :FechaHora,
                  ( SELECT IdEstadoOferta
                    FROM EstadoOferta
                    WHERE UPPER(NombreEstado) = 'DISPONIBLE' AND Eliminado = 0
                    FETCH FIRST 1 ROW ONLY
                  ),
                  0
                )
                RETURNING IdOferta INTO :IdSalida
                ";
        }
    }
}
