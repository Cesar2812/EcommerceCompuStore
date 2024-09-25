using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        //Este metodos devuelven una accion que es retornar una vista dentro de la Pagina master
        public ActionResult Index() // vista  de inicio con Dasboard
        {
            return View();
        }
    }
}