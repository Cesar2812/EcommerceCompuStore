using CapaEntidad;
using CapaNegocio;
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

        //llamando el controlador para mostrar el reporte
        [HttpGet]//tipo get ya que devuelve el reporte
        public JsonResult VistaDashboard()
        {
            Reportes obj = new CNReportes().VerReporte();

            return Json(new { resultado = obj }, JsonRequestBehavior.AllowGet);  //devolvienndo el objeto de tipo dashboard
        }
    }
}