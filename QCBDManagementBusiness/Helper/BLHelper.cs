using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementBusiness.Helper
{
    public static class BLHelper
    {

        public static void check(this string caller, string verifPoint, int excludeValue, string type, Object param)
        {

            if ((int)param == excludeValue && !caller.ToLower().Equals("delete") && !caller.ToLower().Equals("search"))
                throw new ApplicationException(caller + " : " + verifPoint + " equal " + excludeValue + " is exclude");
            if ((int)param >= excludeValue && caller.ToLower().Equals("delete"))
                throw new ApplicationException( caller + " " + type + " : Error While Deleting " + type + "!");

        }

        public static void checkAfter(this string caller,  string verifPoint, int excludeValue, string type, Object param)
        {
            throw new NotImplementedException("Not yet implemented!");
        }

    }
}
