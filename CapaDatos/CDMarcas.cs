using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CapaDatos
{
    public class CDMarcas
    {
        SqlConnection conexion;

        //metodo para listar marcas 
        public List<Marca> ListarMarca()
        {
            List<Marca> lista = new List<Marca>();

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    string consulta = "select id_Marca,Descripcion,Estado  from Marca";

                    SqlCommand comando = new SqlCommand(consulta, conexion);

                    comando.CommandType = CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader read = comando.ExecuteReader())
                    {
                        //mientras se valla leyendo el resultado que agegre los datos a la lista
                        while (read.Read())
                        {
                            //diciendole a la lista que anada un objeto de tipo categoria 
                            lista.Add(
                               new Marca()
                               {
                                   id_Marca = Convert.ToInt32(read["id_Marca"]),
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

                lista = new List<Marca>();
            }
            finally
            {
                conexion.Close();
            }
            return lista;

        }

        //metodo para ingresar Marca
        public int RegistrarMarca(Marca objMarca, out string Mensaje)
        {
            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    //le paso el sp
                    SqlCommand comando = new SqlCommand("sp_RegistrarMarca", conexion);
                    comando.Parameters.AddWithValue("@Descripcion", objMarca.Descripcion);
                    comando.Parameters.AddWithValue("@Estado", objMarca.Estado);
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
            finally
            {
                conexion.Close();
            }
            return idAutogenerado;
        }

        //metodo para editar Marca
        public bool EditarMarca(Marca objMarca, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand comando = new SqlCommand("sp_EditarMarca", conexion);
                    comando.Parameters.AddWithValue("@id_Marca", objMarca.id_Marca);
                    comando.Parameters.AddWithValue("@Descripcion", objMarca.Descripcion);
                    comando.Parameters.AddWithValue("@Estado", objMarca.Estado);
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
            finally
            {
                conexion.Close();
            }

            return resultado;
        }

        //metodo para eiliminar una marca
        public bool EliminarMarca(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand comando = new SqlCommand("sp_EliminarMarca", conexion);
                    comando.Parameters.AddWithValue("@id_Marca", id);
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
            finally
            {
                conexion.Close();
            }
            return (resultado);

        }

        //metodo para listar marcas 
        public List<Marca> ListarMarcaporCategoria(int idCategoria)
        {
            List<Marca> lista = new List<Marca>();

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Select distinct m.id_Marca,m.Descripcion from Producto p");
                    sb.AppendLine("inner join Categoria as c on p.idCategoria=c.id_Categoria");
                    sb.AppendLine("inner join Marca as m on p.idMarca=m.id_Marca and m.Estado=1");
                    sb.AppendLine("where c.id_Categoria= iif(@idcategoria=0, c.id_Categoria,@idcategoria)");

                    SqlCommand comando = new SqlCommand(sb.ToString(), conexion);
                    comando.Parameters.AddWithValue("@idcategoria", idCategoria);

                    comando.CommandType = CommandType.Text;

                    conexion.Open();

                    using (SqlDataReader read = comando.ExecuteReader())
                    {
                        //mientras se valla leyendo el resultado que agegre los datos a la lista
                        while (read.Read())
                        {
                            //diciendole a la lista que anada un objeto de tipo categoria 
                            lista.Add(
                               new Marca()
                               {
                                   id_Marca = Convert.ToInt32(read["id_Marca"]),
                                   Descripcion = read["Descripcion"].ToString(),


                               }
                            );

                        }
                    }
                }
            }
            catch
            {

                lista = new List<Marca>();
            }
            finally
            {
                conexion.Close();
            }
            return lista;

        }
    }
}