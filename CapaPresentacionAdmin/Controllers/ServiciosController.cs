using CapaEntidad;
using CapaNegocio;
using CapaPresentacionAdmin.Filtros;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
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


        public ActionResult ServicioGestion()
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

        [HttpPost]
        public JsonResult RegistrarServicioGestion(string xml)
        {
            int Respuesta = 0;
            Respuesta = new CNServicio().ResgistrarServicioGesrion(xml);
            if (Respuesta != 0)
                return Json(new { estado = true, valor = Respuesta.ToString() }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { estado = false, valor = "" }, JsonRequestBehavior.AllowGet);

        }


        //metodo para retornar la factura del Servicio
        public ActionResult Factura(int id_FacturaServicio=0)
        {

            Factura_Servicio ofac = new CNServicio().ObtenerDetalle(id_FacturaServicio);



            NumberFormatInfo formato = new CultureInfo("es-NI").NumberFormat;
            formato.CurrencyGroupSeparator = ".";


            if (ofac == null)
                ofac = new Factura_Servicio();
            else
            {

                ofac.objServicio = (from dv in ofac.objServicio
                                             select new Detalle_Servicio()
                                             { 
                                                 TipoServicio = dv.TipoServicio,
                                                 NombreDispositivo = dv.NombreDispositivo,
                                                 Descripcion_Servicio = dv.Descripcion_Servicio,
                                                 PrecioUnidad = dv.PrecioUnidad,
                                                 TextoPrecioUnidad = dv.PrecioUnidad.ToString("N", formato), //numero.ToString("C", formato)
                                                 Sub_Total = dv.Sub_Total,
                                                 TextoSub_Total = dv.Sub_Total.ToString("N", formato)
                                             }).ToList();

                ofac.TextoCantidadPagada = ofac.Cantidad_Pagada.ToString("N", formato);
                ofac.TextoCambio = ofac.Cambio.ToString("N", formato);
                ofac.TextoTotalSinIva = ofac.TotalSinIva.ToString("N", formato);
                ofac.TextoTotal = ofac.Total.ToString("N", formato);
            }


            return View(ofac);

        }
    }
}