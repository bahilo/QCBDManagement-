using Microsoft.Win32;
using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementCommon.Structures;
using QCBDManagementGateway.Classes;
using QCBDManagementGateway.Helper.ChannelHelper;
using QCBDManagementGateway.QCBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementGateway.Core
{
    public class GateWayCommand : ICommandManager, INotifyPropertyChanged
    {
        private QCBDManagementWebServicePortTypeClient _channel;
        private Agent _authenticatedUser;
        public event PropertyChangedEventHandler PropertyChanged;

        //private QCBDManagementWebServicePortTypeClient channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");

        public GateWayCommand()
        {
            initChannel();
        }

        private void initChannel()
        {
            _channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");
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
                _authenticatedUser = value;
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


        public async Task<List<Command>> InsertCommand(List<Command> listCommand)
        {
            var formatListCommandToArray = ServiceHelper.CommandTypeToArray(listCommand);
            List<Command> result = new List<Command>();
            try
            {
                result = (await _channel.insert_data_commandAsync(formatListCommandToArray)).ArrayTypeToCommand();
            }
            catch (FaultException)
            {
                Dispose();
                throw;

            }
            catch (CommunicationException )
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

        public async Task<List<Tax_command>> InsertTax_command(List<Tax_command> listTax_command)
        {
            var formatListTax_commandToArray = ServiceHelper.Tax_commandTypeToArray(listTax_command);
            List<Tax_command> result = new List<Tax_command>();
            try
            {

                result = (await _channel.insert_data_tax_commandAsync(formatListTax_commandToArray)).ArrayTypeToTax_command();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Command_item>> InsertCommand_item(List<Command_item> listCommand_item)
        {
            var formatListCommand_itemToArray = ServiceHelper.Command_itemTypeToArray(listCommand_item);
            List<Command_item> result = new List<Command_item>();
            try
            {

                result = (await _channel.insert_data_command_itemAsync(formatListCommand_itemToArray)).ArrayTypeToCommand_item();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Tax>> InsertTax(List<Tax> listTax)
        {
            var formatListTaxToArray = ServiceHelper.TaxTypeToArray(listTax);
            List<Tax> result = new List<Tax>();
            try
            {

                result = (await _channel.insert_data_taxAsync(formatListTaxToArray)).ArrayTypeToTax();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Bill>> InsertBill(List<Bill> listBill)
        {
            var formatListBillToArray = ServiceHelper.BillTypeToArray(listBill);
            List<Bill> result = new List<Bill>();
            try
            {

                result = (await _channel.insert_data_billAsync(formatListBillToArray)).ArrayTypeToBill();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Delivery>> InsertDelivery(List<Delivery> listDelivery)
        {
            var formatListDeliveryToArray = ServiceHelper.DeliveryTypeToArray(listDelivery);
            List<Delivery> result = new List<Delivery>();
            try
            {

                result = (await _channel.insert_data_deliveryAsync(formatListDeliveryToArray)).ArrayTypeToDelivery();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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


        public async Task<List<Command>> DeleteCommand(List<Command> listCommand)
        {
            var formatListCommandToArray = ServiceHelper.CommandTypeToArray(listCommand);
            List<Command> result = new List<Command>();
            try
            {

                result = (await _channel.delete_data_commandAsync(formatListCommandToArray)).ArrayTypeToCommand();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Tax_command>> DeleteTax_command(List<Tax_command> listTax_command)
        {
            var formatListTax_commandToArray = ServiceHelper.Tax_commandTypeToArray(listTax_command);
            List<Tax_command> result = new List<Tax_command>();
            try
            {

                result = (await _channel.delete_data_tax_commandAsync(formatListTax_commandToArray)).ArrayTypeToTax_command();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Command_item>> DeleteCommand_item(List<Command_item> listCommand_item)
        {
            var formatListCommand_itemToArray = ServiceHelper.Command_itemTypeToArray(listCommand_item);
            List<Command_item> result = new List<Command_item>();
            try
            {

                result = (await _channel.delete_data_command_itemAsync(formatListCommand_itemToArray)).ArrayTypeToCommand_item();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Tax>> DeleteTax(List<Tax> listTax)
        {
            var formatListTaxToArray = ServiceHelper.TaxTypeToArray(listTax);
            List<Tax> result = new List<Tax>();
            try
            {

                result = (await _channel.delete_data_taxAsync(formatListTaxToArray)).ArrayTypeToTax();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Bill>> DeleteBill(List<Bill> listBill)
        {
            var formatListBillToArray = ServiceHelper.BillTypeToArray(listBill);
            List<Bill> result = new List<Bill>();
            try
            {

                result = (await _channel.delete_data_billAsync(formatListBillToArray)).ArrayTypeToBill();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Delivery>> DeleteDelivery(List<Delivery> listDelivery)
        {
            var formatListDeliveryToArray = ServiceHelper.DeliveryTypeToArray(listDelivery);
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = (await _channel.delete_data_deliveryAsync(formatListDeliveryToArray)).ArrayTypeToDelivery();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Command>> UpdateCommand(List<Command> listCommand)
        {
            var formatListCommandToArray = ServiceHelper.CommandTypeToArray(listCommand);
            List<Command> result = new List<Command>();
            try
            {

                result = (await _channel.update_data_commandAsync(formatListCommandToArray)).ArrayTypeToCommand();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Tax_command>> UpdateTax_command(List<Tax_command> listTax_command)
        {
            var formatListTax_commandToArray = ServiceHelper.Tax_commandTypeToArray(listTax_command);
            List<Tax_command> result = new List<Tax_command>();
            try
            {

                result = (await _channel.update_data_tax_commandAsync(formatListTax_commandToArray)).ArrayTypeToTax_command();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Command_item>> UpdateCommand_item(List<Command_item> listCommand_item)
        {
            var formatListCommand_itemToArray = ServiceHelper.Command_itemTypeToArray(listCommand_item);
            List<Command_item> result = new List<Command_item>();
            try
            {

                result = (await _channel.update_data_command_itemAsync(formatListCommand_itemToArray)).ArrayTypeToCommand_item();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Tax>> UpdateTax(List<Tax> listTax)
        {
            var formatListTaxToArray = ServiceHelper.TaxTypeToArray(listTax);
            List<Tax> result = new List<Tax>();
            try
            {

                result = (await _channel.update_data_taxAsync(formatListTaxToArray)).ArrayTypeToTax();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Bill>> UpdateBill(List<Bill> listBill)
        {
            var formatListBillToArray = ServiceHelper.BillTypeToArray(listBill);
            List<Bill> result = new List<Bill>();
            try
            {

                result = (await _channel.update_data_billAsync(formatListBillToArray)).ArrayTypeToBill();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Delivery>> UpdateDelivery(List<Delivery> listDelivery)
        {
            var formatListDeliveryToArray = ServiceHelper.DeliveryTypeToArray(listDelivery);
            List<Delivery> result = new List<Delivery>();
            try
            {

                result = (await _channel.update_data_deliveryAsync(formatListDeliveryToArray)).ArrayTypeToDelivery();
            }
            catch (FaultException )
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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


        public async Task<List<Command>> GetCommandData(int nbLine)
        {
            List<Command> result = new List<Command>();
            try
            {
                result = (await _channel.get_data_commandAsync(nbLine.ToString())).ArrayTypeToCommand();

            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Command>> GetCommandDataById(int id)
        {
            List<Command> result = new List<Command>();
            try
            {

                result = (await _channel.get_data_command_by_idAsync(id.ToString())).ArrayTypeToCommand();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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


        public async Task<List<Tax_command>> GetTax_commandData(int nbLine)
        {
            List<Tax_command> result = new List<Tax_command>();
            try
            {
                result = (await _channel.get_data_tax_commandAsync(nbLine.ToString())).ArrayTypeToTax_command();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Tax_command>> GetTax_commandDataByCommandList(List<Command> commandList)
        {
            List<Tax_command> result = new List<Tax_command>();
            try
            {
                result = (await _channel.get_data_tax_command_by_command_listAsync(commandList.CommandTypeToArray())).ArrayTypeToTax_command();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Tax_command>> GetTax_commandDataById(int id)
        {
            List<Tax_command> result = new List<Tax_command>();
            try
            {
                result = (await _channel.get_data_tax_command_by_idAsync(id.ToString())).ArrayTypeToTax_command();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Command_item>> GetCommand_itemData(int nbLine)
        {
            List<Command_item> result = new List<Command_item>();
            try
            {
                result = (await _channel.get_data_command_itemAsync(nbLine.ToString())).ArrayTypeToCommand_item();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Command_item>> GetCommand_itemDataById(int id)
        {
            List<Command_item> result = new List<Command_item>();
            try
            {
                result = (await _channel.get_data_command_item_by_idAsync(id.ToString())).ArrayTypeToCommand_item();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Command_item>> GetCommand_itemByCommandList(List<Command> listCommand)
        {
            var formatListCommandToArray = ServiceHelper.CommandTypeToArray(listCommand);
            List<Command_item> result = new List<Command_item>();
            try
            {
                result = (await _channel.get_data_command_item_by_command_listAsync(formatListCommandToArray)).ArrayTypeToCommand_item();
                var test = result.Where(x => x.CommandId == 3410).ToList();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Tax>> GetTaxData(int nbLine)
        {
            List<Tax> result = new List<Tax>();
            try
            {

                result = (await _channel.get_data_taxAsync(nbLine.ToString())).ArrayTypeToTax();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Tax>> GetTaxDataById(int id)
        {
            List<Tax> result = new List<Tax>();
            try
            {

                result = (await _channel.get_data_tax_by_idAsync(id.ToString())).ArrayTypeToTax();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Bill>> GetBillData(int nbLine)
        {
            List<Bill> result = new List<Bill>();
            try
            {

                result = (await _channel.get_data_billAsync(nbLine.ToString())).ArrayTypeToBill();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Bill>> GetBillDataByCommandList(List<Command> commandList)
        {
            List<Bill> result = new List<Bill>();
            try
            {

                result = (await _channel.get_data_bill_by_command_listAsync(commandList.CommandTypeToArray())).ArrayTypeToBill();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Bill>> GetBillDataById(int id)
        {
            List<Bill> result = new List<Bill>();
            try
            {

                result = (await _channel.get_data_bill_by_idAsync(id.ToString())).ArrayTypeToBill();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Delivery>> GetDeliveryData(int nbLine)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {

                result = (await _channel.get_data_deliveryAsync(nbLine.ToString())).ArrayTypeToDelivery();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Delivery>> GetDeliveryDataByCommandList(List<Command> commandList)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = (await _channel.get_data_delivery_by_command_listAsync(commandList.CommandTypeToArray())).ArrayTypeToDelivery();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Delivery>> GetDeliveryDataById(int id)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {

                result = (await _channel.get_data_delivery_by_idAsync(id.ToString())).ArrayTypeToDelivery();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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


        public async Task<List<Command>> searchCommand(Command command, string filterOperator)
        {
            var formatListCommandToArray = ServiceHelper.CommandTypeToFilterArray(command, filterOperator);
            List<Command> result = new List<Command>();
            try
            {

                result = (await _channel.get_filter_commandAsync(formatListCommandToArray)).ArrayTypeToCommand();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
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

        public async Task<List<Tax_command>> searchTax_command(Tax_command Tax_command, string filterOperator)
        {
            var formatListTax_commandToArray = ServiceHelper.Tax_commandTypeToFilterArray(Tax_command, filterOperator);
            List<Tax_command> result = new List<Tax_command>();
            try
            {

                result = (await _channel.get_filter_tax_commandAsync(formatListTax_commandToArray)).ArrayTypeToTax_command();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Command_item>> searchCommand_item(Command_item Command_item, string filterOperator)
        {
            var formatListCommand_itemToArray = ServiceHelper.Command_itemTypeToFilterArray(Command_item, filterOperator);
            List<Command_item> result = new List<Command_item>();
            try
            {

                result = (await _channel.get_filter_command_itemAsync(formatListCommand_itemToArray)).ArrayTypeToCommand_item();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Tax>> searchTax(Tax Tax, string filterOperator)
        {
            var formatListTaxToArray = ServiceHelper.TaxTypeToFilterArray(Tax, filterOperator);
            List<Tax> result = new List<Tax>();
            try
            {

                result = (await _channel.get_filter_taxAsync(formatListTaxToArray)).ArrayTypeToTax();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Bill>> searchBill(Bill Bill, string filterOperator)
        {
            var formatListBillToArray = ServiceHelper.BillTypeToFilterArray(Bill, filterOperator);
            List<Bill> result = new List<Bill>();
            try
            {

                result = (await _channel.get_filter_billAsync(formatListBillToArray)).ArrayTypeToBill();
            }
            catch (FaultException )
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException )
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Delivery>> searchDelivery(Delivery Delivery, string filterOperator)
        {
            var formatListDeliveryToArray = ServiceHelper.DeliveryTypeToFilterArray(Delivery, filterOperator);
            List<Delivery> result = new List<Delivery>();
            try
            {

                result = (await _channel.get_filter_deliveryAsync(formatListDeliveryToArray)).ArrayTypeToDelivery();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException )
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public async Task<List<Command>> searchCommandFromWebService(Command command, string filterOperator)
        {
            return await searchCommand(command, filterOperator);
        }

        public async Task<List<Command_item>> searchCommand_itemFromWebService(Command_item Command_item, string filterOperator)
        {
            return await searchCommand_item(Command_item, filterOperator);
        }

        public async Task<List<Bill>> searchBillFromWebService(Bill Bill, string filterOperator)
        {
            return await searchBill(Bill, filterOperator);
        }

        public async Task<List<Delivery>> searchDeliveryFromWebService(Delivery Delivery, string filterOperator)
        {
            return await searchDelivery(Delivery, filterOperator);
        }

        public void GeneratePdfDelivery(ParamDeliveryToPdf paramDeliveryToPdf)
        {
            WebClient client = new WebClient();
            try
            {
                client.Credentials = new NetworkCredential(_channel.ClientCredentials.UserName.UserName, _channel.ClientCredentials.UserName.Password);
                var uri = "http://qobdmanagement-001-site1.htempurl.com/fpdf/" + paramDeliveryToPdf.Lang+"/BL_Codsimex.php?num_dev=" + paramDeliveryToPdf.CommandId + " &num_bl=" + paramDeliveryToPdf.DeliveryId;

                System.Diagnostics.Process.Start(uri);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            finally
            {
                client.Dispose();
            }
        }

        public void GeneratePdfCommand(ParamCommandToPdf paramCommandToPdf)
        {
            WebClient client = new WebClient();
            try
            {
                client.Credentials = new NetworkCredential(_channel.ClientCredentials.UserName.UserName, _channel.ClientCredentials.UserName.Password);
                //string uri = "http://qobdmanagement-001-site1.htempurl.com/WebServiceSOAP/fpdf/" + paramCommandToPdf.Lang + "/Facture_Codsimex.php?";
                string uri = "http://qobdmanagement-001-site1.htempurl.com/fpdf/" + paramCommandToPdf.Lang + "/Facture_Codsimex.php?";
                uri += "num_dev=" + paramCommandToPdf.CommandId;
                uri += "&num_fact=" + paramCommandToPdf.BillId;
                uri += "&currency=" + paramCommandToPdf.Currency;
                uri += "&lang=" + paramCommandToPdf.Lang;

                if (paramCommandToPdf.ParamEmail.IsSendEmail)
                {
                    uri += "&relance=" + paramCommandToPdf.ParamEmail.Reminder;
                    uri += "&mail=1";
                    uri += "&subject=" + paramCommandToPdf.ParamEmail.Subject;
                }

                if (paramCommandToPdf.IsQuoteConstructorReferencesVisible)
                    uri += "&refv=" + paramCommandToPdf.IsQuoteConstructorReferencesVisible;

                System.Diagnostics.Process.Start(uri);
            }
            finally
            {
                client.Dispose();
            }
        }

        public void GeneratePdfQuote(ParamCommandToPdf paramCommandToPdf)
        {
            WebClient client = new WebClient();
            try
            {
                client.Credentials = new NetworkCredential(_channel.ClientCredentials.UserName.UserName, _channel.ClientCredentials.UserName.Password);
                string uri = "http://qobdmanagement-001-site1.htempurl.com/fpdf/" + paramCommandToPdf.Lang + "/Devis_Codsimex.php?";

                uri += "num_dev=" + paramCommandToPdf.CommandId;
                uri += "&delay=" + paramCommandToPdf.ValidityDay;
                uri += "&quote=" + paramCommandToPdf.TypeQuoteOrProformat.ToString();
                uri += "&currency=" + paramCommandToPdf.Currency;
                uri += "&lang=" + paramCommandToPdf.Lang;

                if (paramCommandToPdf.ParamEmail.IsSendEmail)
                {
                    uri += "&mail=1";
                    uri += "&subject=" + paramCommandToPdf.ParamEmail.Subject;
                }

                if (paramCommandToPdf.IsQuoteConstructorReferencesVisible)
                    uri += "&refv=" + paramCommandToPdf.IsQuoteConstructorReferencesVisible;

                System.Diagnostics.Process.Start(uri);
            }
            finally
            {
                client.Dispose();
            }
        }
        

        public void Dispose()
        {
            //Debug.WriteLine("Fault Mode detected!");
            _channel.Close();
            //initChannel();
            //setServiceCredential(_authenticatedUser.Login, _authenticatedUser.HashedPassword);
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            throw new NotImplementedException();
        }

        public async Task<Bill> GetLastBill()
        {
            List<Bill> result = new List<Bill>();
            result = await GetBillData(1);
            if (result.Count > 0)
                return result[0];

            return null;
        }

        public void UpdateCommandDependencies(List<Command> listCommand, bool activeProgress = false)
        {
            throw new NotImplementedException();
        }
    } /* end class BLCommande */
}