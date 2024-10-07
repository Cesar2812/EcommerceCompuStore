using CapaEntidad;
using CapaNegocio;
using CapaPresentacionAdmin.Filtros;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    [AuthFilter]
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


        //metodo para exportar a excel la data
        [HttpPost]// es de tipo post ya que se realiza una peticion para exportar la data
        public FileResult ExportarVentas(string fechainicio, string fechafin, string transaccion)
        {
            //creando lista de la clase reporte de ventas
            List<ReporteVentas> lista = new List<ReporteVentas>();
            //rellenando lista 
            lista = new CNReportes().ReporteVentas(fechainicio, fechafin, transaccion);

            //data table imaginario donde almacenara la data
            DataTable dt = new DataTable();

            dt.Locale = new CultureInfo("es-NI");//configurando el excel en el tema de los precios para Nicaragua
            dt.Columns.Add("FechaDeVenta", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Precio", typeof(decimal));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Total", typeof(decimal));
            dt.Columns.Add("NumeroTransaccion", typeof(string));

            //enviando los elementos de la lista
            foreach (ReporteVentas rp in lista)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaDeVenta,
                    rp.Cliente,
                    rp.Producto,
                    rp.Precio,
                    rp.Cantidad,
                    rp.Total,
                    rp.NumeroTransaccion,
                });

            }
            dt.TableName = "Datos";

            using (XLWorkbook wb = new XLWorkbook())
            {
                //agregando la tabla en una hoja de excel
                wb.Worksheets.Add(dt);
                using (MemoryStream Stream = new MemoryStream())
                {
                    wb.SaveAs(Stream);
                    return File(Stream.ToArray(), "application/vnd/openxmlformats-officedocument.spreadsheetml.sheet", "ReporteDeVentas" + DateTime.Now.ToString() + ".xlsx");

                }
            }


        }
    }
}