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
                    TempData["id_Usuario"]=usuario.id_Usuario;

                    return RedirectToAction("CambiarClave");

                }
                //si el usuario fue encontrado
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");// entrando a la vista principal del panel
            }

        }
    }
}