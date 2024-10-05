using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
    }
}