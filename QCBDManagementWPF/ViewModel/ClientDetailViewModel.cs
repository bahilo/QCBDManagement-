using QCBDManagementBusiness;
using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QCBDManagementWPF.Command;
using QCBDManagementCommon.Classes;
using QCBDManagementWPF.Interfaces;

namespace QCBDManagementWPF.ViewModel
{
    public class ClientDetailViewModel : BindBase
    {
        private Cart _cart;
        private Func<Object, Object> _page;
        private Func<string, object> _getObjectFromMainWindowViewModel;
        private NotifyTaskCompletion<ClientModel> _selectedCLientTask;
        private string _title;
        private List<string> _addressTypeList;


        //----------------------------[ Models ]------------------

        private ClientModel _selectedCLientModel;
        private CLientSideBarViewModel _clientSideBarViewModel;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<ClientModel> BtnSearchCommand { get; set; }
        public ButtonCommand<string> BtnDeleteCommand { get; set; }
        public ButtonCommand<string> BtnAddCommand { get; set; }
        public ButtonCommand<string> SelectClientForQuoteCommand { get; set; }
        public ButtonCommand<string> ValidChangeCommand { get; set; }
        public ButtonCommand<Contact> DetailSelectedContactCommand { get; set; }
        public ButtonCommand<Address> DetailSelectedAddressCommand { get; set; }

        public ClientDetailViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onSelectedCLientViewModelChange;
            _selectedCLientTask.PropertyChanged += onSelectedCLientTaskCompletion_saveSelectedClient;
        }

        private void instances()
        {
            _title = "Client Description";
            _selectedCLientTask = new NotifyTaskCompletion<ClientModel>();
            _addressTypeList = new List<string> { "Bill", "Delivery" };
        }

        private void instancesModel()
        {
            _selectedCLientModel = new ClientModel();
            _clientSideBarViewModel = new CLientSideBarViewModel();
        }

        private void instancesCommand()
        {
            SelectClientForQuoteCommand = new ButtonCommand<string>(setCartClientForQuote, canSetCartClientForQuote);
            ValidChangeCommand = new ButtonCommand<string>(saveChanges, canSaveChanges);
            BtnSearchCommand = new ButtonCommand<ClientModel>(selectNewClient, canSelectNewClient);
            BtnDeleteCommand = new ButtonCommand<string>(deleteClientInfo, canDeleteClientInfo);
            BtnAddCommand = new ButtonCommand<string>(addClientInfo, canAddClientInfo);
            DetailSelectedAddressCommand = new ButtonCommand<Address>(detailSelectedAddress, canDetailSelectedAddress);
            DetailSelectedContactCommand = new ButtonCommand<Contact>(detailSelectedContact, canDetailSelectedContact);
        }

        //----------------------------[ Properties ]------------------


        public CLientSideBarViewModel ClientSideBarViewModel
        {
            get { return _clientSideBarViewModel; }
            set { setProperty(ref _clientSideBarViewModel, value, "ClientSideBarViewModel"); }
        }

        public ClientModel SelectedCLientModel
        {
            get { return _selectedCLientModel; }
            set { _selectedCLientModel = value; onPropertyChange("SelectedCLientModel"); }
        }

