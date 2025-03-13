using System;

namespace CapaEntidad
{
    public class Detalle_Venta
    {
        public int id_DetalleVenta { get; set; }

        public int idVenta { get; set; }

        public Producto objProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        public string NumeroTransaccion { get; set; }

        public DateTime FechaVenta { get; set; }

        public decimal TotalIVA { get; set; }
    }
}
