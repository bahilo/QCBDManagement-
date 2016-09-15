using QCBDManagementBusiness;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.ViewModel
{
    public class AgentSideBarViewModel : BindBase
    {
        private Func<object, object> _page;
        private Func<string, object> _getObjectFromMainWindowViewModel;

        //----------------------------[ Models ]------------------

        private AgentModel _selectedAgentModel;
        private MainWindowViewModel _main;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> SetupAgentCommand { get; set; }
        public ButtonCommand<string> UtilitiesCommand { get; set; }


        public AgentSideBarViewModel()
        {
            instances();
            instancesCommand();
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onGetObjectFromMainWindowViewModelChange;
            PropertyChanged += onSelectedAgentModelChange;
        }

        private void instances()
        {

        }

        private void instancesCommand()
        {
            SetupAgentCommand = new ButtonCommand<string>(executeSetupAction, canExcecuteSetupAction);
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
        }


        //----------------------------[ Properties ]------------------

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public AgentModel SelectedAgentModel
        {
            get { return _selectedAgentModel; }
            set { _selectedAgentModel = value; onPropertyChange("SelectedAgentModel"); }
        }

        public Func<object, object> Page
        {
            get { return _page; }
            set { _page = value; onPropertyChange("Page"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }


        //----------------------------[ Actions ]------------------

        private async Task<Agent> loadNewUser(string login, string password)
        {
            Agent newAgent = new Agent();
            if (_main != null)
            {
                newAgent = await Bl.BlSecurity.AuthenticateUser(login, password, isClearPassword: false);
                _main.isNewAgentAuthentication = true;
                _main.SecurityLoginViewModel.AgentModel.Agent = Bl.BlSecurity.GetAuthenticatedUser();
                //_main.isNewAgentAuthentication = false;
            }
            return newAgent;
        }

        private void updateCommand()
        {
            UtilitiesCommand.raiseCanExecuteActionChanged();
            SetupAgentCommand.raiseCanExecuteActionChanged();
        }

        //----------------------------[ Event Handler ]------------------

        private void onGetObjectFromMainWindowViewModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "GetObjectFromMainWindowViewModel"))
            {
                _main = GetObjectFromMainWindowViewModel("main") as MainWindowViewModel;
                if (_main != null)
                {
                    _main.PropertyChanged += onCurrentPageChange_updateCommand;
                }
            }
        }

        private void onCurrentPageChange_updateCommand(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentViewModel")
                && ((_main.CurrentViewModel as AgentDetailViewModel) != null)
                || (_main.CurrentViewModel as AgentViewModel) != null)
                updateCommand();
        }/**/

        private void onSelectedAgentModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedAgentModel"))
                updateCommand();
        }

        //----------------------------[ Action Commands ]------------------


        private bool canExecuteUtilityAction(string arg)
        {
            bool isUpdate = securityCheck(EAction.Agent, ESecurity._Update);
            bool isWrite = securityCheck(EAction.Agent, ESecurity._Write);
            bool isDelete = securityCheck(EAction.Agent, ESecurity._Delete);
            bool isRead = securityCheck(EAction.Agent, ESecurity._Read);


            if (!isUpdate || !isWrite)
                return false;

            if (((Page(null) as AgentDetailViewModel) == null))
                return false;

            if (SelectedAgentModel == null || SelectedAgentModel.Agent.ID == 0)
                return false;

            if (!isUpdate && !isWrite && !isRead && !isDelete
                && arg.Equals("use"))
                return false;

            if (SelectedAgentModel.TxtStatus.Equals(EStatus.Active.ToString())
                && arg.Equals("activate"))
                return false;

            if (SelectedAgentModel.TxtStatus.Equals(EStatus.Deactivated.ToString())
                && arg.Equals("deactivate"))
                return false;

            return true;
        }

        private async void executeUtilityAction(string obj)
        {
            List<Agent> updatedAgentList = new List<Agent>();
            switch (obj)
            {
                case "activate":
                    Dialog.showSearch("Activating Status...");
                    SelectedAgentModel.TxtStatus = EStatus.Active.ToString();
                    updatedAgentList = await Bl.BlAgent.UpdateAgent(new List<Agent> { SelectedAgentModel.Agent });
                    if (updatedAgentList.Count > 0)
                        await Dialog.show("The Agent " + updatedAgentList[0].LastName + "has been successfully activated!");
                    break;
                case "deactivate":// ok
                    Dialog.showSearch("Deactivating Status...");
                    SelectedAgentModel.TxtStatus = EStatus.Deactivated.ToString();
                    updatedAgentList = await Bl.BlAgent.UpdateAgent(new List<Agent> { SelectedAgentModel.Agent });
                    if (updatedAgentList.Count > 0)
                        await Dialog.show("The Agent " + updatedAgentList[0].LastName + " has been successfully deactivated!");
                    break;
                case "use":
                    Dialog.showSearch("Connection new Agent...");
                    var newAgent = await loadNewUser(SelectedAgentModel.Agent.Login, SelectedAgentModel.Agent.HashedPassword);
                    if (newAgent.ID != 0)
                        await Dialog.show("Your are successfully connected as " + newAgent.FirstName + " " + newAgent.LastName);
                    /*var result = Bl.BlSecurity.AuthenticateUser(SelectedAgentModel.Agent.Login, SelectedAgentModel.Agent.HashedPassword);
                    Startup.Dal.SetUserCredential(Bl.BlSecurity.GetAuthenticatedUser());*/
                    break;
            }
            Dialog.IsDialogOpen = false;
            UtilitiesCommand.raiseCanExecuteActionChanged();
        }

        private bool canExcecuteSetupAction(string arg)
        {
            bool isWrite = securityCheck(EAction.Agent, ESecurity._Write);
            if (!isWrite)
                return false;
            return true;
        }

        private void executeSetupAction(string obj)
        {
            switch (obj)
            {
                case "new-agent":
                    if (_main != null)
                    {
                        _main.AgentViewModel.AgentDetailViewModel.SelectedAgentModel = SelectedAgentModel = new AgentModel();
                        Page(new AgentDetailViewModel());
                    }
                    break;
            }
        }


    }
}
