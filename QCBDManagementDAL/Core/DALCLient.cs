using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementDAL.App_Data;
using QCBDManagementDAL.App_Data.QCBDDataSetTableAdapters;
using QCBDManagementDAL.Classes;
using QCBDManagementDAL.Helper.ChannelHelper;
using QCBDManagementGateway.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using System.Collections.Concurrent;
using QCBDManagementCommon.Classes;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementDAL.Core
{
    public class DALClient : IClientManager
    {
        private Func<double, double> _rogressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private GateWayClient _gateWayClient;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();

        public DALClient()
        {
            _gateWayClient = new GateWayClient();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
            _gateWayClient.PropertyChanged += onCredentialChange_loadClientDataFromWebService;
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }

        public GateWayClient GateWayClient
        {
            get { return _gateWayClient; }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                //_loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
                _gateWayClient.initializeCredential(user);
            }
        }

        private void onCredentialChange_loadClientDataFromWebService(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Credential"))
            {
                DALHelper.doActionAsync(retrieveGateWayDataClient);
                _gateWayClient.PropertyChanged -= onCredentialChange_loadClientDataFromWebService;
            }
        }

        private void retrieveGateWayDataClient()
        {
            int loadUnit = 500;

            ConcurrentBag<Client> clientList = new ConcurrentBag<Client>(new NotifyTaskCompletion<List<Client>>(_gateWayClient.GetClientData(_loadSize)).Task.Result);
            ConcurrentBag<Contact> contactList = new ConcurrentBag<Contact>();
            ConcurrentBag<Address> addressList = new ConcurrentBag<Address>();
            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                if (clientList.Count > 0)
                {
                    List<Client> savedClientList = new NotifyTaskCompletion<List<Client>>(UpdateClient(clientList.ToList())).Task.Result;

                    for (int i = 0; i < (savedClientList.Count() / loadUnit) || loadUnit >= savedClientList.Count() && i == 0; i++)
                    {
                        ConcurrentBag<Address> addressFoundList = new ConcurrentBag<Address>(new NotifyTaskCompletion<List<Address>>(_gateWayClient.GetAddressDataByClientList(savedClientList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetItemDataByCommand_itemList(new List<Command_item>(command_itemList.Skip(i * loadUnit).Take(loadUnit)));
                        addressList = new ConcurrentBag<Address>(addressList.Concat(new ConcurrentBag<Address>(addressFoundList)));

                        ConcurrentBag<Contact> contactFoundList = new ConcurrentBag<Contact>(new NotifyTaskCompletion<List<Contact>>(_gateWayClient.GetContactDataByClientList(savedClientList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetItemDataByCommand_itemList(new List<Command_item>(command_itemList.Skip(i * loadUnit).Take(loadUnit)));
                        contactList = new ConcurrentBag<Contact>(contactList.Concat(new ConcurrentBag<Contact>(contactFoundList)));
                    }

                    List<Address> savedAddressList = new NotifyTaskCompletion<List<Address>>(UpdateAddress(addressList.ToList())).Task.Result; // UpdateAddress(addressList.ToList());
                    List<Contact> savedContactList = new NotifyTaskCompletion<List<Contact>>(UpdateContact(new NotifyTaskCompletion<List<Contact>>(_gateWayClient.GetContactDataByClientList(clientList.ToList())).Task.Result)).Task.Result;

                }

            }
            finally
            {
                lock (_lock)
                {
                    _isLodingDataFromWebServiceToLocal = false;
                    _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
                    Log.write("Client loaded!", "TES");
                }
            }
            
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _rogressBarFunc = progressBarFunc;
        }

        public async Task<List<Client>> InsertClient(List<Client> listClient)
        {
            List<Client> result = new List<Client>();
            List<Client> gateWayResultList = new List<Client>();
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayClient.InsertClient(listClient) : listClient;
                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateClient(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Contact>> InsertContact(List<Contact> listContact)
        {
            List<Contact> result = new List<Contact>();
            List<Contact> gateWayResultList = new List<Contact>();
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayClient.InsertContact(listContact) : listContact;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateContact(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Address>> InsertAddress(List<Address> listAddress)
        {
            List<Address> result = new List<Address>();
            List<Address> gateWayResultList = new List<Address>();
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayClient.InsertAddress(listAddress) : listAddress;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateAddress(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Client>> UpdateClient(List<Client> listClient)
        {
            List<Client> result = new List<Client>();
            List<Client> gateWayResultList = new List<Client>();
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayClient.UpdateClient(listClient) : listClient;
                foreach (var client in gateWayResultList)
                {
                    int returnResult = _clientsTableAdapter
                                            .update_data_client(
                                                    client.AgentId,
                                                    client.FirstName,
                                                    client.LastName,
                                                    client.Company,
                                                    client.Email,
                                                    client.Phone,
                                                    client.Fax,
                                                    client.Rib,
                                                    client.CRN,
                                                    client.PayDelay,
                                                    client.Comment,
                                                    client.Status,
                                                    client.MaxCredit,
                                                    client.CompanyName,
                                                    client.ID);
                    if (returnResult > 0)
                        result.Add(client);
                }
            }
            return result;
        }

        public async Task<List<Contact>> UpdateContact(List<Contact> listContact)
        {
            List<Contact> result = new List<Contact>();
            List<Contact> gateWayResultList = new List<Contact>();
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayClient.UpdateContact(listContact) : listContact;
                foreach (var contact in gateWayResultList)
                {
                    int returnResult = _contactsTableAdapter
                                            .update_data_contact(
                                                    contact.ClientId,
                                                    contact.LastName,
                                                    contact.Firstname,
                                                    contact.Position,
                                                    contact.Email,
                                                    contact.Phone,
                                                    contact.Cellphone,
                                                    contact.Fax,
                                                    contact.Comment,
                                                    contact.ID);
                    if (returnResult > 0)
                        result.Add(contact);
                }
            }
            return result;
        }

        public async Task<List<Address>> UpdateAddress(List<Address> listAddress)
        {
            List<Address> result = new List<Address>();
            List<Address> gateWayResultList = new List<Address>();
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayClient.UpdateAddress(listAddress) : listAddress;
                foreach (var address in gateWayResultList)
                {
                    int returnResult = _addressesTableAdapter
                                            .update_data_address(
                                                    address.ClientId,
                                                    address.Name,
                                                    address.Name2,
                                                    address.CityName,
                                                    address.AddressName,
                                                    address.Postcode,
                                                    address.Country,
                                                    address.Comment,
                                                    address.FirstName,
                                                    address.LastName,
                                                    address.Phone,
                                                    address.Email,
                                                    address.ID);
                    if (returnResult > 0)
                        result.Add(address);
                }
            }
            return result;
        }

        public async Task<List<Client>> DeleteClient(List<Client> listClient)
        {
            List<Client> result = listClient;
            List<Client> gateWayResultList = new List<Client>();
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayClient.DeleteClient(listClient) : listClient;
                if (gateWayResultList.Count == 0)
                    foreach (Client client in listClient)
                    {
                        int returnResult = _clientsTableAdapter.delete_data_client(client.ID);
                        if (returnResult > 0)
                            result.Remove(client);
                    }
            }
            return result;
        }

        public async Task<List<Contact>> DeleteContact(List<Contact> listContact)
        {
            List<Contact> result = listContact;
            List<Contact> gateWayResultList = new List<Contact>();
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayClient.DeleteContact(listContact) : listContact;
                if (gateWayResultList.Count == 0)
                    foreach (Contact contact in listContact)
                    {
                        int returnResult = _contactsTableAdapter.Delete(
                                                                    contact.ID,
                                                                    contact.ClientId,
                                                                    contact.LastName,
                                                                    contact.Firstname,
                                                                    contact.Position,
                                                                    contact.Email,
                                                                    contact.Phone,
                                                                    contact.Cellphone,
                                                                    contact.Fax);
                        if (returnResult > 0)
                            result.Remove(contact);
                    }
            }
            return result;
        }

        public async Task<List<Address>> DeleteAddress(List<Address> listAddress)
        {
            List<Address> result = listAddress;
            List<Address> gateWayResultList = new List<Address>();
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayClient.DeleteAddress(listAddress) : listAddress;
                if (gateWayResultList.Count == 0)
                    foreach (Address address in listAddress)
                    {
                        int returnResult = _addressesTableAdapter.delete_data_address(address.ID);
                        if (returnResult > 0)
                            result.Remove(address);
                    }
            }
            return result;
        }


        public async Task<List<Client>> GetClientData(int nbLine)
        {
            List<Client> result = new List<Client>();
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.clientsDataTable>(_clientsTableAdapter.get_data_client)).DataTableTypeToClient();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Client>> GetClientDataByBillList(List<Bill> billList)
        {
            List<Client> result = new List<Client>();
            foreach (Bill bill in billList)
            {
                var clientFound = (await searchClient(new Client { ID = bill.ClientId }, "AND")).OrderByDescending(x => x.ID).FirstOrDefault();
                if (clientFound != null)
                    result.Add(clientFound);
            }
            return result;
        }

        public async Task<List<Contact>> GetContactData(int nbLine)
        {
            List<Contact> result = new List<Contact>();
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.contactsDataTable>(_contactsTableAdapter.get_data_contact)).DataTableTypeToContact();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Contact>> GetContactDataByClientList(List<Client> clientList)
        {
            List<Contact> result = new List<Contact>();
            foreach (Client client in clientList)
            {
                var contactFound = (await searchContact(new Contact { ClientId = client.ID }, "AND")).OrderByDescending(x => x.ID).FirstOrDefault();
                if (contactFound != null)
                    result.Add(contactFound);
            }
            return result;
        }

        public async Task<List<Address>> GetAddressData(int nbLine)
        {
            List<Address> result = new List<Address>();
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.addressesDataTable>(_addressesTableAdapter.get_data_address)).DataTableTypeToAddress();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Address>> GetAddressDataByCommandList(List<Command> commandList)
        {
            List<Address> result = new List<Address>();
            List<int> idList = new List<int>();
            foreach (Command command in commandList)
            {
                var billAddressFound = (await searchAddress(new Address { ID = command.BillAddress }, "AND")).OrderByDescending(x => x.ID).FirstOrDefault();
                var deliveryAddressFound = (await searchAddress(new Address { ID = command.DeliveryAddress }, "AND")).OrderByDescending(x => x.ID).FirstOrDefault();
                if (billAddressFound != null && !idList.Contains(billAddressFound.ID))
                {
                    result.Add(billAddressFound);
                    idList.Add(billAddressFound.ID);
                }

                if (deliveryAddressFound != null && !idList.Contains(deliveryAddressFound.ID))
                {
                    result.Add(deliveryAddressFound);
                    idList.Add(deliveryAddressFound.ID);
                }
            }
            return result;
        }

        public async Task<List<Address>> GetAddressDataByClientList(List<Client> clientList)
        {
            List<Address> result = new List<Address>();
            foreach (Client client in clientList)
            {
                var clientAddressFound = (await searchAddress(new Address { ClientId = client.ID }, "AND")).OrderByDescending(x => x.ID).FirstOrDefault();

                if (clientAddressFound != null)
                {
                    result.Add(clientAddressFound);
                }
            }
            return result;
        }

        public async Task<List<Command>> GetCommandClient(int id)
        {
            using (commandsTableAdapter _commandsTableAdapter = new commandsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.commandsDataTable>(_commandsTableAdapter.get_filter_command_by_client_id, id)).DataTableTypeToCommand().FindAll(x => x.Status.Equals(EStatusCommand.Command.ToString()));
        }

        public async Task<List<Command>> GetQuoteCLient(int id)
        {
            using (commandsTableAdapter _commandsTableAdapter = new commandsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.commandsDataTable>(_commandsTableAdapter.get_filter_command_by_client_id, id)).DataTableTypeToCommand().FindAll(x => x.Status.Equals(EStatusCommand.Quote.ToString()));
        }

        public async Task<List<Client>> GetClientDataById(int id)
        {
            using (clientsTableAdapter _clientsTableAdapter = new clientsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.clientsDataTable>(_clientsTableAdapter.get_data_client_by_id, id)).DataTableTypeToClient();
        }

        public async Task<List<Contact>> GetContactDataById(int id)
        {
            using (contactsTableAdapter _contactsTableAdapter = new contactsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.contactsDataTable>(_contactsTableAdapter.get_data_contact_by_id, id)).DataTableTypeToContact();
        }

        public async Task<List<Address>> GetAddressDataById(int id)
        {
            using (addressesTableAdapter _addressesTableAdapter = new addressesTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.addressesDataTable>(_addressesTableAdapter.get_data_address_by_id, id)).DataTableTypeToAddress();
        }


        public async Task<List<Client>> searchClient(Client client, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return client.ClientTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Contact>> searchContact(Contact Contact, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Contact.ContactTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Address>> searchAddress(Address Address, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Address.AddressTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Client>> searchClientFromWebService(Client client, string filterOperator)
        {
            List<Client> gateWayResultList = new List<Client>();
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayClient.searchClientFromWebService(client, filterOperator);

            }
            return gateWayResultList;
        }

        public async Task<List<Contact>> searchContactFromWebService(Contact Contact, string filterOperator)
        {
            List<Contact> gateWayResultList = new List<Contact>();
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayClient.searchContactFromWebService(Contact, filterOperator);

            }
            return gateWayResultList;
        }

        public async Task<List<Address>> searchAddressFromWebService(Address Address, string filterOperator)
        {
            List<Address> gateWayResultList = new List<Address>();
            using (GateWayClient gateWayClient = new GateWayClient())
            {
                gateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayClient.searchAddressFromWebService(Address, filterOperator);

            }
            return gateWayResultList;
        }

        public void Dispose()
        {
            /*DALHelper.emptyTable<addressesTableAdapter>();
            DALHelper.emptyTable<clientsTableAdapter>();
            DALHelper.emptyTable<contactsTableAdapter>();
            //_clientsTableAdapter.Dispose();*/
        }

        public Task<List<Client>> MoveClientAgentBySelection(List<Client> clientList, Agent toAgent)
        {
            throw new NotImplementedException();
        }
    } /* end class BlCLient */
}