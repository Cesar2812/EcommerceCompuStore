using CapaEntidad;
using CapaNegocio;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class UsuariosController : Controller
    {
        //Metododo que devuelve la vista de Usuarios osea el form dentro de la pagina master para registrar Usuarios
        public ActionResult Usuarios()
        {
            return View();
        }

        //controlador que ira dentro del panel de administracion
        #region Usuarios 
        //metodo para mostrar la lista de usuarios en el el DataTable esto lo estoy creando como un controller
        //esto en formato JSON asi lo devuelve por ende es un HTTPGET
        [HttpGet]
        public JsonResult ListarUsuarios()//metodo para devolver en fromato json 
        {
            //haciendo un list de usuarios de la capa entidad porque las varibales estan pobladas
            List<Usuario> olista = new List<Usuario>();

            //diciendole a la olista que almacene todos los elementos de la capa de negocio de la lista de usuarios
            olista = new CNUsuarios().Listar();

            //retornando un json que recibe como parmetro la olista y le da formato al data table porque eso recibe
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

            //para efectos de prueba le paso el nombre del controlador
            // y a parte tambien el nombre del metodo todoo esto a la URL

        }

        //Metodo para registrar y editar recibe como parametro la capa entidad Usuario
        [HttpPost]//metodo post ya que es una pediticion hecha por el cliente ya sea de insercion o edicion a la base de datos
        public JsonResult GuardarUsuario(Usuario objeto)
        {
            object resultado;
            string mensaje = string.Empty;

            //logica para saber si editar o guardar un usuario
            if (objeto.id_Usuario == 0)//esta ingresando un nuevo usuario
            {
                resultado = new CNUsuarios().Registrar(objeto, out mensaje);

            }
            else
            {
                resultado = new CNUsuarios().Editar(objeto, out mensaje);

            }
            //devolviendo el reultado en JSON
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        //Metodo eilimnar Usuario recibiendo como parametro el idpara eliminar
        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            //almacenando el metodo eliminar en una variable
            respuesta = new CNUsuarios().Eliminar(id, out mensaje);

            //devolviendo el reultado en JSON
            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion //fin de la region de usuarios del panel intero
    }
}