using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QCBDManagementDAL.Classes
{
    public class InputMessageInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            MemoryStream memStream = new MemoryStream();
            XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateBinaryWriter(memStream);
            xdw.WriteStartElement("test", "http://localhost/WebServiceSOAP/server.php?wsdl");

            /*xdw.WriteStartElement(“GetDataResult”, “http://tempuri.org/”);

            xdw.WriteAttributeString(“Units”, “ounces”);

            xdw.WriteString(“10.5”);

            xdw.WriteEndElement();*/

            xdw.WriteEndElement();

            xdw.Flush();

            memStream.Position = 0;
            XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
            XmlDictionaryReader xdr = XmlDictionaryReader.CreateBinaryReader(memStream, quotas);


            Message replacedMessage = Message.CreateMessage(reply.Version, null, xdr);

            replacedMessage.Headers.CopyHeadersFrom(reply.Headers);

            replacedMessage.Properties.CopyProperties(reply.Properties);

            //reply = replacedMessage;
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            //ValidateMessageBody(ref request, true);
            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            Message msgCopy = buffer.CreateMessage();

            string strMessage = buffer.CreateMessage().ToString();
            System.Xml.XmlDictionaryReader xrdr = msgCopy.GetReaderAtBodyContents();
            string bodyData = xrdr.ReadOuterXml();

            strMessage = strMessage.Replace("... stream ...", bodyData);

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            return null;
        }
    }

    public class MyBehavior : IEndpointBehavior

    {

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)

        {

            //no-op

        }


        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            InputMessageInspector inspector = new InputMessageInspector();
            clientRuntime.MessageInspectors.Add(inspector);
        }


        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            //no-op
        }


        public void Validate(ServiceEndpoint endpoint)
        {
            //no-op
        }

    }
}
