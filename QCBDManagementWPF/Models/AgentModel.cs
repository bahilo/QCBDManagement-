using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace QCBDManagementWPF.Models
{
    public class AgentModel : BindBase
    {
        private Agent _agent;
        private List<RoleModel> _roleModelList;
        private string _clearPassword;
        private string _clearPasswordVerification;
        private bool _isModified;
        private List<Role> _roleToAddList;
        private List<Role> _roleToRemoveList;
        private Dictionary<int, int> _rolePosition = new Dictionary<int, int>();

        public AgentModel()
        {
            _agent = new Agent();
            _roleModelList = new List<RoleModel>();
            _roleToAddList = new List<Role>();
            _roleToRemoveList = new List<Role>();
            _rolePosition = new Dictionary<int, int>();
            _clearPassword = "";
            _clearPasswordVerification = "";
            PropertyChanged += onRoleModelListChange_updateUIRole;
        }

        private void onRoleModelListChange_updateUIRole(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("RoleModelList"))
                updateUIRole();
        }

        public Dictionary<int, int> RolePositionDisplay
        {
            get { return _rolePosition; }
            set { _rolePosition = value; onPropertyChange("RoleDisplayPosition"); }
        }

        public List<RoleModel> RoleModelList
        {
            get { return _roleModelList; }
            set { _roleModelList = value; onPropertyChange("RoleModelList"); }
        }

        public string TxtClearPassword
        {
            get { return _clearPassword; }
            set { setProperty(ref _clearPassword, value, "TxtClearPassword"); }
        }

        public string TxtClearPasswordVerification
        {
            get { return _clearPasswordVerification; }
            set { _clearPasswordVerification = value; onPropertyChange("TxtClearPasswordVerification"); }
        }

        public string TxtAdmin
        {
            get { return _agent.Admin; }
            set { _agent.Admin = value; onPropertyChange("TxtAdmin"); }
        }

        public bool IsModified
        {
            get { return _isModified; }
            set { setProperty(ref _isModified, value, "IsModified"); }
        }

        public Agent Agent
        {
            get { return _agent; }
            set { setProperty(ref _agent, value, "Agent"); }
        }

        public string TxtID
        {
            get { return _agent.ID.ToString(); }
            set { _agent.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string TxtFirstName
        {
            get { return _agent.FirstName; }
            set { _agent.FirstName = value; onPropertyChange("TxtFirstName"); }
        }

        public string TxtLastName
        {
            get { return _agent.LastName; }
            set { _agent.LastName = value; onPropertyChange("TxtLastName"); }
        }

        public string TxtPhone
        {
            get { return _agent.Phone; }
            set { _agent.Phone = value; onPropertyChange("TxtPhone"); }
        }

        public string TxtFax
        {
            get { return _agent.Fax; }
            set { _agent.Fax = value; onPropertyChange("TxtFax"); }
        }

        public string TxtEmail
        {
            get { return _agent.Email; }
            set { _agent.Email = value; onPropertyChange("TxtEmail"); }
        }

        public string TxtLogin
        {
            get { return _agent.Login; }
            set { _agent.Login = value; onPropertyChange("TxtLogin"); }
        }

        public string TxtHashedPassword
        {
            get { return _agent.HashedPassword; }
            set { _agent.HashedPassword = value; onPropertyChange("TxtHashedPassword"); }
        }

        public List<Role> RoleList
        {
            get { return _agent.RoleList; }
            set { _agent.RoleList = value; onPropertyChange("RoleList"); }
        }

        public List<Role> RoleToAddList
        {
            get { return _roleToAddList; }
            set { _roleToAddList = value; onPropertyChange("RoleToAddList"); }
        }

        public List<Role> RoleToRemoveList
        {
            get { return _roleToRemoveList; }
            set { _roleToRemoveList = value; onPropertyChange("RoleToRemoveList"); }
        }

        public string TxtStatus
        {
            get { return _agent.Status; }
            set { _agent.Status = value; onPropertyChange("TxtStatus"); }
        }

        public string TxtListSize
        {
            get { return _agent.ListSize.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _agent.ListSize = converted; } else _agent.ListSize = 0;  onPropertyChange("TxtListSize");}
        }

        public bool IsRole1
        {
            get { return (RolePositionDisplay.Count > 0) ? getRoleBooleanByID(RolePositionDisplay[0]) : false; }
            set { if (RolePositionDisplay.Count > 0) { addRoleBy(RolePositionDisplay[0]); onPropertyChange("IsRole1"); } }
        }

        public bool IsRole2
        {
            get { return (RolePositionDisplay.Count > 1) ? getRoleBooleanByID(RolePositionDisplay[1]) : false; }
            set { if (RolePositionDisplay.Count > 1) { addRoleBy(RolePositionDisplay[1]); onPropertyChange("IsRole2"); } }
        }

        public bool IsRole3
        {
            get { return (RolePositionDisplay.Count > 2) ? getRoleBooleanByID(RolePositionDisplay[2]) : false; }
            set { if (RolePositionDisplay.Count > 2) { addRoleBy(RolePositionDisplay[2]); onPropertyChange("IsRole3"); } }
        }

        public bool IsRole4
        {
            get { return (RolePositionDisplay.Count > 3) ? getRoleBooleanByID(RolePositionDisplay[3]) : false; }
            set { if (RolePositionDisplay.Count > 3) { addRoleBy(RolePositionDisplay[3]); onPropertyChange("IsRole4"); } }
        }

        public bool IsRole5
        {
            get { return (RolePositionDisplay.Count > 4) ? getRoleBooleanByID(RolePositionDisplay[4]) : false; }
            set { if (RolePositionDisplay.Count > 4) { addRoleBy(RolePositionDisplay[4]); onPropertyChange("IsRole5"); } }
        }

        public bool IsRole6
        {
            get { return (RolePositionDisplay.Count > 5) ? getRoleBooleanByID(RolePositionDisplay[5]) : false; }
            set { if (RolePositionDisplay.Count > 5) { addRoleBy(RolePositionDisplay[5]); onPropertyChange("IsRole6"); } }
        }

        public bool IsRole7
        {
            get { return (RolePositionDisplay.Count > 6) ? getRoleBooleanByID(RolePositionDisplay[6]) : false; }
            set { if (RolePositionDisplay.Count > 6) { addRoleBy(RolePositionDisplay[6]); onPropertyChange("IsRole7"); } }
        }

        public bool IsRole8
        {
            get { return (RolePositionDisplay.Count > 7) ? getRoleBooleanByID(RolePositionDisplay[7]) : false; }
            set { if (RolePositionDisplay.Count > 7) { addRoleBy(RolePositionDisplay[7]); onPropertyChange("IsRole8"); } }
        }

        public bool IsRole9
        {
            get { return (RolePositionDisplay.Count > 8) ? getRoleBooleanByID(RolePositionDisplay[8]) : false; }
            set { if (RolePositionDisplay.Count > 8) { addRoleBy(RolePositionDisplay[8]); onPropertyChange("IsRole9"); } }
        }

        private bool getRoleBooleanByID(int id)
        {
            object _lock = new object();
            Role roleFound = RoleList.Where(x => x.ID == id).FirstOrDefault();
            lock (_lock)
                if (roleFound != null)
                    return true;
            return false;
        }

        private void addRoleBy(int id)
        {
            object _lock = new object();
            if (RoleModelList != null)
            {
                Role roleFound = RoleModelList.Where(x => x.Role.ID == id).Select(x=>x.Role).FirstOrDefault();
                lock (_lock)
                    if (roleFound != null)
                    {
                        if (RoleList.Where(x=>x.ID == roleFound.ID).Count() == 0)
                        {
                            RoleList.Add(roleFound);
                            RoleToAddList.Add(roleFound);
                        }
                        else
                        {
                            RoleList = RoleList.Where(x => x.ID != roleFound.ID).ToList();
                            RoleToRemoveList.Add(roleFound);
                        }                            
                    }                        
            }  
        }

        private void updateUIRole()
        {
            onPropertyChange("IsRole1");
            onPropertyChange("IsRole2");
            onPropertyChange("IsRole3");
            onPropertyChange("IsRole4");
            onPropertyChange("IsRole5");
            onPropertyChange("IsRole6");
            onPropertyChange("IsRole7");
            onPropertyChange("IsRole8");
            onPropertyChange("IsRole9");
        }




    }
}
