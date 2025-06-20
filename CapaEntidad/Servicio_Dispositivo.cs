using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Servicio_Dispositivo
    {
        public int id_Dispositivo { get; set; }

        public string NombreDispositivo { get; set; }

        public List<Servicio> objServicio { get; set; } = new List<Servicio>();
    }
}
