using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace wsEmaresaWCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IEmaresaService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/GetData")]
        string GetData();

        //[OperationContract]
        //ServiciosWeb GetDataUsingDataContract(ServiciosWeb composite);
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetOK", 
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        
        string GetOK(string jsonString);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetJSONtoXML", 
            Method = "POST",
            //ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]  
        string GetJSONtoXML(string json);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetXMLtoJSON", 
            Method = "POST",
            ResponseFormat = WebMessageFormat.Json)]
        
        string GetXMLtoJSON(string xml);

        // TODO: agregue aquí sus operaciones de servicio
    }


    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class ServiciosWeb
    {
        [DataMember]
        public int value;
        [DataMember]
        public string jsonString { set; get; }
        [DataMember]
        public string xml { set; get; }
        [DataMember]
        public string json { set; get; }
        [DataMember]
        public string test { set; get; }
    }
}
