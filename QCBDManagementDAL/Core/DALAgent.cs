using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementDAL.App_Data;
using QCBDManagementDAL.App_Data.QCBDDataSetTableAdapters;
using QCBDManagementDAL.Helper.ChannelHelper;
using QCBDManagementGateway.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QCBDManagementDAL.Core
{
    public class DALAgent : IAgentManager
    {
        private Func<double, double> _rogressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private GateWayAgent _gateWayAgent;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        public DALAgent()
        {
            _gateWayAgent = new GateWayAgent();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
            _gateWayAgent.PropertyChanged += onCredentialChange_loadAgentDataFromWebService;

        }


        public GateWayAgent GateWayAgent
        {
            get { return _gateWayAgent; }
            set { _gateWayAgent = value; }
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }

        private void onCredentialChange_loadAgentDataFromWebService(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Credential"))
            {
                DALHelper.doActionAsync(retrieveGateWayData);
                _gateWayAgent.PropertyChanged -= onCredentialChange_loadAgentDataFromWebService;
            }
        }
        
        

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                //_loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
                _gateWayAgent.initializeCredential(user);
            }                       
        }

        public void retrieveGateWayData() 
        {
            try
            { 
                lock (_lock) _isLodingDataFromWebServiceToLocal = true;
                var agentList = new NotifyTaskCompletion<List<Agent>>(_gateWayAgent.GetAgentData(_loadSize)).Task.Result;
                if (agentList.Count > 0)
                {
                    var savedAgentList = new NotifyTaskCompletion<List<Agent>>(UpdateAgent(agentList));
                }                
            }
            finally
            {
                lock (_lock)
                {
                    _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
                    _isLodingDataFromWebServiceToLocal = false;
                    Log.write("Agent loaded!","TES");
                }
            }          
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _rogressBarFunc = progressBarFunc;
        }

        public async Task<List<Agent>> DeleteAgent(List<Agent> listAgent)
        {
            List<Agent> result = listAgent;
            List<Agent> gateWayResultList = new List<Agent>();
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
            using (GateWayAgent gateWayAgent = new GateWayAgent())
            {
                gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayAgent.DeleteAgent(listAgent);
                if (gateWayResultList.Count == 0)
                    foreach (Agent agent in gateWayResultList)
                    {
                        int returnResult = _agentsTableAdapter.delete_data_agent(agent.ID);
                        if (returnResult > 0)
                            result.Remove(agent);
                    }
            }
            return result;
        }

        public async Task<List<Agent>> GetAgentData(int nbLine)
        {
            using (GateWayAgent gateWayAgent = new GateWayAgent())
            {
                gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWayAgent.GetAgentData(nbLine);
            }
            /*List<Agent> result = new List<Agent>();
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.agentsDataTable>(_agentsTableAdapter.get_data_agent)).DataTableTypeToAgent();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0,nbLine-1);*/
        }

        public async Task<List<Agent>> GetAgentDataById(int id)
        {
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.agentsDataTable>(_agentsTableAdapter.get_data_agent_by_id, id)).DataTableTypeToAgent();
        }

        public async Task<List<Agent>> GetAgentDataByCommandList(List<Command> commandList)
        {
            List<Agent> result = new List<Agent>();
            using (GateWayAgent gateAwayAgent = new GateWayAgent())
            {
                gateAwayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                result = await gateAwayAgent.GetAgentDataByCommandList(commandList);
            }
            return result;
        }


        public async Task<List<Agent>> InsertAgent(List<Agent> listAgent)
        {
           List<Agent> result = new List<Agent>();
            List<Agent> gateWayResultList = new List<Agent>();
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
            using (GateWayAgent gateWayAgent = new GateWayAgent())
            {
                gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayAgent.InsertAgent(listAgent);

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateAgent(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Agent>> UpdateAgent(List<Agent> listAgent)
        {
            List<Agent> result = new List<Agent>();
            List<Agent> gateWayResultList = new List<Agent>();
            using (agentsTableAdapter _agentsTableAdapter = new agentsTableAdapter())
            using (GateWayAgent gateWayAgent = new GateWayAgent())
            {
                gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayAgent.UpdateAgent(listAgent) : listAgent;
                foreach (var agent in gateWayResultList)
                {
                    int returnResult = _agentsTableAdapter
                                            .update_data_agent(
                                                agent.LastName,
                                                agent.FirstName,
                                                agent.Phone,
                                                agent.Fax,
                                                agent.Email,
                                                agent.Login,
                                                agent.HashedPassword,
                                                agent.Admin,
                                                agent.Status,
                                                agent.ListSize,
                                                agent.ID);
                    if (returnResult > 0)
                        result.Add(agent);
                }
            }
            return result;
        }

        public async Task<List<Agent>> searchAgent(Agent agent, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return agent.AgentTypeToFilterDataTable(filterOperator); });
        }

        public void Dispose()
        {

        }

        public async Task<List<Agent>> searchAgentFromWebService(Agent agent, string filterOperator)
        {
            List<Agent> gateWayResultList = new List<Agent>();
            using (GateWayAgent gateWayAgent = new GateWayAgent())
            {
                gateWayAgent.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayAgent.searchAgentFromWebService(agent, filterOperator);

            }
            return gateWayResultList;
        }

        public Task<List<Client>> MoveAgentClient(Agent fromAgent, Agent toAgent)
        {
            throw new NotImplementedException();
        }
    } /* end class BLAgent */
}