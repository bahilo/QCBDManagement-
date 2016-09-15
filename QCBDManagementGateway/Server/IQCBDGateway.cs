using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace QCBDManagementGateway.Server
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IQCBDGateway" in both code and config file together.
    [ServiceContract]
    public interface IQCBDGateway
    {
        [OperationContract]
        void DoWork();
    }
}
