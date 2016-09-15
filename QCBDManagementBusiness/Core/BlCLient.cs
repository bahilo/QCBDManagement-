using QCBDManagementBusiness.Helper;
using QCBDManagementCommon;
using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementBusiness.Core
{
    public class BlCLient : IClientManager
    {
        // Attributes

        public QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DAC { get; set; }
        public ISendEmail SendEmail { get; set; }

        public BlCLient(QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public async Task<List<Client>> InsertClient(List<Client> clientList)
        {
            if (clientList == null || clientList.Count == 0)
                return new List<Client>();

            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.InsertClient(clientList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Contact>> InsertContact(List<Contact> contactList)
        {
            if (contactList == null || contactList.Count == 0)
                return new List<Contact>();

            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.InsertContact(contactList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Address>> InsertAddress(List<Address> addressList)
        {
            if (addressList == null || addressList.Count == 0)
                return new List<Address>();

            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.InsertAddress(addressList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Client>> UpdateClient(List<Client> clientList)
        {
            if (clientList == null || clientList.Count == 0)
                return new List<Client>();

            if (clientList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating clients(count = " + clientList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.UpdateClient(clientList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Contact>> UpdateContact(List<Contact> contactList)
        {
            if (contactList == null || contactList.Count == 0)
                return new List<Contact>();

            if (contactList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating contacts(count = " + contactList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.UpdateContact(contactList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Address>> UpdateAddress(List<Address> addressList)
        {
            if (addressList == null || addressList.Count == 0)
                return new List<Address>();

            if (addressList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating addresses(count = " + addressList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.UpdateAddress(addressList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }


        public async Task<List<Client>> DeleteClient(List<Client> clientList)
        {
            if (clientList == null || clientList.Count == 0)
                return new List<Client>();

            if (clientList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting clients(count = " + clientList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.DeleteClient(clientList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Contact>> DeleteContact(List<Contact> contactList)
        {
            if (contactList == null || contactList.Count == 0)
                return new List<Contact>();

            if (contactList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting contacts(count = " + contactList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.DeleteContact(contactList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Address>> DeleteAddress(List<Address> addressList)
        {
            if (addressList == null || addressList.Count == 0)
                return new List<Address>();

            if (addressList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting addresses(count = " + addressList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.DeleteAddress(addressList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }


        public async Task<List<Client>> GetClientData(int nbLine)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.GetClientData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Client>> GetClientDataByBillList(List<Bill> billList)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.GetClientDataByBillList(billList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Contact>> GetContactData(int nbLine)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.GetContactData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Contact>> GetContactDataByClientList(List<Client> clientList)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.GetContactDataByClientList(clientList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Address>> GetAddressData(int nbLine)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.GetAddressData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Address>> GetAddressDataByCommandList(List<Command> commandList)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.GetAddressDataByCommandList(commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Address>> GetAddressDataByClientList(List<Client> clientList)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.GetAddressDataByClientList(clientList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }



        public async Task<List<Command>> GetQuoteCLient(int id)
        {
            List<Command> result = new List<Command>();
            try
            {
                var command = new Command();
                command.ClientId = id;
                command.Status = EStatus.Quote.ToString();
                result = await DAC.DALCommand.searchCommand(command, "AND");
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command>> GetCommandClient(int id)
        {
            List<Command> result = new List<Command>();
            try
            {
                var command = new Command();
                command.ClientId = id;
                command.Status = EStatus.Command.ToString();
                result = await DAC.DALCommand.searchCommand(command, "AND");
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Client>> GetClientDataById(int id)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.GetClientDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Contact>> GetContactDataById(int id)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.GetContactDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Address>> GetAddressDataById(int id)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.GetAddressDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }



        public async Task<List<Client>> searchClient(Client client, string filterOperator)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.searchClient(client, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Contact>> searchContact(Contact Contact, string filterOperator)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.searchContact(Contact, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Address>> searchAddress(Address Address, string filterOperator)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.searchAddress(Address, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Client>> searchClientFromWebService(Client client, string filterOperator)
        {
            List<Client> result = new List<Client>();
            try
            {
                result = await DAC.DALClient.searchClientFromWebService(client, filterOperator);
                var localSearchResultList = await searchClient(client, filterOperator);
                if(localSearchResultList.Count == 0)
                {
                    await UpdateClient(result);
                    var contactFoundList = await GetContactDataByClientList(result);
                    await UpdateContact(contactFoundList);
                    var addressFoundList = await GetAddressDataByClientList(result);
                    await UpdateAddress(addressFoundList);
                }
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Contact>> searchContactFromWebService(Contact Contact, string filterOperator)
        {
            List<Contact> result = new List<Contact>();
            try
            {
                result = await DAC.DALClient.searchContactFromWebService(Contact, filterOperator);                
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Address>> searchAddressFromWebService(Address Address, string filterOperator)
        {
            List<Address> result = new List<Address>();
            try
            {
                result = await DAC.DALClient.searchAddressFromWebService(Address, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }
        

        public async Task<List<Client>> MoveClientAgentBySelection(List<Client> clientList, Agent toAgent)
        {
            List<Client> clientListAfter = new List<Client>();
            Client inClient = new Client();
            try
            {
                foreach (var client in clientList)
                {
                    client.AgentId = toAgent.ID;
                }
                clientListAfter = await DAC.DALClient.UpdateClient(clientList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return clientListAfter;
        }
    }
}