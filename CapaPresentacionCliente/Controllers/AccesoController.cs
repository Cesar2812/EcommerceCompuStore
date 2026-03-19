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

        #region Vistas
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

        #endregion Vistas


        [HttpPost]
        public ActionResult RegistrarCliente(Cliente obj)
        {
            int resultado;
            string mensaje = string.Empty;

            //si los datos son vacios se asignan al ViewData para que los vuelva a mostrar en el formulario y no se pierdan
            ViewData["Nombre"] = string.IsNullOrEmpty(obj.Nombre) ? "" : obj.Nombre;
            ViewData["Apellido"] = string.IsNullOrEmpty(obj.Apellido) ? "" : obj.Apellido;
            ViewData["Correo"] = string.IsNullOrEmpty(obj.Correo) ? "" : obj.Correo;

            if (obj.Clave != obj.ConfirmarClave)
            {
                ViewBag.Error = "Las Claves no coinciden";
                return View();
            }
            else
            {
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
        }


        //login
        [HttpPost]
        public ActionResult Index(string correo, string clave)
        { 
            Cliente cliente = null;// variable de tipo cliente para almacenarlo

            cliente = new CNCliente().ListarCliente().Where(item => item.Correo == correo && item.Clave == CNCliente.ConvertirSha256(clave)).FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "Correo o Clave incorrectas";
                return View();
            }
            else
            {
                Session["UsuarioCliente"] = cliente; //se obtiene la sesion del usuario con sus datos para poder usarlos en otras partes de la aplicacion

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


        //restablecer Clave
        [HttpPost]
        public ActionResult RestablecerClaveCliente(string correo)
        {
            Cliente clienteCorreo = new Cliente();

            
            clienteCorreo = new CNCliente().ListarCliente().Where(item => item.Correo == correo).FirstOrDefault();
            if (clienteCorreo == null)
            {
                ViewBag.Error = "Correo Incorrecto";
                return View();
            }
            else
            {
                string mensaje = string.Empty;
                bool respuesta = new CNCliente().RestablecerClave(clienteCorreo.id_Cliente, correo, out mensaje);

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



        [HttpPost]
        public ActionResult CambiarClaveCliente(string id_Cliente, string claveActual, string nuevaClave, string confirmarClave)
        {

            Cliente cliente = new Cliente();

            
            cliente = new CNCliente().ListarCliente().Where(u => u.id_Cliente == int.Parse(id_Cliente)).FirstOrDefault();

            
            if (cliente.Clave != CNCliente.ConvertirSha256(claveActual))
            {
                TempData["id_Cliente"] = id_Cliente;
                ViewData["vclave"] = "";
                ViewBag.Error = "La Clave actual no es correcta";
                return View();

            }
            else if (nuevaClave != confirmarClave) 
            {
                TempData["id_Cliente"] = id_Cliente;
                ViewData["vclave"] = claveActual;
                ViewBag.Error = "Las Claves no Coinciden";
                return View();

            } 

            ViewData["vclave"] = "";
            nuevaClave = CNUsuarios.ConvertirSha256(nuevaClave); 
            string mensaje = string.Empty;
            
            bool respuesta = new CNCliente().CambiarClave(Convert.ToInt32(id_Cliente), nuevaClave, out mensaje);

            
            if (respuesta)
            {
                TempData["SuccessMessage"] = "Clave Cambiada Exitosamente";
                
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