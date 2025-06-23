using System.Collections.Generic;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CNTipoServicio
    {
        //lista de tipo de servicio
        public List<TipoServicio> ListarTipoServicio()
        {
            
            CDTipoServicio obj = new CDTipoServicio();
           
            return obj.ListarTipoServicio();
        }

        //registrando Categorias
        public int RegistrarTipoServicio(TipoServicio objServT, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(objServT.Descripcion) || string.IsNullOrWhiteSpace(objServT.Descripcion))
            {
                Mensaje = "Es Necesario Ingresar el Nombre del Tipo De Servicio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                CDTipoServicio ob = new CDTipoServicio();
                return ob.ResgistrarTipo_Servicio(objServT, out Mensaje);
            }
            else
            {
                return 0;
            }
        }
    }
}
