using CapaEntidad;
using CapaNegocio;
using CapaPresentacionAdmin.Filtros;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{

    public class AccesoController : Controller
    {
        // formulario de login
        public ActionResult Index()
        {
            return View();
        }

        //formulario para cambiar clave
        [AuthFilter]
        public ActionResult CambiarClave()
        {
            return View();
        }

        //formulario para restablcer clave
        public ActionResult RestablecerClave()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {

            //creando un objeto de tipo usuario para el inicio de sesion
            Usuario usuario = new Usuario();
            usuario = new CNUsuarios().Listar().Where(u => u.Correo == correo && u.Clave == CNUsuarios.ConvertirSha256(clave)).FirstOrDefault();

            //si encuentra un Usuario con las credenciales correctas
            if (usuario == null)
            {
                ViewBag.Error = "Correo o Clave Incorrecta";
                return View();

            }
            else
            {
                Session["Usuario"] = usuario.Correo; //se obtiene el usuario en su autenticacion por correo
                //si el usuario accede por primera vez al sistema
                if (usuario.Restablecer)
                {
                    TempData["id_Usuario"] = usuario.id_Usuario;

                    return RedirectToAction("CambiarClave");

                }
                //si el usuario fue encontrado
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");// entrando a la vista principal del panel
            }

        }
        [HttpPost]
        public ActionResult CambiarClave(string id_Usuario, string claveActual, string nuevaClave, string confirmarClave)
        {
            Usuario usuario = new Usuario();

            //trayendo al usuario que va a modificar la clave
            usuario = new CNUsuarios().Listar().Where(u => u.id_Usuario == int.Parse(id_Usuario)).FirstOrDefault();

            //validando si la contrasena es la actual con la que ya tiene primero se convierte la clave si
            if (usuario.Clave != CNUsuarios.ConvertirSha256(claveActual))
            {
                TempData["id_Usuario"] = id_Usuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La Clave actual no es correcta";
                return View();

            }
            else if (nuevaClave != confirmarClave) // si la clave nueva no es igual a confirmar clave 
            {
                TempData["id_Usuario"] = id_Usuario;
                ViewData["vclave"] = claveActual;
                ViewBag.Error = "Las Claves no Coinciden";
                return View();

            }

            //entonces si todo ocurre bien y no hay errores
            ViewData["vclave"] = "";
            nuevaClave = CNUsuarios.ConvertirSha256(nuevaClave); // pasandole la nueva clave para que la encripte
            string mensaje = string.Empty;
            //llamando al metodo para cambiar la clave
            bool respuesta = new CNUsuarios().CambiarClave(Convert.ToInt32(id_Usuario), nuevaClave, out mensaje);

            //si el cambio ha sido exitoso
            if (respuesta)
            {
                TempData["SuccessMessage"] = "Clave Cambiada Exitosamente";
                return View();   
            }
            else
            {
                TempData["id_Usuario"] = id_Usuario;
                ViewBag.Error = mensaje;
                return View();

            }

        }


        //metodo para cambiar clave
        [HttpPost]
        public ActionResult RestablecerClave(string correo)
        {
            Usuario usuario = new Usuario();

            //buscando el correo del usuario al cual se le va a restablecer la clave
            usuario = new CNUsuarios().Listar().Where(item => item.Correo == correo).FirstOrDefault();
            if (usuario == null)
            {
                ViewBag.Error = "Correo Incorrecto";
                return View();

            }
            else
            {
                string mensaje = string.Empty;

                bool respuesta = new CNUsuarios().RestablecerClave(usuario.id_Usuario, correo, out mensaje);

                if (respuesta)
                {
                    TempData["SuccessMessage"] = "Clave Restablecida De Forma Exitosa";
                    return View();

                }
                else
                {
                    ViewBag.Error = mensaje;
                    return View();
                }
            }
        }

        //metodo para cerrar sesion
        public ActionResult CerrarSesion()
        {
            // Elimina la sesión del usuario
            Session["Usuario"] = null;
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Acceso");
        }
    }
}