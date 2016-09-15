using QCBDManagementBusiness.Helper;
using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary> 
namespace QCBDManagementBusiness.Core
{
    public class BLAgent : IAgentManager
    {
        // Attributes
        public QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DAC {get; set;}

        public BLAgent(QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public async Task<List<Agent>> InsertAgent(List<Agent> agentList)
        {
            if (agentList == null || agentList.Count == 0)
                return new List<Agent>();
            
            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.InsertAgent(agentList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Agent>> UpdateAgent(List<Agent> agentList)
        {
            if (agentList == null || agentList.Count == 0)
                return new List<Agent>();

            if (agentList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating agents(count = " + agentList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.UpdateAgent(agentList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Agent>> DeleteAgent(List<Agent> agentList)
        {
            if (agentList == null || agentList.Count == 0)
                return new List<Agent>();

            if (agentList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting agents(count = " + agentList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
                            
            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.DeleteAgent(agentList);
            }
            catch (Exception ex )
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Agent>> GetAgentData(int nbLine)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.GetAgentData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Agent>> GetAgentDataById(int id)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.GetAgentDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Agent>> GetAgentDataByCommandList(List<Command> commandList)
        {
            List<Agent> result = new List<Agent>();
            if (commandList.Count == 0)
                return result;
            try
            {
                result = await DAC.DALAgent.GetAgentDataByCommandList(commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }


        public async Task<List<Agent>> DisableAgent(List<Agent> agentList)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                foreach (var agent in agentList)
                {
                    agent.Status = EStatus.Deactivated.ToString();
                }
                result = await UpdateAgent(agentList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Agent>> EnableAgent(List<Agent> agentList)
        {            
            List<Agent> result = new List<Agent>();
            try
            {
                foreach (var agent in agentList)
                {
                    agent.Status = EStatus.Active.ToString();
                }
                result = await UpdateAgent(agentList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }        
        

        public async Task<List<Client>> MoveAgentClient(Agent fromAgent, Agent toAgent)
        {
            List<Client> fromAgentClientListBefore = new List<Client>(), fromAgentClientListAfter = new List<Client>();
            
            try
            {
                fromAgentClientListBefore = await DAC.DALClient.searchClient(new Client { AgentId=fromAgent.ID }, "AND");
                foreach (var client in fromAgentClientListBefore)
                {
                    client.AgentId = toAgent.ID;
                }
                if(fromAgentClientListBefore.Count > 0)
                    fromAgentClientListAfter = await DAC.DALClient.UpdateClient(fromAgentClientListBefore);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return fromAgentClientListAfter;
        }

        public void Dispose()
        {
            DAC.DALAgent.Dispose();
        }

        public async Task<List<Agent>> searchAgent(Agent agent, string filterOperator)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.searchAgent(agent, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Agent>> searchAgentFromWebService(Agent agent, string filterOperator)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = await DAC.DALAgent.searchAgentFromWebService(agent, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write("[searchAgentFromWebService] " + ex.Message, "ERR");
            }
            return result;
        }



        // Operations


    } /* end class BLAgent */
}