using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace CapaNegocio
{
    public class CNCliente
    {
        private CDCLiente objCapaDatoCliente = new CDCLiente();
        //metodo para registrar Cliente
        public int Registrar(Cliente objCliente, out string Mensaje)
        {
            Mensaje = string.Empty;

            //validacion del nombre no sea vacion
            //entonces
            if (string.IsNullOrEmpty(objCliente.Nombre) || string.IsNullOrWhiteSpace(objCliente.Nombre))
            {
                Mensaje = "Es Necesario Ingresar su Nombre";

            }
            else if (string.IsNullOrEmpty(objCliente.Apellido) || string.IsNullOrWhiteSpace(objCliente.Apellido))
            {
                Mensaje = "Es Necesario Ingresar su Apellido";
            }
            else if (string.IsNullOrEmpty(objCliente.Correo) || string.IsNullOrWhiteSpace(objCliente.Correo))
            {
                Mensaje = "Es Necesario Ingresar su Correo";

            }

            //si cumple todo entonces
            if (string.IsNullOrEmpty(Mensaje))
            {
                //encriptando clave con metodo realizado
                objCliente.Clave = ConvertirSha256(objCliente.Clave);
                return objCapaDatoCliente.RegistrarCliente(objCliente, out Mensaje);//pasando el sp a la capa de negocio

            }
            else
            {
                return 0;
            }
        }

        //listando datos del cliente
        public List<Cliente> ListarCliente()
        {
            return objCapaDatoCliente.ListarCliente();
        }


        //metodo para encriptar la clave
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

        //metodo para cambiarCalve
        public bool CambiarClave(int idCliente, string nuevaClave, out string Mensaje)
        {
            return objCapaDatoCliente.CambiarClave(idCliente, nuevaClave, out Mensaje);
        }

        //Metodo para restablecer la clave
        public bool RestablecerClave(int idCliente, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;

            //generando la clave aleatoria para cada usuario
            string nuevaClave = GenerarClave();
            bool resultado = objCapaDatoCliente.RestablecerClave(idCliente, ConvertirSha256(nuevaClave), out Mensaje);

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

        //metodo para enviar correo al cliente para restablecer la clave
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

        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 8);//retorna codigo unico con formato alfanumericos con una clave de 6 digitos
            return clave;//retornando clave generada
        }

    }
}
