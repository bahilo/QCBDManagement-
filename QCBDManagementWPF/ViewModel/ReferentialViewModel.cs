using QCBDManagementWPF.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementBusiness;
using QCBDManagementWPF.Classes;
using Entity = QCBDManagementCommon.Entities;
using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Models;
using System.ComponentModel;

namespace QCBDManagementWPF.ViewModel
{
    public class ReferentialViewModel : BindBase
    {
        private Func<object, object> _sideBarManagement;
        private Func<string, object> _getObjectFromMainWindowViewModel;
        private Func<object, object> _page;

        //----------------------------[ POCOs ]------------------

        //----------------------------[ Models ]------------------

        private ReferentialSideBarViewModel _referentialSideBarViewModel;
        private OptionSecurityViewModel _optionSecurityViewModel;
        private OptionGeneralViewModel _optionGeneralViewModel;
        private OptionDataAndDisplayViewModel _optionDataAndDisplayViewModel;
        private OptionEmailViewModel _optionEmailViewModel;

        //----------------------------[ Commands ]------------------


        public ReferentialViewModel()
        {
            instances();
            instancesPoco();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onStartupChange;
            PropertyChanged += onDialogChange;
        }

        private void instances()
        {
            
        }

        private void instancesPoco()
        {

        }

        private void instancesModel()
        {
            _referentialSideBarViewModel = new ReferentialSideBarViewModel();
            _optionSecurityViewModel = new OptionSecurityViewModel();
            _optionGeneralViewModel = new OptionGeneralViewModel();
            _optionDataAndDisplayViewModel = new OptionDataAndDisplayViewModel();
            _optionEmailViewModel = new OptionEmailViewModel();
        }

        private void instancesCommand()
        {
            
        }

        //----------------------------[ Properties ]------------------
                

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange( "Bl"); }
        }

        public OptionGeneralViewModel OptionGeneralViewModel
        {
            get { return _optionGeneralViewModel; }
            set { setProperty(ref _optionGeneralViewModel, value, "OptionGeneralViewModel"); }
        }

        public OptionDataAndDisplayViewModel OptionDataAndDisplayViewModel
        {
            get { return _optionDataAndDisplayViewModel; }
            set { setProperty(ref _optionDataAndDisplayViewModel, value, "OptionDataAndDisplayViewModel "); }
        }

        public OptionSecurityViewModel OptionSecurityViewModel
        {
            get { return _optionSecurityViewModel; }
            set { setProperty(ref _optionSecurityViewModel, value, "OptionSecurityViewModel"); }
        }

        public ReferentialSideBarViewModel ReferentialSideBarViewModel
        {
            get { return _referentialSideBarViewModel; }
            set { setProperty(ref _referentialSideBarViewModel, value, "ReferentialSideBarViewModel"); }
        }

        public OptionEmailViewModel OptionEmailViewModel
        {
            get { return _optionEmailViewModel; }
            set { setProperty(ref _optionEmailViewModel, value, "OptionEmailViewModel"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            private set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }

        //----------------------------[ Actions ]------------------
        
                    
        internal void sideBarContentManagement(Func<object, object> sideBarManagement)
        {
            _sideBarManagement = sideBarManagement;
            _optionSecurityViewModel.sideBarContentManagement(sideBarManagement);
            _referentialSideBarViewModel.sideBarContentManagement(sideBarManagement);
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            GetObjectFromMainWindowViewModel = getObject;
            _optionSecurityViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
            _referentialSideBarViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
        }

        internal void mainNavigObject(Func<object, object> navigation)
        {
            _page = navigation;
            ReferentialSideBarViewModel.mainNavigObject(_page);
            OptionSecurityViewModel.mainNavigObject(_page);
        }

        public override void Dispose()
        {
            PropertyChanged -= onStartupChange;
            PropertyChanged -= onDialogChange;
            OptionDataAndDisplayViewModel.Dispose();
            OptionEmailViewModel.Dispose();
            OptionGeneralViewModel.Dispose();
            OptionSecurityViewModel.Dispose();
        }

        //----------------------------[ Event Handler ]------------------


        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                _referentialSideBarViewModel.Startup = Startup;
                _optionSecurityViewModel.Startup = Startup;
                _optionGeneralViewModel.Startup = Startup;
                _optionDataAndDisplayViewModel.Startup = Startup;
                _optionEmailViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                _referentialSideBarViewModel.Dialog = Dialog;
                _optionSecurityViewModel.Dialog = Dialog;
                _optionGeneralViewModel.Dialog = Dialog;
                _optionDataAndDisplayViewModel.Dialog = Dialog;
                _optionEmailViewModel.Dialog = Dialog;
            }
        }

        //----------------------------[ Action Commands ]------------------



        public void executeNavig(string obj)
        {
            _sideBarManagement(ReferentialSideBarViewModel);
            switch (obj)
            {
                case "option":
                    _page(new OptionGeneralViewModel());
                    break;                
            }
        }

        /*internal void setHeaderImageManagement(Func<DisplayAndData.Display.Image, DisplayAndData.Display.Image> headerImageManagement)
        {
            _optionDataAndDisplayViewModel.setHeaderImageManagement(headerImageManagement);
        }*/

        internal void setImageManagement(Func<DisplayAndData.Display.Image, string, DisplayAndData.Display.Image> imageManagement)
        {
            _optionDataAndDisplayViewModel.setImageManagement(imageManagement);
        }
    }
}
