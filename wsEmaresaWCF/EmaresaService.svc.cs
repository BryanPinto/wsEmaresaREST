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

        public XmlNode GetJSONtoXML(object Json)
        {
            try
            {
                var jsonRequest = Json.ToString();
                var xmlNode = JsonConvert.DeserializeXmlNode(jsonRequest);
                //Escribir log
                string rutaLog = HttpRuntime.AppDomainAppPath;
                StringBuilder sb = new StringBuilder();

                sb.Append(Environment.NewLine +
                          DateTime.Now.ToShortDateString() + " " +
                          DateTime.Now.ToShortTimeString() + ": " +
                          "[ConvertirJSONaXML] -- JSON: " + jsonRequest + " | " + "XML: " + xmlNode.OuterXml);
                System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
                sb.Clear();
                return xmlNode;
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
                return GetJSONtoXML(salida);
            }
        }

        public Respuesta GetOK(Dummy JsonString)
        {
            try
            {
                string respuesta = "OK";
                //variable para lanzar número al azar entre 0 y 10
                var NumAzar = new Random().Next(0, 10);

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
                              "[RetornaOK] -- Entrada: " + JsonString.mensaje + " | " + "Salida: " + salida);
                    System.IO.File.AppendAllText(rutaLog + "Log.txt", sb.ToString());
                    sb.Clear();

                    ////declarar xml de creación
                    //string xmlCreacion = @"<?xml version=""1.0""?>
                    //                        <BizAgiWSParam>
                    //                            <domain>domain</domain>
                    //                            <userName>admon</userName>
                    //                            <Cases>
                    //                                <Case>
                    //                                    <Process>CopyProcesoDeCompras</Process>
                    //                                    <Entities>
                    //                                        <ProcesodeCompras>
                    //                                            <NroSolicitudERP>ASD</NroSolicitudERP>
                    //                                            <FechaCotizacion>2018-12-11</FechaCotizacion>
                    //                                            <FechaSolicitud>2018-12-11</FechaSolicitud>
                    //                                            <Solicitante>ASD</Solicitante>
                    //                                            <Condp_Pago>ASD</Condp_Pago>
                    //                                            <Tipo_compra>ASD</Tipo_compra>
                    //                                            <ObservacionSolicitud>ASD</ObservacionSolicitud>
                    //                                            <Itemgeneral>1</Itemgeneral>
                    //                                            <ItemSolicitud>1</ItemSolicitud>
                    //                                            <TotalCotizado>1</TotalCotizado>
                    //                                            <DetalleCotizacion>
                    //                                                <CodProducto>ASD</CodProducto>
                    //                                                <Tipo_Concepto>ASD</Tipo_Concepto>
                    //                                                <DescripcionAmpliada>ASD</DescripcionAmpliada>
                    //                                                <NombreProveedor>ASD</NombreProveedor>
                    //                                                <Cantidad>1</Cantidad>
                    //                                                <UnidadMedida>1</UnidadMedida>
                    //                                                <PrecioUnit>1</PrecioUnit>
                    //                                                <Neto>1</Neto>
                    //                                                <Observacion>ASD</Observacion>
                    //                                                <MotivoRechazo>ASD</MotivoRechazo>
                    //                                            </DetalleCotizacion>
                    //                                        </ProcesodeCompras>
                    //                                    </Entities>
                    //                                </Case>
                    //                            </Cases>
                    //                        </BizAgiWSParam>";
                    ////crear instancia
                    //BizagiCapaSOA.WorkflowEngineSOASoapClient serviceEngine = new BizagiCapaSOA.WorkflowEngineSOASoapClient();

                    //string respuestaBizagi = serviceEngine.createCasesAsString(xmlCreacion);
                    //retornar salida
                    //string dummyEnJson = new JavaScriptSerializer().Serialize(JsonString);
                    //return JsonConvert.DeserializeXmlNode(dummyEnJson);
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
    }
}
