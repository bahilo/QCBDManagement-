using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IActionRecordManager
    {
        Task<List<ActionRecord>> InsertActionRecord(List<ActionRecord> listActionRecord);

        Task<List<ActionRecord>> UpdateActionRecord(List<ActionRecord> listActionRecord);

        Task<List<ActionRecord>> DeleteActionRecord(List<ActionRecord> listActionRecord);

        Task<List<ActionRecord>> GetActionRecordData(int nbLine);

        Task<List<ActionRecord>> searchActionRecord(ActionRecord ActionRecord, string filterOperator);

        Task<List<ActionRecord>> GetActionRecordDataById(int id);
    }
}
