using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IPrivilegeManager
    {
        Task<List<Privilege>> InsertPrivilege(List<Privilege> listPrivilege);

        Task<List<Privilege>> UpdatePrivilege(List<Privilege> listPrivilege);

        Task<List<Privilege>> DeletePrivilege(List<Privilege> listPrivilege);

        Task<List<Privilege>> GetPrivilegeData(int nbLine);

        Task<List<Privilege>> searchPrivilege(Privilege Privilege, string filterOperator);

        Task<List<Privilege>> GetPrivilegeDataById(int id);
    }
}
