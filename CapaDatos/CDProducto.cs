using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace CapaDatos
{
    public class CDProducto
    {
        //metodo para obtener la Lista de todos los productos
        public List<Producto> ListarProducto()
        {
            //declarando lista de Productos
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //creando un objeto string builder
                    StringBuilder sb = new StringBuilder();
                    //abriendo consulta con salto de linea
                    sb.AppendLine("select p.id_Producto,p.Nombre,p.Descripcion,");
                    sb.AppendLine("m.id_Marca,m.Descripcion[DesMarca],");
                    sb.AppendLine("c.id_Categoria,c.Descripcion[DesCatg],");
                    sb.AppendLine("p.Precio,p.Stock,p.RutaImagen,p.NombreImagen,p.Estado");
                    sb.AppendLine("from Producto p");
                    sb.AppendLine("inner join Marca m on p.idMarca = m.id_Marca");
                    sb.AppendLine("inner join Categoria c on p.idCategoria = c.id_Categoria");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lista.Add(new Producto
                            {
                                id_Producto = Convert.ToInt32(rdr["id_Producto"]),
                                Nombre = rdr["Nombre"].ToString(),
                                Descripcion = rdr["Descripcion"].ToString(),
                                objMarca = new Marca() { id_Marca = Convert.ToInt32(rdr["id_Marca"]), Descripcion = rdr["DesMarca"].ToString() },
                                objCategoria = new Categoria() { id_Categoria = Convert.ToInt32(rdr["id_Categoria"]), Descripcion = rdr["DesCatg"].ToString() },
                                Precio = Convert.ToDecimal(rdr["Precio"], new CultureInfo("es-NI")),
                                Stock = Convert.ToInt32(rdr["Stock"]),
                                RutaImagen = rdr["RutaImagen"].ToString(),
                                NombreImagen = rdr["NombreImagen"].ToString(),
                                Estado = Convert.ToBoolean(rdr["Estado"])
                            });

                        }

                    }
                }
            }
            catch
            {
                lista = new List<Producto>();// reinicia la lsta de productos
            }
            return lista;
        }

        //metodo para registrar producto
        public int RegistrarProducto(Producto objp, out string Mensaje)//mensaje de salida
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    //mando el tipo de comando sql en este caso un sp
                    SqlCommand cmd = new SqlCommand("sp_RegistarProducto", conexion);
                    //parametrizo el sp
                    cmd.Parameters.AddWithValue("@Nombre", objp.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", objp.Descripcion);
                    cmd.Parameters.AddWithValue("@idMarca", objp.objMarca.id_Marca);
                    cmd.Parameters.AddWithValue("@idCategoria", objp.objCategoria.id_Categoria);
                    cmd.Parameters.AddWithValue("@Precio", objp.Precio);
                    cmd.Parameters.AddWithValue("@Stock", objp.Stock);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;//parametro de direccion de salida
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;//diciendole que el comando es un tipo sp

                    //abriendo conexion
                    conexion.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);//pasndole al id el valor del reultado
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado; 
            
        }

        //metodo para editar Producto
        public bool EditarProducto(Producto objp, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", conexion);
                    cmd.Parameters.AddWithValue("@id_Producto", objp.id_Producto);
                    cmd.Parameters.AddWithValue("@Nombre", objp.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", objp.Descripcion);
                    cmd.Parameters.AddWithValue("@idMarca", objp.objMarca.id_Marca);
                    cmd.Parameters.AddWithValue("@idCategoria", objp.objCategoria.id_Categoria);
                    cmd.Parameters.AddWithValue("@Precio", objp.Precio);
                    cmd.Parameters.AddWithValue("@Stock", objp.Stock);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();//devolvindo el mensaje si no se ejecuta la edicion


                }

            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        //metodo para editar campos de nombre imagen y la ruta
        public bool GuardarDataImagen(Producto objp, out string Mensaje)
        {
            bool resultado = false; // guarda el resultado de la operacion
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    string consulta = " update Producto set RutaImagen=@rutaimagen, NombreImagen=@nombreimagen where id_Producto=@id_Producto";
                    SqlCommand cmd = new SqlCommand(consulta, conexion);
                    cmd.Parameters.AddWithValue("@rutaimagen", objp.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreimagen", objp.NombreImagen);
                    cmd.Parameters.AddWithValue("@id_Producto", objp.id_Producto);
                    cmd.CommandType = CommandType.Text;

                    //abriendo conexion
                    conexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar la imagen";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        //metodo para eliminar producto pasandole el id del producto a eliminar
        public bool EliminarProducto(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", conexion);
                    cmd.Parameters.AddWithValue("@id_Producto", id);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }
    }
}
