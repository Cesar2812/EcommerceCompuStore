using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;// libreria para enviar correo
using System.Security.Cryptography;
using System.Text;


namespace CapaNegocio
{
    public class CNUsuarios
    {
        //metodo para encriptar clave 
        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));
                foreach (byte b in result)
                {
                    Sb.Append(b.ToString("x2"));

                }
            }

            return Sb.ToString();
        }


        //instanceo la clase Uusario de la capadatos para acceder a sus metodos
        private CDUsuarios objCapaDatos = new CDUsuarios();

        //creando un metodo que devuelva la lista usuario que se esta obteniendo de la capaDatos de la clase usuario
        public List<Usuario> Listar()
        {
            return objCapaDatos.Listar();//retorna la lista de la capadatos osea la muestra
        }



        //metodo para registrar usuario
        public int Registrar(Usuario objUsuario, out string Mensaje)
        {
            Mensaje = string.Empty;

            //validacion del nombre no sea vacion
            //entonces
            if (string.IsNullOrEmpty(objUsuario.Nombre) || string.IsNullOrWhiteSpace(objUsuario.Nombre))
            {
                Mensaje = "Es Necesario Ingresar el Nombre del Usuario";

            }
            else if (string.IsNullOrEmpty(objUsuario.Apellido) || string.IsNullOrWhiteSpace(objUsuario.Apellido))
            {
                Mensaje = "Es Necesario Ingresar el Apellido del Usuario";
            }
            else if (string.IsNullOrEmpty(objUsuario.Correo) || string.IsNullOrWhiteSpace(objUsuario.Correo))
            {
                Mensaje = "Es Necesario Ingresar el Correo del Usuario";

            }

            //si cumple todo entonces
            if (string.IsNullOrEmpty(Mensaje))
            {
                //generando la clave aleatoria para cada usuario
                string clave = GenerarClave();

                string asunto = "Creacion De Cuenta Para Acceso";
                string mensajeCorreo = "<h3> Su cuenta fue creada correctamente</h3></br><p>Su clave de usuario para acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", clave);

                bool respuesta = EnviarCorreo(objUsuario.Correo, asunto, mensajeCorreo);//pasandole a la funcion los parametros para enviar correo

                if (respuesta)
                {
                    //encriptando clave con metodo realizado
                    objUsuario.Clave = ConvertirSha256(clave);
                    return objCapaDatos.Registrar(objUsuario, out Mensaje);//pasando el sp a la capa de negocio

                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        //metodo para editar Usuario
        public bool Editar(Usuario objUser, out string Mensaje)
        {
            Mensaje = string.Empty;

            //validacion del nombre no sea vacion
            //entonces
            if (string.IsNullOrEmpty(objUser.Nombre) || string.IsNullOrWhiteSpace(objUser.Nombre))
            {
                Mensaje = "Es Necesario Ingresa el Nombre del Usuario";

            }
            else if (string.IsNullOrEmpty(objUser.Apellido) || string.IsNullOrWhiteSpace(objUser.Apellido))
            {
                Mensaje = "Es Necesario Ingresar el Apellido del Usuario";
            }
            else if (string.IsNullOrEmpty(objUser.Correo) || string.IsNullOrWhiteSpace(objUser.Correo))
            {
                Mensaje = "Es Necesario Ingresar el Correo del Usuario";

            }

            if (string.IsNullOrEmpty(Mensaje))
            {

                return objCapaDatos.Editar(objUser, out Mensaje);
            }
            else
            {

                return false;
            }
        }

        //eliminar Usuario
        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.Eliminar(id, out Mensaje);
        }

        //metodo para cambiarCalve
        public bool CambiarClave(int idUsuario, string nuevaClave, out string Mensaje)
        {
            return objCapaDatos.CambiarClave(idUsuario, nuevaClave, out Mensaje);
        }



        //Metodo para restablecer la clave
        public bool RestablecerClave(int idUsuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;

            //generando la clave aleatoria para cada usuario
            string nuevaClave = GenerarClave();
            bool resultado = objCapaDatos.RestablecerClave(idUsuario, ConvertirSha256(nuevaClave), out Mensaje);

            if (resultado)
            {
                string asunto = "Clave Restablecida";
                string mensajeCorreo = "<h3> Su Calve fue restablecida correctamente</h3></br><p>Su clave de usuario para ahora acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", nuevaClave);

                bool respuesta = EnviarCorreo(correo, asunto, mensajeCorreo);

                if (respuesta)
                {
                    return true;

                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }


            }
            else
            {
                Mensaje = "No se pudo reestablecer la clave";
                return false;
            }
        }


        //metodo para generar clave automatica que sera enviada por coreo al usuario
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 8);//retorna codigo unico con formato alfanumericos con una clave de 6 digitos
            return clave;//retornando clave generada
        }


        //metodo para enviar correo al usuario
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.To.Add(correo);
                    mail.From = new MailAddress("cesarcerdacomputer@gmail.com");
                    mail.Subject = asunto;
                    mail.Body = mensaje;
                    mail.IsBodyHtml = true;
                    mail.Headers.Add("X-Priority", "1"); // Alta prioridad
                    mail.Headers.Add("X-MSMail-Priority", "High");
                    mail.Headers.Add("Importance", "High");

                    using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("cesarcerdacomputer@gmail.com", "vjqhpeaglpjsmhuy");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                resultado = true;
            }
            catch (Exception e)
            {
                // Registra o muestra el mensaje de error
                Console.WriteLine($"Error al enviar correo: {e.Message}");
            }

            return resultado;
        }
    }
}
