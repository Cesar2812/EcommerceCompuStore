namespace CapaEntidad
{
    public class Cliente_Producto
    {
        public int idProducto_Cliente { get; set; }

        public Cliente objcliente { get; set; }

        public Producto objProd { get; set; }

        public int Cantidad { get; set; }

    }
}
