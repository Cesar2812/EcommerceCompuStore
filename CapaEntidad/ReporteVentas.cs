namespace CapaEntidad
{
    public class ReporteVentas
    {
        public string FechaDeVenta { get; set; }

        public string Cliente { get; set; }

        public string Producto { get; set; }

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        public string NumeroTransaccion { get; set; }
    }
}
