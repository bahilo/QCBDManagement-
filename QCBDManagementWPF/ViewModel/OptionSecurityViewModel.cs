using QCBDManagementBusiness;
using Entity = QCBDManagementCommon.Entities;
using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.ViewModel
{
    public class OptionSecurityViewModel : BindBase
    {
        private Func<object, object> _currentSideBarViewModelFunc;
        private Func<string, object> _getObjectFromMainWindowViewModel;
        private Func<object, object> _page;
        private List<RoleModel> _roleModelList;
        private List<AgentModel> _agentModelList;
        private Dictionary<int, Role> _agentCreddentailTableHeaders;
        private Dictionary<int, int> _rolePosition = new Dictionary<int, int>();
        private string _title;
        
        //----------------------------[ Models ]------------------

        public ReferentialSideBarViewModel ReferentialSideBarViewModel { get; set; }


        //----------------------------[ Commands ]------------------

        public ButtonCommand<object> UpdateCredentialCommand { get; set; }
        public ButtonCommand<RoleModel> CbxGetSelectedRoleCommand { get; set; }
        public ButtonCommand<AgentModel> CbxGetSelectedAgentCommand { get; set; }


        public OptionSecurityViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {

        }

        private void instances()
        {
            _roleModelList = new List<RoleModel>();
            _agentCreddentailTableHeaders = new Dictionary<int, Role>();
            _rolePosition = new Dictionary<int, int>();
            _agentModelList = new List<AgentModel>();
            _title = "Security Management";
        }

        private void instancesModel()
        {
            ReferentialSideBarViewModel = new ReferentialSideBarViewModel();

        }

        private void instancesCommand()
        {
            CbxGetSelectedRoleCommand = new ButtonCommand<RoleModel>(getCurrentRoleModel, canGetCurrentRoleModel);
            CbxGetSelectedAgentCommand = new ButtonCommand<AgentModel>(getCurrentAgentModel, canGetCurrentAgentModel);
            UpdateCredentialCommand = new ButtonCommand<object>(updateSecurityCredential, canUpdateSecurityCredential);
        }

        //----------------------------[ Properties ]------------------


        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public Role HeaderRole1
        {
            get { return (_rolePosition.Count > 0) ? AgentCreddentailTableHeaders[_rolePosition[0]] : new Role { Name = "HeaderRole1" }; }
        }

        public Role HeaderRole2
        {
            get { return (_rolePosition.Count > 1) ? AgentCreddentailTableHeaders[_rolePosition[1]] : new Role { Name = "HeaderRole2" }; }
        }

        public Role HeaderRole3
        {
            get { return (_rolePosition.Count > 2) ? AgentCreddentailTableHeaders[_rolePosition[2]] : new Role { Name = "HeaderRole3" }; }
        }

        public Role HeaderRole4
        {
            get { return (_rolePosition.Count > 3) ? AgentCreddentailTableHeaders[_rolePosition[3]] : new Role { Name = "HeaderRole4" }; }
        }

        public Role HeaderRole5
        {
            get { return (_rolePosition.Count > 4) ? AgentCreddentailTableHeaders[_rolePosition[4]] : new Role { Name = "HeaderRole5" }; }
        }

        public Role HeaderRole6
        {
            get { return (_rolePosition.Count > 5) ? AgentCreddentailTableHeaders[_rolePosition[5]] : new Role { Name = "HeaderRole6" }; }
        }

        public Role HeaderRole7
        {
            get { return (_rolePosition.Count > 6) ? AgentCreddentailTableHeaders[_rolePosition[6]] : new Role { Name = "HeaderRole7" }; }
        }

        public Role HeaderRole8
        {
            get { return (_rolePosition.Count > 7) ? AgentCreddentailTableHeaders[_rolePosition[7]] : new Role { Name = "HeaderRole8" }; }
        }

        public Role HeaderRole9
        {
            get { return (_rolePosition.Count > 8) ? AgentCreddentailTableHeaders[_rolePosition[8]] : new Role { Name = "HeaderRole9" }; }
        }

        public Dictionary<int, Role> AgentCreddentailTableHeaders
        {
            get { return _agentCreddentailTableHeaders; }
            set { setProperty(ref _agentCreddentailTableHeaders, value, "AgentCreddentailTableHead"); }
        }

        public List<AgentModel> AgentModelList
        {
            get { return _agentModelList; }
            set { setProperty(ref _agentModelList, value, "AgentModelList"); }
        }

        public List<RoleModel> RoleModelList
        {
            get { return _roleModelList; }
            set { setProperty(ref _roleModelList, value, "RoleModelList"); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            private set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }

        //----------------------------[ Actions ]------------------

        public async Task<List<RoleModel>> getRoleModel()
        {
            List<Role> roleList = await Bl.BlSecurity.GetRoleData(999);
            List<RoleModel> roleModelList = new List<RoleModel>();
            int i = 0;
            foreach (var role in roleList)
            {
                if (role.ActionList != null && role.ActionList.Count > 0)
                {
                    foreach (var action in role.ActionList)
                    {
                        RoleModel roleModel = new RoleModel();
                        ActionModel actionModel = new ActionModel();
                        actionModel.Action = action;
                        actionModel.PrivilegeModel.Privilege = action.Right;
                        roleModel.Role = role;
                        roleModel.ActionModel = actionModel;
                        roleModelList.Add(roleModel);
                    }
                    //_rolePosition[i] = role.ID;
                    //i++;
                    //AgentCreddentailTableHeaders[role.ID] = role;
                }
                /*else
                {
                    //RoleModel roleModel = new RoleModel();
                    //ActionModel actionModel = new ActionModel();
                    roleModelList.Add(new RoleModel { Role = role });
                }*/

                _rolePosition[i] = role.ID;
                i++;
                AgentCreddentailTableHeaders[role.ID] = role;
            }
            onPropertyChange("HeaderRole1");
            onPropertyChange("HeaderRole2");
            onPropertyChange("HeaderRole3");
            onPropertyChange("HeaderRole4");
            onPropertyChange("HeaderRole5");
            onPropertyChange("HeaderRole6");
            onPropertyChange("HeaderRole7");
            onPropertyChange("HeaderRole8");
            onPropertyChange("HeaderRole9");
            return roleModelList;
        }

        public async Task<List<AgentModel>> getAgentModel()
        {
            List<Agent> agentList = await Bl.BlAgent.GetAgentData(999);
            List<AgentModel> agentModelList = new List<AgentModel>();
            foreach (var agent in agentList)
            {
                AgentModel agentModel = new AgentModel();
                agentModel.RoleModelList = RoleModelList.OrderBy(x=>x.TxtID).Distinct().ToList();
                agentModel.Agent = agent;
                agentModel.RolePositionDisplay = _rolePosition;
                agentModelList.Add(agentModel); 
            }
            return agentModelList;
        }

        public async void loadData()
        {
            Dialog.showSearch("Security roles loading...");
            RoleModelList = await getRoleModel();
            Dialog.showSearch("Agent credentials loading...");
            AgentModelList = await getAgentModel();
            Dialog.IsDialogOpen = false;
        }

        internal void sideBarContentManagement(Func<object, object> sideBarManagement)
        {
            _currentSideBarViewModelFunc = sideBarManagement;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            GetObjectFromMainWindowViewModel = getObject;
        }

        internal void mainNavigObject(Func<object, object> navigation)
        {
            _page = navigation;
        }

        //----------------------------[ Event Handler ]------------------


        //----------------------------[ Action Commands ]------------------



        private void getCurrentRoleModel(RoleModel obj)
        {
            obj.IsModified = true;
        }

        private bool canGetCurrentRoleModel(RoleModel arg)
        {
            return true;
        }

        private async void updateSecurityCredential(object obj)
        {
            Dialog.showSearch("Credentials updating...");
            // update role right on actions
            List<Privilege> privilegeToSaveList = new List<Privilege>();
            List<RoleModel> roleModelModifiedList = RoleModelList.Where(x => x.IsModified).ToList();
            foreach (var roleModel in roleModelModifiedList)
            {
                foreach (Entity.Action action in roleModel.Role.ActionList)
                {
                    if (action.Right.ID != 0)
                        privilegeToSaveList.Add(action.Right);
                }
            }
            var savedPrivilegeList = await Bl.BlSecurity.UpdatePrivilege(privilegeToSaveList);
            if (savedPrivilegeList.Count > 0)
                await Dialog.show("Privileges updated successfully!");
            var savedRoleList = await Bl.BlSecurity.UpdateRole(roleModelModifiedList.Where(x => x.Role.Name != "Anonymous").Select(x => x.Role).Distinct().ToList());
            if (savedRoleList.Count > 0)
                await Dialog.show("Role updated successfully!");


            // update agent role
            var agentModifiedList = AgentModelList.Where(x => x.IsModified).ToList();
            foreach (var agentModel in agentModifiedList)
            {
                List<Agent_role> agent_roleToAddList = new List<Agent_role>();
                List<Agent_role> agent_roleToRemoveList = new List<Agent_role>();
                
                // add role to agent
                foreach (var role in agentModel.RoleToAddList)
                {
                    Agent_role agent_role = new Agent_role();
                    agent_role.AgentId = agentModel.Agent.ID;
                    agent_role.RoleId = role.ID;
                    agent_roleToAddList.Add(agent_role);
                }

                // delete agent role
                foreach (var role in agentModel.RoleToRemoveList)
                {
                    var agent_roleFoundList = await Bl.BlSecurity.searchAgent_role(new Agent_role { AgentId = agentModel.Agent.ID, RoleId = role.ID }, "AND");
                    agent_roleToRemoveList = new List<Agent_role>(agent_roleToRemoveList.Concat(agent_roleFoundList));
                }
                var savedAgent_roleList = await Bl.BlSecurity.InsertAgent_role(agent_roleToAddList);
                if (savedAgent_roleList.Count > 0)
                    await Dialog.show("Agent role has been saved successfuly");
                await Bl.BlSecurity.DeleteAgent_role(agent_roleToRemoveList);
                agentModel.IsModified = false;
                agentModel.RoleToAddList.Clear();
                agentModel.RoleToRemoveList.Clear();
            }
            Dialog.IsDialogOpen = false;
            _page(this);
        }

        private bool canUpdateSecurityCredential(object arg)
        {
            return true;
        }

        private void getCurrentAgentModel(AgentModel obj)
        {
            obj.IsModified = true;
            /*if (!obj.IsModified)
                obj.RoleModelList.IsChecked = true;
            else
                obj.RoleModelList.IsChecked = false;*/
        }

        private bool canGetCurrentAgentModel(AgentModel arg)
        {
            return true;
        }



    }
}
