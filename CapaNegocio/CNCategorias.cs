using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;
namespace CapaNegocio
{
    public class CNCategorias
    {
        private CDCategorias objCapaDatoCat = new CDCategorias();


        //listando Categorias
        public List<Categoria> ListarCategoria()
        {
            return objCapaDatoCat.ListarCategorias();
        }

        //registrando Categorias
        public int RegistrarCategoria(Categoria objCatg, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objCatg.Descripcion) || string.IsNullOrWhiteSpace(objCatg.Descripcion))
            {
                Mensaje = "Es Necesario Ingresar el Nombre de la Categoria";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatoCat.RegistrarCategoria(objCatg, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        //editando Categorias
        public bool EditarCategoria(Categoria objCat, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objCat.Descripcion) || string.IsNullOrWhiteSpace(objCat.Descripcion))
            {
                Mensaje = "Es Necesario Ingrear el Nombre de la Categoria";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatoCat.EditarCatgeoria(objCat, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        //eliminando Categoria
        public bool EliminarCategoria(int id, out string Mensaje)
        {
            return objCapaDatoCat.EliminarCategoria(id, out Mensaje);
        }
    }
}
