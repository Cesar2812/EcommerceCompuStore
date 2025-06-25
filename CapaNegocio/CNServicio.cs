using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;
using System.Data;

namespace CapaNegocio
{
    public class CNServicio
    { 
        private CDServicio objCapaDatoServicio = new CDServicio();

        public bool RegistrarServicio(Servicio_Dispositivo servicio_dispo, DataTable servicio, out string Mensaje)
        {
            return objCapaDatoServicio.RegistarServicio(servicio_dispo, servicio, out Mensaje);
        } 

        public List<Servicio> ListarServicio()
        {
            return objCapaDatoServicio.ListarServicio();
        }

        public int ResgistrarServicioGesrion(string Detalle)
        {
            return objCapaDatoServicio.ResgistrarServicioGestion(Detalle);
        }

        //metodo de negocio que retorna la factura de una venta por id
        public Factura_Servicio ObtenerDetalle(int idFacturaServicio)
        {
            return objCapaDatoServicio.ObtenerFactura(idFacturaServicio);
        }
    }
}
