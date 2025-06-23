using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CDTipoServicio
    {
        SqlConnection conexion;//variable global de conexion
        public List<TipoServicio> ListarTipoServicio()
        {
            List<TipoServicio> lista = new List<TipoServicio>();

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    string consulta = "select id_TipoServcio,Descripcion from Tipo_Servicio";

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
                               new TipoServicio()
                               {
                                   id_TipoServicio = Convert.ToInt32(read["id_TipoServcio"]),
                                   Descripcion = read["Descripcion"].ToString()  
                               }
                            );

                        }
                    }
                }
            }
            catch
            {
                //si ocurre un problema que reinicie le lista
                lista = new List<TipoServicio>();

            }
            finally
            {

                conexion.Close();//cerrendo conexion para liberar recursos 
            }
            // retorna toda la lista de categoria
            return lista;
        }


        public int ResgistrarTipo_Servicio(TipoServicio objTipoServicio, out string Mensaje)
        {
            int idAutogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    //le paso el sp
                    SqlCommand comando = new SqlCommand("sp_RegistrarTipo_Servicio", conexion);
                    comando.Parameters.AddWithValue("@Descripcion", objTipoServicio.Descripcion);
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
    }
}
