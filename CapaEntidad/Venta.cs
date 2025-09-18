using System.Collections.Generic;

namespace CapaEntidad
{
    public class Venta
    {
        public int id_Venta { get; set; }

        public int id_Cliente { get; set; }

        public int TotalProducto { get; set; }

        public decimal MontoTotal { get; set; }

        public string idMunicipio { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public string NumeroTransaccion { get; set; }

        public string FechaTexto { get; set; }

        public List<Detalle_Venta> objDetalleVenta { get; set; }= new List<Detalle_Venta>();

        public decimal MontoTotalIva { get; set; }




        //campos para la Factura en la vista del admin
        public Cliente objCliente { get; set; }
        public Municipio objMunicipio { get; set; }


        //para el dasboard en la vista del admin
        public int Cantidad { get; set; }
        public string mes { get; set; }
    }
}
