using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementGateway.Helper.ChannelHelper;
using QCBDManagementGateway.QCBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementGateway.Core
{
    public class GateWaySecurity : ISecurityManager
    {
        private QCBDManagementWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public Agent _authenticatedUser;

        public GateWaySecurity()
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
                _authenticatedUser = value;
                onPropertyChange("Credential");
            }
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void setServiceCredential(string login, string password)
        {
            _channel.Close();
            _channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");
            _channel.ClientCredentials.UserName.UserName = login;
            _channel.ClientCredentials.UserName.Password = password;
        }

        public async Task<Agent> AuthenticateUser(string username, string password, bool isClearPassword = true)
        {
            Agent agentFound = new Agent();
            try
            {
                setServiceCredential(username, password);
                //AgentQCBDManagement[] agentArray = new AgentQCBDManagement[1];
                AgentQCBDManagement agentArray = await _channel.get_authenticate_userAsync(username, password);
                agentFound = new AgentQCBDManagement[] { agentArray}.ArrayTypeToAgent().FirstOrDefault();
                if (agentFound == null || agentFound.ID == 0)
                    throw new ApplicationException(string.Format("Your login {0} does not match any user in our database!", username));
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException ex)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
                throw;
            }
            return agentFound;
        }

        public async Task<List<ActionRecord>> InsertActionRecord(List<ActionRecord> listActionRecord)
        {
            var formatListActionRecordToArray = listActionRecord.ActionRecordTypeToArray();
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.insert_data_actionRecordAsync(formatListActionRecordToArray)).ArrayTypeToActionRecord();
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

        public async Task<List<Role>> InsertRole(List<Role> listRole)
        {
            var formatListRoleToArray = ServiceHelper.RoleTypeToArray(listRole);
            List<Role> result = new List<Role>();
            try
            {

                result = (await _channel.insert_data_roleAsync(formatListRoleToArray)).ArrayTypeToRole();
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

        public async Task<List<QCBDManagementCommon.Entities.Action>> InsertAction(List<QCBDManagementCommon.Entities.Action> listAction)
        {
            var formatListActionToArray = ServiceHelper.ActionTypeToArray(listAction);
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = (await _channel.insert_data_actionAsync(formatListActionToArray)).ArrayTypeToAction();
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

        public async Task<List<Role_action>> InsertRole_action(List<Role_action> listRole_action)
        {
            var formatListRole_actionToArray = listRole_action.Role_actionTypeToArray();
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.insert_data_role_actionAsync(formatListRole_actionToArray)).ArrayTypeToRole_action();
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

        public async Task<List<Agent_role>> InsertAgent_role(List<Agent_role> listAgent_role)
        {
            var formatListAgent_roleToArray = listAgent_role.Agent_roleTypeToArray();
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.insert_data_agent_roleAsync(formatListAgent_roleToArray)).ArrayTypeToAgent_role();
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

        public async Task<List<Privilege>> InsertPrivilege(List<Privilege> listPrivilege)
        {
            var formatListPrivilegeToArray = ServiceHelper.PrivilegeTypeToArray(listPrivilege);
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = (await _channel.insert_data_privilegeAsync(formatListPrivilegeToArray)).ArrayTypeToPrivilege();
            }
            catch (FaultException)
            {
                Dispose();
                throw;

            }
            catch (CommunicationException ex)
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

        public async Task<List<ActionRecord>> DeleteActionRecord(List<ActionRecord> listActionRecord)
        {
            var formatListActionRecordToArray = ServiceHelper.ActionRecordTypeToArray(listActionRecord);
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.delete_data_actionRecordAsync(formatListActionRecordToArray)).ArrayTypeToActionRecord();
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

        public async Task<List<Role>> DeleteRole(List<Role> listRole)
        {
            var formatListRoleToArray = ServiceHelper.RoleTypeToArray(listRole);
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.delete_data_roleAsync(formatListRoleToArray)).ArrayTypeToRole();
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

        public async Task<List<QCBDManagementCommon.Entities.Action>> DeleteAction(List<QCBDManagementCommon.Entities.Action> listAction)
        {
            var formatListActionToArray = ServiceHelper.ActionTypeToArray(listAction);
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = (await _channel.delete_data_actionAsync(formatListActionToArray)).ArrayTypeToAction();
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

        public async Task<List<Agent_role>> DeleteAgent_role(List<Agent_role> listAgent_role)
        {
            var formatListAgent_roleToArray = ServiceHelper.Agent_roleTypeToArray(listAgent_role);
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.delete_data_agent_roleAsync(formatListAgent_roleToArray)).ArrayTypeToAgent_role();
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

        public async Task<List<Role_action>> DeleteRole_action(List<Role_action> listRole_action)
        {
            var formatListRole_actionToArray = ServiceHelper.Role_actionTypeToArray(listRole_action);
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.delete_data_role_actionAsync(formatListRole_actionToArray)).ArrayTypeToRole_action();
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

        public async Task<List<Privilege>> DeletePrivilege(List<Privilege> listPrivilege)
        {
            var formatListPrivilegeToArray = ServiceHelper.PrivilegeTypeToArray(listPrivilege);
            List<Privilege> result = new List<Privilege>();
            try
            {

                result = (await _channel.delete_data_privilegeAsync(formatListPrivilegeToArray)).ArrayTypeToPrivilege();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException ex)
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

        public async Task<List<ActionRecord>> UpdateActionRecord(List<ActionRecord> listActionRecord)
        {
            var formatListActionRecordToArray = ServiceHelper.ActionRecordTypeToArray(listActionRecord);
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.update_data_actionRecordAsync(formatListActionRecordToArray)).ArrayTypeToActionRecord();
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

        public async Task<List<Role>> UpdateRole(List<Role> listRole)
        {
            var formatListRoleToArray = ServiceHelper.RoleTypeToArray(listRole);
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.update_data_roleAsync(formatListRoleToArray)).ArrayTypeToRole();
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

        public async Task<List<QCBDManagementCommon.Entities.Action>> UpdateAction(List<QCBDManagementCommon.Entities.Action> listAction)
        {
            var formatListActionToArray = ServiceHelper.ActionTypeToArray(listAction);
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = (await _channel.update_data_actionAsync(formatListActionToArray)).ArrayTypeToAction();
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

        public async Task<List<Agent_role>> UpdateAgent_role(List<Agent_role> listAgent_role)
        {
            var formatListAgent_roleToArray = ServiceHelper.Agent_roleTypeToArray(listAgent_role);
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.update_data_agent_roleAsync(formatListAgent_roleToArray)).ArrayTypeToAgent_role();
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

        public async Task<List<Role_action>> UpdateRole_action(List<Role_action> listRole_action)
        {
            var formatListRole_actionToArray = ServiceHelper.Role_actionTypeToArray(listRole_action);
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.update_data_role_actionAsync(formatListRole_actionToArray)).ArrayTypeToRole_action();
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

        public async Task<List<Privilege>> UpdatePrivilege(List<Privilege> listPrivilege)
        {
            var formatListPrivilegeToArray = ServiceHelper.PrivilegeTypeToArray(listPrivilege);
            List<Privilege> result = new List<Privilege>();
            try
            {

                result = (await _channel.update_data_privilegeAsync(formatListPrivilegeToArray)).ArrayTypeToPrivilege();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException ex)
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

        public List<Agent> DisableAgent(List<Agent> listAgent)
        {
            throw new NotImplementedException();
        }

        public List<Agent> EnableAgent(List<Agent> listAgent)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ActionRecord>> GetActionRecordData(int nbLine)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.get_data_actionRecordAsync(nbLine.ToString())).ArrayTypeToActionRecord();
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

        public async Task<List<ActionRecord>> GetActionRecordDataById(int id)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.get_data_actionRecord_by_idAsync(id.ToString())).ArrayTypeToActionRecord();
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

        public async Task<List<Role>> GetRoleData(int nbLine)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.get_data_roleAsync(nbLine.ToString())).ArrayTypeToRole();
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

        public async Task<List<Role>> GetRoleDataById(int id)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.get_data_role_by_idAsync(id.ToString())).ArrayTypeToRole();
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

        public async Task<List<QCBDManagementCommon.Entities.Action>> GetActionData(int nbLine)
        {
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = (await _channel.get_data_actionAsync(nbLine.ToString())).ArrayTypeToAction();
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

        public async Task<List<QCBDManagementCommon.Entities.Action>> GetActionDataById(int id)
        {
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = (await _channel.get_data_action_by_idAsync(id.ToString())).ArrayTypeToAction();
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

        public async Task<List<Agent_role>> GetAgent_roleData(int nbLine)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.get_data_agent_roleAsync(nbLine.ToString())).ArrayTypeToAgent_role();
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

        public async Task<List<Agent_role>> GetAgent_roleDataById(int id)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.get_data_agent_role_by_idAsync(id.ToString())).ArrayTypeToAgent_role();
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

        public async Task<List<Role_action>> GetRole_actionData(int nbLine)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.get_data_role_actionAsync(nbLine.ToString())).ArrayTypeToRole_action();
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

        public async Task<List<Role_action>> GetRole_actionDataById(int id)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.get_data_role_action_by_idAsync(id.ToString())).ArrayTypeToRole_action();
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

        public async Task<List<Privilege>> GetPrivilegeData(int nbLine)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = (await _channel.get_data_privilegeAsync(nbLine.ToString())).ArrayTypeToPrivilege();

            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException ex)
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

        public async Task<List<Privilege>> GetPrivilegeDataById(int id)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {

                result = (await _channel.get_data_privilege_by_idAsync(id.ToString())).ArrayTypeToPrivilege();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException ex)
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

        public async Task<List<ActionRecord>> searchActionRecord(ActionRecord ActionRecord, string filterOperator)
        {
            var formatListActionRecordToArray = ServiceHelper.ActionRecordTypeToFilterArray(ActionRecord, filterOperator);
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = (await _channel.get_filter_actionRecordAsync(formatListActionRecordToArray)).ArrayTypeToActionRecord();
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

        public Agent UseAgent(Agent inAgent)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _channel.Close();
        }

        public async Task<List<Role>> searchRole(Role Role, string filterOperator)
        {
            var formatListRoleToArray = ServiceHelper.RoleTypeToFilterArray(Role, filterOperator);
            List<Role> result = new List<Role>();
            try
            {
                result = (await _channel.get_filter_roleAsync(formatListRoleToArray)).ArrayTypeToRole();
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

        public async Task<List<QCBDManagementCommon.Entities.Action>> searchAction(QCBDManagementCommon.Entities.Action Action, string filterOperator)
        {
            var formatListActionToArray = ServiceHelper.ActionTypeToFilterArray(Action, filterOperator);
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = (await _channel.get_filter_actionAsync(formatListActionToArray)).ArrayTypeToAction();
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

        public async Task<List<Agent_role>> searchAgent_role(Agent_role Agent_role, string filterOperator)
        {
            var formatListAgent_roleToArray = ServiceHelper.Agent_roleTypeToFilterArray(Agent_role, filterOperator);
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = (await _channel.get_filter_agent_roleAsync(formatListAgent_roleToArray)).ArrayTypeToAgent_role();
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

        public async Task<List<Role_action>> searchRole_action(Role_action Role_action, string filterOperator)
        {
            var formatListRole_actionToArray = ServiceHelper.Role_actionTypeToFilterArray(Role_action, filterOperator);
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = (await _channel.get_filter_role_actionAsync(formatListRole_actionToArray)).ArrayTypeToRole_action();
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

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Privilege>> searchPrivilege(Privilege Privilege, string filterOperator)
        {
            throw new NotImplementedException();
        }

        public Agent GetAuthenticatedUser()
        {
            throw new NotImplementedException();
        }

        public string CalculateHash(string clearTextPassword)
        {
            throw new NotImplementedException();
        }
    } /* end class BlSecurity */
}