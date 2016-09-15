using QCBDManagementCommon.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IRoleManager
    {
        Task<List<Role>> InsertRole(List<Role> listRole);

        Task<List<Role>> UpdateRole(List<Role> listRole);

        Task<List<Role>> DeleteRole(List<Role> listRole);

        Task<List<Role>> GetRoleData(int nbLine);

        Task<List<Role>> searchRole(Role Role, string filterOperator);

        Task<List<Role>> GetRoleDataById(int id);
    }
}
