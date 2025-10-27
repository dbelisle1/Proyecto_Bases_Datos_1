namespace Proyecto_Inmuebles.Queries
{
    public class NotificacionesQueries
    {
       public static string SelectNotificacionesIdCompradorQuery()
        {
            return @"SELECT n.IdNotificacion,
                   CASE WHEN n.IdOferta IS NOT NULL THEN 'Oferta' ELSE 'Contraoferta' END AS Tipo,
                   n.FechaHora,
                   n.Descripcion,
                   n.IdOferta,
                   n.IdContraoferta
            FROM Notificaciones n
            JOIN Ofertas o
              ON (n.IdOferta = o.IdOferta)
            WHERE o.IdComprador = :IdComprador
              AND n.Eliminado = 0 AND o.Eliminado = 0

            UNION ALL

            SELECT n.IdNotificacion,
                   'Contraoferta' AS Tipo,
                   n.FechaHora,
                   n.Descripcion,
                   n.IdOferta,
                   n.IdContraoferta
            FROM Notificaciones n
            JOIN Contraofertas co    ON n.IdContraoferta = co.IdContraoferta
            JOIN Ofertas o           ON o.IdOferta = co.IdOferta
            WHERE o.IdComprador = :IdComprador
              AND n.Eliminado = 0 AND co.Eliminado = 0 AND o.Eliminado = 0
            ORDER BY FechaHora DESC";
        }

        public static string SelectNotificacionesIdVendedorQuery()
        {
            return @"SELECT n.IdNotificacion,
                       'Oferta' AS Tipo,
                       n.FechaHora,
                       n.Descripcion,
                       n.IdOferta,
                       n.IdContraoferta
                FROM Notificaciones n
                JOIN Ofertas o          ON n.IdOferta = o.IdOferta
                JOIN Publicaciones p    ON p.IdPublicacion = o.IdPublicacion
                JOIN Inmuebles i        ON i.IdInmueble = p.IdInmueble
                WHERE i.IdVendedor = :IdVendedor
                  AND n.Eliminado = 0 AND o.Eliminado = 0 AND p.Eliminado = 0 AND i.Eliminado = 0

                UNION ALL

                SELECT n.IdNotificacion,
                       'Contraoferta' AS Tipo,
                       n.FechaHora,
                       n.Descripcion,
                       n.IdOferta,
                       n.IdContraoferta
                FROM Notificaciones n
                JOIN Contraofertas co   ON n.IdContraoferta = co.IdContraoferta
                JOIN Ofertas o          ON o.IdOferta = co.IdOferta
                JOIN Publicaciones p    ON p.IdPublicacion = o.IdPublicacion
                JOIN Inmuebles i        ON i.IdInmueble = p.IdInmueble
                WHERE i.IdVendedor = :IdVendedor
                  AND n.Eliminado = 0 AND co.Eliminado = 0 AND o.Eliminado = 0
                  AND p.Eliminado = 0 AND i.Eliminado = 0
                ORDER BY FechaHora DESC";      
        }

        public static string SelectNotificacionesIdAgenteQuery()
        {
            return @"SELECT n.IdNotificacion,
                       'Oferta' AS Tipo,
                       n.FechaHora,
                       n.Descripcion,
                       n.IdOferta,
                       n.IdContraoferta
                FROM Notificaciones n
                JOIN Ofertas o          ON n.IdOferta = o.IdOferta
                JOIN Publicaciones p    ON p.IdPublicacion = o.IdPublicacion
                WHERE p.IdAgente = :IdAgente
                  AND n.Eliminado = 0 AND o.Eliminado = 0 AND p.Eliminado = 0

                UNION ALL

                SELECT n.IdNotificacion,
                       'Contraoferta' AS Tipo,
                       n.FechaHora,
                       n.Descripcion,
                       n.IdOferta,
                       n.IdContraoferta
                FROM Notificaciones n
                JOIN Contraofertas co   ON n.IdContraoferta = co.IdContraoferta
                JOIN Ofertas o          ON o.IdOferta = co.IdOferta
                JOIN Publicaciones p    ON p.IdPublicacion = o.IdPublicacion
                WHERE p.IdAgente = :IdAgente
                  AND n.Eliminado = 0 AND co.Eliminado = 0 AND o.Eliminado = 0 AND p.Eliminado = 0
                ORDER BY FechaHora DESC";
        }

        public static string InsertNotificacionOfertaQuery()
        {
            return @"INSERT INTO Notificaciones (IdNotificacion, IdOferta, IdContraoferta, FechaHora, Descripcion, Eliminado)
            VALUES (DEFAULT, :IdOferta, NULL, SYSTIMESTAMP, :Descripcion, 0)
            RETURNING IdNotificacion INTO :IdSalida";
        }

        public static string InsertNotificacionContraOfertaQuery()
        {
            return @"INSERT INTO Notificaciones (IdNotificacion, IdOferta, IdContraoferta, FechaHora, Descripcion, Eliminado)
            VALUES (DEFAULT, NULL, :IdContraoferta, SYSTIMESTAMP, :Descripcion, 0)
            RETURNING IdNotificacion INTO :IdSalida";
        }
    }
}
