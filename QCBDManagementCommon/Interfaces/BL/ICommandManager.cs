using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.BL
{
    public interface ICommandManager : ITax_commandManager, ICommand_itemManager, ITaxManager, IBillManager, IDeliveryManager, IGeneratePDF, IDisposable
    {
        // Operations

        Task<List<Command>> InsertCommand(List<Command> commandList);

        Task<List<Command>> UpdateCommand(List<Command> commandList);

        Task<List<Command>> DeleteCommand(List<Command> commandList);

        Task<List<Command>> GetCommandData(int nbLine);

        Task<List<Command>> GetCommandDataById(int id);

        Task<List<Command>> searchCommand(Command command, string filterOperator);

        Task<List<Command>> searchCommandFromWebService(Command command, string filterOperator);

    } /* end interface ICommandManager */
}