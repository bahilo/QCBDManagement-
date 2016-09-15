using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementDAL.App_Data;
using QCBDManagementDAL.App_Data.QCBDDataSetTableAdapters;
using QCBDManagementDAL.Helper.ChannelHelper;
using QCBDManagementGateway.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Entity = QCBDManagementCommon.Entities;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementDAL.Core
{
    public class DALSecurity : ISecurityManager
    {
        public Agent AuthenticatedUser { get; set; }
        private GateWaySecurity _gateWaySecurity;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private Func<double, double> _progressBarFunc;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALSecurity()
        {
            _gateWaySecurity = new GateWaySecurity();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
        }

        public void initializeCredential(Agent user)
        {
            AuthenticatedUser = user;
            _loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
            _gateWaySecurity.initializeCredential(AuthenticatedUser);
               
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }
        

        public async Task<Agent> AuthenticateUser(string username, string password , bool isClearPassword = true)
        {
            return await _gateWaySecurity.AuthenticateUser(username, password);
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _progressBarFunc = progressBarFunc;
        }

        public async Task<List<ActionRecord>> InsertActionRecord(List<ActionRecord> listActionRecord)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.InsertActionRecord(listActionRecord);
            }
            /*List<ActionRecord> result = new List<ActionRecord>();
            List<ActionRecord> gateWayResultList = new List<ActionRecord>();
            using (actionRecordsTableAdapter _actionRecordsTableAdapter = new actionRecordsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.InsertActionRecord(listActionRecord) : listActionRecord;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateActionRecord(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;*/
        }

        public async Task<List<Role>> InsertRole(List<Role> listRole)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.InsertRole(listRole);
            }
            /*List<Role> result = new List<Role>();
            List<Role> gateWayResultList = new List<Role>();
            using (rolesTableAdapter _rolesTableAdapter = new rolesTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.InsertRole(listRole) : listRole;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateRole(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;*/
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> InsertAction(List<QCBDManagementCommon.Entities.Action> listAction)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.InsertAction(listAction);
            }
            /*List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            List<QCBDManagementCommon.Entities.Action> gateWayResultList = new List<QCBDManagementCommon.Entities.Action>();
            using (actionsTableAdapter _actionsTableAdapter = new actionsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.InsertAction(listAction) : listAction;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateAction(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;*/
        }

        public async Task<List<Agent_role>> InsertAgent_role(List<Agent_role> listAgent_role)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.InsertAgent_role(listAgent_role);
            }
            /*List<Agent_role> result = new List<Agent_role>();
            List<Agent_role> gateWayResultList = new List<Agent_role>();
            using (actionRecordsTableAdapter _actionRecordsTableAdapter = new actionRecordsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.InsertAgent_role(listAgent_role) : listAgent_role;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateAgent_role(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;*/
        }

        public async Task<List<Role_action>> InsertRole_action(List<Role_action> listRole_action)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.InsertRole_action(listRole_action);
            }
            /*List<Role_action> result = new List<Role_action>();
            List<Role_action> gateWayResultList = new List<Role_action>();
            using (actionRecordsTableAdapter _actionRecordsTableAdapter = new actionRecordsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.InsertRole_action(listRole_action) : listRole_action;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateRole_action(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;*/
        }

        public async Task<List<Privilege>> InsertPrivilege(List<Privilege> listPrivilege)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.InsertPrivilege(listPrivilege);
            }
        }

        public async Task<List<ActionRecord>> DeleteActionRecord(List<ActionRecord> listActionRecord)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.DeleteActionRecord(listActionRecord);
            }
            /*List<ActionRecord> result = new List<ActionRecord>();
            List<ActionRecord> gateWayResultList = new List<ActionRecord>();
            using (actionRecordsTableAdapter _actionRecordsTableAdapter = new actionRecordsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.DeleteActionRecord(listActionRecord) : listActionRecord;
                if (gateWayResultList.Count == 0)
                    foreach (ActionRecord actionRecord in listActionRecord)
                    {
                        int returnValue = _actionRecordsTableAdapter.delete_data_actionRecord(actionRecord.ID);

                        ActionRecord LastInsertedActionRecord = (await searchActionRecord(actionRecord, "AND")).OrderBy(x => x.ID).Where(x => x.ID != 0).ToList().LastOrDefault();
                        if (LastInsertedActionRecord != null)
                            result.Add(LastInsertedActionRecord);
                    }
            }
            return result;*/
        }

        public async Task<List<Role>> DeleteRole(List<Role> listRole)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.DeleteRole(listRole);
            }
            /*List<Role> result = new List<Role>();
            List<Role> gateWayResultList = new List<Role>();
            using (rolesTableAdapter _rolesTableAdapter = new rolesTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.DeleteRole(listRole) : listRole;
                if (gateWayResultList.Count == 0)
                    foreach (Role role in listRole)
                    {
                        int returnValue = _rolesTableAdapter.delete_data_role(role.ID);

                        Role LastInsertedRole = (await searchRole(role, "AND")).OrderBy(x => x.ID).Where(x => x.ID != 0).ToList().LastOrDefault();
                        if (LastInsertedRole != null)
                            result.Add(LastInsertedRole);
                    }
            }
            return result;*/
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> DeleteAction(List<QCBDManagementCommon.Entities.Action> listAction)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.DeleteAction(listAction);
            }
            /*List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            List<QCBDManagementCommon.Entities.Action> gateWayResultList = new List<QCBDManagementCommon.Entities.Action>();
            using (actionsTableAdapter _actionsTableAdapter = new actionsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.DeleteAction(listAction) : listAction;
                if (gateWayResultList.Count == 0)
                    foreach (QCBDManagementCommon.Entities.Action action in listAction)
                    {
                        int returnValue = _actionsTableAdapter.delete_data_action(action.ID);

                        QCBDManagementCommon.Entities.Action LastInsertedAction = (await searchAction(action, "AND")).OrderBy(x => x.ID).Where(x => x.ID != 0).ToList().LastOrDefault();
                        if (LastInsertedAction != null)
                            result.Add(LastInsertedAction);
                    }
            }
            return result;*/
        }

        public async Task<List<Agent_role>> DeleteAgent_role(List<Agent_role> listAgent_role)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.DeleteAgent_role(listAgent_role);
            }
                
            /*List<Agent_role> result = new List<Agent_role>();
            List<Agent_role> gateWayResultList = new List<Agent_role>();
            using (actionRecordsTableAdapter _actionRecordsTableAdapter = new actionRecordsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.DeleteAgent_role(listAgent_role) : listAgent_role;
                if (gateWayResultList.Count == 0)
                    foreach (Agent_role actionRecord in listAgent_role)
                    {
                        int returnValue = _actionRecordsTableAdapter.delete_data_actionRecord(actionRecord.ID);

                        Agent_role LastInsertedAgent_role = (await searchAgent_role(actionRecord, "AND")).OrderBy(x => x.ID).Where(x => x.ID != 0).ToList().LastOrDefault();
                        if (LastInsertedAgent_role != null)
                            result.Add(LastInsertedAgent_role);
                    }
            }
            return result;*/
        }

        public async Task<List<Role_action>> DeleteRole_action(List<Role_action> listRole_action)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.DeleteRole_action(listRole_action);
            }

            /*List<Role_action> result = new List<Role_action>();
            List<Role_action> gateWayResultList = new List<Role_action>();
            using (actionRecordsTableAdapter _actionRecordsTableAdapter = new actionRecordsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.DeleteRole_action(listRole_action) : listRole_action;
                if (gateWayResultList.Count == 0)
                    foreach (Role_action actionRecord in listRole_action)
                    {
                        int returnValue = _actionRecordsTableAdapter.delete_data_actionRecord(actionRecord.ID);

                        Role_action LastInsertedRole_action = (await searchRole_action(actionRecord, "AND")).OrderBy(x => x.ID).Where(x => x.ID != 0).ToList().LastOrDefault();
                        if (LastInsertedRole_action != null)
                            result.Add(LastInsertedRole_action);
                    }
            }
            return result;*/
        }

        public async Task<List<Privilege>> DeletePrivilege(List<Privilege> listPrivilege)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.DeletePrivilege(listPrivilege);
            }
        }

        public async Task<List<ActionRecord>> UpdateActionRecord(List<ActionRecord> listActionRecord)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.UpdateActionRecord(listActionRecord);
            }
            /*List<ActionRecord> result = new List<ActionRecord>();
            List<ActionRecord> gateWayResultList = new List<ActionRecord>();
            using (actionRecordsTableAdapter _actionRecordsTableAdapter = new actionRecordsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.UpdateActionRecord(listActionRecord) : listActionRecord;
                foreach (var actionRecord in gateWayResultList)
                {
                    var returnResult = _actionRecordsTableAdapter
                                            .Update_data_actionRecord(
                                                actionRecord.AgentId,
                                                actionRecord.Date,
                                                actionRecord.Action,
                                                actionRecord.TargetName,
                                                actionRecord.TargetId,
                                                actionRecord.ID);

                    ActionRecord LastInsertedActionRecord = (await searchActionRecord(actionRecord, "AND")).OrderBy(x => x.ID).Where(x => x.ID != 0).ToList().LastOrDefault();
                    if (LastInsertedActionRecord != null)
                        result.Add(LastInsertedActionRecord);
                }
            }
            return result;*/
        }

        public async Task<List<Role>> UpdateRole(List<Role> listRole)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.UpdateRole(listRole);
            }
            /*List<Role> result = new List<Role>();
            List<Role> gateWayResultList = new List<Role>();
            using (rolesTableAdapter _rolesTableAdapter = new rolesTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.UpdateRole(listRole) : listRole;
                foreach (var role in gateWayResultList)
                {
                    var returnResult = _rolesTableAdapter
                                            .Update_data_role(
                                                role.Name,
                                                role.ID);

                    Role LastInsertedRole = (await searchRole(role, "AND")).OrderBy(x => x.ID).Where(x => x.ID != 0).ToList().LastOrDefault();
                    if (LastInsertedRole != null)
                        result.Add(LastInsertedRole);
                }
            }
            return result;*/
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> UpdateAction(List<QCBDManagementCommon.Entities.Action> listAction)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.UpdateAction(listAction);
            }
            /*List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            List<QCBDManagementCommon.Entities.Action> gateWayResultList = new List<QCBDManagementCommon.Entities.Action>();
            using (actionsTableAdapter _actionsTableAdapter = new actionsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.UpdateAction(listAction) : listAction;
                foreach (var action in gateWayResultList)
                {
                    var returnResult = _actionsTableAdapter
                                            .Update_data_action(
                                                action.Name,
                                                action.ID);

                    QCBDManagementCommon.Entities.Action LastInsertedAction = (await searchAction(action, "AND")).OrderBy(x => x.ID).Where(x => x.ID != 0).ToList().LastOrDefault();
                    if (LastInsertedAction != null)
                        result.Add(LastInsertedAction);
                }
            }
            return result;*/
        }

        public async Task<List<Agent_role>> UpdateAgent_role(List<Agent_role> listAgent_role)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.UpdateAgent_role(listAgent_role);
            }
            /*List<Agent_role> result = new List<Agent_role>();
            List<Agent_role> gateWayResultList = new List<Agent_role>();
            using (agent_rolesTableAdapter _agent_rolesTableAdapter = new agent_rolesTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.UpdateAgent_role(listAgent_role) : listAgent_role;
                foreach (var agent_role in gateWayResultList)
                {
                    var returnResult = _agent_rolesTableAdapter
                                            .Update_data_agent_role(
                                                agent_role.RoleId,
                                                agent_role.AgentId,
                                                agent_role.IsOnAllUsers,
                                                agent_role.Date,
                                                agent_role.ID);

                    Agent_role LastInsertedAgent_role = (await searchAgent_role(agent_role, "AND")).OrderBy(x => x.ID).Where(x => x.ID != 0).ToList().LastOrDefault();
                    if (LastInsertedAgent_role != null)
                        result.Add(LastInsertedAgent_role);
                }
            }
            return result;*/
        }

        public async Task<List<Role_action>> UpdateRole_action(List<Role_action> listRole_action)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.UpdateRole_action(listRole_action);
            }
            /*List<Role_action> result = new List<Role_action>();
            List<Role_action> gateWayResultList = new List<Role_action>();
            using (role_actionsTableAdapter _role_actionsTableAdapter = new role_actionsTableAdapter())
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWaySecurity.UpdateRole_action(listRole_action) : listRole_action;
                foreach (var role_action in gateWayResultList)
                {
                    var returnResult = _role_actionsTableAdapter
                                            .Update_data_role_action(
                                                role_action.RoleId,
                                                role_action.ActionId,
                                                role_action.ID);

                    Role_action LastInsertedRole_action = (await searchRole_action(role_action, "AND")).OrderBy(x => x.ID).Where(x => x.ID != 0).ToList().LastOrDefault();
                    if (LastInsertedRole_action != null)
                        result.Add(LastInsertedRole_action);
                }
            }
            return result;*/
        }

        public async Task<List<ActionRecord>> GetActionRecordData(int nbLine)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetActionRecordData(nbLine);
            }
            /*List<ActionRecord> result = new List<ActionRecord>();
            using (actionRecordsTableAdapter _actionRecordsTableAdapter = new actionRecordsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.actionRecordsDataTable>(_actionRecordsTableAdapter.GetData)).DataTableTypeToActionRecord();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);*/
        }

        public async Task<List<ActionRecord>> GetActionRecordDataById(int id)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetActionRecordDataById(id);
            }
            //return await searchActionRecord(new ActionRecord { ID = id }, "AND");
        }

        public async Task<List<Role>> GetRoleData(int nbLine)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetRoleData(nbLine);
            }
            /*List<Role> result = new List<Role>();
            List<Role> outputRoleList = new List<Role>();
            using (rolesTableAdapter _rolesTableAdapter = new rolesTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.rolesDataTable>(_rolesTableAdapter.GetData)).DataTableTypeToRole();
            
            foreach (var role in result)
            {
                outputRoleList.Add(await GetActionByRole(role));
            }
            if (nbLine.Equals(999) || result.Count == 0)
                return outputRoleList;

            return outputRoleList.GetRange(0, nbLine);*/
        }

        /*private async Task<Role> GetActionByRole(Role role)
        {
            //DALReferential dalRef = new DALReferential();
            //dalRef.
            //List<Privilege> privileges = 
            List<Entity.Action> actionList = new List<Entity.Action>();
            List<Role_action> role_actionFoundList = await searchRole_action(new Role_action { RoleId = role.ID }, "AND");
            foreach (var role_action in role_actionFoundList)
            {
                //--- here search for wright
                var actionFoundList = await GetActionDataById(role_action.ActionId);
                if (actionFoundList.Count > 0)
                    actionList.Add(actionFoundList[0]);
            }
            role.ActionList = actionList;
            return role;
        }*/

        public async Task<List<Role>> GetRoleDataById(int id)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetRoleDataById(id);
            }
            //return await searchRole(new Role { ID = id }, "AND");
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> GetActionData(int nbLine)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetActionData(nbLine);
            }
            /*List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            using (actionsTableAdapter _actionsTableAdapter = new actionsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.actionsDataTable>(_actionsTableAdapter.GetData)).DataTableTypeToAction();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);*/
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> GetActionDataById(int id)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetActionDataById(id);
            }
            //return await searchAction(new QCBDManagementCommon.Entities.Action { ID = id }, "AND");
        }

        public async Task<List<Agent_role>> GetAgent_roleData(int nbLine)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetAgent_roleData(nbLine);
            }
            /*List<Agent_role> result = new List<Agent_role>();
            using (agent_rolesTableAdapter _agent_rolesTableAdapter = new agent_rolesTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.agent_rolesDataTable>(_agent_rolesTableAdapter.GetData)).DataTableTypeToAgent_role();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);*/
        }

        public async Task<List<Agent_role>> GetAgent_roleDataById(int id)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetAgent_roleDataById(id);
            }
            //return await searchAgent_role(new Agent_role { ID = id }, "AND");
        }

        public async Task<List<Role_action>> GetRole_actionData(int nbLine)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetRole_actionData(nbLine);
            }
            /*List<Role_action> result = new List<Role_action>();
            using (role_actionsTableAdapter _role_actionsTableAdapter = new role_actionsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.role_actionsDataTable>(_role_actionsTableAdapter.GetData)).DataTableTypeToRole_action();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);*/
        }

        public async Task<List<Role_action>> GetRole_actionDataById(int id)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetRole_actionDataById(id);
            }
            //return await searchRole_action(new Role_action { ID = id }, "AND");
        }

        public async Task<List<Privilege>> GetPrivilegeData(int nbLine)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetPrivilegeData(nbLine);
            }
        }

        public async Task<List<Privilege>> GetPrivilegeDataById(int id)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.GetPrivilegeDataById(id);
            }
        }

        public Agent GetAuthenticatedUser()
        {
            return AuthenticatedUser;
        }

        public async Task<List<ActionRecord>> searchActionRecord(ActionRecord ActionRecord, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return ActionRecord.ActionRecordTypeToFilterDataTable(filterOperator); });
        }

        public void Dispose()
        {
            
        }

        public async Task<List<Role>> searchRole(Role Role, string filterOperator)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.searchRole(Role, filterOperator);
            }
            //return await Task.Factory.StartNew(() => { return Role.RoleTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> searchAction(QCBDManagementCommon.Entities.Action Action, string filterOperator)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.searchAction(Action, filterOperator);
            }
            //return await Task.Factory.StartNew(() => { return Action.ActionTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Agent_role>> searchAgent_role(Agent_role Agent_role, string filterOperator)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.searchAgent_role(Agent_role, filterOperator);
            }
            //return await Task.Factory.StartNew(() => { return Agent_role.Agent_roleTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Role_action>> searchRole_action(Role_action Role_action, string filterOperator)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.searchRole_action(Role_action, filterOperator);
            }
            //return await Task.Factory.StartNew(() => { return Role_action.Role_actionTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Privilege>> UpdatePrivilege(List<Privilege> listPrivilege)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.UpdatePrivilege(listPrivilege);
            }
        }

        public async Task<List<Privilege>> searchPrivilege(Privilege Privilege, string filterOperator)
        {
            using (GateWaySecurity gateWaySecurity = new GateWaySecurity())
            {
                gateWaySecurity.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await gateWaySecurity.searchPrivilege(Privilege, filterOperator);
            }
        }

        public string CalculateHash(string clearTextPassword)
        {
            throw new NotImplementedException();
        }

        public List<Agent> DisableAgent(List<Agent> listAgent)
        {
            throw new NotImplementedException();
        }

        public List<Agent> EnableAgent(List<Agent> listAgent)
        {
            throw new NotImplementedException();
        }

        public Agent UseAgent(Agent inAgent)
        {
            throw new NotImplementedException();
        }

    } /* end class BlSecurity */
}