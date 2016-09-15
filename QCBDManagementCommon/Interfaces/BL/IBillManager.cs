using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IBillManager
    {
        Task<List<Bill>> InsertBill(List<Bill> listBill);

        Task<List<Bill>> UpdateBill(List<Bill> listBill);

        Task<List<Bill>> DeleteBill(List<Bill> listBill);

        Task<List<Bill>> GetBillData(int nbLine);

        Task<List<Bill>> GetBillDataByCommandList(List<Command> commandList);

        Task<List<Bill>> searchBill(Bill Bill, string filterOperator);

        Task<List<Bill>> searchBillFromWebService(Bill Bill, string filterOperator);

        Task<List<Bill>> GetBillDataById(int id);

        Task<Bill> GetLastBill();
    }
}
