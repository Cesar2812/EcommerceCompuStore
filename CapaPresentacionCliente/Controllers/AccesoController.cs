using CapaEntidad;
using CapaNegocio;
using CapaPresentacionCliente.Filtros;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionCliente.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistrarCliente()
        {
            return View();
        }

        public ActionResult RestablecerClaveCliente()
        {
            return View();
        }

        //formulario para cambiar clave
        [AuthFilter]
        public ActionResult CambiarClaveCliente()
        {
            return View();
        }



        //metodo para que se registre el cliente
        [HttpPost]
        public ActionResult RegistrarCliente(Cliente obj)
        {
            int resultado;

            string mensaje = string.Empty;

            ViewData["Nombre"] = string.IsNullOrEmpty(obj.Nombre) ? "" : obj.Nombre;
            ViewData["Apellido"] = string.IsNullOrEmpty(obj.Apellido) ? "" : obj.Apellido;
            ViewData["Correo"] = string.IsNullOrEmpty(obj.Correo) ? "" : obj.Correo;

            if (obj.Clave != obj.ConfirmarClave)
            {
                ViewBag.Error = "Las Claves no coinciden";
                return View();
            }

            resultado = new CNCliente().Registrar(obj, out mensaje);

            if (resultado > 0)
            {
                TempData["SuccessMessage"] = "Cuenta Creada Exitosamente";
                return View();
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }


        //metodo de incio de sesion del cliente
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            Cliente cliente = null;

            cliente = new CNCliente().ListarCliente().Where(item => item.Correo == correo && item.Clave == CNCliente.ConvertirSha256(clave)).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "Correo o Clave incorrectas";
                return View();

            }
            else
            {
                Session["UsuarioCliente"] = cliente; //se obtiene el usuario en su autenticacion por correo
                                                     //si el usuario accede por primera vez al sistema

                if (cliente.Restablecer)
                {

                    TempData["id_Cliente"] = cliente.id_Cliente;
                    return RedirectToAction("CambiarClaveCliente", "Acceso");

                }
                else
                {
                    TempData["ClienteNombre"] = cliente.Nombre;
                    FormsAuthentication.SetAuthCookie(cliente.Correo, false);
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");

                }
            }
        }

        //metodo para Restablecer clave
        [HttpPost]
        public ActionResult RestablecerClaveCliente(string correo)
        {
            Cliente cliente = new Cliente();

            //buscando el correo del cliente al cual se le va a restablecer la clave
            cliente = new CNCliente().ListarCliente().Where(item => item.Correo == correo).FirstOrDefault();
            if (cliente == null)
            {
                ViewBag.Error = "Correo Incorrecto";
                return View();

            }
            else
            {
                string mensaje = string.Empty;

                bool respuesta = new CNCliente().RestablecerClave(cliente.id_Cliente, correo, out mensaje);

                if (respuesta)
                {
                    TempData["SuccessMessage"] = "Clave Recuperada De Forma Exitosa";
                    return View();

                }
                else
                {
                    ViewBag.Error = mensaje;
                    return View();
                }
            }

        }


        //metodo para cambiar clave del cliente
        [HttpPost]
        public ActionResult CambiarClaveCliente(string id_Cliente, string claveActual, string nuevaClave, string confirmarClave)
        {

            Cliente cliente = new Cliente();

            //trayendo al cliente que va a modificar la clave
            cliente = new CNCliente().ListarCliente().Where(u => u.id_Cliente == int.Parse(id_Cliente)).FirstOrDefault();

            //validando si la contrasena es la actual con la que ya tiene primero se convierte la clave si
            if (cliente.Clave != CNCliente.ConvertirSha256(claveActual))
            {
                TempData["id_Cliente"] = id_Cliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La Clave actual no es correcta";
                return View();

            }
            else if (nuevaClave != confirmarClave) // si la clave nueva no es igual a confirmar clave 
            {
                TempData["id_Cliente"] = id_Cliente;
                ViewData["vclave"] = claveActual;
                ViewBag.Error = "Las Claves no Coinciden";
                return View();

            }

            //entonces si todo ocurre bien y no hay errores
            ViewData["vclave"] = "";
            nuevaClave = CNUsuarios.ConvertirSha256(nuevaClave); // pasandole la nueva clave para que la encripte
            string mensaje = string.Empty;
            //llamando al metodo para cambiar la clave
            bool respuesta = new CNCliente().CambiarClave(Convert.ToInt32(id_Cliente), nuevaClave, out mensaje);

            //si el cambio ha sido exitoso
            if (respuesta)
            {
                TempData["SuccessMessage"] = "Clave Cambiada Exitosamente";
                // Elimina la sesión del usuario
                Session["UsuarioCliente"] = null;
                Session.Clear();
                Session.Abandon();
                return View();

            }
            else
            {
                TempData["id_Cliente"] = id_Cliente;
                ViewBag.Error = mensaje;
                return View();

            }

        }

        //metodo para Cerrar Sesion
        public ActionResult CerrarSesion()
        {
            // Elimina la sesión del usuario
            Session["UsuarioCliente"] = null;
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Acceso");
        }
    }
}