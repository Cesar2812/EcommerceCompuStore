using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
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
    }
}