using QCBDManagementBusiness;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.ViewModel
{
    public class ReferentialSideBarViewModel : BindBase
    {
        //----------------------------[ Models ]------------------

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> UtilitiesCommand { get; set; }
        public ButtonCommand<string> SetupCommand { get; set; }
        private Func<object, object> _sideBarManagement;
        private Func<object, object> _page;
        private string _navigTo;

        public ReferentialSideBarViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();


        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onNavigToChange;

        }

        private void instances()
        {

        }


        private void instancesModel()
        {

        }

        private void instancesCommand()
        {
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
            SetupCommand = new ButtonCommand<string>(executeSetupAction, canExecuteSetupAction);
        }

        //----------------------------[ Properties ]------------------


        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public string NavigTo
        {
            get { return _navigTo; }
            set { setProperty(ref _navigTo, value, "NavigTo"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel { get; private set; }

        //----------------------------[ Actions ]------------------

        public async void executeNavig(string obj)
        {
            _sideBarManagement(this);
            switch (obj.ToLower())
            {
                case "monitoring":
                    await Dialog.show("Navig to Monitoring");
                    break;
                case "credential":
                    _page(new OptionSecurityViewModel());
                    break;
                case "data-display":
                    _page(new OptionDataAndDisplayViewModel());
                    break;
                case "email":
                    _page(new OptionEmailViewModel());
                    break;
            }
        }

        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _page = navigObject;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            GetObjectFromMainWindowViewModel = getObject;
        }

        internal void sideBarContentManagement(Func<object, object> sideBarManagement)
        {
            _sideBarManagement = sideBarManagement;
        }

        public override void Dispose()
        {
            PropertyChanged -= onNavigToChange;
            
        }

        //----------------------------[ Event Handler ]------------------

        private void onNavigToChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "NavigTo"))
            {
                executeNavig(NavigTo);
            }
        }

        //----------------------------[ Action Commands ]------------------

        private void executeSetupAction(string obj)
        {
            switch (obj)
            {
                case "data-display":
                    NavigTo = obj;
                    break;
                case "credential":
                    NavigTo = obj;
                    break;

            }
        }

        private bool canExecuteSetupAction(string arg)
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

        private bool canExecuteUtilityAction(string arg)
        {
            if (arg.Equals("monitoring"))
                return false;
            return true;
        }

        private void executeUtilityAction(string obj)
        {
            switch (obj)
            {
                case "monitoring":
                    NavigTo = obj;
                    break;
                case "email":
                    NavigTo = obj;
                    break;
            }
        }
    }
}
