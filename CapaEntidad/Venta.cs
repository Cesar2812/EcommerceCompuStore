using System.Collections.Generic;

namespace CapaEntidad
{
    public class Venta
    {
        public int id_Venta { get; set; }

        public int idCliente { get; set; }

        public int TotalProducto { get; set; }

        public decimal MontoTotal { get; set; }

        public string Contacto { get; set; }

        public int idDepartamento { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public string NumeroTransaccion { get; set; }

        public string FechaTexto { get; set; }

        public List<Detalle_Venta> objDetalleVenta { get; set; }
    }
}
