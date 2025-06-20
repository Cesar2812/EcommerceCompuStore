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
    }
}
