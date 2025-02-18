using CapaEntidad;
using CapaNegocio;
using CapaPresentacionAdmin.Filtros;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{ 
    //se le aplica un filtro a todos los modulos o menus de la app del admin para que no se pueda accceder sin antes iniciar sesion
    [AuthFilter]
    public class CatalogosController : Controller
    {
        public ActionResult Categorias()//vista de Categorias
        {
            return View();
        }

        public ActionResult Marcas()//vista de Marcas
        {
            return View();
        }

        public ActionResult Productos()//vista de Productos
        {
            return View();
        }


        #region CATEGORIA
        //-----INICIO DE LOS CONTROLADORES PARA CATEGORIAS-----------
        [HttpGet]//tipo get porque obtiene datos de la tabla de categrias
        public JsonResult ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            lista = new CNCategorias().ListarCategoria();

            //retornando la lista en formato JSON
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]//POST ya que es una peticion de insert hacia la base de datos
        public JsonResult GuardarCategorias(Categoria objeto)
        {
            object resultado = 0;
            string Mensaje = string.Empty;

            if (objeto.id_Categoria == 0)
            {
                resultado = new CNCategorias().RegistrarCategoria(objeto, out Mensaje);
            }
            else
            {
                resultado = new CNCategorias().EditarCategoria(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]//POST porque se envia el id para eliminar la categoria
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CNCategorias().EliminarCategoria(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        //---FIN DE LOS CONTROLADORES PARA CATEGORIAS--------------------------------------------------
        #endregion


        #region MARCA
        //-----INICIO DE LOS CONTROLADORES PARA MARCAS-----------
        [HttpGet]//tipo get porque obtiene datos de la tabla de categrias
        public JsonResult ListarMarca()
        {
            List<Marca> lista = new List<Marca>();

            lista = new CNMarcas().ListarMarca();

            //retornando la lista en formato JSON
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]//POST ya que es una peticion de insert hacia la base de datos
        public JsonResult GuardarMarcas(Marca objeto)
        {
            object resultado = 0;
            string Mensaje = string.Empty;

            if (objeto.id_Marca == 0)
            {
                resultado = new CNMarcas().RegistrarMarca(objeto, out Mensaje);
            }
            else
            {
                resultado = new CNMarcas().EditarMarca(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]//POST porque se envia el id para eliminar la categoria
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CNMarcas().EliminarMarca(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        //---FIN DE LOS CONTROLADORES PARA MARCAS--------------------------------------------------
        #endregion

        //-----INICIO DE CONTROLADORES PARA PRODUCTOS-----
        #region Productos
        [HttpGet] //metodo get porque se obtienen datos del servidor para mostrarlos en el data table
        public JsonResult ListarProductos()
        {
            List<Producto> lista = new List<Producto>();
            lista = new CNProducto().ListarProductos();
            //retornando la data en formato JSON
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);//metodo get porque agarra la data
        }




        //metodo para registrarProductos
        //metodo post porque es una peticion de insercion por parte del usuario hacia la Base De Datos
        [HttpPost]
        public JsonResult GuardarProductos(string objeto, HttpPostedFileBase archivoImagen)
        {
            string Mensaje = string.Empty;

            bool operacionExitosa = true;
            bool guardarImagenExito = true;

            //convirtiendo el string en un objetoProducto
            Producto objProd = new Producto();
            objProd = JsonConvert.DeserializeObject<Producto>(objeto);// descerializando el objeto string y se convierte en un objeto de tipo producto 

            //validando el formato del precio 
            decimal precio;

            if (decimal.TryParse(objProd.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-NI"), out precio))
            {
                objProd.Precio = precio;//almacenando en el obj precio el valor del precio 
            }
            else
            {
                return Json(new { operacionExitosa = false, Mensaje = "El formato del Precio debe de ser #####.##" }, JsonRequestBehavior.AllowGet);
            }

            //si la opcion es ingresar osea se pasa un id nuevo del obj a nivel de servidor osea es un id nuevo igual a 0
            if (objProd.id_Producto == 0)
            {
                int idProductoGenerado = new CNProducto().RegistrarProducto(objProd, out Mensaje);//metodo que retorna un mensaje y el valor del id

                if (idProductoGenerado != 0)
                {
                    objProd.id_Producto = idProductoGenerado;
                }
                else
                {
                    operacionExitosa = false;
                }
            }
            else
            {
                operacionExitosa = new CNProducto().EditarProducto(objProd, out Mensaje);
            }

            //registrando Imagen osea la oprecion exitosa es guardar porque es true
            if (operacionExitosa)
            {
                if (archivoImagen != null)// si el archivo de la imagen es diferente de null osea se ha elegido una imagen 
                {
                    //haciendo lectura de la ruta que contiene el webConfig
                    string rutaGuardar = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(archivoImagen.FileName);//obteniendo la extension del archivo de la imagen 
                    string nombreImagen = string.Concat(objProd.id_Producto.ToString(), extension);// poniendole el nombre de la imagen y el el id del producto almacenado 

                    try
                    {
                        archivoImagen.SaveAs(Path.Combine(rutaGuardar, nombreImagen)); // guardando la imagen y se le pasa la ruta y el nombre 

                    }
                    catch (Exception ex)
                    {
                        string men = ex.Message;
                        guardarImagenExito = false;
                    }

                    if (guardarImagenExito)// si la imagen se carga de forma exitosa osea es true 
                    {
                        objProd.RutaImagen = rutaGuardar;
                        objProd.NombreImagen = nombreImagen;
                        bool rspta = new CNProducto().GuardarDataImagen(objProd, out Mensaje);// guardando el nombre de la imagen en la base de datos 
                    }
                    else
                    {
                        Mensaje = "Se guardo el producto pero la imagen no";
                    }

                }
            }
            return Json(new { operacionExitosa = operacionExitosa, idGenerado = objProd.id_Producto, Mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }


        //metodo para devolver cadena en base 64 para la imagen al momento de editar el producto 
        //osea cargara la imagen en la etiqueta img 
        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion;

            Producto oprod = new CNProducto().ListarProductos().Where(p => p.id_Producto == id).FirstOrDefault();

            string textoBase64 = Recursos.convertirBase64(Path.Combine(oprod.RutaImagen, oprod.NombreImagen), out conversion);

            return Json(new
            {
                conversion = conversion,
                textoBase64 = textoBase64,
                extension = Path.GetExtension(oprod.NombreImagen)
            }, JsonRequestBehavior.AllowGet);
        }

        //metodo de eliminarProducto
        public JsonResult EliminarProducto(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new CNProducto().EliminarProducto(id, out mensaje);
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion
    }
}