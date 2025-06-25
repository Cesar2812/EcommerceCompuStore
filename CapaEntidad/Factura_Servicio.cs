using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public  class Factura_Servicio
    { 
        public int id_FacturaServicio { get; set; }

        public string NombreCliente { get; set; }

        public string TelefonoCliente { get; set; }
        public string FechaTexto { get; set; }

        public decimal Total { get; set; }

        public string TextoTotal { get; set; }

        public decimal Cantidad_Pagada { get; set; }

        public string TextoCantidadPagada { get; set; }

        public decimal Cambio { get; set; }

        public string TextoCambio { get; set; }

        public string NumeroTransaccion { get; set; }

        public decimal TotalSinIva { get; set; }

        public string TextoTotalSinIva { get; set; }

        public List<Detalle_Servicio> objServicio { get; set; }

       
     
    }
}
