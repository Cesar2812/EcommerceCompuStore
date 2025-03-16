using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CDVenta
    {
        public bool RegistrarVenta(Venta objVenta, DataTable Detalle_Venta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarVenta", conexion);
                    cmd.Parameters.AddWithValue("@idCliente", objVenta.id_Cliente);
                    cmd.Parameters.AddWithValue("@TotalProducto", objVenta.TotalProducto);
                    cmd.Parameters.AddWithValue("@MontoTotal", objVenta.MontoTotal);
                    cmd.Parameters.AddWithValue("@idMunicipio", objVenta.idMunicipio);
                    cmd.Parameters.AddWithValue("@Telefono", objVenta.Telefono);
                    cmd.Parameters.AddWithValue("@direccion", objVenta.Direccion);
                    cmd.Parameters.AddWithValue("@idTransaccion", objVenta.NumeroTransaccion);
                    cmd.Parameters.AddWithValue("@MontoTotalIva", objVenta.MontoTotalIva);
                    cmd.Parameters.AddWithValue("@DetalleVenta", Detalle_Venta);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();

                }

            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;

            }
            return respuesta;

        }


        //funcion para listar las compras de un cliente en especifico dentro de la vista del cliente
        public List<Detalle_Venta> ListarCompras(int idCliente)
        {
            //declarando lista de Productos
            List<Detalle_Venta> lista = new List<Detalle_Venta>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from fn_ListarCompraCliente(@idCliente)";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lista.Add(new Detalle_Venta
                            {
                                objProducto = new Producto()
                                {

                                    Nombre = rdr["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(rdr["Precio"], new CultureInfo("es-NI")),
                                    RutaImagen = rdr["RutaImagen"].ToString(),
                                    NombreImagen = rdr["NombreImagen"].ToString(),


                                },
                                Cantidad = Convert.ToInt32(rdr["Cantidad"]),
                                Total = Convert.ToDecimal(rdr["Total"], new CultureInfo("es-NI")),
                                NumeroTransaccion = rdr["NumeroTransaccion"].ToString(),
                                FechaVenta = Convert.ToDateTime(rdr["FechaDeVenta"])

                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Detalle_Venta>();
            }
            return lista;
        }
    }
}
