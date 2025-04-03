using CapaEntidad;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CDUbicacion
    {
        SqlConnection conexion;
        public List<Departamento> ObtenerDepartamento()
        {

            List<Departamento> lista = new List<Departamento>();
            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {

                    string consulta = "select * from Departamento";

                    SqlCommand comando = new SqlCommand(consulta, conexion);

                    comando.CommandType = CommandType.Text;

                    conexion.Open();
                    using (SqlDataReader dr = comando.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            lista.Add
                            (
                                new Departamento()
                                {
                                    idDepartamento = dr["idDepartamento"].ToString(),
                                    NombreDepartamento = dr["NombreDepartamento"].ToString()
                                }
                            );
                        }

                    }
                }

            }
            catch
            {

                lista = new List<Departamento>();
            }
            finally
            {
                conexion.Close();
            }
            return lista;

        }

        //obteniendo los municipios de los departamentos
        public List<Municipio> ObtenerMunicipio(string idDepartamento)
        {

            List<Municipio> lista = new List<Municipio>();
            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {

                    string consulta = "select * from Municipio where iddepartamento=@iddepartamento";

                    SqlCommand comando = new SqlCommand(consulta, conexion);
                    comando.Parameters.AddWithValue("@iddepartamento", idDepartamento);

                    comando.CommandType = CommandType.Text;

                    conexion.Open();
                    using (SqlDataReader dr = comando.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            lista.Add
                            (
                                new Municipio()
                                {
                                    idMunicipio = dr["idMunicipio"].ToString(),
                                    NombreMunicipio = dr["NombreMunicipio"].ToString(),
                                }
                            );
                        }

                    }
                }

            }
            catch
            {

                lista = new List<Municipio>();
            }
            finally
            {
                conexion.Close();
            }
            return lista;

        }
    }
}