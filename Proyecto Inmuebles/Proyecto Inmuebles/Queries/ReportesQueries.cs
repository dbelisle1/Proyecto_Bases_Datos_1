using Microsoft.AspNetCore.Mvc.RazorPages;
using Proyecto_Inmuebles.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Proyecto_Inmuebles.Queries
{
    public class ReportesQueries
    {


        //Formas de pago usadas por un comprador en sus ventas
        public static string FormasPagoCompradorQuery() {

            return @"SELECT u.NOMBREUSUARIO AS COMPRADOR, fp.NOMBREFORMAPAGO, COUNT(*) AS VECES
                FROM VENTAS v
                JOIN OFERTAS o ON o.IDOFERTA = v.IDOFERTAACEPTADA
                JOIN COMPRADORES c ON c.IDCOMPRADOR = o.IDCOMPRADOR
                JOIN USUARIOS u ON u.IDUSUARIO = c.IDUSUARIO
                LEFT JOIN FORMASPAGO fp ON fp.IDFORMAPAGO = v.IDFORMAPAGO
                WHERE c.IDCOMPRADOR = :IdComprador
                  AND NVL(v.ELIMINADO,0)=0
                GROUP BY u.NOMBREUSUARIO, fp.NOMBREFORMAPAGO
                ORDER BY VECES DESC";
        
        }

        //Inmuebles por vendedor y tipo(con tipo)
        public static string InmueblesVendendorTipoQuery()
        {

            return @"SELECT v.IDVENDEDOR,
                   v.NOMBRES || ' ' || v.APELLIDOS AS VENDEDOR,
                   ti.NOMBRETIPO,
                   COUNT(*) AS INMUEBLES,
                   SUM(i.PRECIO) AS TOTAL_LISTADO,
                   ROUND(AVG(i.PRECIO)) AS PRECIO_PROM
            FROM INMUEBLES i
            JOIN VENDEDORES v ON v.IDVENDEDOR = i.IDVENDEDOR
            JOIN TIPOSINMUEBLE ti ON ti.IDTIPOINMUEBLE = i.IDTIPOINMUEBLE
            WHERE NVL(i.ELIMINADO,0)=0 AND v.IdVendedor = :IdVendedor AND ti.IDTIPOINMUEBLE = :IdTipoInmueble  
            GROUP BY v.IDVENDEDOR, v.NOMBRES, v.APELLIDOS, ti.NOMBRETIPO
            ORDER BY INMUEBLES DESC";

        }

        //Top interés por inmueble
        //WHERE o.FECHAHORA BETWEEN DATE '2025-10-09' AND DATE  '2025-10-18
        public static string TopInteresInmuebleQuery()
        {

            return @"SELECT i.IDINMUEBLE, i.DIRECCION, COUNT(o.IDOFERTA) AS OFERTAS
                FROM INMUEBLES i
                JOIN PUBLICACIONES p ON p.IDINMUEBLE = i.IDINMUEBLE
                LEFT JOIN OFERTAS o ON o.IDPUBLICACION = p.IDPUBLICACION
                WHERE o.FECHAHORA BETWEEN TO_DATE(:StartDate, 'YYYY-MM-DD') AND TO_DATE(:EndDate,   'YYYY-MM-DD')
                GROUP BY i.IDINMUEBLE, i.DIRECCION
                ORDER BY OFERTAS DESC";

        }


        //Resumen por vendedor
        public static string ResumenVendedorQuery()
        {

            return @"SELECT vnd.IDVENDEDOR,
                   vnd.NOMBRES || ' ' || vnd.APELLIDOS AS VENDEDOR,
                   COUNT(DISTINCT i.IDINMUEBLE) AS INMUEBLES_LISTADOS,
                   COUNT(DISTINCT v.IDOFERTAACEPTADA) AS INMUEBLES_VENDIDOS,
                   NVL(SUM(v.PRECIOFINAL), 0) AS INGRESOS
            FROM VENDEDORES vnd
            LEFT JOIN INMUEBLES i ON i.IDVENDEDOR = vnd.IDVENDEDOR AND NVL(i.ELIMINADO,0)=0
            LEFT JOIN PUBLICACIONES p ON p.IDINMUEBLE = i.IDINMUEBLE AND NVL(p.ELIMINADO,0)=0
            LEFT JOIN OFERTAS o     ON o.IDPUBLICACION = p.IDPUBLICACION AND NVL(o.ELIMINADO,0)=0
            LEFT JOIN VENTAS v      ON v.IDOFERTAACEPTADA = o.IDOFERTA AND NVL(v.ELIMINADO,0)=0
            WHERE vnd.IDVENDEDOR = :IdVendedor
            GROUP BY vnd.IDVENDEDOR, vnd.NOMBRES, vnd.APELLIDOS
            ORDER BY INGRESOS DESC";

        }


        //Ventas por agente y forma de pago
        public static string VentasAgenteFormaPagoQuery()
        {

            return @"SELECT a.CODIGO,
                   a.NOMBRES || ' ' || a.APELLIDOS AS AGENTE,
                   fp.NOMBREFORMAPAGO,
                   COUNT(*) AS VENTAS,
                   SUM(v.PRECIOFINAL) AS INGRESOS,
                    ROUND(AVG(v.PRECIOFINAL)) AS TICKET_PROM
            FROM VENTAS v
            JOIN OFERTAS o  ON o.IDOFERTA = v.IDOFERTAACEPTADA
            JOIN PUBLICACIONES p ON p.IDPUBLICACION = o.IDPUBLICACION
            JOIN AGENTES a ON a.IDAGENTE = p.IDAGENTE
            LEFT JOIN FORMASPAGO fp ON fp.IDFORMAPAGO = v.IDFORMAPAGO
            WHERE v.FECHACIERRE BETWEEN TO_DATE(:StartDate, 'YYYY-MM-DD') AND TO_DATE(:EndDate,   'YYYY-MM-DD')
              AND NVL(v.ELIMINADO,0)=0
            GROUP BY a.CODIGO, a.NOMBRES, a.APELLIDOS, fp.NOMBREFORMAPAGO
            ORDER BY INGRESOS DESC";
        }

        //Tasa de aceptación de ofertas por agente
        public static string TasaAceptacionAgenteQuery()
        {

            return @"SELECT a.CODIGO,
                   SUM(CASE WHEN o.IDESTADOOFERTA= 2 THEN 1 ELSE 0 END) AS ACEPTADAS,
                    COUNT(*) AS TOTAL,
                   ROUND(100 * SUM(CASE WHEN o.IDESTADOOFERTA = 2 THEN 1 ELSE 0 END) / NULLIF(COUNT(*), 0), 2) AS PCT_ACEPTACION
            FROM OFERTAS o
            JOIN PUBLICACIONES p ON p.IDPUBLICACION = o.IDPUBLICACION
            JOIN AGENTES a ON a.IDAGENTE = p.IDAGENTE
            WHERE o.FECHAHORA BETWEEN TO_DATE(:StartDate, 'YYYY-MM-DD') AND TO_DATE(:EndDate,   'YYYY-MM-DD')
              AND NVL(o.ELIMINADO,0)=0
            GROUP BY a.CODIGO
            ORDER BY PCT_ACEPTACION DESC";
        }



        // Contraofertas: volumen y efectividad por agente
        public static string ContraOfertaEfectividadQuery()
        {

            return @" SELECT a.CODIGO,
                   COUNT(*) AS CONTRAOFERTAS,
                   SUM(CASE WHEN c.IDESTADOOFERTA = 2 THEN 1 ELSE 0 END) AS ACEPTADAS,
                   ROUND(100 * SUM(CASE WHEN c.IDESTADOOFERTA = 2 THEN 1 ELSE 0 END) / NULLIF(COUNT(*), 0), 2) AS PCT_ACEPTADAS
            FROM CONTRAOFERTAS c
            JOIN OFERTAS o ON o.IDOFERTA = c.IDOFERTA
            JOIN PUBLICACIONES p ON p.IDPUBLICACION = o.IDPUBLICACION
            JOIN AGENTES a ON a.IDAGENTE = p.IDAGENTE
            WHERE c.FECHAHORA BETWEEN TO_DATE(:StartDate, 'YYYY-MM-DD') AND  TO_DATE(:EndDate,   'YYYY-MM-DD')
              AND NVL(c.ELIMINADO,0)=0
            GROUP BY a.CODIGO
            ORDER BY CONTRAOFERTAS DESC";
        }

        //Resultados por agente All Time
        public static string ResultadosAgenteQuery()
        {

            return @" SELECT a.CODIGO,
                   COUNT(DISTINCT p.IDPUBLICACION) AS PUBLICACIONES,
                    COUNT(DISTINCT v.IDOFERTAACEPTADA) AS VENTAS,
                   NVL(SUM(v.PRECIOFINAL), 0) AS INGRESOS

            FROM AGENTES a
            LEFT JOIN PUBLICACIONES p ON p.IDAGENTE = a.IDAGENTE AND NVL(p.ELIMINADO,0)=0
            LEFT JOIN OFERTAS o ON o.IDPUBLICACION = p.IDPUBLICACION AND NVL(o.ELIMINADO,0)=0
            LEFT JOIN VENTAS v ON v.IDOFERTAACEPTADA = o.IDOFERTA AND NVL(v.ELIMINADO,0)=0
            WHERE a.IDAGENTE = :IdAgente
            GROUP BY a.CODIGO
            ORDER BY INGRESOS DESC NULLS LAST";
        }

    }
}
