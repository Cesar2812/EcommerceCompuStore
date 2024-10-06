using CapaEntidad;
using CapaNegocio;
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


        [HttpPost]//en este metodo traemos al usuario al cual deseamos modificar su clave 
        public ActionResult CambiarClave(string id_Usuario, string claveActual, string nuevaClave, string confirmarClave)
        {
            Usuario usuario = new Usuario();

            usuario = new CNUsuarios().Listar().Where(u => u.id_Usuario == int.Parse(id_Usuario)).FirstOrDefault();

            //validando si la contra es la misma que ya tiene
            if (usuario.Clave != CNUsuarios.ConvertirSha256(claveActual))
            {
                TempData["id_Usuario"] = id_Usuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La Clave Actual No es Correcta";

            }
            else if (nuevaClave != confirmarClave)
            {
                TempData["id_Usuario"] = id_Usuario;
                ViewData["vclave"] = claveActual;
                ViewBag.Error = "Las Claves no Coinciden";
                return View();
            }
            ViewData["vclave"] = "";
            nuevaClave = CNUsuarios.ConvertirSha256(nuevaClave);

            string mensaje = string.Empty;

            bool respuesta = new CNUsuarios().CambiarClave(int.Parse(id_Usuario), nuevaClave, out mensaje);
            if (respuesta) //si la respuesta es correcta va a redirecionar al formulario de logueo
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["id_Usuario"] = id_Usuario;
                ViewBag.Error = mensaje;
                return View();
            }
        }
    }
}