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
namespace QCBDManagementCommon.Interfaces.DAC
{
    public interface ICommandManager : ITax_commandManager, ICommand_itemManager, ITaxManager, IBillManager, IDeliveryManager, IGeneratePDF, IDisposable
    {
        //Agent AuthenticatedUser { get; set; }

        void initializeCredential(Agent user);

        Task<List<Command>> InsertCommand(List<Command> listCommand);

        Task<List<Command>> UpdateCommand(List<Command> listCommand);

        void UpdateCommandDependencies(List<Command> listCommand, bool isActiveProgress = false);

        Task<List<Command>> DeleteCommand(List<Command> listCommand);

        Task<List<Command>> GetCommandData(int nbLine);

        Task<List<Command>> GetCommandDataById(int id);

        Task<List<Command>> searchCommand(Command command, string filterOperator);

        Task<List<Command>> searchCommandFromWebService(Command command, string filterOperator);

        void progressBarManagement(Func<double, double> progressBarFunc);

    } /* end interface ICommandManager */
}