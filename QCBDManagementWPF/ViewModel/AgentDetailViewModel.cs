using QCBDManagementBusiness;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Interfaces;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace QCBDManagementWPF.ViewModel
{
    public class AgentDetailViewModel : BindBase
    {
        private string _listSize;
        private string _title;
        private Func<string, object> _getObjectFromMainWindowViewModel;

        //----------------------------[ Models ]------------------

        private AgentModel _selectedAgentModel;
        private IEnumerable<AgentModel> _agentsViewModel;
        public AgentSideBarViewModel AgentSideBarViewModel { get; set; }


        //----------------------------[ Commands ]------------------

        public ButtonCommand<object> UpdateCommand { get; set; }
        public ButtonCommand<AgentModel> SearchCommand { get; set; }


        public AgentDetailViewModel() : base()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();

        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onSelectedAgentModelChange;
        }

        private void onSelectedAgentModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedAgentModel"))
            {
                AgentSideBarViewModel.SelectedAgentModel = SelectedAgentModel;
            }
        }

        private void instances()
        {
            _title = "Agent Description";
        }

        private void instancesModel()
        {
            _selectedAgentModel = new AgentModel();
        }

        private void instancesCommand()
        {
            UpdateCommand = new ButtonCommand<object>(updateAgent, canUpdateAgent);
            SearchCommand = new ButtonCommand<AgentModel>(searchAgent, canSearchAgent);
        }


        //----------------------------[ Properties ]------------------


        public AgentModel SelectedAgentModel
        {
            get { return _selectedAgentModel; }
            set { _selectedAgentModel = value; onPropertyChange("SelectedAgentModel"); }
        }

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

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }

        public IEnumerable<AgentModel> AgentModelList
        {
            get { return _agentsViewModel; }
            set { _agentsViewModel = value; onPropertyChange("AgentModelList"); }
        }


        //----------------------------[ Actions ]------------------

        //----------------------------[ Event Handler ]------------------


        internal async void onPwdBoxVerificationPasswordChange_updateTxtClearPasswordVerification(object sender, RoutedEventArgs e)
        {
            PasswordBox pwd = ((PasswordBox)sender);
            if (pwd.Password.Count() > 0)
            {
                SelectedAgentModel.TxtClearPasswordVerification = pwd.Password;
                if (!SelectedAgentModel.TxtClearPassword.Equals(SelectedAgentModel.TxtClearPasswordVerification))
                {
                    await Dialog.show("Password are not Identical!");
                }
            }


            //SelectedAgentModel.TxtClearPasswordVerification = ((PasswordBox)sender).Password;
        }

        internal void onPwdBoxPasswordChange_updateTxtClearPassword(object sender, RoutedEventArgs e)
        {
            PasswordBox pwd = ((PasswordBox)sender);
            if (pwd.Password.Count() > 0)
            {
                SelectedAgentModel.TxtClearPassword = pwd.Password;                
            }
        }


        //----------------------------[ Action Commands ]------------------

        private async void updateAgent(object obj)
        {
            bool isPasswordIdentical = false ;
            if (!string.IsNullOrEmpty(SelectedAgentModel.TxtClearPasswordVerification))
            {
                if (SelectedAgentModel.TxtClearPassword.Equals(SelectedAgentModel.TxtClearPasswordVerification))
                {
                    SelectedAgentModel.TxtHashedPassword = Bl.BlSecurity.CalculateHash(SelectedAgentModel.TxtClearPassword);
                    isPasswordIdentical = true;
                }
            }
            

            if (SelectedAgentModel.Agent.ID == 0)
            {
                if (isPasswordIdentical)
                {
                    Dialog.showSearch("Creating Agent " + SelectedAgentModel.Agent.LastName + "...");
                    SelectedAgentModel.Agent.Status = EStatus.Deactivated.ToString();
                    var insertedAgentList = await Bl.BlAgent.InsertAgent(new List<Agent> { SelectedAgentModel.Agent });
                    if (insertedAgentList.Count > 0)
                        await Dialog.show("Agent " + SelectedAgentModel.Agent.LastName + " Successfully Created!");
                    Dialog.IsDialogOpen = false;
                }
                await Dialog.show("Password are not Identical!");
            }
            else
            {
                if (!string.IsNullOrEmpty(SelectedAgentModel.TxtClearPasswordVerification) && !isPasswordIdentical)
                    await Dialog.show("Password are not Identical (update will occur except the password)!");

                Dialog.showSearch("Updating Agent " + SelectedAgentModel.Agent.LastName + "...");
                var updatedAgentList = await Bl.BlAgent.UpdateAgent(new List<Agent> { SelectedAgentModel.Agent });
                if (updatedAgentList.Count > 0)
                   await  Dialog.show("Agent "+ SelectedAgentModel.Agent .LastName+ " Successfully Updated!");
                Dialog.IsDialogOpen = false;
            }
        }

        private bool canUpdateAgent(object arg)
        {
            bool isWrite = securityCheck(EAction.Agent, ESecurity._Write);
            bool isUpdate = securityCheck(EAction.Agent, ESecurity._Update);
            if (isUpdate && isWrite)
                return true;

            return false;
        }

        private void searchAgent(AgentModel obj)
        {
            SelectedAgentModel = obj;
        }

        private bool canSearchAgent(AgentModel arg)
        {
            return true;
        }

    }
}
