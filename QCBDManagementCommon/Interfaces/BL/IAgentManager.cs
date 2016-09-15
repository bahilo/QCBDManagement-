using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using System;
using System.Collections;
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
    public interface IAgentManager : IDisposable
    {
        // Operations

        Task<List<Agent>> InsertAgent(List<Agent> listAgent);

        Task<List<Agent>> UpdateAgent(List<Agent> listAgent);

        Task<List<Agent>> DeleteAgent(List<Agent> listAgent);

        Task<List<Agent>> GetAgentData(int nbLine);

        Task<List<Agent>> GetAgentDataById(int id);

        Task<List<Agent>> GetAgentDataByCommandList(List<Command> commandList);

        Task<List<Agent>> searchAgent(Agent agent, string filterOperator);

        Task<List<Agent>> searchAgentFromWebService(Agent agent, string filterOperator);

        Task<List<Client>> MoveAgentClient(Agent fromAgent, Agent toAgent);

    } /* end interface IAgentManager */
}