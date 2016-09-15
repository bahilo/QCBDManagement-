using QCBDManagementBusiness;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MaterialDesignThemes.Wpf;
using System.Windows;
using QCBDManagementCommon.Classes;
using System.Threading;
using System.Windows.Controls;

namespace QCBDManagementWPF.ViewModel
{
    public class SecurityLoginViewModel : BindBase
    {
        private Func<Object, Object> _page;
        private Func<string, object> _getObjectFromMainWindowViewModel;
        private MainWindow _mainWindow;
        private string _errorMessage;
        private NotifyTaskCompletion<bool> _DialogtaskCompletion;
        private NotifyTaskCompletion<object> _authenticateUsertaskCompletion;
        private string _clearPassword;
        private string _login;

        //----------------------------[ Models ]------------------

        private AgentModel _agentModel;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<object> LogoutCommand { get; set; }


        public SecurityLoginViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
            //showView();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            AgentModel.PropertyChanged += onAgentChange;
            AgentModel.PropertyChanged += onAgentChange_goToHomePage;
            _DialogtaskCompletion.PropertyChanged += onDialogDisplayTaskComplete_authenticateUser;
            _authenticateUsertaskCompletion.PropertyChanged += onAuthenticateUserTaskComplete_checkIfUserFound;
        }

        private void instances()
        {
            _errorMessage = "";
            _DialogtaskCompletion = new NotifyTaskCompletion<bool>();
            _authenticateUsertaskCompletion = new NotifyTaskCompletion<object>();
            LogoutCommand = new ButtonCommand<object>(logOut, canLogOut);
        }

        private void instancesModel()
        {
            _agentModel = new AgentModel();
        }

        private void instancesCommand()
        {

        }
        //----------------------------[ Properties ]------------------

        public AgentModel AgentModel
        {
            get { return _agentModel; }
            set { setProperty(ref _agentModel, value, "AgentModel"); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public string TxtErrorMessage
        {
            get { return _errorMessage; }
            set { setProperty(ref _errorMessage, value, "TxtErrorMessage"); }
        }

        public string TxtClearPassword
        {
            get { return _clearPassword; }
            set { _clearPassword = value; onPropertyChange("TxtClearPassword"); }
        }

        public string TxtLogin
        {
            get { return _login; }
            set { _login = value; onPropertyChange("TxtLogin"); }
        }

        //----------------------------[ Actions ]------------------


        public void showView()
        {
            //AgentModel = new AgentModel();
            _DialogtaskCompletion.initializeNewTask(Dialog.show(this));
        }

        private void ClosingEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        {

        }

        private async Task<bool> closeApp()
        {
            bool returnValue = false;
            if (Dialog != null)
            {
                returnValue = await Dialog.show("Do you really want to leave the application ?");
                if (returnValue)
                {
                    if (_mainWindow != null)
                        _mainWindow.Close();
                }
            }
            return returnValue;
        }

        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _page = navigObject;
        }

        private async Task<object> authenticateAgent()
        {
            //Dialog.showSearch("Searching...");
            var agentFound = await Bl.BlSecurity.AuthenticateUser(TxtLogin, TxtClearPassword);
            if (agentFound != null && agentFound.ID != 0)
            {
                AgentModel.Agent = agentFound;
            }
            else
            {
                TxtErrorMessage = "Your User Name or password is incorrect!";
            }               
            
            return null;
            //Dialog.IsDialogOpen = false;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            _getObjectFromMainWindowViewModel = getObject;
            _mainWindow = getObject("window") as MainWindow;
        }

        public async void startAuthentication()
        {
            TxtLogin = "codsimex212";
            TxtClearPassword = "codsimex212";
            await authenticateAgent();
        }

        //----------------------------[ Event Handler ]------------------

        private void onAgentChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Agent"))
            {
                _page(new HomeViewModel());
            }
        }

        internal void onPwdBoxPasswordChange_updateTxtClearPassword(object sender, RoutedEventArgs e)
        {
            TxtClearPassword = ((PasswordBox)sender).Password;
        }

        private void onAgentChange_goToHomePage(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Agent"))
                _page(new HomeViewModel());
        }

        private void onDialogDisplayTaskComplete_authenticateUser(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName.Equals("IsSuccessfullyCompleted"))
            {
                //Thread.Sleep(500);
                bool result = _DialogtaskCompletion.Result;
                if (!string.IsNullOrEmpty(TxtLogin) && !string.IsNullOrEmpty(TxtClearPassword) && result)
                    _authenticateUsertaskCompletion.initializeNewTask(authenticateAgent());
                else
                    showView();
                //if (AgentModel.Agent.ID == 0)
                //{
                //    showView();
                //}
            }
        }

        private void onAuthenticateUserTaskComplete_checkIfUserFound(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSuccessfullyCompleted"))
            {
                if (AgentModel.Agent.ID == 0)
                {
                    showView();
                }
            }
        }

        //----------------------------[ Action Commands ]------------------

        private void logOut(object obj)
        {
            AgentModel = new AgentModel();
            showView();
        }

        private bool canLogOut(object arg)
        {
            return true;
        }

        //private bool canCloseApp(Window arg)
        //{
        //    return true;
        //}




    }
}
