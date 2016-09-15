using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Enum
{
    public enum ESecurity
    {
        Salt=59,
        _Write=0,
        _Read,
        _Update,
        _Delete,
        SendEmail
    }
}
