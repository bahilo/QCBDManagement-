using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface ICommand_itemManager
    {
        Task<List<Command_item>> InsertCommand_item(List<Command_item> listCommand_item);

        Task<List<Command_item>> UpdateCommand_item(List<Command_item> listCommand_item);

        Task<List<Command_item>> DeleteCommand_item(List<Command_item> listCommand_item);

        Task<List<Command_item>> GetCommand_itemByCommandList(List<Command> listCommand);

        Task<List<Command_item>> GetCommand_itemData(int nbLine);

        Task<List<Command_item>> searchCommand_item(Command_item Command_item, string filterOperator);

        Task<List<Command_item>> searchCommand_itemFromWebService(Command_item Command_item, string filterOperator);

        Task<List<Command_item>> GetCommand_itemDataById(int id);
    }
}
