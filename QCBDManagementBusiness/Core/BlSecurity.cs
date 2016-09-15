using QCBDManagementBusiness.Helper;
using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementBusiness.Core
{
    public class BlSecurity : ISecurityManager
    {
        // Attributes

        public QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DAC { get; private set; }
        public Safe Safe { get; private set; }

        public BlSecurity(QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
            Safe = new Safe();
        }

        public async Task<Agent> AuthenticateUser(string username, string password, bool isClearPassword = true)
        {
            try
            {
                if(isClearPassword)
                    Safe.AuthenticatedUser = await DAC.DALSecurity.AuthenticateUser(username, CalculateHash(password));  //DAC.DALSecurity.AuthenticateUser(username, CalculateHash(password));
                else
                    Safe.AuthenticatedUser = await DAC.DALSecurity.AuthenticateUser(username, password);  //DAC.DALSecurity.AuthenticateUser(username, CalculateHash(password));

                Safe.IsAuthenticated = true;
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
                Safe.AuthenticatedUser = new Agent();
                Safe.IsAuthenticated = false;
                return Safe.AuthenticatedUser;
            }         
            return Safe.AuthenticatedUser;
        }

        public string CalculateHash(string clearTextPassword)
        {
            // Convert the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(clearTextPassword + (int)ESecurity.Salt);
            // Use the hash algorithm to calculate the hash
            MD5 algorithm = MD5.Create();
            byte[] hash = algorithm.ComputeHash(saltedHashBytes);
            // Return the hash string
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public Agent UseAgent(Agent inAgent)
        {
            //var result = DAC.DALAgent.UseAgent(inAgent);
            Agent result = new Agent();
            try
            {
                //result = BlSecurity;
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            BLHelper.check("Use", "attempt", 0, "Agent", result.ID);
            //if (result == null)
            //    throw new ApplicationException("Use Agent: Error Occurred!");
            return result;
        }

        public async Task<List<ActionRecord>> InsertActionRecord(List<ActionRecord> listActionRecord)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.InsertActionRecord(listActionRecord);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Insert", "List", 0, "ActionRecord", result.Count);
            return result;
        }

        public async Task<List<Role>> InsertRole(List<Role> Rolelist)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.InsertRole(Rolelist);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Insert", "List", 0, "Role", result.Count);
            return result;
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> InsertAction(List<QCBDManagementCommon.Entities.Action> listAction)
        {
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.InsertAction(listAction);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Insert", "List", 0, "QCBDManagementCommon.Entities.Action", result.Count);
            return result;
        }

        public async Task<List<Agent_role>> InsertAgent_role(List<Agent_role> Agent_rolelist)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.InsertAgent_role(Agent_rolelist);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Insert", "List", 0, "Agent_role", result.Count);
            return result;
        }

        public async Task<List<Role_action>> InsertRole_action(List<Role_action> listRole_action)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.InsertRole_action(listRole_action);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Insert", "List", 0, "Role_action", result.Count);
            return result;
        }

        public async Task<List<Privilege>> InsertPrivilege(List<Privilege> listPrivilege)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.InsertPrivilege(listPrivilege);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Insert", "List", 0, "Privilege", result.Count);
            return result;
        }

        public async Task<List<ActionRecord>> DeleteActionRecord(List<ActionRecord> listActionRecord)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.DeleteActionRecord(listActionRecord);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Delete", "List", 0, "ActionRecord", result.Count);
            return result;
        }

        public async Task<List<Role>> DeleteRole(List<Role> RoleList)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.DeleteRole(RoleList);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Delete", "List", 0, "Role", result.Count);
            return result;
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> DeleteAction(List<QCBDManagementCommon.Entities.Action> ActionList)
        {
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.DeleteAction(ActionList);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Delete", "List", 0, "QCBDManagementCommon.Entities.Action", result.Count);
            return result;
        }

        public async Task<List<Agent_role>> DeleteAgent_role(List<Agent_role> listAgent_role)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.DeleteAgent_role(listAgent_role);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Delete", "List", 0, "Agent_role", result.Count);
            return result;
        }

        public async Task<List<Role_action>> DeleteRole_action(List<Role_action> listRole_action)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.DeleteRole_action(listRole_action);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Delete", "List", 0, "Role_action", result.Count);
            return result;
        }

        public async Task<List<Privilege>> DeletePrivilege(List<Privilege> listPrivilege)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.DeletePrivilege(listPrivilege);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Delete", "List", 0, "Privilege", result.Count);
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

        /*public ActionRecord SaveUserAction()
        {
            throw new NotImplementedException();
        }

        public ActionRecord UpdateUserAction()
        {
            throw new NotImplementedException();
        }*/

        public async Task<List<ActionRecord>> UpdateActionRecord(List<ActionRecord> ActionRecordList)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.UpdateActionRecord(ActionRecordList);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Update", "List", 0, "ActionRecord", result.Count);
            return result;
        }

        public async Task<List<Role>> UpdateRole(List<Role> RoleList)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.UpdateRole(RoleList);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Update", "List", 0, "Role", result.Count);
            return result;
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> UpdateAction(List<QCBDManagementCommon.Entities.Action> ActionList)
        {
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.UpdateAction(ActionList);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Update", "List", 0, "Action", result.Count);
            return result;
        }

        public async Task<List<Role_action>> UpdateRole_action(List<Role_action> Role_actionList)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.UpdateRole_action(Role_actionList);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Update", "List", 0, "Role_action", result.Count);
            return result;
        }

        public async Task<List<Privilege>> UpdatePrivilege(List<Privilege> PrivilegeList)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.UpdatePrivilege(PrivilegeList);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Update", "List", 0, "Privilege", result.Count);
            return result;
        }

        public async Task<List<Agent_role>> UpdateAgent_role(List<Agent_role> Agent_roleList)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.UpdateAgent_role(Agent_roleList);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Update", "List", 0, "Agent_role", result.Count);
            return result;
        }

        public async Task<List<ActionRecord>> GetActionRecordData(int nbLine)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.GetActionRecordData(nbLine);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetActionRecordData", "List", 0, "All data ActionRecords", result.Count);
            return result;
        }

        public async Task<List<ActionRecord>> GetActionRecordDataById(int id)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.GetActionRecordDataById(id);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetActionRecordDataById", "ID", 0, "All data ActionRecords", result.Count);
            return result;
        }

        public Agent GetAuthenticatedUser()
        {
            return Safe.AuthenticatedUser;
        }

        public async Task<List<Role>> GetRoleData(int nbLine)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.GetRoleData(nbLine);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetRoleData", "List", 0, "All data Roles", result.Count);
            return result;
        }

        public async Task<List<Role>> GetRoleDataById(int id)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.GetRoleDataById(id);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetRoleDataById", "ID", 0, "All data Roles", result.Count);
            return result;
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> GetActionData(int nbLine)
        {
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.GetActionData(nbLine);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetActionData", "List", 0, "All data Actions", result.Count);
            return result;
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> GetActionDataById(int id)
        {
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.GetActionDataById(id);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetActionDataById", "ID", 0, "All data Actions", result.Count);
            return result;
        }

        public async Task<List<Agent_role>> GetAgent_roleData(int nbLine)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.GetAgent_roleData(nbLine);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetAgent_roleData", "List", 0, "All data Agent_roles", result.Count);
            return result;
        }

        public async Task<List<Agent_role>> GetAgent_roleDataById(int id)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.GetAgent_roleDataById(id);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetAgent_roleDataById", "ID", 0, "All data Agent_roles", result.Count);
            return result;
        }

        public async Task<List<Role_action>> GetRole_actionData(int nbLine)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.GetRole_actionData(nbLine);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetRole_actionData", "List", 0, "All data Role_actions", result.Count);
            return result;
        }

        public async Task<List<Role_action>> GetRole_actionDataById(int id)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.GetRole_actionDataById(id);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetRole_actionDataById", "ID", 0, "All data Role_actions", result.Count);
            return result;
        }

        public async Task<List<Privilege>> GetPrivilegeData(int nbLine)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.GetPrivilegeData(nbLine);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetPrivilegeData", "List", 0, "All data Privileges", result.Count);
            return result;
        }

        public async Task<List<Privilege>> GetPrivilegeDataById(int id)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.GetPrivilegeDataById(id);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("GetPrivilegeDataById", "ID", 0, "All data Privileges", result.Count);
            return result;
        }

        public async Task<List<ActionRecord>> searchActionRecord(ActionRecord ActionRecord, string filterOperator)
        {
            List<ActionRecord> result = new List<ActionRecord>();
            try
            {
                result = await DAC.DALSecurity.searchActionRecord(ActionRecord, filterOperator);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Search", "List", 0, "ActionRecord", result.Count);
            return result;
        }

        public async Task<List<Role>> searchRole(Role Role, string filterOperator)
        {
            List<Role> result = new List<Role>();
            try
            {
                result = await DAC.DALSecurity.searchRole(Role, filterOperator);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Search", "List", 0, "Role", result.Count);
            return result;
        }

        public async Task<List<QCBDManagementCommon.Entities.Action>> searchAction(QCBDManagementCommon.Entities.Action Action, string filterOperator)
        {
            List<QCBDManagementCommon.Entities.Action> result = new List<QCBDManagementCommon.Entities.Action>();
            try
            {
                result = await DAC.DALSecurity.searchAction(Action, filterOperator);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Search", "List", 0, "Action", result.Count);
            return result;
        }

        public async Task<List<Agent_role>> searchAgent_role(Agent_role Agent_role, string filterOperator)
        {
            List<Agent_role> result = new List<Agent_role>();
            try
            {
                result = await DAC.DALSecurity.searchAgent_role(Agent_role, filterOperator);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Search", "List", 0, "Agent_role", result.Count);
            return result;
        }

        public async Task<List<Role_action>> searchRole_action(Role_action Role_action, string filterOperator)
        {
            List<Role_action> result = new List<Role_action>();
            try
            {
                result = await DAC.DALSecurity.searchRole_action(Role_action, filterOperator);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Search", "List", 0, "Role_action", result.Count);
            return result;
        }

        public async Task<List<Privilege>> searchPrivilege(Privilege Privilege, string filterOperator)
        {
            List<Privilege> result = new List<Privilege>();
            try
            {
                result = await DAC.DALSecurity.searchPrivilege(Privilege, filterOperator);
            }
            catch (Exception ex)
            {
                // Debug.WriteLine(ex.Message); // Display message
            }
            // BLHelper.check("Search", "List", 0, "Privilege", result.Count);
            return result;
        }
    }
}