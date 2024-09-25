namespace CapaEntidad
{
    public class Usuario
    {

        public int id_Usuario { get; set; }


        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Correo { get; set; }

        public string Clave { get; set; }

        public bool Restablecer { get; set; }

        public bool Activo { get; set; }

    }
}
