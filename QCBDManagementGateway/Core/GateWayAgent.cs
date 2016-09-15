using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementGateway.Helper.ChannelHelper;
using QCBDManagementGateway.QCBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Linq;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementGateway.Core
{
    public class GateWayAgent : IAgentManager
    {
        private QCBDManagementWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;
        

        public GateWayAgent()
        {
            _channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");// (binding, endPoint);
        }

        public void initializeCredential(Agent user)
        {
            Credential = user;
        }

        public Agent Credential
        {
            set
            {
                setServiceCredential(value.Login, value.HashedPassword);
                onPropertyChange("Credential");
            }
        }


        public void setServiceCredential(string login, string password)
        {
            _channel.Close();
            _channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");
            _channel.ClientCredentials.UserName.UserName = login;
            _channel.ClientCredentials.UserName.Password = password;
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public QCBDManagementWebServicePortTypeClient AgentGatWayChannel
        {
            get
            {
                return _channel;
            }
        }

        public async Task<List<Agent>> DeleteAgent(List<Agent> listAgent)
        {
            var formatListAgentToArray = listAgent.AgentTypeToArray();
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.delete_data_agentAsync(formatListAgentToArray)).ArrayTypeToAgent();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Agent>> GetAgentData(int nbLine)
        {
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.get_data_agentAsync(nbLine.ToString())).ArrayTypeToAgent().OrderBy(x=>x.ID).ToList();
             }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Agent>> GetAgentDataById(int id)
        {
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.get_data_agent_by_idAsync(id.ToString())).ArrayTypeToAgent();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Agent>> GetAgentDataByCommandList(List<Command> commandList)
        {
            List<Agent> result = new List<Agent>();
            try
            {
                result = (await _channel.get_data_agent_by_command_listAsync(commandList.CommandTypeToArray())).ArrayTypeToAgent();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }


        public async Task<List<Agent>> InsertAgent(List<Agent> listAgent)
        {
            var formatListAgentToArray = listAgent.AgentTypeToArray();
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.insert_data_agentAsync(formatListAgentToArray)).ArrayTypeToAgent();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Agent>> UpdateAgent(List<Agent> listAgent)
        {
            var formatListAgentToArray = listAgent.AgentTypeToArray();
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.update_data_agentAsync(formatListAgentToArray)).ArrayTypeToAgent();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Agent>> searchAgent(Agent agent, string filterOperator)
        {
            var formatListAgentToArray = agent.AgentTypeToFilterArray(filterOperator);
            List<Agent> result = new List<Agent>();
            try
            {                
                result = (await _channel.get_filter_agentAsync(formatListAgentToArray)).ArrayTypeToAgent();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Agent>> searchAgentFromWebService(Agent item, string filterOperator)
        {
            return await searchAgent(item, filterOperator);
        }

        public void Dispose()
        {
            _channel.Close();
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> MoveAgentClient(Agent fromAgent, Agent toAgent)
        {
            throw new NotImplementedException();
        }
    } /* end class BLAgent */
}