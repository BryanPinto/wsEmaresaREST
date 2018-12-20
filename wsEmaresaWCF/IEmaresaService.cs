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
        Respuesta GetJSONtoXML(Dummy Json);

        
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
