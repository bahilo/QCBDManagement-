using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementBusiness;
using QCBDManagementWPF.Classes;
using System.Collections;
using QCBDManagementWPF.Models;
using QCBDManagementCommon.Enum;
using QCBDManagementWPF.Interfaces;
using System.Windows.Threading;

namespace QCBDManagementWPF.ViewModel
{
    public class AgentViewModel : BindBase
    {
        private string _navigTo;
        private List<string> _saveSearchParametersList;
        private Func<Object, Object> _page;
        private Func<Object, Object> _currentSideBarViewModelFunc;
        private Func<string, object> _getObjectFromMainWindowViewModel;        
        private List<Agent> _agents;
        private List<Agent> _clientAgentToMoveList;
        private string _title;

        //----------------------------[ Models ]------------------

        private AgentModel _agentModel;
        private IEnumerable<AgentModel> _activeAgentsViewModel;
        private IEnumerable<AgentModel> _deactivedAgentsViewModel;
        private AgentDetailViewModel _agentDetailViewModel;

        //----------------------------[ Commands ]------------------
        
        public ButtonCommand<AgentModel> CheckBoxCommand { get; set; }
        public ButtonCommand<string> NavigCommand { get; set; }
        public ButtonCommand<AgentModel> GetCurrentAgentCommand { get; set; }
        public ButtonCommand<AgentModel> ClientMoveCommand { get; set; }


        public AgentViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onSelectedAgentChange;
            PropertyChanged += onNavigToChange;
            PropertyChanged += onStartupChange;
            PropertyChanged += onDialogChange;
        }

        private void instances()
        {
            _title = "Agent Management";
            _navigTo = "client";
            _saveSearchParametersList = new List<string>();
            _deactivedAgentsViewModel = new List<AgentModel>();
            _activeAgentsViewModel = new List<AgentModel>();
            _clientAgentToMoveList = new List<Agent>();
            _agents = new List<Agent>();
        }
        

        private void instancesModel()
        {
            _agentModel = new AgentModel();
            _agentDetailViewModel = new AgentDetailViewModel();
            AgentSideBarViewModel = new AgentSideBarViewModel();
        }

        private void instancesCommand()
        {
            ClientMoveCommand = new ButtonCommand<AgentModel>(moveAgentCLient, canMoveClientAgent);
            CheckBoxCommand = new ButtonCommand<AgentModel>(saveResultGridChecks, canSaveResultGridChecks);
            NavigCommand = new ButtonCommand<string>(executeNavig, canExecuteNavig);
            GetCurrentAgentCommand = new ButtonCommand<AgentModel>(selectAgent, canSelectAgent);         
        }
        


        //----------------------------[ Properties ]------------------

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public string NavigTo
        {
            get { return _navigTo; }
            set { _navigTo = value; onPropertyChange("NavigTo"); }
        }

        public AgentModel AgentModel
        {
            get { return _agentModel; }
            set { _agentModel = value; onPropertyChange("AgentModel"); }
        }

        public IEnumerable<AgentModel> AgentModelList
        {
            get { return _agentDetailViewModel.AgentModelList; }
            set { _agentDetailViewModel.AgentModelList = value; onPropertyChange("AgentModelList"); }
        }

        public IEnumerable<AgentModel> ActiveAgentModelList
        {
            get { return _activeAgentsViewModel; }
            set { _activeAgentsViewModel = value; onPropertyChange("ActiveAgentModelList"); }
        }

        public IEnumerable<AgentModel> DeactivatedAgentModelList
        {
            get { return _deactivedAgentsViewModel; }
            set { _deactivedAgentsViewModel = value; onPropertyChange("DeactivatedAgentModelList"); }
        }

        public AgentModel SelectedAgentModel
        {
            get { return AgentDetailViewModel.SelectedAgentModel; }
            set { AgentDetailViewModel.SelectedAgentModel = value; onPropertyChange("SelectedAgentModel"); }
        }

        public AgentDetailViewModel AgentDetailViewModel
        {
            get { return _agentDetailViewModel; }
            set { _agentDetailViewModel = value; onPropertyChange("AgentDetailViewModel"); }
        }

