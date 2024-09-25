using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace CapaDatos
{
    public class CDUsuarios
    {
        //metodo para listar o mostrar Usuarios desde la base de datos en el dataTable 
        public List<Usuario> Listar()
        {

            List<Usuario> lista = new List<Usuario>();

            try
            {
                /*Lo que se hace aca es usar el metodo using donde se instancia la clase SqlConnection
                 en la cual se parametriza con la clase Conexion que contiene a la variable cn que tiene
                 nuestra cadena
                 */
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //consulta a la base de datos a la tabla usuario
                    string consulta = "select id_Usuario,Nombre,Apellido,Correo,Restablecer,Activo from Usuario";

                    //comando para ejecutar la consulta 
                    //la clase sqcommand recibe como parametro la consulta y la conexion a la base para poderla ejecutar 
                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    //le pasamos al comando que tipo de comando va a ejecutar
                    comando.CommandType = CommandType.Text;//comando de tipo texto osea la consulta

                    //abrimos la conexion
                    conexion.Open();

                    //ejecutar el query por medio del metodo using, con la clase sqlDataReader leemos el resultado del quiery
                    using (SqlDataReader dr = comando.ExecuteReader())
                    {
                        //mientras el dr esta leyendo fila por fila->
                        //rellename la lista un objeto usuario de la capa entidad
                        while (dr.Read())
                        {
                            /* lo que se hace aca es agregar a la lista los campos del query 
                             * mientras el dr va leyendo
                             */
                            lista.Add
                            (
                                new Usuario()
                                {
                                    id_Usuario = Convert.ToInt32(dr["id_Usuario"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Apellido = dr["Apellido"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Restablecer = Convert.ToBoolean(dr["Restablecer"]),//convierte los valores a booleanos
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                }
                            );
                        }

                    }
                }

            }
            catch
            {
                //le voy a decir que si ocurre un problema reinicia la lista de forma vacia
                lista = new List<Usuario>();

            }
            return lista;

        }

        //Funcion para editar usuario devuelve un entero
        public int Registrar(Usuario objUsuario, out string Mensaje)
        {
            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //ejecuando el sp mediante sqlComand recibe el nombre del sp y la conexion y pasandole los valores
                    SqlCommand comando = new SqlCommand("sp_RegistrarUsuario", conexion);
                    comando.Parameters.AddWithValue("@Nombre", objUsuario.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", objUsuario.Apellido);
                    comando.Parameters.AddWithValue("@Correo", objUsuario.Correo);
                    comando.Parameters.AddWithValue("@Clave", objUsuario.Clave);
                    comando.Parameters.AddWithValue("@Activo", objUsuario.Activo);
                    comando.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;//pasando el parametro de respuesta del servidor ya sea 0 o 1
                    comando.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;//pasando la respuesta del servidor despues del registro ya sea malo o bueno en base a la validacion en el server
                    comando.CommandType = CommandType.StoredProcedure;//diciendo que es un tipo comando sp hacia el servidor

                    //se abre la conexion
                    conexion.Open();

                    //se ejecuta el query
                    comando.ExecuteNonQuery();

                    //conviertiendo el id para la validacion
                    idAutogenerado = Convert.ToInt32(comando.Parameters["@Resultado"].Value);
                    Mensaje = comando.Parameters["@Mensaje"].Value.ToString();//convierte el mensaje en una cadena de texto

                }

            }
            catch (Exception ex)
            {
                //reiniciando el id si ocurre error
                idAutogenerado = 0;
                Mensaje = ex.Message;

            }

            return idAutogenerado;
        }

        //funcion para editar usuario parametrizado con la capa entidad y el mensaje de entrada
        public bool Editar(Usuario objUser, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", conexion);
                    cmd.Parameters.AddWithValue("@id_Usuario", objUser.id_Usuario);
                    cmd.Parameters.AddWithValue("@Nombre", objUser.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", objUser.Apellido);
                    cmd.Parameters.AddWithValue("@Correo", objUser.Correo);
                    cmd.Parameters.AddWithValue("@Activo", objUser.Activo);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;

        }

        //metodo de eliminar
        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //pasando el id del usuario para eliminar
                    SqlCommand comando = new SqlCommand("delete top(1) from Usuario where id_Usuario= @id", conexion);
                    comando.Parameters.AddWithValue("@id", id);
                    comando.CommandType = CommandType.Text;
                    conexion.Open();
                    resultado = comando.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }
    }
}
