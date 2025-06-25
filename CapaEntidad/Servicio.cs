using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Servicio
    { 
        public int id_Servicio { get; set; }

        public int id_TipoDispositivo { get; set; }

        public string NombreDispositivo { get; set; }

        public int id_TipoServicio { get; set; }

        public TipoServicio objTipoServicio { get; set; }

        public string Descripcion_Servicio { get; set; }

        public decimal Precio { get; set; }

        public decimal Sub_Total { get;set; }



    }
}
