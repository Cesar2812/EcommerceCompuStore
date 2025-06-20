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

        //listado de compras del cliente
        public List<Detalle_Venta> ListarCompras(int idCliente)
        {
            return objCapaDatoVenta.ListarCompras(idCliente);
        }

        //metodo de negocio que regresa los productos mas vendidos
        public List<Detalle_Venta> ListarProductosMasVendidos()
        {
            return objCapaDatoVenta.ListarProductosMasVendidos();
        }


        //metodo de negocio que regresa las ventas por mes para el dashboard
        public List<Venta> ListarVentasPorMes()
        {
            return objCapaDatoVenta.ListarVentasDasboard();
        }

        //metodo de negocio que regresa los productos mas vendidos por mes para el dashboard
        public List<Producto> ListarProductosDasboard() 
        { 
            return objCapaDatoVenta.ListarProductosDasboard();
        }


        //metodo de negocio que retorna la factura de una venta por id
        public Venta ObtenerVentaDetalle(int id_Venta)
        {
            return objCapaDatoVenta.ObtenerVentaPorId(id_Venta);
        }
    }
}
