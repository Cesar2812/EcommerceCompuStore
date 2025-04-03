using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace CapaDatos
{
    public class CDCliente_Producto
    {
        SqlConnection conexion;

        //devuelve si un  producto existe dentro del carrito de un cliente
        public bool ExisteCarrito(int idCliente, int idProducto)
        {
            bool resultado = false;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", conexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);

                }

            }
            catch
            {
                resultado = false;
            }
            finally
            {
                conexion.Close();
            }

            return resultado;
        }


        //metodo para agregar al carrito
        public bool OperacionCarrito(int idCliente, int idProducto, bool sumar, out string mensaje)
        {
            bool resultado = true;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", conexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    cmd.Parameters.AddWithValue("@Sumar", sumar);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;

            }
            finally
            {
                conexion.Close();
            }

            return resultado;
        }

        //cantidad de productos en el carrito del cliente
        public int CantidadEnCarrito(int idCliente)
        {
            int resultado = 0;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from Cliente_Producto where idCliente=@idCliente", conexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.CommandType = CommandType.Text;
                    conexion.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }

            }
            catch
            {
                resultado = 0;
            }
            finally
            {
                conexion.Close();
            }
            return resultado;

        }


        //metodo para listar los productos del cliente
        public List<Cliente_Producto> ListarProductoCarrito(int idCliente)
        {
            //declarando lista de Productos
            List<Cliente_Producto> lista = new List<Cliente_Producto>();

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from fn_obtenerCarritoCliente(@idCliente)";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lista.Add(new Cliente_Producto
                            {
                                objProd = new Producto()
                                {
                                    id_Producto = Convert.ToInt32(rdr["id_Producto"]),
                                    Nombre = rdr["Nombre"].ToString(),
                                    Precio = Convert.ToDecimal(rdr["Precio"], new CultureInfo("es-NI")),
                                    RutaImagen = rdr["RutaImagen"].ToString(),
                                    NombreImagen = rdr["NombreImagen"].ToString(),
                                    objMarca = new Marca() { Descripcion = rdr["DesMarca"].ToString() },

                                },
                                Cantidad = Convert.ToInt32(rdr["Cantidad"])

                            });

                        }

                    }
                }
            }
            catch
            {
                lista = new List<Cliente_Producto>();// reinicia la lsta de productos del cliente
            }
            finally
            { 
                conexion.Close(); 
            }
            return lista;
        }

        public bool EliminarProductoEnCarrito(int idCliente, int idProducto)
        {
            bool resultado = true;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProductoCarrito", conexion);
                    cmd.Parameters.AddWithValue("@idCliente", idCliente);
                    cmd.Parameters.AddWithValue("@idProducto", idProducto);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);

                }

            }
            catch
            {
                resultado = false;

            }
            finally
            {
                conexion.Close();
            }

            return resultado;
        }
    }
}
