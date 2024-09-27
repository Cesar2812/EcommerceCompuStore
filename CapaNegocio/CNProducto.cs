using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CNProducto
    {
        private CDProducto objCapaProd = new CDProducto();

        //metodo para listar productos
        public List<Producto> ListarProductos()
        {
            return objCapaProd.ListarProducto();
        }

        //metodo para registrar Productos
        public int RegistrarProducto(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //validaciones para campos vacios si no se ingresa algo
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "Es necesario darle un nombre al producto";

            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "Es necesario darle una descripcion al producto";
            }
            else if (obj.objMarca.id_Marca == 0)
            {
                Mensaje = "Es necesaria una marca para el producto";
            }
            else if (obj.objCategoria.id_Categoria == 0)
            {
                Mensaje = "Es necesaria una categoria para el producto";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "Es necesario un precio para el producto";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Es necesario el stock del producto";
            }

            //validacion si ya se llenan los campos si no encuentra ningun error
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaProd.RegistrarProducto(obj, out Mensaje);

            }
            else
            {
                return 0;// si encuentra un error retorna 0
            }
        }

        //metodo para editar un producto
        public bool EditarProducto(Producto obj, out string Mensaje)
        {
            Mensaje=string.Empty;

            //validaciones para campos vacios si no se ingresa algo
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "Es necesario darle un nombre al producto";

            }
            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "Es necesario darle una descripcion al producto";
            }
            else if (obj.objMarca.id_Marca == 0)
            {
                Mensaje = "Es necesaria una marca para el producto";
            }
            else if (obj.objCategoria.id_Categoria == 0)
            {
                Mensaje = "Es necesaria una categoria para el producto";
            }
            else if (obj.Precio == 0)
            {
                Mensaje = "Es necesario un precio para el producto";
            }
            else if (obj.Stock == 0)
            {
                Mensaje = "Es necesario el stock del producto";
            }

            //si no encuentra ningun error
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaProd.EditarProducto(obj, out Mensaje);    
            }
            else
            {
                return false; // si encuentra un error retorna falso
            }
        }

        //metodo para eliminar un producto
        public bool EliminarProducto(int id,out string Mensaje)
        {
            return objCapaProd.EliminarProducto(id,out Mensaje);
        }
        
        //funcion para guardar imagen del producto
        public bool GuardarDataImagen(Producto obj,out string Mensaje)
        {
            return objCapaProd.GuardarDataImagen(obj,out Mensaje);
        }
    }
}
