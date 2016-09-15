using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface ITax_commandManager
    {
        Task<List<Tax_command>> InsertTax_command(List<Tax_command> listTax_command);

        Task<List<Tax_command>> UpdateTax_command(List<Tax_command> listTax_command);

        Task<List<Tax_command>> DeleteTax_command(List<Tax_command> listTax_command);

        Task<List<Tax_command>> GetTax_commandData(int nbLine);

        Task<List<Tax_command>> GetTax_commandDataByCommandList(List<Command> commandList);

        Task<List<Tax_command>> searchTax_command(Tax_command Tax_command, string filterOperator);

        Task<List<Tax_command>> GetTax_commandDataById(int id);
    }
}
