using CapaEntidad;
using System;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CDReportes
    {
        public Reportes VerReporte()
        {
            Reportes objto = new Reportes(); //creando un objeto de los reportes para mostrarlos en el dashboard

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand comando = new SqlCommand("sp_ReporteDashboard", conexion);
                    comando.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    using (SqlDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            objto = new Reportes()
                            {
                                TotalClientes = Convert.ToInt32(dr["TotalClientes"]),
                                TotalVenta = Convert.ToInt32(dr["TotalVenta"]),
                                TotalProducto = Convert.ToInt32(dr["TotalProducto"]),
                                GananciaTotal = Convert.ToDecimal(dr["GananciaTotal"])
                            };
                        }
                    }
                }
            }
            catch
            {
                objto = new Reportes();

            }
            return objto;
        }
    }
}
