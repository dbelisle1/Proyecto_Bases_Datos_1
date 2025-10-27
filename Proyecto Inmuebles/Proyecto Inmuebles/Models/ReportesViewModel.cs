namespace Proyecto_Inmuebles.Models
{
    public class ReportesViewModel
    {
        public List<FormasPagoUsadasPorComprador> Reporte1 = new List<FormasPagoUsadasPorComprador>();
        public List<InmueblesPorVendedorYTipo> Reporte2 = new List<InmueblesPorVendedorYTipo>();
        public List<TopInteresPorInmueble> Reporte3 = new List<TopInteresPorInmueble>();
        public List<ResumenPorVendedor> Reporte4 = new List<ResumenPorVendedor>();
        public List<VentasPorAgenteFormaPago> Reporte5 = new List<VentasPorAgenteFormaPago>();
        public List<TasaAceptacionPorAgente> Reporte6 = new List<TasaAceptacionPorAgente>();
        public List<ContraofertasPorAgente> Reporte7 = new List<ContraofertasPorAgente>();
        public List<ResultadosPorAgenteAllTime> Reporte8 = new List<ResultadosPorAgenteAllTime>();

        // ---------------- R1) Formas de pago usadas por un comprador en sus ventas ----------------
        public int? R1_IdComprador { get; set; }

        // ---------------- R2) Inmuebles por vendedor y tipo (con tipo) ----------------
        public int? R2_IdVendedor { get; set; }
        public int? R2_IdTipoInmueble { get; set; }

        // ---------------- R3) Top interés por inmueble (rango en OFERTAS.FECHAHORA) ----------------
        // Use format "YYYY-MM-DD"
        public string? R3_StartDateOfertas { get; set; }
        public string? R3_EndDateOfertas { get; set; }

        // ---------------- R4) Resumen por vendedor (opcional rango en PUBLICACIONES.FECHAPUBLICACION) ----------------
        public int? R4_IdVendedor { get; set; }

        // ---------------- R5) Ventas por agente y forma de pago (rango en VENTAS.FECHACIERRE) ----------------
        public string? R5_StartDateVentas { get; set; }
        public string? R5_EndDateVentas { get; set; }

        // ---------------- R6) Tasa de aceptación de ofertas por agente (rango en OFERTAS.FECHAHORA) ----------------
        public string? R6_StartDateOfertas { get; set; }
        public string? R6_EndDateOfertas { get; set; }

        // ---------------- R7) Contraofertas: volumen y efectividad por agente (rango en CONTRAOFERTAS.FECHAHORA) ----------------
        public string? R7_StartDateContraofertas { get; set; }
        public string? R7_EndDateContraofertas { get; set; }

        // ---------------- R8) Resultados por agente (All Time) + opcional rango en PUBLICACIONES.FECHAPUBLICACION ----------------
        public int? R8_IdAgente { get; set; }
    }
}
