namespace CapaEntidad
{
    public class Producto
    {
        public int id_Producto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public Marca objMarca { get; set; }
        public Categoria objCategoria { get; set; }

        public decimal Precio { get; set; }

        public string PrecioTexto { get; set; }

        public int Stock { get; set; }

        public string RutaImagen { get; set; }

        public string NombreImagen { get; set; }

        public bool Estado { get; set; }

        public string Base64 { get; set; }

        public string Extension { get; set; }


        //para el dashboard en la vista del admin
        public string producto { get; set; }
        public int cantidad { get; set; }


    }
}
