using QCBDManagementGateway.QCBDServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementGateway
{
    public class Class1
    {
        public ChannelFactory<QCBDManagementWebServicePortType> factory;
        QCBDManagementWebServicePortType channel;
        //QCBDManagementGateway.QCBDServiceReference.QCBDManagementWebServicePortTypeClient client = new QCBDManagementGateway.QCBDServiceReference.QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");
        //ChannelFactory<QCBDManagementGateway.QCBDServiceReference.QCBDManagementWebServicePortType> factory =
        //    new ChannelFactory<QCBDManagementWebServicePortType>("QCBDManagementWebServicePort");

        public Class1()
        {
            factory = new ChannelFactory<QCBDManagementWebServicePortType>(
                                                                            new BasicHttpBinding(),
                                                                            new EndpointAddress("http://localhost/WebServiceSOAP/server.php"));
            channel = factory.CreateChannel();
        }
        

        
        public AgentQCBDManagement[] test (int l)
        {
            return channel.get_data_agent(l.ToString());
            //AgentQCBDManagement[] jj = null;
            //return jj;// client.get_data_agent(l.ToString());
        }
    }
}
