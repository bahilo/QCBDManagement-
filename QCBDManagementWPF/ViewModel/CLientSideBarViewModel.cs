
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QCBDManagementBusiness;

namespace QCBDManagementWPF.ViewModel
{
    public class CLientSideBarViewModel : BindBase
    {
        private Func<Object, Object> _page;        
        private Cart _cart;
        private Func<string, object> _getObjectFromMainWindowViewModel;


        //----------------------------[ Models ]------------------

        private MainWindowViewModel _main;
        private ClientModel _selectedClient;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> CLientSetupCommand { get; set; }
        public ButtonCommand<string> ClientUtilitiesCommand { get; set; }



        public CLientSideBarViewModel() : base()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onGetObjectFromMainWindowViewModelChange;
        }

        private void instances()
        {
            _cart = new Cart();
        }

        private void instancesModel()
        {
            _selectedClient = new ClientModel();
        }

        private void instancesCommand()
        {
            CLientSetupCommand = new ButtonCommand<string>(executeSetupAction, canExecuteSetupAction);
            ClientUtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);

        }

        //----------------------------[ Properties ]------------------

        public Cart Cart
        {
            get { return _cart; }
            set { setProperty(ref _cart, value, "Cart"); }
        }

        public ClientModel SelectedClient
        {
            get { return _selectedClient; }
            set { setProperty(ref _selectedClient, value, "SelectedClient"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }

        //----------------------------[ Actions ]------------------

        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _page = navigObject;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            GetObjectFromMainWindowViewModel = getObject;
        }

        private object getCurrentPage()
        {
            if (_page != null)
            {
                Object PageViewModel = _page(null) as ClientDetailViewModel;
                if (PageViewModel != null)
                    return PageViewModel;

                PageViewModel = _page(null) as ClientViewModel;
                if (PageViewModel != null)
                    return PageViewModel;
            }
            return null;
        }

        private void updateCommand()
        {
            ClientUtilitiesCommand.raiseCanExecuteActionChanged();
            CLientSetupCommand.raiseCanExecuteActionChanged();
        }

        public override void Dispose()
        {
            PropertyChanged -= onGetObjectFromMainWindowViewModelChange;
        }

        //----------------------------[ Event Handler ]------------------

        private void onSelectedCLientChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedClient"))
                updateCommand();
        }

        private void onGetObjectFromMainWindowViewModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "GetObjectFromMainWindowViewModel"))
            {
                _main = _getObjectFromMainWindowViewModel("main") as MainWindowViewModel;
                if (_main != null)
                    _main.PropertyChanged += onCurrentPageChange_updateCommand;

                var itemViewModelFound = GetObjectFromMainWindowViewModel("item") as ItemViewModel;
                if (itemViewModelFound != null)
                {
                    Cart = itemViewModelFound.Cart;
                }
            }
        }

        private void onCurrentPageChange_updateCommand(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentViewModel"))
                updateCommand();
        }

        //----------------------------[ Action Commands ]------------------
        
        private void executeUtilityAction(string obj)
        {
            if (_main != null)
            {
                switch (obj)
                {
                    case "search-client":
                        _page(new ClientDetailViewModel());
                        break;
                    case "select-quote-client":
                        Cart.Client = SelectedClient;
                        _page(_main.QuoteViewModel);
                        break;
                    case "client-command":
                        _main.CommandViewModel.SelectedClient = SelectedClient;
                        _page(_main.CommandViewModel);
                        break;
                    case "client-quote":
                        _main.QuoteViewModel.SelectedClient = SelectedClient;
                        _page(_main.QuoteViewModel);
                        break;
                }
            }
        }

        private bool canExecuteUtilityAction(string arg)
        {
            // to remove after debugging
            /*if (arg.Equals("client-quote") || arg.Equals("client-command"))
                return false;*/

            object currentPage = getCurrentPage();

            if (currentPage == null)
                return false;
            if (currentPage.GetType() == typeof(ClientDetailViewModel)
                && arg.Equals("search-client"))
                return false;
            if ((currentPage.GetType() != typeof(ClientDetailViewModel) || SelectedClient.Client.ID == 0)
                && (arg.Equals("client-command")
                || arg.Equals("client-quote")
                || arg.Equals("select-quote-client")))
                return false;

            return true;
        }

        private void executeSetupAction(string obj)
        {
            ClientDetailViewModel clientDetail = new ClientDetailViewModel();

            switch (obj)
            {
                case "new-client":
                    SelectedClient.Client = new QCBDManagementCommon.Entities.Client();
                    SelectedClient.Address = new QCBDManagementCommon.Entities.Address();
                    SelectedClient.AddressList = new List<QCBDManagementCommon.Entities.Address>();
                    SelectedClient.Contact = new QCBDManagementCommon.Entities.Contact();
                    SelectedClient.ContactList = new List<QCBDManagementCommon.Entities.Contact>();
                    _page(new ClientDetailViewModel());
                    break;
                case "new-address":
                    SelectedClient.Address = new QCBDManagementCommon.Entities.Address();
                    break;
                case "new-contact":
                    SelectedClient.Contact = new QCBDManagementCommon.Entities.Contact();
                    break;
            }
        }

        private bool canExecuteSetupAction(string arg)
        {
            object currentPage = getCurrentPage();

            bool isUpdate = securityCheck(QCBDManagementCommon.Enum.EAction.Client, QCBDManagementCommon.Enum.ESecurity._Update);
            bool isWrite = securityCheck(QCBDManagementCommon.Enum.EAction.Client, QCBDManagementCommon.Enum.ESecurity._Write);
            if ((!isUpdate || !isWrite)
                && (arg.Equals("new-client")
                || arg.Equals("new-contact")
                || arg.Equals("new-address")))
                return false;

            if (currentPage == null)
                return false;
            if ((currentPage.GetType() != typeof(ClientDetailViewModel) || SelectedClient.Client.ID == 0)
                && (arg.Equals("new-contact")
                || arg.Equals("new-address")))
                return false;

            if (SelectedClient.AddressList.Count == 0
                && arg.Equals("new-address"))
                return false;

            if (SelectedClient.ContactList.Count == 0
                && arg.Equals("new-contact"))
                return false;

            return true;
        }

        

        

    }
}
