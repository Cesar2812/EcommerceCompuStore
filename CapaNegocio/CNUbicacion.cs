using CapaDatos;
using CapaEntidad;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CNUbicacion
    {
        private CDUbicacion objUbicacion = new CDUbicacion();

        public List<Departamento> ObtenerDepartamento()
        {
            return objUbicacion.ObtenerDepartamento();
        }

        public List<Municipio> ObtenerMunicipio(string idDepartamento)
        {
            return objUbicacion.ObtenerMunicipio(idDepartamento);

        }
    }
}
