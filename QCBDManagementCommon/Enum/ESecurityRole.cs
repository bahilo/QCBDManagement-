using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Enum
{
    public enum ESecurityRole
    {
        Anonymous,          // All user by default
        Admin,              // Granted of all rights
        Initiator,          // Create quotes, create and update Items
        Buyer,              // Validate pre command to command
        Validator,          // validate Quotes to pre commande, command to billed, update Command
        Finalizer,          // billed to close
        Submitter,          // change pre command to valid, back to Quote
        Operator,           // Have all necessary rights to deploy the app
        Monitor,            // access the monitoring of the activity

    }
}
