namespace CapaEntidad
{
    public class Cliente
    {
        public int id_Cliente { get; set; }
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Correo { get; set; }

        public string Clave { get; set; }

        public string ConfirmarClave { get; set; }
        public bool Restablecer { get; set; }

    }
}
