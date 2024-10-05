using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
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


        //llamando el metodo para listar las ventas
        [HttpGet]//tipo get ya que devuelve el reporte de las ventas por parametros pasados
        public JsonResult ListaVentas(string fechainicio, string fechafin, string numeroTransaccion)
        {
            List<ReporteVentas> lista = new List<ReporteVentas>();

            lista = new CNReportes().ReporteVentas(fechainicio, fechafin, numeroTransaccion);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
    }
}