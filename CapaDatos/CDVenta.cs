using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace CapaDatos
{
    public class CDVenta
    {
        SqlConnection conexion;

        //funcion para registrar una venta, recibe un objeto Venta y un DataTable con el detalle de la venta
        //esta se hace en la vista del cliente carrito de compras
        public bool RegistrarVenta(Venta objVenta, DataTable Detalle_Venta, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
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
            finally
            {
                conexion.Close();
            }
            return respuesta;

        }


        //funcion para listar las compras de un cliente en especifico dentro de la vista del cliente
        // esta funcion recibe el id del cliente y retorna una lista de Detalle_Venta
        public List<Detalle_Venta> ListarCompras(int idCliente)
        {
            //declarando lista de Productos
            List<Detalle_Venta> lista = new List<Detalle_Venta>();

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
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
            finally
            {
                conexion.Close();
            }
            return lista;
        }


        //este se metodo se ejecuta en la pagina de inicio que muestra los productos mas vendidos
        public List<Detalle_Venta> ListarProductosMasVendidos()
        {
            List<Detalle_Venta> listadetalleProducto = new List<Detalle_Venta>();
            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    //creando un objeto string builder
                    StringBuilder sb = new StringBuilder();
                    //abriendo consulta con salto de linea
                    sb.AppendLine("SELECT P.id_Producto,P.Nombre,C.Descripcion as Categoria,P.Precio,P.Stock,");
                    sb.AppendLine("P.RutaImagen,P.NombreImagen,");
                    sb.AppendLine("SUM(DV.Cantidad) AS TotalVendidos");
                    sb.AppendLine("FROM Detalle_Venta DV");
                    sb.AppendLine("INNER JOIN Producto P ON P.id_Producto = DV.idProducto");
                    sb.AppendLine("INNER JOIN Categoria C on P.idCategoria=C.id_Categoria");
                    sb.AppendLine("GROUP BY P.id_Producto, P.Nombre,C.Descripcion, P.Precio, P.Stock, P.RutaImagen, P.NombreImagen");
                    sb.AppendLine("ORDER BY TotalVendidos DESC");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            listadetalleProducto.Add(new Detalle_Venta
                            {
                                objProducto = new Producto()
                                {
                                    id_Producto = Convert.ToInt32(rdr["id_Producto"]),
                                    Nombre = rdr["Nombre"].ToString(),
                                    objCategoria= new Categoria()
                                    {
                                        Descripcion = rdr["Categoria"].ToString()
                                    },
                                    Precio = Convert.ToDecimal(rdr["Precio"], new CultureInfo("es-NI")),
                                    Stock = Convert.ToInt32(rdr["Stock"]),
                                    RutaImagen = rdr["RutaImagen"].ToString(),
                                    NombreImagen = rdr["NombreImagen"].ToString(),
                                },
                                TotalProductosVendidos = Convert.ToInt32(rdr["TotalVendidos"])
                            });

                        }

                    }
                }
            }
            catch
            {
                listadetalleProducto = new List<Detalle_Venta>();// reinicia la lsta de productos mas vendidos
            }
            finally
            {
                conexion.Close();
            }
            return listadetalleProducto;
        }

        //ventas en el dashboard de la vista admin
        public List<Venta> ListarVentasDasboard()
        {
            List<Venta> lista = new List<Venta>();

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "sp_VentasDasboard";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType =CommandType.StoredProcedure;
                    conexion.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lista.Add(new Venta
                            {
                                mes = rdr["Mes"].ToString(),
                                Cantidad = Convert.ToInt32(rdr["Cantidad"]),
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Venta>();
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }

        //reporte de productos mas vendidos en el dashboard vista admin
        public List<Producto> ListarProductosDasboard()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "sp_ProductosDasboard";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lista.Add(new Producto
                            {
                                producto = rdr["Producto"].ToString(),
                                cantidad = Convert.ToInt32(rdr["Cantidad"])
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Producto>();
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }


        //obtener detalle de venta por id para la factura en la vista admin 
        public Venta ObtenerVentaPorId(int idVenta)
        {
            Venta venta = null;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ObtenerDetalleVentaPorId", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);

                    conexion.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            if (venta == null)
                            {
                                venta = new Venta
                                {
                                    objCliente= new Cliente
                                    {
                                        Nombre = rdr["NombreCliente"].ToString(), 
                                    },
                                    objMunicipio= new Municipio
                                    {
                                        NombreMunicipio = rdr["NombreMunicipio"].ToString(),
                                        objDepartamento = new Departamento
                                        {
                                            NombreDepartamento = rdr["NombreDepartamento"].ToString()
                                        }
                                    },
                                    FechaTexto = rdr["FechaDeVenta"].ToString(),
                                    Telefono = rdr["Telefono"].ToString(),
                                    NumeroTransaccion = rdr["NumeroTransaccion"].ToString(),
                                    MontoTotal = Convert.ToDecimal(rdr["MontoTotal"]),
                                    MontoTotalIva = Convert.ToDecimal(rdr["MontoTotalIva"]),
                                };
                            }

                            venta.objDetalleVenta.Add(new Detalle_Venta
                            { 
                                Cantidad= Convert.ToInt32(rdr["Cantidad"]),
                                objProducto = new Producto
                                {
                                    Nombre = rdr["NombreProducto"].ToString(),
                                    Precio = Convert.ToDecimal(rdr["Precio"]), 
                                },
                                Total = Convert.ToDecimal(rdr["SubTotal"]),

                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error al obtener la venta: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }

            return venta;
        }
    }
}