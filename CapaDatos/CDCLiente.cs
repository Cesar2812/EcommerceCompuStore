using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CDCLiente
    {
        //Funcion para Registrar Cliente  devuelve un entero
        public int RegistrarCliente(Cliente objCliente, out string Mensaje)
        {
            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //ejecuando el sp mediante sqlComand recibe el nombre del sp y la conexion y pasandole los valores
                    SqlCommand comando = new SqlCommand("sp_RegistrarCliente", conexion);
                    comando.Parameters.AddWithValue("@Nombre", objCliente.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", objCliente.Apellido);
                    comando.Parameters.AddWithValue("@Correo", objCliente.Correo);
                    comando.Parameters.AddWithValue("@Clave", objCliente.Clave);
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


        //metodo para listar un cliente que esta en la base de datos 
        public List<Cliente> ListarCliente()
        {

            List<Cliente> lista = new List<Cliente>();

            try
            {
                /*Lo que se hace aca es usar el metodo using donde se instancia la clase SqlConnection
                 en la cual se parametriza con la clase Conexion que contiene a la variable cn que tiene
                 nuestra cadena
                 */
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //consulta a la base de datos a la tabla usuario
                    string consulta = "Select id_Cliente,Nombre,Apellido,Correo,Clave,Restablecer from Cliente";

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
                                new Cliente()
                                {
                                    id_Cliente = Convert.ToInt32(dr["id_Cliente"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Apellido = dr["Apellido"].ToString(),
                                    Correo = dr["Correo"].ToString(),
                                    Clave = dr["Clave"].ToString(),
                                    Restablecer = Convert.ToBoolean(dr["Restablecer"]),//convierte los valores a booleanos

                                }
                            );
                        }

                    }
                }

            }
            catch
            {
                //le voy a decir que si ocurre un problema reinicia la lista de forma vacia
                lista = new List<Cliente>();
            }
            return lista;

        }

        //metodo para cambiar clave del cliente
        public bool CambiarClave(int idCliente, string nuevaClave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //pasando el id del usuario para CambiarLaClave
                    SqlCommand comando = new SqlCommand("update Cliente set Clave = @nuevaClave,Restablecer = 0 where id_Cliente = @idCliente", conexion);
                    comando.Parameters.AddWithValue("@idUsuario", idCliente);
                    comando.Parameters.AddWithValue("@nuevaClave", nuevaClave);
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

        //metodo para reestablecer clave del cliente
        public bool RestablecerClave(int idCliente, string Clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //pasando el id del usuario para Restablecer la clave
                    SqlCommand comando = new SqlCommand("update Cliente set Clave = @Clave,Restablecer = 1 where id_Cliente = @idCliente", conexion);
                    comando.Parameters.AddWithValue("@idCliente", idCliente);
                    comando.Parameters.AddWithValue("@Clave", Clave);
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
