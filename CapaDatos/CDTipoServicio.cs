using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
