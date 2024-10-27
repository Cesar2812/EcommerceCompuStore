namespace CapaEntidad
{
    public class Municipio
    {

        public string idMunicipio { get; set; }

        public string NombreMunicipio { get; set; }

        public Departamento objDepartamento { get; set; }
    }
}
