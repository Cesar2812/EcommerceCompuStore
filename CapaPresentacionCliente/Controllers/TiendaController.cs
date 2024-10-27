using CapaEntidad;
using CapaNegocio;
using CapaPresentacionCliente.Filtros;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace CapaPresentacionCliente.Controllers
{
    public class TiendaController : Controller
    {
        //Vista de incio
        public ActionResult Index()
        {
            return View();
        }

        //vista del carrito
        [AuthFilter]
        public ActionResult Carrito()
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


        //metodo para obtener Categorias
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

        //Metodo para listar Productos en base a una categoria y marca seleccionada
        [HttpPost]
        public JsonResult ListarProductos(int idcategoria, int idMarca)
        {
            List<Producto> lista = new List<Producto>();

            bool conversion;

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
                Activo = p.Activo
            }).Where(p =>
                p.objCategoria.id_Categoria == (idcategoria == 0 ? p.objCategoria.id_Categoria : idcategoria) &&
                p.objMarca.id_Marca == (idMarca == 0 ? p.objMarca.id_Marca : idMarca) &&
                p.Stock > 0 && p.Activo == true
            ).ToList();

            var jsonResult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
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
        public JsonResult CantidadEnCarrito()
        {
            int idCliente = ((Cliente)Session["UsuarioCliente"]).id_Cliente;
            int cantidad = new CNCliente_Producto().CantidadEnCarrito(idCliente);
            return Json(new { cantidad = cantidad }, JsonRequestBehavior.AllowGet);

        }


        //metodo para listarlos productos en el carrito por parte de un cliente para hacer la compra
        [HttpPost]
        public JsonResult ListarProductosCarrito()
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
        public JsonResult OperacionCarro(int idProducto, bool sumar)
        {
            int idCliente = ((Cliente)Session["UsuarioCliente"]).id_Cliente;

            bool respuesta = false;

            string mensaje = string.Empty;

            respuesta = new CNCliente_Producto().OperacionCarrito(idCliente, idProducto, true, out mensaje);
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


    }
}