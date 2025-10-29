namespace Proyecto_Inmuebles.Queries
{
    public class VentasQueries
    {
        public static string SelectVentasByIdAgenteQuery()
        {
            return @"SELECT
                      v.IdVenta,
                      v.FechaCierre,
                      v.PrecioFinal,
                      i.IdInmueble,
                      i.Direccion,
                      i.IdTipoInmueble,
                      i.IdVendedor,   
                      o.IdComprador    
                    FROM Ventas v
                    JOIN Ofertas o        ON o.IdOferta = v.IdOfertaAceptada
                    JOIN Publicaciones p  ON p.IdPublicacion = o.IdPublicacion
                    JOIN Inmuebles i      ON i.IdInmueble = p.IdInmueble
                    WHERE p.IdAgente = :IdAgente
                      AND v.Eliminado = 0
                      AND o.Eliminado = 0
                      AND p.Eliminado = 0
                      AND i.Eliminado = 0
                    ORDER BY v.FechaCierre DESC";
        }


        public static string InsertVentaAceptadaQueryQuery()
        {
            return @"INSERT INTO Ventas (
                  IdVenta, IdOfertaAceptada, IdPrestamo, IdFormaPago, FechaCierre, PrecioFinal, PlazoDias, Eliminado
                ) VALUES (
                  DEFAULT, :IdOfertaAceptada, :IdPrestamo, :IdFormaPago, SYSDATE, :PrecioFinal, :PlazoDias, 0
                )
                RETURNING IdVenta INTO :IdSalida";
        }

    }
}