        public AgentSideBarViewModel AgentSideBarViewModel
        {
            get { return _agentDetailViewModel.AgentSideBarViewModel; }
            set { _agentDetailViewModel.AgentSideBarViewModel = value; onPropertyChange("AgentSideBarViewModel"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }


        //----------------------------[ Actions ]------------------

        public List<AgentModel> agentListToModelViewList(List<Agent> AgentList)
        {
            List<AgentModel> output = new List<AgentModel>();
            foreach (Agent Agent in AgentList)
            {
                AgentModel avm = new AgentModel();
                avm.Agent = Agent;
                _agents.Add(Agent);
                output.Add(avm);
            }
            return output;
        }

        public async void loadAgents()
        {
            Dialog.showSearch("loading...");
            AgentModelList = agentListToModelViewList(await Bl.BlAgent.GetAgentData(-999));
            ActiveAgentModelList = AgentModelList.Where(x => x.TxtStatus.Equals(EStatus.Active.ToString()));
            DeactivatedAgentModelList = AgentModelList.Where(x => x.TxtStatus.Equals(EStatus.Deactivated.ToString()));
            Dialog.IsDialogOpen = false;
        }
        

        private List<Agent> retrieveAgentFrom(ArrayList properties)
        {
            List<Agent> output = new List<Agent>();
            foreach (Agent Agent in properties)
            {
                output.Add(Agent);
            }
            return output;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            GetObjectFromMainWindowViewModel = getObject;
            AgentSideBarViewModel.GetObjectFromMainWindowViewModel = getObject;
            AgentDetailViewModel.GetObjectFromMainWindowViewModel = getObject;
        }

        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _page = navigObject;
            AgentSideBarViewModel.Page = navigObject;
        }

        public void Dispose()
        {
            Bl.BlAgent.Dispose();
        }


        //----------------------------[ Event Handler ]------------------


        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                AgentDetailViewModel.Startup = Startup;
                AgentSideBarViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                AgentDetailViewModel.Dialog = Dialog;
                AgentSideBarViewModel.Dialog= Dialog;
            }
        }

        private void onNavigToChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "NavigTo"))
            {
                executeNavig(NavigTo);
            }
        }

        private void onSelectedAgentChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedAgentModel"))
            {
                NavigTo = "agent-detail";
            }
        }

        //----------------------------[ Action Commands ]------------------

        private async void moveAgentCLient(AgentModel obj)
        {
            Dialog.showSearch("Moving CLients to "+obj.TxtLastName+" in progress...");
            List<Client> clientMovedList = new List<Client>();
            if(obj != null)
                foreach (var fromAgent in _clientAgentToMoveList)
                {
                    clientMovedList = clientMovedList.Concat( await Bl.BlAgent.MoveAgentClient(fromAgent, obj.Agent)).ToList();
                }
            if (clientMovedList.Count > 0)
                await Dialog.show(string.Format("CLients moved successfully to {0}.", obj.TxtLastName));
            _clientAgentToMoveList.Clear();
            Dialog.IsDialogOpen = false;
            _page(this);
        }

        private bool canMoveClientAgent(AgentModel arg)
        {
            bool _isUserAdmin = securityCheck(QCBDManagementCommon.Enum.EAction.Security, QCBDManagementCommon.Enum.ESecurity.SendEmail)
                             && securityCheck(QCBDManagementCommon.Enum.EAction.Security, QCBDManagementCommon.Enum.ESecurity._Delete)
                                 && securityCheck(QCBDManagementCommon.Enum.EAction.Security, QCBDManagementCommon.Enum.ESecurity._Read)
                                     && securityCheck(QCBDManagementCommon.Enum.EAction.Security, QCBDManagementCommon.Enum.ESecurity._Update)
                                         && securityCheck(QCBDManagementCommon.Enum.EAction.Security, QCBDManagementCommon.Enum.ESecurity._Write);

            if (_isUserAdmin)
                return true;

            return false;
        }

        public void saveResultGridChecks(AgentModel param)
        {
            if (!_clientAgentToMoveList.Contains(param.Agent))
                _clientAgentToMoveList.Add(param.Agent);
            else
                _clientAgentToMoveList.Remove(param.Agent);
        }

        private bool canSaveResultGridChecks(AgentModel arg)
        {
            return true;
        }

        internal void sideBarContentManagement(Func<object, object> sideBarManagement)
        {
            _currentSideBarViewModelFunc = sideBarManagement;
        }
        
        private void selectAgent(AgentModel obj)
        {
            SelectedAgentModel = obj;
        }

        private bool canSelectAgent(AgentModel arg)
        {
            return true;
        }

        public void executeNavig(string obj)
        {
            _currentSideBarViewModelFunc(AgentSideBarViewModel);
            switch (obj)
            {
                case "agent":
                    _page(this);
                    break;
                case "agent-detail":
                    _page(AgentDetailViewModel);
                    break;
                default:
                    goto case "agent";
            }
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }

        


    }
}
