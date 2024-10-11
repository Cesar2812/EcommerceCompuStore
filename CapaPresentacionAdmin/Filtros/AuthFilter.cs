using System.Web.Mvc;

namespace CapaPresentacionAdmin.Filtros
{
    public class AuthFilter : ActionFilterAttribute
    {
        //este metodo lo que realiza es crear un filtro para que el usuario si no esta logueado no pueda acceder al sistema
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["Usuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/Index");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}