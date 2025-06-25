using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;


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
                    sb.AppendLine("select id_Servicio,td.NombreDispositovo[Dispositivo],ts.Descripcion[Tipo De Servicio],");
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
                                id_Servicio = Convert.ToInt32(rdr["id_Servicio"]),
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

        //metodo para Insertar ServicioDe GESTION en base de datos
        public int ResgistrarServicioGestion(string Detalle)
        {
            int respuesta = 0;
            string Mensaje = string.Empty;
            using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("spRegistrar_GestionServicio", oConexion);
                    cmd.Parameters.Add("Detalle", SqlDbType.Xml).Value = Detalle;
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                    Mensaje = ex.Message;
                }
            }
            return respuesta;
        }


        //obtener detalle de la gestion del servicio por id para la factura en la vista admin 
        public Factura_Servicio ObtenerFactura(int id_facturaServicio)
        {
            Factura_Servicio rptDetalle = new Factura_Servicio();

            using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerDetalle", oConexion);
                cmd.Parameters.AddWithValue("@IdFacturaServicio", id_facturaServicio);
                cmd.CommandType = CommandType.StoredProcedure;

                var NuevaCultura = CultureInfo.GetCultureInfo("es-NI");
                try
                {
                    oConexion.Open();
                    using (XmlReader dr = cmd.ExecuteXmlReader())
                    {
                        while (dr.Read())
                        {
                            XDocument doc = XDocument.Load(dr);
                            if (doc.Element("DETALLE_SERVICIO") != null)
                            {
                                rptDetalle = (from dato in doc.Elements("DETALLE_SERVICIO")
                                                   select new Factura_Servicio()
                                                   {
                                                       NumeroTransaccion = dato.Element("Codigo").Value,
                                                       NombreCliente = dato.Element("Cliente").Value,
                                                       TelefonoCliente = dato.Element("Telefono").Value,
                                                       TotalSinIva = decimal.Parse(dato.Element("TotalSinIva").Value, NuevaCultura),
                                                       Total= decimal.Parse(dato.Element("Total").Value, NuevaCultura),
                                                       Cantidad_Pagada = decimal.Parse(dato.Element("Pago").Value, NuevaCultura),
                                                       Cambio = decimal.Parse(dato.Element("Cambio").Value, NuevaCultura),
                                                       FechaTexto = dato.Element("FechaRegistro").Value
                                                       
                                                        
                                                     
                                                   }).FirstOrDefault();
                                rptDetalle.objServicio = (from servicio in doc.Element("DETALLE_SERVICIO").Element("DETALLE").Elements("SERVICIO")
                                                                      select new Detalle_Servicio()
                                                                      {
                                                                       
                                                                          NombreDispositivo = servicio.Element("NombreDispositivo").Value,
                                                                          TipoServicio = servicio.Element("TipoServicio").Value,
                                                                          Descripcion_Servicio = servicio.Element("Descripcion_Servicio").Value,
                                                                          PrecioUnidad = decimal.Parse(servicio.Element("PrecioUnidad").Value, NuevaCultura),
                                                                          Sub_Total= decimal.Parse(servicio.Element("Sub_Total").Value, NuevaCultura),
                                                                      }).ToList();
                            }
                            else
                            {
                                rptDetalle = null;
                            }
                        }

                        dr.Close();

                    }

                    return rptDetalle;
                }
                catch (Exception)
                {
                    rptDetalle = null;
                    return rptDetalle;
                }
            }
        }

    }
}
