using CapaEntidad;
using CapaNegocio;
using CapaPresentacionAdmin.Filtros;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
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

        //llamando el controlador para mostrar el reporte sobre los dashboard de los productos
        [HttpGet]//tipo get ya que devuelve el reporte en los card del dashboard
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
        [HttpPost]// es de tipo post ya que se realiza una peticion para exportar la data en excel es un sp que se ejecuta a nivel de servidor SQL 
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

            //enviando los elementos de la lista a un dataTable Virtual
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

        //metodo para retornar la vista de los reportes de productos y ventas en los dasboard
        [HttpGet]
        public JsonResult ReporteVentas()
        {
            CNVenta objVenta = new CNVenta();

            var lista=objVenta.ListarVentasPorMes();

            return Json(lista, JsonRequestBehavior.AllowGet);//para que se pueda consumir desde el cliente en forma de JSON

        }

        //metodo para retornar la vista de los reportes de productos y ventas en los dasboard
        [HttpGet]
        public JsonResult ReporteProductos()
        {
            CNVenta objVenta = new CNVenta();

            var lista = objVenta.ListarProductosDasboard();

            return Json(lista, JsonRequestBehavior.AllowGet);//para que se pueda consumir desde el cliente en forma de JSON

        }

        //metodo para retornar la factura de la venta
        public ActionResult Factura(int id_Venta)
        {
            CNVenta cnVenta = new CNVenta();

            Venta venta = cnVenta.ObtenerVentaDetalle(id_Venta);


            NumberFormatInfo formato = new CultureInfo("es-NI").NumberFormat;
            formato.CurrencyGroupSeparator = ".";

            if (venta== null)
            {
                venta = new Venta(); // para evitar null en la vista
            }
            else
            {
                // Suponiendo que Venta tiene lista objDetalleVenta y quieres formatear precios y subtotales:
                venta.objDetalleVenta = venta.objDetalleVenta.Select(dv => new Detalle_Venta
                {
                    Cantidad = dv.Cantidad,
                    objProducto = new Producto
                    {
                        Nombre = dv.objProducto.Nombre,
                        Precio = dv.objProducto.Precio,
                        // Puedes agregar un campo string para precio formateado, si quieres
                    },
                    Total = dv.Total,
                    // Puedes agregar un campo string para total formateado, si quieres
                }).ToList();

                // Ejemplo de campos formateados (si quieres agregar propiedades tipo TextoPrecio)
                // venta.TextoMontoTotal = venta.MontoTotal.ToString("N", formato);
            }

            return View(venta);
        }
    }
}