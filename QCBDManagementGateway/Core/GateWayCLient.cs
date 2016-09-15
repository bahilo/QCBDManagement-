using QCBDManagementGateway;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementGateway.Classes;
using QCBDManagementGateway.Helper.ChannelHelper;
using QCBDManagementGateway.QCBDServiceReference;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementGateway.Core
{
    public class GateWayClient : IClientManager, INotifyPropertyChanged
    {
        private QCBDManagementWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public GateWayClient()
        {
            _channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");// (binding, endPoint);
            //_channel.Endpoint.EndpointBehaviors.Add(new MyBehavior());
        }

        public void initializeCredential(Agent user)
        {
            Credential = user;
        }

        public Agent Credential
        {
            set
            {
                setServiceCredential(value.Login, value.HashedPassword);
                onPropertyChange("Credential");
            }
        }


        public void setServiceCredential(string login, string password)
        {
            _channel.Close();
            _channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");
            _channel.ClientCredentials.UserName.UserName = login;
            _channel.ClientCredentials.UserName.Password = password;
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<List<Client>> InsertClient(List<Client> listClient)
        {
            var formatListClientToArray = ServiceHelper.ClientTypeToArray(listClient);
            List<Client> result = new List<Client>();
            try
            {
                
                result = (await _channel.insert_data_clientAsync(formatListClientToArray)).ArrayTypeToClient();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Contact>> InsertContact(List<Contact> listContact)
        {
            var formatListContactToArray = ServiceHelper.ContactTypeToArray(listContact);
            List<Contact> result = new List<Contact>();
            try
            {
                
                result = (await _channel.insert_data_contactAsync(formatListContactToArray)).ArrayTypeToContact();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Address>> InsertAddress(List<Address> listAddress)
        {
            var formatListAddressToArray = ServiceHelper.AddressTypeToArray(listAddress);
            List<Address> result = new List<Address>();
            try
            {
                
                result = (await _channel.insert_data_addressAsync(formatListAddressToArray)).ArrayTypeToAddress();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Client>> UpdateClient(List<Client> listClient)
        {
            var formatListClientToArray = ServiceHelper.ClientTypeToArray(listClient);
            List<Client> result = new List<Client>();
            try
            {
                
                result = (await _channel.update_data_clientAsync(formatListClientToArray)).ArrayTypeToClient();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException ex)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Contact>> UpdateContact(List<Contact> listContact)
        {
            var formatListContactToArray = ServiceHelper.ContactTypeToArray(listContact);
            List<Contact> result = new List<Contact>();
            try
            {
                
                result = (await _channel.update_data_contactAsync(formatListContactToArray)).ArrayTypeToContact();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Address>> UpdateAddress(List<Address> listAddress)
        {
            var formatListAddressToArray = ServiceHelper.AddressTypeToArray(listAddress);
            List<Address> result = new List<Address>();
            try
            {
                
                result = (await _channel.update_data_addressAsync(formatListAddressToArray)).ArrayTypeToAddress();
             }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Client>> DeleteClient(List<Client> listClient)
        {
            var formatListClientToArray = ServiceHelper.ClientTypeToArray(listClient);
            List<Client> result = new List<Client>();
            try
            {
                
                result = (await _channel.delete_data_clientAsync(formatListClientToArray)).ArrayTypeToClient();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Contact>> DeleteContact(List<Contact> listContact)
        {
            var formatListContactToArray = ServiceHelper.ContactTypeToArray(listContact);
            List<Contact> result = new List<Contact>();
            try
            {
                
                result = (await _channel.delete_data_contactAsync(formatListContactToArray)).ArrayTypeToContact();
             }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Address>> DeleteAddress(List<Address> listAddress)
        {
            var formatListAddressToArray = ServiceHelper.AddressTypeToArray(listAddress);
            List<Address> result = new List<Address>();
            try
            {
                
                result = (await _channel.delete_data_addressAsync(formatListAddressToArray)).ArrayTypeToAddress();
             }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }


        public async Task<List<Client>> GetClientData(int nbLine)
        {
            List<Client> result = new List<Client>();
            try
            {                
                result = (await _channel.get_data_clientAsync(nbLine.ToString())).ArrayTypeToClient();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Client>> GetClientDataByBillList(List<Bill> billList)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = (await _channel.get_data_client_by_bill_listAsync(billList.BillTypeToArray())).ArrayTypeToClient();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Contact>> GetContactData(int nbLine)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                
                result = (await _channel.get_data_contactAsync(nbLine.ToString())).ArrayTypeToContact();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Contact>> GetContactDataByClientList(List<Client> clientList)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = (await _channel.get_data_contact_by_client_listAsync(clientList.ClientTypeToArray())).ArrayTypeToContact();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Address>> GetAddressData(int nbLine)
        {
            List<Address> result = new List<Address>();
            try
            {
                
                result = (await _channel.get_data_addressAsync(nbLine.ToString())).ArrayTypeToAddress();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Address>> GetAddressDataByCommandList(List<Command> commandList)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = (await _channel.get_data_address_by_command_listAsync(commandList.CommandTypeToArray())).ArrayTypeToAddress();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Address>> GetAddressDataByClientList(List<Client> clientList)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = (await _channel.get_data_address_by_client_listAsync(clientList.ClientTypeToArray())).ArrayTypeToAddress();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Command>> GetCommandClient(int id)
        {
            List<Command> result = new List<Command>();
            try
            {
                
                result = (await _channel.get_commands_clientAsync(id.ToString())).ArrayTypeToCommand();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Command>> GetQuoteCLient(int id)
        {
            List<Command> result = new List<Command>();
            try
            {                
                result = (await _channel.get_quotes_clientAsync(id.ToString())).ArrayTypeToCommand();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Client>> GetClientDataById(int id)
        {
            List<Client> result = new List<Client>();
            try
            {
                
                result = (await _channel.get_data_client_by_idAsync(id.ToString())).ArrayTypeToClient();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Contact>> GetContactDataById(int id)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                
                result = (await _channel.get_data_contact_by_idAsync(id.ToString())).ArrayTypeToContact();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Address>> GetAddressDataById(int id)
        {
            List<Address> result = new List<Address>();
            try
            {
                
                result = (await _channel.get_data_address_by_idAsync(id.ToString())).ArrayTypeToAddress();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }


        public async Task<List<Client>> searchClient(Client client, string filterOperator)
        {
            var formatListClientToArray = ServiceHelper.ClientTypeToFilterArray(client, filterOperator);
            List<Client> result = new List<Client>();
            try
            {
                
                result = (await _channel.get_filter_ClientAsync(formatListClientToArray)).ArrayTypeToClient();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }


        public async Task<List<Contact>> searchContact(Contact Contact, string filterOperator)
        {
            var formatListContactToArray = ServiceHelper.ContactTypeToFilterArray(Contact, filterOperator);
            List<Contact> result = new List<Contact>();
            try
            {
                
                result = (await _channel.get_filter_contactAsync(formatListContactToArray)).ArrayTypeToContact();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Address>> searchAddress(Address Address, string filterOperator)
        {
            var formatListAddressToArray = ServiceHelper.AddressTypeToFilterArray(Address, filterOperator);
            List<Address> result = new List<Address>();
            try
            {
                
                result = (await _channel.get_filter_addressAsync(formatListAddressToArray)).ArrayTypeToAddress();
            }
            catch (FaultException)
            {
                _channel.Close();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Client>> searchClientFromWebService(Client client, string filterOperator)
        {
            return await searchClient(client, filterOperator);
        }

        public async Task<List<Contact>> searchContactFromWebService(Contact Contact, string filterOperator)
        {
            return await searchContact(Contact, filterOperator);
        }

        public async Task<List<Address>> searchAddressFromWebService(Address Address, string filterOperator)
        {
            return await searchAddress(Address, filterOperator);
        }

        public void Dispose()
        {
            _channel.Close();
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> MoveClientAgentBySelection(List<Client> clientList, Agent toAgent)
        {
            throw new NotImplementedException();
        }
    } /* end class BlCLient */
}