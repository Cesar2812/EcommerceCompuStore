using System;
using System.IO;

namespace CapaNegocio
{
    public class Recursos
    {
        //metodo para convertir una imagen en base 64
        public static string convertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            //conversion que se obtiene como parametro de salida
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);// el archivo o imagen que se optiene en la ruta que lo convierta en un array de bytes
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch
            {
                conversion = false;

            }
            return textoBase64;
        }
    }
}
