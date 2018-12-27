using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using Newtonsoft.Json;

namespace wsEmaresaWCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class EmaresaService : IEmaresaService
    {
        public string GetData()
        {
            string value = "prueba";
            return value;
        }

        //public string GetData(int value)
        //{
        //    return string.Format("You entered: {0}", value);
        //}

        //public ServiciosWeb GetDataUsingDataContract(ServiciosWeb composite)
        //{
        //    if (composite == null)
        //    {
        //        throw new ArgumentNullException("composite");
        //    }
        //    if (composite.BoolValue)
        //    {
        //        composite.StringValue += "Suffix";
        //    }
        //    return composite;
        //}

        public RootObject GetJSONtoXML(RootObject Json)
        {
            try
            {
                //Serializar objeto JSON
                var jsonRequest = JsonConvert.SerializeObject(Json);
                //Deserializar el archivo JSON para
                var xmlNode = JsonConvert.DeserializeXmlNode(jsonRequest);
                var xmlRespuesta = xmlNode.OuterXml;
                //Escribir log
                string rutaLog = HttpRuntime.AppDomainAppPath;
                StringBuilder sb = new StringBuilder();

                sb.Append(Environment.NewLine +
                          DateTime.Now.ToShortDateString() + " " +
                          DateTime.Now.ToShortTimeString() + ": " +
                          "[ConvertirJSONaXML] -- JSON: " + jsonRequest + " | " + "XML: " + xmlRespuesta);
                System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
                sb.Clear();
                //return xmlNode;
                Respuesta error = new Respuesta();
                error.mensaje = xmlRespuesta;

                //Obtener valores de cada tag del XML transformado (aún en formato XML, NO STRING), para insertar valores en XML de creación de caso
                string nroSolicitud = xmlNode.GetElementsByTagName("numeroSolicitud")[0].InnerText;
                string fechaCotizacion = xmlNode.GetElementsByTagName("fechaCotizacion")[0].InnerText;
                string centroCosto = xmlNode.GetElementsByTagName("centroCosto")[0].InnerText;
                string fechaSoliFeemdo = xmlNode.GetElementsByTagName("FEEMDO")[0].InnerText;
                string solicitante = xmlNode.GetElementsByTagName("solicitante")[0].InnerText;
                string condPago = xmlNode.GetElementsByTagName("condicionPago")[0].InnerText;
                string observaciones = xmlNode.GetElementsByTagName("observaciones")[0].InnerText;
                int totalItems = Convert.ToInt32(xmlNode.GetElementsByTagName("totalItems")[0].InnerText);

                //Eliminar espacios en blanco de los valores
                fechaCotizacion = fechaCotizacion.Trim();
                centroCosto = centroCosto.Trim();
                fechaSoliFeemdo = fechaSoliFeemdo.Trim();
                solicitante = solicitante.Trim();
                condPago = condPago.Trim();
                observaciones = observaciones.Trim();

                //Elementos de la coleccion
                string xmlProductos = String.Empty;
                string productos = String.Empty;
                for (var i = 0; i < xmlNode.GetElementsByTagName("docLines").Count; i++)
                {
                    long codMercaderia = Convert.ToInt64(xmlNode.GetElementsByTagName("codigoMercaderia")[i].InnerText);
                    string descripcion = xmlNode.GetElementsByTagName("descripcion")[i].InnerText;
                    string proveedor = xmlNode.GetElementsByTagName("proveedor")[i].InnerText;
                    int cantidad = Convert.ToInt32(xmlNode.GetElementsByTagName("cantidad")[i].InnerText);
                    string unidadMedida = xmlNode.GetElementsByTagName("unidadMedida")[i].InnerText;
                    int precioUnitario = Convert.ToInt32(xmlNode.GetElementsByTagName("precioUnitario")[i].InnerText);
                    string observaciones2 = xmlNode.GetElementsByTagName("observaciones")[i + 1].InnerText;

                    //Eliminar espacios en blanco de los valores
                    descripcion = descripcion.Trim();
                    proveedor = proveedor.Trim();
                    unidadMedida = unidadMedida.Trim();
                    observaciones2 = observaciones2.Trim();

                    int neto = precioUnitario * cantidad;

                    xmlProductos += @" <DetalleCotizacion>
                                                                <CodProducto>" + codMercaderia + @"</CodProducto>
                                                                <Tipo_Concepto></Tipo_Concepto>
                                                                <DescripcionAmpliada>" + descripcion + @"</DescripcionAmpliada>
                                                                <NombreProveedor>" + proveedor + @"</NombreProveedor>
                                                                <Cantidad>" + cantidad + @"</Cantidad>
                                                                <UnidadMedida>" + unidadMedida + @"</UnidadMedida>
                                                                <PrecioUnit>" + precioUnitario + @"</PrecioUnit>
                                                                <Neto>" + neto + @"</Neto>
                                                                <Observacion>" + observaciones2 + @"</Observacion>
                                                                <MotivoRechazo></MotivoRechazo>
                                                            </DetalleCotizacion>";
                }

                //Crear caso con extracto de XML
                //declarar xml de creación
                string xmlCreacion = @"<?xml version=""1.0""?>
                                        <BizAgiWSParam>
                                            <domain>domain</domain>
                                            <userName>admon</userName>
                                            <Cases>
                                                <Case>
                                                    <Process>CopyProcesoDeCompras</Process>
                                                    <Entities>
                                                        <ProcesodeCompras>
                                                            <NroSolicitudERP>" + nroSolicitud + @"</NroSolicitudERP>
                                                            <FechaCotizacion>" + fechaCotizacion + @"</FechaCotizacion>
                                                            <CentroCosto>" + centroCosto + @"</CentroCosto>
                                                            <FechaSolicitud>" + fechaSoliFeemdo + @"</FechaSolicitud>
                                                            <Solicitante>" + solicitante + @"</Solicitante>
                                                            <Condp_Pago>" + condPago + @"</Condp_Pago>
                                                            <Tipo_compra></Tipo_compra>
                                                            <ObservacionSolicitud>" + observaciones + @"</ObservacionSolicitud>
                                                            <Itemgeneral></Itemgeneral>
                                                            <ItemSolicitud>" + totalItems + @"</ItemSolicitud>
                                                                " + xmlProductos + @"
                                                        </ProcesodeCompras>
                                                    </Entities>
                                                </Case>
                                            </Cases>
                                        </BizAgiWSParam>";

                //Reemplazar "\r" ,"\n" y "\t" del xml de creación
                xmlCreacion = xmlCreacion.Replace("\n", "");
                xmlCreacion = xmlCreacion.Replace("\t", "");
                xmlCreacion = xmlCreacion.Replace("\r", "");
                //string xmlCreacion = @"<?xml version=""1.0""?>
                //                                <BizAgiWSParam>
                //                                    <domain>domain</domain>
                //                                    <userName>admon</userName>
                //                                    <Cases>
                //                                        <Case>
                //                                            <Process>CopyProcesoDeCompras</Process>
                //                                            <Entities>
                //                                                <ProcesodeCompras>
                //                                                    <NroSolicitudERP>ASD</NroSolicitudERP>
                //                                                    <FechaCotizacion>2018-12-11</FechaCotizacion>
                //                                                    <FechaSolicitud>2018-12-11</FechaSolicitud>
                //                                                    <Solicitante>ASD</Solicitante>
                //                                                    <Condp_Pago>ASD</Condp_Pago>
                //                                                    <Tipo_compra>ASD</Tipo_compra>
                //                                                    <ObservacionSolicitud>ASD</ObservacionSolicitud>
                //                                                    <Itemgeneral>1</Itemgeneral>
                //                                                    <ItemSolicitud>1</ItemSolicitud>
                //                                                    <TotalCotizado>1</TotalCotizado>
                //                                                    <DetalleCotizacion>
                //                                                        <CodProducto>ASD</CodProducto>
                //                                                        <Tipo_Concepto>ASD</Tipo_Concepto>
                //                                                        <DescripcionAmpliada>ASD</DescripcionAmpliada>
                //                                                        <NombreProveedor>ASD</NombreProveedor>
                //                                                        <Cantidad>1</Cantidad>
                //                                                        <UnidadMedida>1</UnidadMedida>
                //                                                        <PrecioUnit>1</PrecioUnit>
                //                                                        <Neto>1</Neto>
                //                                                        <Observacion>ASD</Observacion>
                //                                                        <MotivoRechazo>ASD</MotivoRechazo>
                //                                                    </DetalleCotizacion>
                //                                                    <DetalleCotizacion>
                //                                                        <CodProducto>ASD</CodProducto>
                //                                                        <Tipo_Concepto>ASD</Tipo_Concepto>
                //                                                        <DescripcionAmpliada>ASD</DescripcionAmpliada>
                //                                                        <NombreProveedor>ASD</NombreProveedor>
                //                                                        <Cantidad>1</Cantidad>
                //                                                        <UnidadMedida>1</UnidadMedida>
                //                                                        <PrecioUnit>1</PrecioUnit>
                //                                                        <Neto>1</Neto>
                //                                                        <Observacion>ASD</Observacion>
                //                                                        <MotivoRechazo>ASD</MotivoRechazo>
                //                                                    </DetalleCotizacion>
                //                                                </ProcesodeCompras>
                //                                            </Entities>
                //                                        </Case>
                //                                    </Cases>
                //                                </BizAgiWSParam>";
                //crear instancia
                BizagiCapaSOA.WorkflowEngineSOASoapClient serviceEngine = new BizagiCapaSOA.WorkflowEngineSOASoapClient();

                string respuestaBizagi = serviceEngine.createCasesAsString(xmlCreacion);
                //
                return Json;
            }
            catch (Exception ex)
            {
                //Replicar formato JSON para la respuesta error del método
                string salida = "{\"Error\":\"" + ex.Message + "\"}";
                //Escribir log
                string rutaLog = HttpRuntime.AppDomainAppPath;
                StringBuilder sb = new StringBuilder();

                sb.Append(Environment.NewLine +
                          DateTime.Now.ToShortDateString() + " " +
                          DateTime.Now.ToShortTimeString() + ": " +
                          "[ConvertirJSONaXML] -- Salida: " + salida);
                System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
                sb.Clear();
                //retornar salida
                //return GetJSONtoXML(salida);
                Respuesta error = new Respuesta();
                error.mensaje = ex.Message;
                return Json;
            }
        }

        public Respuesta GetOK(Dummy JsonString)
        {
            try
            {
                string respuesta = "OK";
                //variable para lanzar número al azar entre 0 y 10
                var NumAzar = new Random().Next(0, 10);

                //Si NumAzar es 1 u 8, se ejecuta el catch
                if (NumAzar == 1 || NumAzar == 8)
                {
                    throw new Exception("Error en respuesta");
                }
                else
                {
                    //Replicar formato JSON para la respuesta del método
                    string salida = "{\"Respuesta\":\"" + respuesta + "\"}";
                    //Escribir log
                    string rutaLog = HttpRuntime.AppDomainAppPath;
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Environment.NewLine +
                              DateTime.Now.ToShortDateString() + " " +
                              DateTime.Now.ToShortTimeString() + ": " +
                              "[RetornaOK] -- Entrada: " + JsonString + " | " + "Salida: " + salida);
                    System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
                    sb.Clear();
                    //fin log

                    //generar nueva instancia de respuesta, para retornar el mensaje de salida encapsulado en "error"
                    Respuesta error = new Respuesta();
                    error.mensaje = respuesta;
                    return (error);
                }
            }
            catch (Exception ex)
            {
                //Replicar formato JSON para la respuesta error del método
                string salida = "{\"Error\":\"" + ex.Message + "\"}";
                //Escribir log
                string rutaLog = HttpRuntime.AppDomainAppPath;
                StringBuilder sb = new StringBuilder();

                sb.Append(Environment.NewLine +
                          DateTime.Now.ToShortDateString() + " " +
                          DateTime.Now.ToShortTimeString() + ": " +
                          "[RetornaOK] -- Entrada: " + JsonString + " | " + "Salida: " + salida);
                System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
                sb.Clear();
                //retornar salida
                Respuesta error = new Respuesta();
                error.mensaje = ex.Message;
                var json = new JavaScriptSerializer().Serialize(error);

                return (error);
            }
        }

        public string GetXMLtoJSON(string Xml)
        {
            try
            {
                //Declarar variable vacía para convertir el xml a json 
                string json = string.Empty;
                //Generar un nuevo documento XML
                XmlDocument doc = new XmlDocument();
                //Asignar al documento el XML enviado
                doc.LoadXml(Xml);
                //Utilizar variable json para realizar conversión
                json = JsonConvert.SerializeXmlNode(doc);
                //Escribir log
                string rutaLog = HttpRuntime.AppDomainAppPath;
                StringBuilder sb = new StringBuilder();

                sb.Append(Environment.NewLine +
                          DateTime.Now.ToShortDateString() + " " +
                          DateTime.Now.ToShortTimeString() + ": " +
                          "[ConvertirXMLaJSON] -- XML: " + Xml + " | " + "Salida JSON: " + json);
                System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
                sb.Clear();
                //retornar salida

                return (json);
            }
            catch (Exception ex)
            {
                //Replicar formato JSON para la respuesta error del método
                string salida = "{\"Error\":\"" + ex.Message + "\"}";
                //Escribir log
                string rutaLog = HttpRuntime.AppDomainAppPath;
                StringBuilder sb = new StringBuilder();

                sb.Append(Environment.NewLine +
                          DateTime.Now.ToShortDateString() + " " +
                          DateTime.Now.ToShortTimeString() + ": " +
                          "[ConvertirXMLaJSON] -- Salida: " + salida);
                System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
                sb.Clear();
                //retornar salida
                return salida;
            }
        }

        //string xmlCreacion = @"<?xml version=""1.0""?>
        //                                <BizAgiWSParam>
        //                                    <domain>domain</domain>
        //                                    <userName>admon</userName>
        //                                    <Cases>
        //                                        <Case>
        //                                            <Process>CopyProcesoDeCompras</Process>
        //                                            <Entities>
        //                                                <ProcesodeCompras>
        //                                                    <NroSolicitudERP>ASD</NroSolicitudERP>
        //                                                    <FechaCotizacion>2018-12-11</FechaCotizacion>
        //                                                    <FechaSolicitud>2018-12-11</FechaSolicitud>
        //                                                    <Solicitante>ASD</Solicitante>
        //                                                    <Condp_Pago>ASD</Condp_Pago>
        //                                                    <Tipo_compra>ASD</Tipo_compra>
        //                                                    <ObservacionSolicitud>ASD</ObservacionSolicitud>
        //                                                    <Itemgeneral>1</Itemgeneral>
        //                                                    <ItemSolicitud>1</ItemSolicitud>
        //                                                    <TotalCotizado>1</TotalCotizado>
        //                                                    <DetalleCotizacion>
        //                                                        <CodProducto>ASD</CodProducto> CODIGO MERCANCIA?
        //                                                        <Tipo_Concepto>ASD</Tipo_Concepto>
        //                                                        <DescripcionAmpliada>ASD</DescripcionAmpliada> DESCRIPCION AMPLIADA?
        //                                                        <NombreProveedor>ASD</NombreProveedor> OK
        //                                                        <Cantidad>1</Cantidad> OK
        //                                                        <UnidadMedida>1</UnidadMedida>
        //                                                        <PrecioUnit>1</PrecioUnit> OK
        //                                                        <Neto>1</Neto> OK
        //                                                        <Observacion>ASD</Observacion> OBSERVACIONES?
        //                                                        <MotivoRechazo>ASD</MotivoRechazo>
        //                                                    </DetalleCotizacion>
        //                                                </ProcesodeCompras>
        //                                            </Entities>
        //                                        </Case>
        //                                    </Cases>
        //                                </BizAgiWSParam>";
    }
}
