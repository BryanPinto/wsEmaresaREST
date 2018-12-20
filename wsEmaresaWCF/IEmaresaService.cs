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
    public class Dummy
    {
        public string mensaje { get; set; }
    }

    public class Respuesta
    {
        public string mensaje { get; set; }
    }

    //public class MProductosCargados
    //{
    //public string Id_Familia { get; set; }
    //public string Pais { get; set; }
    //}

    //public class Entities
    //{
    //    public MProductosCargados M_ProductosCargados { get; set; }
    //}

    //public class BizAgiWSParam
    //{
    //    public Entities Entities { get; set; }
    //}

    //public class Dummy
    //{
    //    public BizAgiWSParam BizAgiWSParam { get; set; }
    //}

    //ESTRUCTURA JSON EMARESA
    public class DocHeader
    {
        public DateTime fechaCotizacion { get; set; }
        public string numeroSolicitud { get; set; }
        public string solicitante { get; set; }
        public string centroCosto { get; set; }
        public string condicionPago { get; set; }
        public string observaciones { get; set; }
        public int totalItems { get; set; }
    }

    public class DocLine
    {
        public string codigoMercaderia { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public string unidadMedida { get; set; }
        public string proveedor { get; set; }
        public int precioUnitario { get; set; }
        public string observaciones { get; set; }
    }

    public class RootObject
    {
        public string docType { get; set; }
        public DocHeader docHeader { get; set; }
        public List<DocLine> docLines { get; set; }
        public string docNumber { get; set; }
        public int IDMAEEDO { get; set; }
        public string TIDO { get; set; }
        public string NUDO { get; set; }
        public string LIBRO { get; set; }
        public string EMPRESA { get; set; }
        public DateTime FEEMDO { get; set; }
    }
    //FIN ESTRUCTURA

    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IEmaresaService
    {
        
        [WebGet(UriTemplate = "/GetData")]
        [OperationContract]
        string GetData();

        //[OperationContract]
        //ServiciosWeb GetDataUsingDataContract(ServiciosWeb composite);
        
        [WebInvoke(UriTemplate = "/GetOK", 
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        Respuesta GetOK(Dummy JsonString);

        
        [WebInvoke(UriTemplate = "/GetJSONtoXML", 
            Method = "POST",
            ResponseFormat = WebMessageFormat.Xml,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        [OperationContract]
        Dummy GetJSONtoXML(Dummy Json);

        
        [WebInvoke(UriTemplate = "/GetXMLtoJSON", 
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string GetXMLtoJSON(string Xml);

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
    }
}
