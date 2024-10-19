using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionCliente.Controllers
{
    public class TiendaController : Controller
    {
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }


        //metodo para obtener Categorias
        [HttpGet]
        public JsonResult ListaCategorias()
        { 
            //creando lista de categorias
            List<Categoria> lista = new List<Categoria>();

            lista=new CNCategorias().ListarCategoria();// almacenando categorias en la lista
            return Json(new {data= lista},JsonRequestBehavior.AllowGet); //retornando categorias en un JSON
        }

        [HttpPost]
        public JsonResult ListaMarcaPorCategoria( int idCategoria)
        {
            //creando lista de categorias
            List<Marca> lista = new List<Marca>();

            lista = new CNMarcas().ListarMarcaporCategoria(idCategoria);// almacenando las marcas en la lista
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet); //retornando las marcas en un JSON
        }

        //Metodo para listar Productos en base a una categoria y marca seleccionada
        [HttpPost]
        public JsonResult ListarProductos(int idcategoria,int idMarca)
        {
            List<Producto> lista= new List<Producto>();

            bool conversion;

            lista= new CNProducto().ListarProductos().Select(p=>new Producto()
            {
                id_Producto=p.id_Producto,
                Nombre=p.Nombre,
                Descripcion=p.Descripcion,
                objMarca=p.objMarca,
                objCategoria=p.objCategoria,
                Precio=p.Precio,    
                Stock=p.Stock,  
                RutaImagen=p.RutaImagen,
                Base64=Recursos.convertirBase64(Path.Combine(p.RutaImagen,p.NombreImagen),out conversion),
                Extension=Path.GetExtension(p.NombreImagen),
                Activo=p.Activo
            }).Where(p=>
                p.objCategoria.id_Categoria ==(idcategoria == 0 ? p.objCategoria.id_Categoria:idcategoria)&&
                p.objMarca.id_Marca==(idMarca ==0? p.objMarca.id_Marca: idMarca) &&
                p.Stock>0 && p.Activo==true
            ).ToList();

            var jsonResult = Json(new { data = lista }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength=int.MaxValue;
            return jsonResult;
                
        }
    }
}