using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;


namespace CapaDatos
{
    public class CDServicio
    {
        SqlConnection conexion;

        //funcion para registrar una venta, recibe un objeto Venta y un DataTable con el detalle de la venta
        //esta se hace en la vista del cliente carrito de compras
        public bool RegistarServicio(Servicio_Dispositivo objServicio_dispositivo, DataTable Servicio, out string Mensaje)
        {
            bool respuesta = false;
            Mensaje = string.Empty;

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarServicio", conexion);
                    cmd.Parameters.AddWithValue("@NombreDispositivo", objServicio_dispositivo.NombreDispositivo);
                    cmd.Parameters.AddWithValue("@Servicio", Servicio);
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



        public List<Servicio> ListarServicio()
        {
            //declarando lista de Productos
            List<Servicio> lista = new List<Servicio>();

            try
            {
                using (conexion = new SqlConnection(Conexion.cn))
                {
                    //creando un objeto string builder
                    StringBuilder sb = new StringBuilder();
                    //abriendo consulta con salto de linea
                    sb.AppendLine("select td.NombreDispositovo[Dispositivo],ts.Descripcion[Tipo De Servicio],");
                    sb.AppendLine("Precio_Servicio[Precio Servicio],Descipcion_Servicio from Servicio");
                    sb.AppendLine("inner join Tipo_Dispositivo td on Servicio.id_tipo_dispositivo=td.id_Dispositivo");
                    sb.AppendLine("inner join Tipo_Servicio ts on Servicio.id_tipo_servcio=ts.id_TipoServcio");
                   
                    SqlCommand cmd = new SqlCommand(sb.ToString(), conexion);
                    cmd.CommandType = CommandType.Text;

                    conexion.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            lista.Add(new Servicio
                            {
                                NombreDispositivo =rdr["Dispositivo"].ToString(),
                                objTipoServicio=new TipoServicio() { Descripcion = rdr["Tipo De Servicio"].ToString() },
                                Precio = Convert.ToDecimal(rdr["Precio Servicio"], new CultureInfo("es-NI")),
                                Descripcion_Servicio = rdr["Descipcion_Servicio"].ToString()
                            });

                        }

                    }
                }
            }
            catch
            {
                lista = new List<Servicio>();// reinicia la lsta de productos
            }
            finally
            {
                conexion.Close();
            }
            return lista;
        }

    }
}
