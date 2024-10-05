using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CDReportes
    {
        //metodo para que caregue la data en los card 
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


        //metodo para ver el reporte de ventas por fecha pasando un sp
        public List<ReporteVentas> ReporteVentas(string fechainicio, string fechafin, string numeroTransaccion) //parametrizando el metodo por los datos que se interesa buscar
        {
            List<ReporteVentas> lista = new List<ReporteVentas>(); //creando una lista con los campos del reporte
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //ejecutando el sp
                    SqlCommand cmd = new SqlCommand("sp_ReporteVentas", conexion);
                    cmd.Parameters.AddWithValue("@fechainicio", fechainicio);
                    cmd.Parameters.AddWithValue("@fechafin", fechafin);
                    cmd.Parameters.AddWithValue("@numeroTransaccion", numeroTransaccion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    //ejecutando el lector
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new ReporteVentas()
                                {
                                    FechaDeVenta = dr["FechaDeVenta"].ToString(),
                                    Cliente = dr["Cliente"].ToString(),
                                    Producto = dr["Producto"].ToString(),
                                    Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-NI")),
                                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                                    Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-NI")),
                                    NumeroTransaccion = dr["NumeroTransaccion"].ToString()
                                }
                            );
                        }
                    }
                }
            }
            catch
            {
                lista = new List<ReporteVentas>();
            }
            return lista;
        }

    }
}
