using CapaEntidad;
using CapaNegocio;
using CapaPresentacionAdmin.Filtros;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    [AuthFilter]
    public class ServiciosController : Controller
    {
        public ActionResult Reparacion()
        {

            return View();
        }


        public ActionResult Servicio()
        {

            return View();
        }

        //listando los tipos de servicio
        public JsonResult ListarTipoServicio()
        {
            List<TipoServicio> lista = new List<TipoServicio>();

            lista = new CNTipoServicio().ListarTipoServicio();

            //retornando la lista en formato JSON
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]//POST ya que es una peticion de insert hacia la base de datos
        public JsonResult GuardarTipoServicio(TipoServicio objeto)
        {
            object resultado = 0;
            string Mensaje = string.Empty;

            if (objeto.id_TipoServicio == 0)
            {
                resultado = new CNTipoServicio().RegistrarTipoServicio(objeto, out Mensaje);
            }
            
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult RegistrarServicio(Servicio_Dispositivo objServicio_dispositivo, List<Servicio> Detalle_Servicio)
        {
            string Mensaje = string.Empty;
            bool respuesta = false;

            // Crear DataTable para detalles
            var dt = new DataTable();
            dt.Columns.Add("id_TipoServicio", typeof(int));
            dt.Columns.Add("id_TipoDispositivo", typeof(int));
            dt.Columns.Add("Descripcion", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));

            foreach (var item in Detalle_Servicio)
            {
                dt.Rows.Add(item.id_TipoServicio,item.id_TipoDispositivo, item.Descripcion_Servicio, item.Precio);
            }

            respuesta = new CNServicio().RegistrarServicio(objServicio_dispositivo, dt, out Mensaje);

            return Json(new {respuesta,Mensaje}, JsonRequestBehavior.AllowGet);

        }


        [HttpGet] //metodo get porque se obtienen datos del servidor para mostrarlos en el data table
        public JsonResult ListarServicios()
        {
            List<Servicio> lista = new List<Servicio>();
            lista = new CNServicio().ListarServicio();
            //retornando la data en formato JSON
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);//metodo get porque agarra la data
        }
    }
}