using CapaEntidad;
using CapaNegocio;
using CapaPresentacionAdmin.Filtros;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacionAdmin.Controllers
{

    public class AccesoController : Controller
    {

       
        public ActionResult Index()
        {
            return View();
        }

       
        [AuthFilter]
        public ActionResult CambiarClave()
        {
            return View();
        }

       
        public ActionResult RestablecerClave()
        {
            return View();
        }


        //Inicio de Sesion
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {

            
            Usuario usuario = new Usuario();
            usuario = new CNUsuarios().Listar().Where(u => u.Correo == correo && u.Clave == CNUsuarios.ConvertirSha256(clave)).FirstOrDefault();
 
            if (usuario == null)
            {
                ViewBag.Error = "Correo o Clave Incorrecta";
                return View();
            }
            else
            {
               
                Session["Usuario"] = usuario.Correo; 

                if (usuario.Restablecer)
                {
                    TempData["id_Usuario"] = usuario.id_Usuario;

                    return RedirectToAction("CambiarClave");

                }
               
                TempData["NombreUsuario"] = usuario.Nombre;
                FormsAuthentication.SetAuthCookie(usuario.Correo, false); 
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
        }


        
        [HttpPost]
        public ActionResult CambiarClave(string id_Usuario, string claveActual, string nuevaClave, string confirmarClave)
        {
            Usuario usuario = new Usuario();

            
            usuario = new CNUsuarios().Listar().Where(u => u.id_Usuario == int.Parse(id_Usuario)).FirstOrDefault();

            
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

            
            ViewData["vclave"] = "";
            nuevaClave = CNUsuarios.ConvertirSha256(nuevaClave); 
            string mensaje = string.Empty;
            
            bool respuesta = new CNUsuarios().CambiarClave(Convert.ToInt32(id_Usuario), nuevaClave, out mensaje);

           
            if (respuesta)
            {
                TempData["SuccessMessage"] = "Clave Cambiada Exitosamente";
               
                Session["Usuario"] = null;
                Session.Clear();
                Session.Abandon();

                return View();
            }
            else
            {
                TempData["id_Usuario"] = id_Usuario;
                ViewBag.Error = mensaje;
                return View();

            }

        }


       
        [HttpPost]
        public ActionResult RestablecerClave(string correo)
        {
            Usuario usuario = new Usuario();
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

       
        public ActionResult CerrarSesion()
        {
            Session["Usuario"] = null;
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Acceso");
        }
    }
}