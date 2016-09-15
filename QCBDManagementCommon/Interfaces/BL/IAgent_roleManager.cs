using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IAgent_roleManager
    {
        Task<List<Agent_role>> InsertAgent_role(List<Agent_role> listAgent_role);

        Task<List<Agent_role>> UpdateAgent_role(List<Agent_role> listAgent_role);

        Task<List<Agent_role>> DeleteAgent_role(List<Agent_role> listAgent_role);

        Task<List<Agent_role>> GetAgent_roleData(int nbLine);

        Task<List<Agent_role>> searchAgent_role(Agent_role Agent_role, string filterOperator);

        Task<List<Agent_role>> GetAgent_roleDataById(int id);
    }
}
