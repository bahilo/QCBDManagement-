using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IRole_actionManager
    {
        Task<List<Role_action>> InsertRole_action(List<Role_action> listRole_action);

        Task<List<Role_action>> UpdateRole_action(List<Role_action> listRole_action);

        Task<List<Role_action>> DeleteRole_action(List<Role_action> listRole_action);

        Task<List<Role_action>> GetRole_actionData(int nbLine);

        Task<List<Role_action>> searchRole_action(Role_action Role_action, string filterOperator);

        Task<List<Role_action>> GetRole_actionDataById(int id);
    }
}
