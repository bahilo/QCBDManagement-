using QCBDManagementCommon.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface ISecurityActionManager
    {
        Task<List<Action>> InsertAction(List<Action> listAction);

        Task<List<Action>> UpdateAction(List<Action> listAction);

        Task<List<Action>> DeleteAction(List<Action> listAction);

        Task<List<Action>> GetActionData(int nbLine);

        Task<List<Action>> searchAction(Action Action, string filterOperator);

        Task<List<Action>> GetActionDataById(int id);
    }
}
