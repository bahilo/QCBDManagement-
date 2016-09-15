using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementBusiness;

namespace QCBDManagementWPF.ViewModel
{
    public class StatisticViewModel
    {
        private BusinessLogic _bl;

        internal void setLogicAccess(BusinessLogic bl)
        {
            _bl = bl;
        }
    }
}
