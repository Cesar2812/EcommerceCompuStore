using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;
namespace CapaNegocio
{
    public class CNMarcas
    {
        private CDMarcas objCapaDatoMar = new CDMarcas();


        //listando Marcas
        public List<Marca> ListarMarca()
        {
            return objCapaDatoMar.ListarMarca();
        }

        //registrando Marcas
        public int RegistrarMarca(Marca objMarca, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objMarca.Descripcion) || string.IsNullOrWhiteSpace(objMarca.Descripcion))
            {
                Mensaje = "Es Necesario Ingresar el Nombre de la Marca";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatoMar.RegistrarMarca(objMarca, out Mensaje);
            }
            else
            {
                return 0;
            }
        }

        //editando Marcas
        public bool EditarMarca(Marca objMar, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objMar.Descripcion) || string.IsNullOrWhiteSpace(objMar.Descripcion))
            {
                Mensaje = "Es Necesario Ingresar el Nombre de la Marca";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatoMar.EditarMarca(objMar, out Mensaje);
            }
            else
            {
                return false;
            }
        }
        //eliminando Marca
        public bool EliminarMarca(int id, out string Mensaje)
        {
            return objCapaDatoMar.EliminarMarca(id, out Mensaje);
        }
    }
}
