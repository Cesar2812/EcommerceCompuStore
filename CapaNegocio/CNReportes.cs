using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CNReportes
    {
        private CDReportes objReporte = new CDReportes();


        //metodo para listar los card del dasboard
        public Reportes VerReporte()
        {
            return objReporte.VerReporte();
        }

           
        //metodo para listar en el dataTable las ventas
        public List<ReporteVentas> ReporteVentas(string fechainicio, string fechafin, string transaccion)
        {

            return objReporte.ReporteVentas(fechainicio, fechafin, transaccion);
        }
    }
}
