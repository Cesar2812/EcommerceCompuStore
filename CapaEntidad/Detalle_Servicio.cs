using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public  class Detalle_Servicio
    {
        
 

       public string NombreDispositivo { get; set; }
        public string Nombre_TipoDispositivo { get; set; }

        public string TipoServicio { get; set; }

        public string Descripcion_Servicio { get; set; }

        public decimal PrecioUnidad { get; set; }
        public string TextoPrecioUnidad { get; set; }
        public decimal Sub_Total { get; set; }
        public string TextoSub_Total { get; set; }


    }
}
