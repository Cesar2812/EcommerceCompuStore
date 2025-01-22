using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;
using System.Data;

namespace CapaNegocio
{
    public class CNVenta
    {
        private CDVenta objCapaDatoVenta = new CDVenta();
        public bool RegistrarVenta(Venta objVenta, DataTable Detalle_Venta, out string Mensaje)
        {
            return objCapaDatoVenta.RegistrarVenta(objVenta, Detalle_Venta, out Mensaje);

        }

        public List<Detalle_Venta> ListarCompras(int idCliente)
        {
            return objCapaDatoVenta.ListarCompras(idCliente);
        }



    }
}
