using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CDCategorias
    {
        public List<Categoria> ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string consulta = "select id_Categoria,Descripcion,Estado from Categoria";

                    SqlCommand cmd = new SqlCommand(consulta, conexion);

                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader read = cmd.ExecuteReader())
                    {
                        //mientras se valla leyendo el resultado que agegre los datos a la lista
                        while (read.Read())
                        {
                            //diciendole a la lista que anada un objeto de tipo categoria 
                            lista.Add(
                               new Categoria()
                               {
                                   id_Categoria= Convert.ToInt32(read["id_Categoria"]),
                                   Descripcion = read["Descripcion"].ToString(),
                                   Estado = Convert.ToBoolean(read["Estado"])
                               }
                            );

                        }
                    }

                }

            }
            catch
            {
                //si ocurre un problema que reinicie le lista
                lista = new List<Categoria>();

            }

            //al final retorna toda la lista de categoria
            return lista;

        }

        //metodo para registrar Categoria
        public int RegistrarCategoria(Categoria objCategoria, out string Mensaje)
        {
            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //le paso el sp
                    SqlCommand comando = new SqlCommand("sp_RegistrarCategoria", conexion);
                    comando.Parameters.AddWithValue("@Descripcion", objCategoria.Descripcion);
                    comando.Parameters.AddWithValue("@Estado", objCategoria.Estado);
                    comando.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;//pasando el parametro de respuesta del servidor ya sea 0 o 1
                    comando.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;//pasando la respuesta del servidor despues del registro ya sea malo o bueno en base a la validacion en el server
                    comando.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    //se ejecuta el query
                    comando.ExecuteNonQuery();

                    //conviertiendo el id para la validacion
                    idAutogenerado = Convert.ToInt32(comando.Parameters["@Resultado"].Value);
                    Mensaje = comando.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;
                idAutogenerado = 0;
            }
            return idAutogenerado;
        }


        //metodo para editar Categoria
        public bool EditarCatgeoria(Categoria objCateg, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand comando = new SqlCommand("sp_EditarCategoria", conexion);
                    comando.Parameters.AddWithValue("@id_Categoria", objCateg.id_Categoria);
                    comando.Parameters.AddWithValue("@Descripcion", objCateg.Descripcion);
                    comando.Parameters.AddWithValue("@Estado", objCateg.Estado);
                    comando.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    comando.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(comando.Parameters["@Resultado"].Value);
                    Mensaje = comando.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }

        //metodo para eilimnar Categoria
        public bool EliminarCategoria(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand comando = new SqlCommand("sp_EliminarCategoria", conexion);
                    comando.Parameters.AddWithValue("@id_Categoria", id);
                    comando.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    comando.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    comando.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    comando.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(comando.Parameters["@Resultado"].Value);
                    Mensaje = comando.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return (resultado);

        }
    }
}
