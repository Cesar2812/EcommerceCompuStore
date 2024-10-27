using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CNCliente_Producto
    {
        private CDCliente_Producto objCliente_Produto = new CDCliente_Producto();

        public bool ExisteCarrito(int idCliente, int idProducto)
        {
            return objCliente_Produto.ExisteCarrito(idCliente, idProducto);
        }

        public bool OperacionCarrito(int idCliente, int idProducto, bool sumar, out string mensaje)
        {
            return objCliente_Produto.OperacionCarrito(idCliente, idProducto, sumar, out mensaje);
        }

        public int CantidadEnCarrito(int idCliente)
        {
            return objCliente_Produto.CantidadEnCarrito(idCliente);
        }

        public List<Cliente_Producto> ListarProductoCarrito(int idCliente)
        {
            return objCliente_Produto.ListarProductoCarrito(idCliente);
        }

        public bool EliminarProductoEnCarrito(int idCliente, int idProducto)
        {
            return objCliente_Produto.EliminarProductoEnCarrito(idCliente, idProducto);
        }
    }
}
