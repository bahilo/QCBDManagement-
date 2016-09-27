using MaterialDesignThemes.Wpf;
using QCBDManagementBusiness;
using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Interfaces;
using QCBDManagementWPF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace QCBDManagementWPF.ViewModel
{
    public class ClientViewModel : BindBase, IDisposable
    {
        private List<string> _idList;
        private List<string> _saveSearchParametersList;
        private List<string> _companyList;
        private Func<Object, Object> _page;
        private Func<Object, Object> _currentSideBarViewModelFunc;
        private Func<string, object> _getObjectFromMainWindowViewModel;
        private NotifyTaskCompletion<ClientModel> _selectedCLientTask;
        private string _title;
        private List<Agent> _agentList;
        private List<Address> _addressList;
        private List<Client> _clientList;
        private List<Client> _saveResultParametersList;

        //----------------------------[ Models ]------------------

        private ClientModel _clientModel;
        private List<ClientModel> _clientsModel;
        private ClientDetailViewModel _clientDetailViewModel;

        public CLientSideBarViewModel ClientSideBarViewModel { get; set; }

        //----------------------------[ Commands ]------------------

        public ButtonCommand<ClientModel> checkBoxResultGridCommand { get; set; }
        public ButtonCommand<string> checkBoxSearchCommand { get; set; }
        public ButtonCommand<string> rBoxSearchCommand { get; set; }
        public ButtonCommand<Agent> btnComboBxCommand { get; set; }
        public ButtonCommand<string> btnSearchCommand { get; set; }
        public ButtonCommand<string> NavigCommand { get; set; }
        public ButtonCommand<ClientModel> GetCurrentItemCommand { get; set; }
        //public ButtonCommand<string> CLientSetupCommand { get; set; }
        

        public ClientViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            _clientDetailViewModel.PropertyChanged += onSelectedClientChange;
            _selectedCLientTask.PropertyChanged += onSelectedCLientTaskCompletion_saveSelectedClient;
            PropertyChanged += onStartupChange;
            PropertyChanged += onDialogChange;
        }

        private void instances()
        {
            _title = "Client Management";
            _idList = new List<string>();
            _companyList = new List<string>();
            _saveSearchParametersList = new List<string>();
            _saveResultParametersList = new List<Client>();
            _selectedCLientTask = new NotifyTaskCompletion<ClientModel>();
            _agentList = new List<Agent>();
            _clientList = new List<Client>();
            _addressList = new List<Address>();
        }
        

        //internal void initCart(ref Cart _cart)
        //{
        //    ClientDetailViewModel.initCart(ref _cart);
        //}

        private void instancesModel()
        {
            _clientDetailViewModel = new ClientDetailViewModel();
            _clientsModel = new List<ClientModel>();
            _clientModel = new ClientModel();
            ClientSideBarViewModel = new CLientSideBarViewModel();
            _clientDetailViewModel.ClientSideBarViewModel = ClientSideBarViewModel;
        }

        private void instancesCommand()
        {
            checkBoxResultGridCommand = new ButtonCommand<ClientModel>(saveResultGridChecks, canSaveResultGridChecks);
            checkBoxSearchCommand = new ButtonCommand<string>(saveSearchChecks, canSaveSearchChecks);
            rBoxSearchCommand = new ButtonCommand<string>(saveSearchRadioButtonSelection, canSaveSearchRadioButtonSelection);
            btnComboBxCommand = new ButtonCommand<Agent>(moveCLientAgent, canMoveClientAgent);
            btnSearchCommand = new ButtonCommand<string>(filterClient, canFilterClient);
            NavigCommand = new ButtonCommand<string>(executeNavig, canExecuteNavig);
            GetCurrentItemCommand = new ButtonCommand<ClientModel>(selectCurrentClient, canSelectedCurrentClient);

            //CLientSetupCommand = new ButtonCommand<string>(test, canTest);
        }
        
        //----------------------------[ Properties ]------------------

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value;  }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public List<ClientModel> ClientModelList
        {
            get { return _clientsModel; }
            set { setProperty(ref _clientsModel, value, "ClientModelList"); }
        }

        public List<Agent> AgentList
        {
            get { return _agentList; }
            set { setProperty(ref _agentList, value, "AgentList"); }
        }

        public ClientDetailViewModel ClientDetailViewModel
        {
            get { return _clientDetailViewModel; }
            set { _clientDetailViewModel = value; onPropertyChange("ClientDetailViewModel"); }
        }

        public ClientModel SelectedCLientModel
        {
            get { return _clientDetailViewModel.SelectedCLientModel; }
            set { _clientDetailViewModel.SelectedCLientModel = value; onPropertyChange("SelectedCLientModel"); }
        }

        public ClientModel ClientModel
        {
            get { return _clientModel; }
            set { _clientModel = value; onPropertyChange("ClientModel"); }
        }


        public List<string> IdList
        {
            get { return _idList; }
            set { _idList = value; onPropertyChange("IdList"); }
        }

        public List<string> CompanyList
        {
            get { return _companyList; }
            set { _companyList = value; onPropertyChange("CompanyList"); }
        }

        public List<Address> AddressList
        {
            get { return _addressList; }
            set { _addressList = value; onPropertyChange("AddressList"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }


        //----------------------------[ Actions ]------------------

        public async void loadClients()
        {
            Dialog.showSearch("Loading...");
            AgentList = await Bl.BlAgent.GetAgentData(-999); // -999 =>  get the agent list without their roles
            ClientModelList = clientListToModelViewList(await Bl.BlClient.GetClientData(999));
            Dialog.IsDialogOpen = false;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            GetObjectFromMainWindowViewModel = getObject;
            ClientDetailViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
            ClientSideBarViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
        }

        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _page = navigObject;
            ClientDetailViewModel.mainNavigObject(navigObject);
            ClientSideBarViewModel.mainNavigObject(navigObject);
        }

        public List<ClientModel> clientListToModelViewList(List<Client> clientList)
        {
            List<ClientModel> output = new List<ClientModel>();


            Parallel.ForEach(clientList, (client) =>
            {
                ClientModel cvm = new ClientModel();

                if (AgentList.Count() > 0)
                {
                    var result = AgentList.Where(x => x.ID.Equals(client.AgentId)).ToList();
                    cvm.Agent.Agent = (result.Count > 0) ? result[0] : new Agent();
                }

                /*Address address = new Address { ClientId = client.ID };
                var results = Bl.BlClient.searchAddress(address, "AND");
                if (results.Count > 0)
                {
                    cvm.Address = results[0];
                }*/

                cvm.Client = client;
                //cvm._clients.Add(client);

                //_clients.Add(client);
                output.Add(cvm);
                //yield return this;

            });
            return output;
        }

        public async Task<ClientModel> loadContactsAndAddresses(ClientModel cLientViewModel)
        {
            cLientViewModel.AddressList = await Bl.BlClient.searchAddress(new Address { ClientId = cLientViewModel.Client.ID }, "AND");
            cLientViewModel.Address = (cLientViewModel.AddressList.Count() > 0) ? cLientViewModel.AddressList.OrderBy(x => x.ID).Last() : new Address();
            cLientViewModel.ContactList = await Bl.BlClient.GetContactDataByClientList(new List<Client> { new Client { ID = cLientViewModel.Client.ID } });
            cLientViewModel.Contact = (cLientViewModel.ContactList.Count() > 0) ? cLientViewModel.ContactList.OrderBy(x => x.ID).Last() : new Contact();

            return cLientViewModel;
        }

        /*private void contextManagement(IState nextPage)
        {
            var mainViewModel = _getObjectFromMainWindowViewModel("main") as MainWindowViewModel;
            if (mainViewModel != null)
            {
                mainViewModel.Context.PreviousState = this;
                mainViewModel.Context.NextState = nextPage;
            }
        }*/

        public override void Dispose()
        {
            _clientDetailViewModel.PropertyChanged -= onSelectedClientChange;
            _selectedCLientTask.PropertyChanged -= onSelectedCLientTaskCompletion_saveSelectedClient;
            PropertyChanged -= onStartupChange;
            PropertyChanged -= onDialogChange;
            ClientDetailViewModel.Dispose();
            ClientSideBarViewModel.Dispose();
        }

        internal void sideBarContentManagement(Func<object, object> sideBarManagement)
        {
            _currentSideBarViewModelFunc = sideBarManagement;
        }

        internal void setInitCart(ref Cart cart)
        {
            ClientDetailViewModel.Cart = cart;

        }

        //----------------------------[ Event Handler ]------------------



        private void onSelectedClientChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "SelectedCLientModel"))
            {
                executeNavig("client-detail");
            }
        }

        private void onSelectedCLientTaskCompletion_saveSelectedClient(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "IsSuccessfullyCompleted"))
            {
                SelectedCLientModel = _selectedCLientTask.Result;
            }
        }

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                _clientDetailViewModel.Startup = Startup; ClientSideBarViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                _clientDetailViewModel.Dialog = Dialog; ClientSideBarViewModel.Dialog = Dialog;
            }
        }

        //----------------------------[ Action Commands ]------------------

        private void selectCurrentClient(ClientModel obj)
        {
            _selectedCLientTask.initializeNewTask(loadContactsAndAddresses(obj));            
        }

        private bool canSelectedCurrentClient(ClientModel arg)
        {
            return true;
        }

        public void executeNavig(string obj)
        {
            _currentSideBarViewModelFunc(ClientSideBarViewModel);
            switch (obj)
            {
                case "client":
                    //contextManagement(this);
                    _page(this);
                    break;
                case "client-detail":
                    //contextManagement(ClientDetailViewModel);
                    _page(ClientDetailViewModel);
                    break;
                case "client-new":
                    //contextManagement(ClientDetailViewModel);
                    SelectedCLientModel = new ClientModel();
                    _page(ClientDetailViewModel);
                    break;
                default:
                    goto case "client";
            }
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }

        private async void moveCLientAgent(Agent obj)
        {
            Dialog.showSearch("Moving clients...");
            var movedClientList = await Bl.BlClient.MoveClientAgentBySelection(_saveResultParametersList, obj);
            if (movedClientList.Count > 0)
                await Dialog.show(movedClientList.Count +" have been moved to "+obj.LastName+" successfully!");

            _saveResultParametersList.Clear();
            Dialog.IsDialogOpen = false;
            _page(this);
        }

        private bool canMoveClientAgent(Agent arg) 
        {
            return true;
        }

        private async void filterClient(string obj)
        {
            Client client = new Client();
            string restrict = "";
            bool isDeep = false;
            Dialog.showSearch("Searching...");
            //new Thread(delegate ()
            //{
            //    Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
            //     {
            foreach (string checkedValue in _saveSearchParametersList)
            {
                switch (checkedValue)
                {
                    case "cbContact":
                        client.FirstName = obj;
                        client.LastName = obj;
                        break;
                    case "cbCompany":
                        client.Company = obj;
                        //client.CompanyName = obj;
                        break;
                    case "Client":
                        restrict = EStatus.Client.ToString();
                        client.Status = EStatus.Client.ToString();
                        break;
                    case "Prospect":
                        restrict = EStatus.Prospect.ToString();
                        client.Status = EStatus.Prospect.ToString();
                        break;
                    case "cbDeep":
                        isDeep = true;
                        break;
                }
            }

            List<Client> resultAfterFilter = new List<Client>();
            List<Client> resultBeforeFilter = new List<Client>();

            if(isDeep)
                resultBeforeFilter = await Bl.BlClient.searchClientFromWebService(client, "AND");
            else
                resultBeforeFilter = await Bl.BlClient.searchClient(client, "AND");

            if (string.Equals(restrict, EStatus.Client.ToString()))
            {
                resultAfterFilter = resultBeforeFilter.Where(x => x.Status == EStatus.Client.ToString()).ToList();
            }
            else if (string.Equals(restrict, EStatus.Prospect.ToString()))
            {
                resultAfterFilter = resultBeforeFilter.Where(x => x.Status == EStatus.Prospect.ToString()).ToList();
            }
            else
            {
                resultAfterFilter = resultBeforeFilter;
            }

            ClientModelList = clientListToModelViewList(resultAfterFilter);
            Dialog.IsDialogOpen = false;
        }

        private bool canFilterClient(string arg)
        {
            return true;
        }

        public void saveResultGridChecks(ClientModel param)
        {
            //Properties.Settings.Default.cbResultArrayValue.Add(param._client);
            if (!_saveResultParametersList.Contains(param.Client))
                _saveResultParametersList.Add(param.Client);
            else
                _saveResultParametersList.Remove(param.Client);
        }

        public bool canSaveResultGridChecks(ClientModel param)
        {
            return true;
        }

        private void saveSearchChecks(string obj)
        {
            if (!_saveSearchParametersList.Contains(obj))
                _saveSearchParametersList.Add(obj);
            else
                _saveSearchParametersList.Remove(obj);
        }

        private bool canSaveSearchChecks(string arg)
        {
            return true;
        }


        private void saveSearchRadioButtonSelection(string obj)
        {
            if (!_saveSearchParametersList.Contains(obj) && string.Equals(obj, "Client"))
            {
                _saveSearchParametersList.Add(obj);
                _saveSearchParametersList.Remove("Prospect");
            }
            else
            {
                _saveSearchParametersList.Add(obj);
                _saveSearchParametersList.Remove("Client");
            }

        }

        private bool canSaveSearchRadioButtonSelection(string arg)
        {
            return true;
        }


    }
}
