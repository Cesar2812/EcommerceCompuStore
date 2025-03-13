using CapaEntidad;
using CapaEntidad.Paypal;
using CapaNegocio;
using CapaPresentacionCliente.Filtros;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace CapaPresentacionCliente.Controllers
{
    public class TiendaController : Controller
    {

        //Vista de incio que carga los productos del catalogo de la base de datos 
        public ActionResult Index()
        {
            return View();
        }


        //vista del carrito
        [AuthFilter]
        public ActionResult Carrito() // metodo action para ir al carrrito, no se puede ir sin antes iniciar sesion
        {
            return View();
        }


        //vista de detalle de producto
        public ActionResult DetalleProducto(int id_Producto = 0)
        {
            // Verifica si el id_Producto es nulo o 0
            if (id_Producto <= 0)
            {
                // Redirige al usuario a la página de índice
                return RedirectToAction("Index"); // Cambia "Index" por el nombre de tu acción de índice
            }
            Producto producto = new Producto();
            bool conversion;

            producto = new CNProducto().ListarProductos().FirstOrDefault(p => p.id_Producto == id_Producto);
            // Si el producto no se encuentra, redirige al índice
            if (producto == null)
            {
                return RedirectToAction("Index"); // Cambia "Index" por el nombre de tu acción de índice
            }

            // Si se encuentra el Producto
            producto.Base64 = Recursos.convertirBase64(Path.Combine(producto.RutaImagen, producto.NombreImagen), out conversion);
            producto.Extension = Path.GetExtension(producto.NombreImagen);

            return View(producto);
        }




        //metodo para obtener Categorias en el CARD que permita buscar 
        [HttpGet]
        public JsonResult ListaCategorias()
        {
            //creando lista de categorias
            List<Categoria> lista = new List<Categoria>();

            lista = new CNCategorias().ListarCategoria();// almacenando categorias en la lista
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet); //retornando categorias en un JSON
        }


        [HttpPost]
        public JsonResult ListaMarcaPorCategoria(int idCategoria)
        {
            //creando lista de categorias
            List<Marca> lista = new List<Marca>();

            lista = new CNMarcas().ListarMarcaporCategoria(idCategoria);// almacenando las marcas en la lista
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet); //retornando las marcas en un JSON
        }


        // Método para listar productos con paginación
        [HttpPost]
        public JsonResult ListarProductos(int idcategoria, int idMarca, int pageNumber = 1, int pageSize = 8)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

            // Lista los productos filtrados por categoría y marca
            lista = new CNProducto().ListarProductos().Select(p => new Producto()
            {
                id_Producto = p.id_Producto,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                objMarca = p.objMarca,
                objCategoria = p.objCategoria,
                Precio = p.Precio,
                Stock = p.Stock,
                RutaImagen = p.RutaImagen,
                Base64 = Recursos.convertirBase64(Path.Combine(p.RutaImagen, p.NombreImagen), out conversion),
                Extension = Path.GetExtension(p.NombreImagen),
                Estado = p.Estado
            }).Where(p =>
                p.objCategoria.id_Categoria == (idcategoria == 0 ? p.objCategoria.id_Categoria : idcategoria) &&
                p.objMarca.id_Marca == (idMarca == 0 ? p.objMarca.id_Marca : idMarca) &&
                p.Stock > 0 && p.Estado == true
            ).Skip((pageNumber - 1) * pageSize) // Salta los productos de las páginas anteriores
             .Take(pageSize) // Toma solo la cantidad de productos de la página actual
             .ToList();

            // Obtén el total de productos para la paginación
            var totalProductos = new CNProducto().ListarProductos().Where(p =>
                p.objCategoria.id_Categoria == (idcategoria == 0 ? p.objCategoria.id_Categoria : idcategoria) &&
                p.objMarca.id_Marca == (idMarca == 0 ? p.objMarca.id_Marca : idMarca) &&
                p.Stock > 0 && p.Estado == true
            ).Count();

            var jsonResult = Json(new { data = lista, total = totalProductos }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //metodo para agregar al carrito
        [HttpPost]
        public JsonResult AgregarAlCarrito(int idProducto)
        {
            int idCliente = ((Cliente)Session["UsuarioCliente"]).id_Cliente;

            bool existe = new CNCliente_Producto().ExisteCarrito(idCliente, idProducto);

            bool respuesta = false;

            string mensaje = string.Empty;

            if (existe)
            {
                mensaje = "El producto ya existe en el carrito";
            }
            else
            {
                respuesta = new CNCliente_Producto().OperacionCarrito(idCliente, idProducto, true, out mensaje);
            }
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }



        //devuelve la cantidad de productos del cliente como Query al iniciar Sesion 
        [HttpGet]
        public JsonResult CantidadEnCarrito() //funcion que muestra o devuelve la cantidad de productos en el carrito por un cliente especifico en el inicio de sesion
        {
            int idCliente = ((Cliente)Session["UsuarioCliente"]).id_Cliente;
            int cantidad = new CNCliente_Producto().CantidadEnCarrito(idCliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);
        }



        //metodo para listarlos productos en el carrito por parte de un cliente para hacer la compra
        [HttpPost]
        public JsonResult ListarProductosCarrito() //controlador que muestra una lista de productos en el carrito 
        {
            int idCliente = ((Cliente)Session["UsuarioCliente"]).id_Cliente;

            List<Cliente_Producto> lista = new List<Cliente_Producto>();

            bool conversion;

            lista = new CNCliente_Producto().ListarProductoCarrito(idCliente).Select(oc => new Cliente_Producto()
            {
                objProd = new Producto()
                {
                    id_Producto = oc.objProd.id_Producto,
                    Nombre = oc.objProd.Nombre,
                    objMarca = oc.objProd.objMarca,
                    Precio = oc.objProd.Precio,
                    RutaImagen = oc.objProd.RutaImagen,
                    Base64 = Recursos.convertirBase64(Path.Combine(oc.objProd.RutaImagen, oc.objProd.NombreImagen), out conversion),
                    Extension = Path.GetExtension(oc.objProd.NombreImagen)
                },
                Cantidad = oc.Cantidad
            }).ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet); //devolviendo lista de productos del carrito de un cliente determinado
        }



        //metodo para agregar al carrito
        [HttpPost]
        public JsonResult OperacionCarro(int idProducto, bool sumar) //metodo para agregar productos al carrito
        {
            int idCliente = ((Cliente)Session["UsuarioCliente"]).id_Cliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CNCliente_Producto().OperacionCarrito(idCliente, idProducto, sumar, out mensaje);
            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }




        //metodo para eliminar un producto en el carrito
        [HttpPost]
        public JsonResult EliminarProductoCarrito(int idProducto)
        {
            int idCliente = ((Cliente)Session["UsuarioCliente"]).id_Cliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CNCliente_Producto().EliminarProductoEnCarrito(idCliente, idProducto);

            return Json(new { respuesta = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        //metodo para obtener departamento
        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> lista = new List<Departamento>();

            lista = new CNUbicacion().ObtenerDepartamento();

            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);
        }



        //metodo para obtener municipio por departamento
        [HttpPost]
        public JsonResult ObtenerMunicipio(string iddepartamento)
        {
            List<Municipio> lista = new List<Municipio>();

            lista = new CNUbicacion().ObtenerMunicipio(iddepartamento);

            return Json(new { lista = lista }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost] //metodo de tipo Task o de hilo para que ambos procesos se ejecuten secuencialmente mediante una dependencia funcional de la API
        public async Task<JsonResult> ProcesarPago(List<Cliente_Producto> olistaCarrito, Venta oVenta)
        {
            const decimal tasadolar = 36.50m;
            decimal total = 0;
            decimal totalIva = 0;
            decimal Total = 0;
            decimal TotalEnDolar = 0;

            DataTable detalle_Venta = new DataTable();

            detalle_Venta.Locale = new CultureInfo("es-NI");
            //creando columnas
            detalle_Venta.Columns.Add("idProducto", typeof(string));
            detalle_Venta.Columns.Add("Cantidad", typeof(int));
            detalle_Venta.Columns.Add("Total", typeof(decimal));

            List<Item> olistaItem = new List<Item>();

            //iterando la lista del carrito para pasarlo a PayPal junto con el precio 
            foreach (Cliente_Producto oCarrito in olistaCarrito)
            {
                decimal subTotal = Convert.ToDecimal(oCarrito.Cantidad.ToString()) * oCarrito.objProd.Precio;

                total += subTotal;//subtotal sin iva
                totalIva = Math.Round(total * 0.15m);//aplicandole el Iva al SubTotal
                Total = Math.Round(total + totalIva);//sumandole la tasa de Iva al subTotal
                TotalEnDolar = Math.Round(Total / tasadolar);

                olistaItem.Add(new Item()
                {
                    name = oCarrito.objProd.Nombre,
                    quantity = oCarrito.Cantidad.ToString(),
                    unit_amount = new UnitAmount()
                    {
                        currency_code = "USD",
                        value = oCarrito.objProd.Precio.ToString("G", new CultureInfo("es-NI")),
                    }

                });

                //almacennado el detalle de venta para que valla a la tabla detalle de la base de datos
                detalle_Venta.Rows.Add(new object[]
                {
                    oCarrito.objProd.id_Producto,
                    oCarrito.Cantidad,
                    subTotal
                });
            }

            PurchaseUnit purchasetUnit = new PurchaseUnit() //unidades de compras
            {
                amount = new Amount()
                {
                    currency_code = "USD",
                    value = TotalEnDolar.ToString("G", new CultureInfo("es-NI")),
                    breakdown = new Breakdown()
                    {
                        item_total = new ItemTotal()
                        {
                            currency_code = "USD",
                            value = TotalEnDolar.ToString("G", new CultureInfo("es-NI")),

                        }
                    }
                },
                description = "Compra de Articulos de CompuStore",
                items = olistaItem
            };

            checkout_order oChekckout = new checkout_order() //orden de pago
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>()
                {
                     purchasetUnit
                },
                application_context = new ApplicationContext()
                {
                    brand_name = "CompuStore.com",
                    landing_page = "NO_PREFERENCE",
                    user_action = "PAY_NOW",
                    return_url = "https://localhost:44309/Tienda/PagoEfectuado", //hay que cambiar estas lineas para producccion, cambiar tambien el protocolo
                    cancel_url = "https://localhost:44309/Tienda/Carrito"
                }
            };

            oVenta.MontoTotal = total;
            oVenta.MontoTotalIva = Total;
            oVenta.id_Cliente = ((Cliente)Session["UsuarioCliente"]).id_Cliente;

            TempData["Venta"] = oVenta;
            TempData["DetalleVenta"] = detalle_Venta;

            CNPaypal opaypal = new CNPaypal();
            Response_Paypal<Response_checkout> response_paypal = new Response_Paypal<Response_checkout>();
            response_paypal = await opaypal.CrearSolicitud(oChekckout);


            return Json(response_paypal, JsonRequestBehavior.AllowGet);

        }



        [AuthFilter]
        public async Task<ActionResult> PagoEfectuado() //metodo que llama a PayPal
        {
            string token = Request.QueryString["token"];
            CNPaypal paypall = new CNPaypal();
            Response_Paypal<Response_capture> response_paypal = new Response_Paypal<Response_capture>();
            response_paypal = await paypall.AprobarPago(token);




            ViewData["Status"] = response_paypal.Status;

            if (response_paypal.Status)
            {
                Venta oVenta = (Venta)TempData["Venta"];
                DataTable detalle_Venta = (DataTable)TempData["DetalleVenta"];

                oVenta.NumeroTransaccion = response_paypal.Response.purchase_units[0].payments.captures[0].id;

                string mensaje = string.Empty;

                bool respuesta = new CNVenta().RegistrarVenta(oVenta, detalle_Venta, out mensaje);

                ViewData["idTransaccion"] = oVenta.NumeroTransaccion;
            }

            return View();
        }

        //controlador para listar las compras de un cliente 
        [AuthFilter]
        public ActionResult ListarComprasCliente(int page = 1)
        {
            int pageSize = 2; // Cantidad de registros por página
            int idCliente = ((Cliente)Session["UsuarioCliente"]).id_Cliente;

            List<Detalle_Venta> lista = new CNVenta().ListarCompras(idCliente).Select(oc => new Detalle_Venta()
            {
                objProducto = new Producto()
                {
                    Nombre = oc.objProducto.Nombre,
                    Precio = oc.objProducto.Precio,
                    Base64 = Recursos.convertirBase64(Path.Combine(oc.objProducto.RutaImagen, oc.objProducto.NombreImagen), out bool conversion),
                    Extension = Path.GetExtension(oc.objProducto.NombreImagen)
                },
                Cantidad = oc.Cantidad,
                Total = oc.Total,
                TotalIVA= Math.Round(oc.Total + (oc.Total * 0.15m), 2),//calculando el IVA del Producto
                NumeroTransaccion = oc.NumeroTransaccion,
                FechaVenta = oc.FechaVenta
            }).OrderByDescending(x => x.FechaVenta).ToList();

            int totalItems = lista.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedList = lista.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.PageNumber = page;
            ViewBag.TotalPages = totalPages;

            return View(paginatedList);
        }
    }
}