using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace wsEmaresaWCF
{
    //Estructura para retornar mensajes dentro de los métodos
    public class Dummy
    {
        public string mensaje { get; set; }
    }

    public class Respuesta
    {
        public string mensaje { get; set; }
    }


    //ESTRUCTURA JSON EMARESA
    public class DocHeader
    {
        public string fechaCotizacion { get; set; }
        public int centroCosto { get; set; }
        public string condicionPago { get; set; }
        public string numeroSolicitud { get; set; }
        public string observaciones { get; set; }
        public string solicitante { get; set; }
        public long totalItems { get; set; }
    }

    public class DocLine
    {
        public long cantidad { get; set; }
        public string codigoMercaderia { get; set; }
        public string descripcion { get; set; }
        public string observaciones { get; set; }
        public long precioUnitario { get; set; }
        public string proveedor { get; set; }
        public string unidadMedida { get; set; }
    }

    public class Raiz
    {
        public string EMPRESA { get; set; }
        public string FEEMDO { get; set; }
        public long IDMAEEDO { get; set; }
        public string LIBRO { get; set; }
        public string NUDO { get; set; }
        public string TIDO { get; set; }
        public DocHeader docHeader { get; set; }
        public List<DocLine> docLines { get; set; }
        public string docNumber { get; set; }
        public string docType { get; set; }
    }
    public class EstadoCotizacion 
    {
        public int codigoEstado { get; set; }
        public string detalleRespuesta { get; set; }
    }
    public class RootObject
    {
        public Raiz Raiz { get; set; }
    }
    public class Retorno
    {
        public RootObject root { get; set; }
        public string MensajeSalida { get; set; }
    }
    //FIN ESTRUCTURA

    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IEmaresaService
    {
        
        [WebGet(UriTemplate = "/GetData")]
        [OperationContract]
        string GetData();
        
        [WebInvoke(UriTemplate = "/GetOK", 
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        Retorno GetOK(RootObject JsonString);

        
        [WebInvoke(UriTemplate = "/GetJSONtoXML", 
            Method = "POST",
            ResponseFormat = WebMessageFormat.Xml,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        RootObject GetJSONtoXML(RootObject Json);

        //[WebInvoke(UriTemplate = "/GetXMLtoJSON",
        //   Method = "POST",
        //   RequestFormat = WebMessageFormat.Xml,
        //   ResponseFormat = WebMessageFormat.Json)]
        //[OperationContract]
        //string InyeccionRandom(string Xml);

        [WebInvoke(UriTemplate = "/GetStatus", 
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        EstadoCotizacion GetStatus(int response);

        // TODO: agregue aquí sus operaciones de servicio
    }


    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class ServiciosWeb
    {
        [DataMember]
        public int value;
        [DataMember]
        public string JsonString { set; get; }
        [DataMember]
        public string Xml { set; get; }
        [DataMember]
        public object Json { set; get; }
        [DataMember]
        public string Test { set; get; }
        [DataMember]
        public int response { set; get; }
    }
}
