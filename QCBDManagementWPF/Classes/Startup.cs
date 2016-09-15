using QCBDManagementBusiness;
using QCBDManagementBusiness.Core;
using QCBDManagementCommon.Entities;
using QCBDManagementDAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.Classes
{
    public class Startup
    {
        public BusinessLogic Bl { get; set; }
        public DataAccess Dal { get; set; }

        public Startup() {
            Dal = new DataAccess(
                                new DALAgent(),
                                new DALClient(),
                                new DALItem(),
                                new DALCommand(),
                                new DALSecurity(),
                                new DALStatisitc(),
                                new DALReferential());

            BlSecurity BlSecurity = new BlSecurity(Dal);

            //Agent authenticatedUser = BlSecurity.AuthenticateUser("codsimex212", "e6299fbfe0ebe192cf9acf3975a3d087");
            //Dal.SetUserCredential(authenticatedUser);

                Bl = new BusinessLogic(
                                                new BLAgent(Dal),
                                                new BlCLient(Dal),
                                                new BLItem(Dal),
                                                new BLCommand(Dal),
                                                BlSecurity,
                                                new BlDisplay(),
                                                new BLStatisitc(Dal),
                                                new BlReferential(Dal));
        }

    }



}