        public List<string> AddressTypeList
        {
            get { return _addressTypeList; }
            set { _addressTypeList = value; onPropertyChange("AddressTypeList"); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public Cart Cart
        {
            get { return _cart; }
            set { setProperty(ref _cart, value, "Cart"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public bool IsNewContact { get; set; }
        public bool IsNewAddress { get; set; }

        //----------------------------[ Actions ]------------------

        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _page = navigObject;
        }

        /*internal void initCart(ref Cart cart)
        {
            _cart = cart;
        }*/

        public async Task<bool> confirmDelting(string obj)
        {
            return await Dialog.show(string.Format("Do you want to delete the {0}? ", obj));
        }

        public async Task<ClientModel> loadContactsAndAddresses(ClientModel cLientModel)
        {
            cLientModel.AddressList = await Bl.BlClient.searchAddress(new Address { ClientId = cLientModel.Client.ID }, "AND");
            cLientModel.Address = (cLientModel.AddressList.Count() > 0) ? cLientModel.AddressList.OrderBy(x => x.ID).Last() : new Address();
            cLientModel.ContactList = await Bl.BlClient.GetContactDataByClientList(new List<Client> { new Client { ID = cLientModel.Client.ID } });
            cLientModel.Contact = (cLientModel.ContactList.Count() > 0) ? cLientModel.ContactList.OrderBy(x => x.ID).Last() : new Contact();

            return cLientModel;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            GetObjectFromMainWindowViewModel = getObject;
        }

        //----------------------------[ Event Handler ]------------------

        private void onSelectedCLientViewModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedCLientModel"))
            {
                //loadContactsAndAddresses();
                ClientSideBarViewModel.SelectedClient = SelectedCLientModel;
            }
        }


        private void onSelectedCLientTaskCompletion_saveSelectedClient(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "IsSuccessfullyCompleted"))
            {
                SelectedCLientModel = _selectedCLientTask.Result;
            }
        }

        //----------------------------[ Action Commands ]------------------

        private async void saveChanges(string obj)
        {
            Dialog.showSearch("Please wait while we are dealing with your request...");
            List<Client> savedClientList = new List<Client>();
            List<Address> savedAddressList = new List<Address>();
            List<Contact> savedContactList = new List<Contact>();

            bool isClientMandatoryFieldEmpty = !string.IsNullOrEmpty(SelectedCLientModel.TxtFirstName)
                                                && !string.IsNullOrEmpty(SelectedCLientModel.TxtLastName)
                                                    && !string.IsNullOrEmpty(SelectedCLientModel.TxtEmail)
                                                        && !string.IsNullOrEmpty(SelectedCLientModel.TxtCompany)
                                                            && !string.IsNullOrEmpty(SelectedCLientModel.TxtStatus)
                                                                && SelectedCLientModel.Client.MaxCredit != 0
                                                                    && SelectedCLientModel.Client.PayDelay != 0;

            bool isAddressMandatoryFieldEmpty = !string.IsNullOrEmpty(SelectedCLientModel.Address.Name)
                                                    && !string.IsNullOrEmpty(SelectedCLientModel.Address.AddressName)
                                                        && !string.IsNullOrEmpty(SelectedCLientModel.Address.CityName)
                                                            && !string.IsNullOrEmpty(SelectedCLientModel.Address.Postcode)
                                                                && !string.IsNullOrEmpty(SelectedCLientModel.Address.Country);

            bool isContactMandatoryFieldEmpty = ((!string.IsNullOrEmpty(SelectedCLientModel.Contact.Firstname)
                                                    && !string.IsNullOrEmpty(SelectedCLientModel.Contact.LastName))
                                                        && !string.IsNullOrEmpty(SelectedCLientModel.Contact.Phone)
                                                            && !string.IsNullOrEmpty(SelectedCLientModel.Contact.Email))
                                                                || (string.IsNullOrEmpty(SelectedCLientModel.Contact.Firstname)
                                                                    && string.IsNullOrEmpty(SelectedCLientModel.Contact.LastName));

            if (isClientMandatoryFieldEmpty && isAddressMandatoryFieldEmpty && isContactMandatoryFieldEmpty)
            {
                var updateCount = 0;
                var createCount = 0;
                // Client updating and creating
                if (SelectedCLientModel.Client.ID != 0)
                {
                    var updatedList = await Bl.BlClient.UpdateClient(new List<Client> { SelectedCLientModel.Client });
                    updateCount++;
                }                    
                else
                {
                    SelectedCLientModel.Client.AgentId = Bl.BlSecurity.GetAuthenticatedUser().ID;
                    savedClientList = await Bl.BlClient.InsertClient(new List<Client> { SelectedCLientModel.Client });
                    if (savedClientList.Count() > 0)
                    {
                        SelectedCLientModel.Client = savedClientList[0];
                        createCount++;
                    }
                }

                // Address updating and creating
                if (SelectedCLientModel.Address.ID != 0)
                {
                    var updatedList = await Bl.BlClient.UpdateAddress(new List<Address> { SelectedCLientModel.Address });
                    updateCount++;
                }
                    
                else
                {
                    SelectedCLientModel.Address.ClientId = SelectedCLientModel.Client.ID;
                    savedAddressList = await Bl.BlClient.InsertAddress(new List<Address> { SelectedCLientModel.Address });
                    if (savedAddressList.Count() > 0)
                    {
                        SelectedCLientModel.AddressList.Add(savedAddressList[0]);
                        SelectedCLientModel.Address = savedAddressList[0];
                        createCount++;
                    }
                }

                // Contact updating and creating
                if (SelectedCLientModel.Contact.ID != 0)
                {
                    var updatedList = await Bl.BlClient.UpdateContact(new List<Contact> { SelectedCLientModel.Contact });
                    updateCount++;
                }                    
                else
                {
                    SelectedCLientModel.Contact.ClientId = SelectedCLientModel.Client.ID;
                    savedContactList = await Bl.BlClient.InsertContact(new List<Contact> { SelectedCLientModel.Contact });
                    if (savedContactList.Count() > 0)
                    {
                        SelectedCLientModel.ContactList.Add(savedContactList[0]);
                        SelectedCLientModel.Contact = savedContactList[0];
                        createCount++;
                    }
                }

                if (updateCount > 0)
                    await Dialog.show("Client has been successfully updated!");
                else if(createCount > 0)
                    await Dialog.show("Client has been successfully created!");
            }
            else
            {
                if (!isClientMandatoryFieldEmpty)
                    await Dialog.show("Please fill up mandatory Main detail fields.");
                if (!isAddressMandatoryFieldEmpty)
                    await Dialog.show("Please fill up mandatory Address detail fields.");
                if (!isContactMandatoryFieldEmpty)
                    await Dialog.show("Please fill up mandatory Contact detail fields.");
            }

            Dialog.IsDialogOpen = false;       
            
            _page(new ClientDetailViewModel());

        }

        private bool canSaveChanges(string arg)
        {
            bool isUpdate = securityCheck(QCBDManagementCommon.Enum.EAction.Client, QCBDManagementCommon.Enum.ESecurity._Update);
            bool isWrite = securityCheck(QCBDManagementCommon.Enum.EAction.Client, QCBDManagementCommon.Enum.ESecurity._Write);
            if (isUpdate && isWrite)
                return true;
            return false;
        }

        private void setCartClientForQuote(string obj)
        {
            Cart.Client = SelectedCLientModel;
            _page(new QuoteViewModel());
        }

        private bool canSetCartClientForQuote(string arg)
        {
            bool isWrite = securityCheck(QCBDManagementCommon.Enum.EAction.Quote, QCBDManagementCommon.Enum.ESecurity._Write);
            if (isWrite)
                return true;
            return false;
        }

        private async void deleteClientInfo(string obj)
        {            
            switch (obj)
            {
                case "client":
                    if (SelectedCLientModel != null && SelectedCLientModel.Client.ID != 0 && await confirmDelting(obj))
                    {
                        Dialog.showSearch(string.Format("Client Deleting {0}...", obj));
                        var deletedAddressList = await Bl.BlClient.DeleteAddress(_selectedCLientModel.AddressList);
                        var deletedContactList = await Bl.BlClient.DeleteContact(_selectedCLientModel.ContactList);
                        var deletedClientList = await Bl.BlClient.DeleteClient(new List<Client> { SelectedCLientModel.Client });
                        if (deletedClientList.Count == 0)
                        {
                            Dialog.showSearch("Client deleted successfully!");
                            _page(new ClientViewModel());
                        }                            
                    }
                    break;
                case "address":
                    if (SelectedCLientModel.Address != null && SelectedCLientModel.Address.ID != 0 && await confirmDelting(obj))
                    {
                        Dialog.showSearch(string.Format("Address Deleting {0}...", obj));
                        var deletedAddressList = await Bl.BlClient.DeleteAddress(new List<Address> { SelectedCLientModel.Address });
                        var clientModel = await loadContactsAndAddresses(SelectedCLientModel);
                        SelectedCLientModel.AddressList = clientModel.AddressList;
                        SelectedCLientModel.Address = clientModel.Address;
                        if (deletedAddressList.Count == 0)
                        {
                            Dialog.showSearch("Address deleted successfully!");
                            _page(new ClientDetailViewModel());
                        }                            
                    }
                    break;
                case "contact":
                    if (SelectedCLientModel.Contact != null && SelectedCLientModel.Contact.ID != 0 && await confirmDelting(obj))
                    {
                        Dialog.showSearch(string.Format("Contact Deleting {0}...", obj));
                        var deletedContactList = await Bl.BlClient.DeleteContact(new List<Contact> { SelectedCLientModel.Contact });
                        var clientModel = await loadContactsAndAddresses(SelectedCLientModel);
                        SelectedCLientModel.ContactList = clientModel.ContactList;
                        SelectedCLientModel.Contact = clientModel.Contact;
                        if (deletedContactList.Count == 0)
                        {
                            Dialog.showSearch("Contact deleted successfully!");
                            _page(new ClientDetailViewModel());
                        } 
                    }
                    break;
            }
            Dialog.IsDialogOpen = false;
            
        }

        private bool canDeleteClientInfo(string arg)
        {
            bool isDelete = securityCheck(QCBDManagementCommon.Enum.EAction.Client, QCBDManagementCommon.Enum.ESecurity._Delete);
            if (isDelete)
                return true;
            return false;
        }

        private void addClientInfo(string obj)
        {
            switch (obj)
            {
                case "address":
                    SelectedCLientModel.Address = new Address();
                    break;
                case "contact":
                    SelectedCLientModel.Contact = new Contact();
                    break;
            }
        }

        private bool canAddClientInfo(string arg)
        {
            bool isUpdate = securityCheck(QCBDManagementCommon.Enum.EAction.Client, QCBDManagementCommon.Enum.ESecurity._Update);
            bool isWrite = securityCheck(QCBDManagementCommon.Enum.EAction.Client, QCBDManagementCommon.Enum.ESecurity._Write);
            if (isUpdate && isWrite)
                return true;
            return false;
        }

        private void selectNewClient(ClientModel obj)
        {
            _selectedCLientTask.initializeNewTask(loadContactsAndAddresses(obj));
            //SelectedCLientModel = obj;
        }

        private bool canSelectNewClient(ClientModel arg)
        {
            return true;
        }

        private void detailSelectedContact(Contact obj)
        {
            SelectedCLientModel.Contact = obj;
        }

        private bool canDetailSelectedContact(Contact arg)
        {
            return true;
        }

        private void detailSelectedAddress(Address obj)
        {
            SelectedCLientModel.Address = obj;
        }

        private bool canDetailSelectedAddress(Address arg)
        {
            return true;
        }
    }
}
