using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CNReportes
    {
        private CDReportes objReporte = new CDReportes();

        public Reportes VerReporte()
        {
            return objReporte.VerReporte();
        }
    }
}
