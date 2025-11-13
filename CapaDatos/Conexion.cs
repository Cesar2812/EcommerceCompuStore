using System.Configuration;

namespace CapaDatos
{
    public class Conexion
    {
        //toda esta clase se comunica con el archivo webconfig mediante el nodo creado con nuestras credenciales del servidor de 
        // base de datos
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();

    }
}
