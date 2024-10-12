using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionCliente.Filtros
{
    public class AuthFilter:ActionFilterAttribute
    {

        //este metodo lo que realiza es crear un filtro para que el usuario cliente si no esta logueado no pueda acceder al sistema
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["UsuarioCliente"] == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/Index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}