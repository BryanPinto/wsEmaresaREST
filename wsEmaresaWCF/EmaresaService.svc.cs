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
                //retornar mensaje de error
                Respuesta error = new Respuesta();
                error.mensaje = ex.Message;
                return Json;
            }
        }

        public Retorno GetOK(RootObject JsonString)
        {
            try
            {
                    string respuesta = "Inicio";

                    //Escribir log
                    string rutaLog = HttpRuntime.AppDomainAppPath; StringBuilder sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[--------------------Inicio Método--------------------] "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                    //fin log
                
                    Retorno respuestaSalida = new Retorno();
                    respuestaSalida.MensajeSalida = respuesta;
                    //respuestaSalida.root = JsonString;
                    //Serializar objeto JSON
                    var jsonRequest = JsonConvert.SerializeObject(JsonString);
                    //Deserializar el archivo JSON para
                    var xmlNode = JsonConvert.DeserializeXmlNode(jsonRequest);
                    //Replicar formato JSON para la respuesta del método
                    string salida = "{\"Respuesta\":\"" + respuesta + "\"}";
                    //Escribir log
                     rutaLog = HttpRuntime.AppDomainAppPath;
                     sb = new StringBuilder();
                    sb.Append(Environment.NewLine +
                              DateTime.Now.ToShortDateString() + " " +
                              DateTime.Now.ToShortTimeString() + ": " +
                              "[RetornaOK] -- Entrada: " + jsonRequest + " | " + "Salida: " + salida);
                    System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
                    sb.Clear();
                    //fin log
                    //Escribir log
                     rutaLog = HttpRuntime.AppDomainAppPath;  sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[Punto método] -Obtener valores de cada tag del XML transformado (aún en formato XML, NO STRING)- "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                    //fin log
                    //Obtener valores de cada tag del XML transformado (aún en formato XML, NO STRING), para insertar valores en XML de creación de caso
                    string nroSolicitud = xmlNode.GetElementsByTagName("numeroSolicitud")[0].InnerText;
                    string fechaCotizacion = xmlNode.GetElementsByTagName("fechaCotizacion")[0].InnerText;
                    int centroCosto = Convert.ToInt32(xmlNode.GetElementsByTagName("centroCosto")[0].InnerText);
                    string fechaSoliFeemdo = xmlNode.GetElementsByTagName("FEEMDO")[0].InnerText;
                    string solicitante = xmlNode.GetElementsByTagName("solicitante")[0].InnerText;
                    string condPago = xmlNode.GetElementsByTagName("condicionPago")[0].InnerText;
                    string observaciones = xmlNode.GetElementsByTagName("observaciones")[0].InnerText;
                    string TIDO = xmlNode.GetElementsByTagName("TIDO")[0].InnerText;
                    string NUDO = xmlNode.GetElementsByTagName("NUDO")[0].InnerText;
                    string EMPRESA = xmlNode.GetElementsByTagName("EMPRESA")[0].InnerText;
                    int totalItems = Convert.ToInt32(xmlNode.GetElementsByTagName("totalItems")[0].InnerText);
                
                //rescatar id centro de costos para buscar valor del atributo "Codigo"
                //crear instancia
                BizagiSOAEntity.EntityManagerSOASoapClient servicio = new BizagiSOAEntity.EntityManagerSOASoapClient();
                //armar xml para obtener entidad
                string xmlCentro = @"<BizAgiWSParam>
                     <EntityData>
                        <EntityName>CentrodeCostos</EntityName>
                            <Filters>
                                <![CDATA[Codigo = '"+centroCosto+@"']]>
                            </Filters>
                     </EntityData>
                    </BizAgiWSParam>";
                
                string CodigoBizagi = servicio.getEntitiesAsString(xmlCentro);
                
                //var xmlCodigo = JsonConvert.DeserializeXmlNode(CodigoBizagi);
                XmlDocument doc = new XmlDocument();
                //Asignar al documento el XML enviado
                doc.LoadXml(CodigoBizagi);

                int idCodigoCentro = Convert.ToInt32(doc.GetElementsByTagName("CentrodeCostos")[0].Attributes["key"].Value);


                //Escribir log
                rutaLog = HttpRuntime.AppDomainAppPath; sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[Punto método] -xmlCentro ==> " + xmlCentro + " - "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                //fin log
                //Eliminar espacios en blanco de los valores
                fechaCotizacion = fechaCotizacion.Trim();
                    //centroCosto = centroCosto.Trim();
                    fechaSoliFeemdo = fechaSoliFeemdo.Trim();
                    solicitante = solicitante.Trim();
                    condPago = condPago.Trim();
                    observaciones = observaciones.Trim();
                    TIDO = TIDO.Trim();
                    NUDO = NUDO.Trim();
                    EMPRESA = EMPRESA.Trim();
                    
                    //Escribir log
                     rutaLog = HttpRuntime.AppDomainAppPath;  sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[Punto método] -Elementos de la coleccion- "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                    //fin log
                    //Elementos de la coleccion
                    string xmlProductos = String.Empty;
                    string productos = String.Empty;
                    for (var i = 0; i < xmlNode.GetElementsByTagName("docLines").Count; i++)
                    {
                        string codMercaderia = xmlNode.GetElementsByTagName("codigoMercaderia")[i].InnerText;
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
                        //Escribir log
                     rutaLog = HttpRuntime.AppDomainAppPath;  sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[Punto método] -Elemento "+i+"- "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                    //fin log
                    }

                    //Crear caso con extracto de XML
                    //declarar xml de creación
                    //Escribir log
                     rutaLog = HttpRuntime.AppDomainAppPath;  sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[Punto método] -Crear caso con extracto de XML y declarar xml de creación- "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                    //fin log
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
                                                            <CentrodeCostos>" + idCodigoCentro + @"</CentrodeCostos>
                                                            <FechaSolicitud>" + fechaSoliFeemdo + @"</FechaSolicitud>
                                                            <Solicitante>" + solicitante + @"</Solicitante>
                                                            <Condp_Pago>" + condPago + @"</Condp_Pago>
                                                            <TIDO> " + TIDO + @"</TIDO>
                                                            <NUDO> " + NUDO + @"</NUDO>
                                                            <EMPRESA> "+ EMPRESA + @"</EMPRESA>
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
                    
                    //Escribir log
                     rutaLog = HttpRuntime.AppDomainAppPath;  sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + @"[Punto método] -Reemplazar \r ,\n y \t del xml de creación- "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                    //fin log
                    //Reemplazar "\r" ,"\n" y "\t" del xml de creación
                    xmlCreacion = xmlCreacion.Replace("\n", "");
                    xmlCreacion = xmlCreacion.Replace("\t", "");
                    xmlCreacion = xmlCreacion.Replace("\r", "");
                    
                    //Escribir log
                     rutaLog = HttpRuntime.AppDomainAppPath;  sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[Punto método] -Crear instancia Bizagi SOA- "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                    //fin log
                    //crear instancia
                    BizagiCapaSOA.WorkflowEngineSOASoapClient serviceEngine = new BizagiCapaSOA.WorkflowEngineSOASoapClient();
                    


                    string respuestaBizagi = serviceEngine.createCasesAsString(xmlCreacion);

                    
                //
                //return (error);
                respuestaSalida.MensajeSalida = respuestaBizagi;


                //Escribir log
                rutaLog = HttpRuntime.AppDomainAppPath; sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[Punto método] -Respuesta Bizagi ==> " + respuestaSalida.MensajeSalida + " - "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                //fin log

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(respuestaBizagi);
                respuestaSalida.MensajeSalida = xml.GetElementsByTagName("processId")[0].InnerText;

                //Escribir log
                rutaLog = HttpRuntime.AppDomainAppPath; sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[Punto método] -Numero Caso ==> " + respuestaSalida.MensajeSalida + " - "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                //fin log
                //Escribir log
                rutaLog = HttpRuntime.AppDomainAppPath; sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[--------------------Fin método creación--------------------] "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                //fin log
                return (respuestaSalida);
                
            }
            catch (Exception ex)
            {
                //Escribir log
                string rutaLog = HttpRuntime.AppDomainAppPath; StringBuilder sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[--------------------Inicio excepción creación--------------------] "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                //fin log

                //Creación objeto retorno
                Retorno respuestaSalida = new Retorno();
                //Escribir log
                rutaLog = HttpRuntime.AppDomainAppPath; sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[Excepción] -Creación objeto Retorno-"); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                //fin log

                //Serializar objeto JSON
                var jsonRequest = JsonConvert.SerializeObject(JsonString);
                //Escribir log
                rutaLog = HttpRuntime.AppDomainAppPath; sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[Excepción] -Serializar objeto Json-"); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                //fin log


                respuestaSalida.MensajeSalida = ex.Message;
                //Replicar formato JSON para la respuesta error del método
                string salida = "{\"Error\":\"" + ex.Message + "\"}";
                //Escribir log
                 rutaLog = HttpRuntime.AppDomainAppPath;
                 sb = new StringBuilder();

                sb.Append(Environment.NewLine +
                          DateTime.Now.ToShortDateString() + " " +
                          DateTime.Now.ToShortTimeString() + ": " +
                          "[Excepción] -- Entrada: " + jsonRequest + " | " + "Salida: " + salida);
                System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
                sb.Clear();
                //retornar salida
                Respuesta error = new Respuesta();
                error.mensaje = ex.Message;
                var json = new JavaScriptSerializer().Serialize(error);

                //Escribir log
                rutaLog = HttpRuntime.AppDomainAppPath; sb = new StringBuilder(); sb.Append(Environment.NewLine + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + "[--------------------Fin excepción creación--------------------] "); System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString()); sb.Clear();
                //fin log

                return (respuestaSalida);
            }
        }

        public EstadoCotizacion GetStatus(int response)
        {
            
            EstadoCotizacion json = new EstadoCotizacion();
            if (response == 2)
            {
                //Approve
                json.codigoEstado = response;
                json.detalleRespuesta = "Aprobado";


            }
            else if (response == 1)
            {
                //Reject
                json.codigoEstado = response;
                json.detalleRespuesta = "Rechazado";
            }
            else
            {
                //Undefined
                json.codigoEstado = 0;
                json.detalleRespuesta = "Estado Indefinido";
            }
            return (json);
        }
        //public string InyeccionRandom(string Xml)
        //{
        //    try
        //    {
        //        //Declarar variable vacía para convertir el xml a json
        //        string json = string.Empty;
        //        //Generar un nuevo documento XML
        //        XmlDocument doc = new XmlDocument();
        //        //Asignar al documento el XML enviado
        //        doc.LoadXml(Xml);
        //        //Utilizar variable json para realizar conversión
        //        json = JsonConvert.SerializeXmlNode(doc);
        //        //Escribir log
        //        string rutaLog = HttpRuntime.AppDomainAppPath;
        //        StringBuilder sb = new StringBuilder();

        //        sb.Append(Environment.NewLine +
        //                  DateTime.Now.ToShortDateString() + " " +
        //                  DateTime.Now.ToShortTimeString() + ": " +
        //                  "[InyeccionRandom] -- Xml: " + Xml + " | " + "JSON: " + json);
        //        System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
        //        sb.Clear();
        //        return json;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Replicar formato JSON para la respuesta error del método
        //        string salida = "{\"Error\":\"" + ex.Message + "\"}";
        //        //Escribir log
        //        string rutaLog = HttpRuntime.AppDomainAppPath;
        //        StringBuilder sb = new StringBuilder();

        //        sb.Append(Environment.NewLine +
        //                  DateTime.Now.ToShortDateString() + " " +
        //                  DateTime.Now.ToShortTimeString() + ": " +
        //                  "[ConvertirJSONaXML] -- Salida: " + salida);
        //        System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
        //        sb.Clear();

        //        return salida;
        //    }
        //}

        //public string GetXMLtoJSON(string Xml)
        //{
        //    try
        //    {
        //        //Declarar variable vacía para convertir el xml a json 
        //        string json = string.Empty;
        //        //Generar un nuevo documento XML
        //        XmlDocument doc = new XmlDocument();
        //        //Asignar al documento el XML enviado
        //        doc.LoadXml(Xml);
        //        //Utilizar variable json para realizar conversión
        //        json = JsonConvert.SerializeXmlNode(doc);
        //        //Escribir log
        //        string rutaLog = HttpRuntime.AppDomainAppPath;
        //        StringBuilder sb = new StringBuilder();

        //        sb.Append(Environment.NewLine +
        //                  DateTime.Now.ToShortDateString() + " " +
        //                  DateTime.Now.ToShortTimeString() + ": " +
        //                  "[ConvertirXMLaJSON] -- XML: " + Xml + " | " + "Salida JSON: " + json);
        //        System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
        //        sb.Clear();
        //        //retornar salida

        //        return (json);
        //    }
        //    catch (Exception ex)
        //    {
        //        //Replicar formato JSON para la respuesta error del método
        //        string salida = "{\"Error\":\"" + ex.Message + "\"}";
        //        //Escribir log
        //        string rutaLog = HttpRuntime.AppDomainAppPath;
        //        StringBuilder sb = new StringBuilder();

        //        sb.Append(Environment.NewLine +
        //                  DateTime.Now.ToShortDateString() + " " +
        //                  DateTime.Now.ToShortTimeString() + ": " +
        //                  "[ConvertirXMLaJSON] -- Salida: " + salida);
        //        System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
        //        sb.Clear();
        //        //retornar salida
        //        return salida;
        //    }
        //}


    }
}
