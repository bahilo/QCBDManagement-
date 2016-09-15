using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementDAL.App_Data;
using QCBDManagementDAL.App_Data.QCBDDataSetTableAdapters;
using QCBDManagementDAL.Helper.ChannelHelper;
using QCBDManagementGateway.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ComponentModel;
using QCBDManagementCommon.Structures;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Threading;
using System.Collections.Concurrent;
using QCBDManagementCommon.Enum;
using System.Threading.Tasks.Dataflow;
using QCBDManagementCommon.Classes;

namespace QCBDManagementDAL.Core
{
    public class DALCommand : ICommandManager
    {
        private Func<double, double> _progressBarFunc;
        public Agent AuthenticatedUser { get; set; }
        private GateWayCommand _gateWayCommand;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();

        public DALCommand()
        {
            _gateWayCommand = new GateWayCommand();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
            _gateWayCommand.PropertyChanged += onCredentialChange_loadCommandDataFromWebService;
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; }
        }

        public GateWayCommand GateWayCommand
        {
            get { return _gateWayCommand; }
        }

        private void onCredentialChange_loadCommandDataFromWebService(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Credential"))
            {
                DALHelper.doActionAsync(retrieveGateWayDataCommand);
                //retrieveGateWayDataCommand();
            }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
                _gateWayCommand.initializeCredential(AuthenticatedUser);
            }
        }

        /*private async void retrieveGateWayDataBill()
        {
            await UpdateBill(await _gateWayCommand.GetBillData(_loadSize));
            _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
        }

        private async void retrieveGateWayDataDelivery()
        {
            await UpdateDelivery(await _gateWayCommand.GetDeliveryData(_loadSize));
            _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
        }

        private async void retrieveGateWayDataTax_command()
        {
            await UpdateTax_command(await _gateWayCommand.GetTax_commandData(_loadSize));
            _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
        }

        private async void retrieveGateWayDataTax()
        {
            await UpdateTax(await _gateWayCommand.GetTaxData(_loadSize));
            _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
        }

        private async void retrieveGateWayDataCommand()
        {
            await UpdateCommand(await _gateWayCommand.GetCommandData(_loadSize));
            _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
        }*/

        private void retrieveGateWayDataCommand()
        {
            UpdateCommandDependencies(new NotifyTaskCompletion<List<Command>>(_gateWayCommand.searchCommand(new Command { AgentId = AuthenticatedUser.ID }, "AND")).Task.Result.Take(_loadSize).ToList(), true);

            Log.write("Command loaded!", "TES");
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _progressBarFunc = progressBarFunc;
        }


        public async Task<List<Command>> InsertCommand(List<Command> listCommand)
        {
            List<Command> result = new List<Command>();
            List<Command> gateWayResultList = new List<Command>();
            using (commandsTableAdapter _commandsTableAdapter = new commandsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.InsertCommand(listCommand) : listCommand;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateCommand(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Tax_command>> InsertTax_command(List<Tax_command> listTax_command)
        {
            List<Tax_command> result = new List<Tax_command>();
            List<Tax_command> gateWayResultList = new List<Tax_command>();
            using (tax_commandsTableAdapter _tax_commandsTableAdapter = new tax_commandsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.InsertTax_command(listTax_command) : listTax_command;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateTax_command(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Command_item>> InsertCommand_item(List<Command_item> listCommand_item)
        {
            List<Command_item> result = new List<Command_item>();
            List<Command_item> gateWayResultList = new List<Command_item>();
            using (command_itemsTableAdapter _command_itemsTableAdapter = new command_itemsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.InsertCommand_item(listCommand_item) : listCommand_item;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateCommand_item(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Tax>> InsertTax(List<Tax> listTax)
        {
            List<Tax> result = new List<Tax>();
            List<Tax> gateWayResultList = new List<Tax>();
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.InsertTax(listTax) : listTax;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateTax(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Bill>> InsertBill(List<Bill> listBill)
        {
            List<Bill> result = new List<Bill>();
            List<Bill> gateWayResultList = new List<Bill>();
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.InsertBill(listBill) : listBill;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateBill(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Delivery>> InsertDelivery(List<Delivery> listDelivery)
        {
            List<Delivery> result = new List<Delivery>();
            List<Delivery> gateWayResultList = new List<Delivery>();
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.InsertDelivery(listDelivery) : listDelivery;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateDelivery(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }


        public async Task<List<Command>> DeleteCommand(List<Command> listCommand)
        {
            List<Command> result = new List<Command>();
            List<Command> gateWayResultList = new List<Command>();
            using (commandsTableAdapter _commandsTableAdapter = new commandsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.DeleteCommand(listCommand) : listCommand;
                if (gateWayResultList.Count == 0)
                    foreach (Command command in listCommand)
                    {
                        int returnValue = _commandsTableAdapter.delete_data_command(command.ID);
                        if (returnValue > 0)
                            result.Add(command);
                    }
            }
            return result;
        }

        public async Task<List<Tax_command>> DeleteTax_command(List<Tax_command> listTax_command)
        {
            List<Tax_command> result = listTax_command;
            List<Tax_command> gateWayResultList = new List<Tax_command>();
            using (tax_commandsTableAdapter _tax_commandsTableAdapter = new tax_commandsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.DeleteTax_command(listTax_command) : listTax_command;
                if (gateWayResultList.Count == 0)
                    foreach (Tax_command tax_command in listTax_command)
                    {
                        int returnValue = _tax_commandsTableAdapter.delete_data_tax_command(tax_command.ID);
                        if(returnValue > 0)
                            result.Remove(tax_command);
                    }
            }
            return result;
        }

        public async Task<List<Command_item>> DeleteCommand_item(List<Command_item> listCommand_item)
        {
            List<Command_item> result = listCommand_item;
            List<Command_item> gateWayResultList = new List<Command_item>();
            using (command_itemsTableAdapter _command_itemsTableAdapter = new command_itemsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.DeleteCommand_item(listCommand_item) : listCommand_item;
                if (gateWayResultList.Count == 0)
                    foreach (Command_item command_item in listCommand_item)
                    {
                        int returnValue = _command_itemsTableAdapter.delete_data_command_item(command_item.ID);
                        if (returnValue > 0)
                            result.Remove(command_item);
                    }
            }
            return result;
        }

        public async Task<List<Tax>> DeleteTax(List<Tax> listTax)
        {
            List<Tax> result = listTax;
            List<Tax> gateWayResultList = new List<Tax>();
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.DeleteTax(listTax) : listTax;
                if (gateWayResultList.Count == 0)
                    foreach (Tax tax in listTax)
                    {
                        int returnValue = _taxesTableAdapter.delete_data_tax(tax.ID);
                        if (returnValue > 0)
                            result.Remove(tax);
                    }
            }
            return result;
        }

        public async Task<List<Bill>> DeleteBill(List<Bill> listBill)
        {
            List<Bill> result = listBill;
            List<Bill> gateWayResultList = new List<Bill>();
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.DeleteBill(listBill) : listBill;
                if (gateWayResultList.Count == 0)
                    foreach (Bill bill in listBill)
                    {
                        int returnValue = _billsTableAdapter.delete_data_bill(bill.ID);
                        if (returnValue > 0)
                            result.Remove(bill);
                    }
            }
            return result;
        }

        public async Task<List<Delivery>> DeleteDelivery(List<Delivery> listDelivery)
        {
            List<Delivery> result = listDelivery;
            List<Delivery> gateWayResultList = new List<Delivery>();
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.DeleteDelivery(listDelivery) : listDelivery;
                if (gateWayResultList.Count == 0)
                    foreach (Delivery delivery in listDelivery)
                    {
                        int returnValue = _deliveriesTableAdapter.delete_data_delivery(delivery.ID);
                        if (returnValue > 0)
                            result.Remove(delivery);
                    }
            }
            return result;
        }

        public async Task<List<Command>> UpdateCommand(List<Command> listCommand)
        {
            List<Command> result = new List<Command>();
            List<Command> gateWayResultList = new List<Command>();
            using (commandsTableAdapter _commandsTableAdapter = new commandsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.UpdateCommand(listCommand) : listCommand;
                foreach (var command in gateWayResultList)
                {
                    int returnResult = _commandsTableAdapter
                                            .update_data_command(
                                                    command.AgentId,
                                                    command.ClientId,
                                                    command.Comment1,
                                                    command.Comment2,
                                                    command.Comment3,
                                                    command.BillAddress,
                                                    command.DeliveryAddress,
                                                    command.Status,
                                                    command.Date,
                                                    command.Tax,
                                                    command.ID);
                        if (returnResult > 0)
                        result.Add(command);
                }
            }
            return result;
        }

        public async Task<List<Tax_command>> UpdateTax_command(List<Tax_command> listTax_command)
        {
            List<Tax_command> result = new List<Tax_command>();
            List<Tax_command> gateWayResultList = new List<Tax_command>();
            using (tax_commandsTableAdapter _tax_commandsTableAdapter = new tax_commandsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.UpdateTax_command(listTax_command) : listTax_command;
                foreach (var tax_command in gateWayResultList)
                {
                    int returnResult = _tax_commandsTableAdapter
                                            .update_data_tax_command(
                                                    tax_command.CommandId,
                                                    tax_command.TaxId,
                                                    tax_command.Date_insert,
                                                    tax_command.Tax_value.ToString(),
                                                    tax_command.Target,
                                                    tax_command.ID);
                        if (returnResult > 0)
                        result.Add(tax_command);
                }
            }
            return result;
        }

        public async Task<List<Command_item>> UpdateCommand_item(List<Command_item> listCommand_item)
        {
            List<Command_item> result = new List<Command_item>();
            List<Command_item> gateWayResultList = new List<Command_item>();
            using (command_itemsTableAdapter _command_itemsTableAdapter = new command_itemsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.UpdateCommand_item(listCommand_item) : listCommand_item;
                foreach (var command_item in gateWayResultList)
                {
                    int returnResult = _command_itemsTableAdapter
                                            .update_command_item(
                                                    command_item.CommandId,
                                                    command_item.ItemId,
                                                    command_item.Item_ref,
                                                    command_item.Quantity,
                                                    command_item.Quantity_delivery,
                                                    command_item.Quantity_current,
                                                    command_item.Comment_Purchase_Price,
                                                    command_item.Price,
                                                    command_item.Price_purchase,
                                                    command_item.Order,
                                                    command_item.ID);
                    if (returnResult > 0)
                        result.Add(command_item);
                }
            }
            return result;
        }

        public async Task<List<Tax>> UpdateTax(List<Tax> listTax)
        {
            List<Tax> result = new List<Tax>();
            List<Tax> gateWayResultList = new List<Tax>();
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.UpdateTax(listTax) : listTax;
                foreach (var tax in gateWayResultList)
                {
                    int returnResult = _taxesTableAdapter
                                            .update_data_tax(
                                                tax.Type,
                                                tax.Date_insert,
                                                tax.Value.ToString(),
                                                tax.Comment,
                                                tax.Tax_current,
                                                tax.ID);
                    if (returnResult > 0)
                        result.Add(tax);
                }
            }
            return result;
        }

        public async Task<List<Bill>> UpdateBill(List<Bill> listBill)
        {
            List<Bill> result = new List<Bill>();
            List<Bill> gateWayResultList = new List<Bill>();
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.UpdateBill(listBill) : listBill;
                foreach (var bill in gateWayResultList)
                {
                    int returnResult = _billsTableAdapter
                                            .update_data_bill(
                                                bill.ClientId,
                                                bill.CommandId,
                                                bill.PayMod,
                                                bill.Pay,
                                                bill.PayReceived,
                                                bill.Comment1,
                                                bill.Comment2,
                                                bill.Date,
                                                bill.DateLimit,
                                                bill.PayDate,
                                                bill.ID);
                    if (returnResult > 0)
                        result.Add(bill);
                }
            }
            return result;
        }

        public async Task<List<Delivery>> UpdateDelivery(List<Delivery> listDelivery)
        {
            List<Delivery> result = new List<Delivery>();
            List<Delivery> gateWayResultList = new List<Delivery>();
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayCommand.UpdateDelivery(listDelivery) : listDelivery;
                foreach (var delivery in gateWayResultList)
                {
                    int returnResult = _deliveriesTableAdapter
                                            .update_data_delivery(
                                                delivery.CommandId,
                                                delivery.BillId,
                                                delivery.Package,
                                                delivery.Date,
                                                delivery.Status,
                                                delivery.ID);
                    if (returnResult > 0)
                        result.Add(delivery);
                }
            }
            return result;
        }


        public async Task<List<Command>> GetCommandData(int nbLine)
        {
            List<Command> result = new List<Command>();
            using (commandsTableAdapter _commandsTableAdapter = new commandsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.commandsDataTable>(_commandsTableAdapter.get_data_command)).DataTableTypeToCommand();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Command>> GetCommandDataById(int id)
        {
            using (commandsTableAdapter _commandsTableAdapter = new commandsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.commandsDataTable>(_commandsTableAdapter.get_data_command_by_id, id)).DataTableTypeToCommand();
        }


        public async Task<List<Tax_command>> GetTax_commandData(int nbLine)
        {
            List<Tax_command> result = new List<Tax_command>();
            using (tax_commandsTableAdapter _tax_commandsTableAdapter = new tax_commandsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.tax_commandsDataTable>(_tax_commandsTableAdapter.get_data_tax_command)).DataTableTypeToTax_command();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Tax_command>> GetTax_commandDataByCommandList(List<Command> commandList)
        {
            List<Tax_command> result = new List<Tax_command>();
            foreach (Command command in commandList)
            {
                var tax_commandFoundList = await GetTax_commandByCommandId(command.ID);
                if (tax_commandFoundList.Count() > 0)
                    result.Add(tax_commandFoundList.First());
            }

            return result;
        }

        public async Task<List<Tax_command>> GetTax_commandByCommandId(int commandId)
        {
            using (tax_commandsTableAdapter _tax_commandsTableAdapter = new tax_commandsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.tax_commandsDataTable>(_tax_commandsTableAdapter.get_data_tax_command_by_id, commandId)).DataTableTypeToTax_command();
        }

        public async Task<List<Tax_command>> GetTax_commandDataById(int id)
        {
            using (tax_commandsTableAdapter _tax_commandsTableAdapter = new tax_commandsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.tax_commandsDataTable>(_tax_commandsTableAdapter.get_data_tax_command_by_id, id)).DataTableTypeToTax_command();
        }

        public async Task<List<Command_item>> GetCommand_itemData(int nbLine)
        {
            List<Command_item> result = new List<Command_item>();
            using (command_itemsTableAdapter _command_itemsTableAdapter = new command_itemsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.command_itemsDataTable>(_command_itemsTableAdapter.get_data_command_item)).DataTableTypeToCommand_item();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Command_item>> GetCommand_itemByCommandList(List<Command> listCommand)
        {
            List<Command_item> result = new List<Command_item>();
            foreach (Command command in listCommand)
            {
                var command_itemFoundList = await GetCommand_itemDataByCommandId(command.ID);
                if (command_itemFoundList.Count() > 0)
                    result.Add(command_itemFoundList.First());
            }

            return result;
        }

        public async Task<List<Command_item>> GetCommand_itemDataById(int id)
        {
            using (command_itemsTableAdapter _command_itemsTableAdapter = new command_itemsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.command_itemsDataTable>(_command_itemsTableAdapter.get_data_command_item_by_id, id)).DataTableTypeToCommand_item();
        }

        public async Task<List<Command_item>> GetCommand_itemDataByCommandId(int commandId)
        {
            using (command_itemsTableAdapter _command_itemsTableAdapter = new command_itemsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.command_itemsDataTable>(_command_itemsTableAdapter.get_filter_command_item_by_command_id, commandId)).DataTableTypeToCommand_item();
        }

        public async Task<List<Tax>> GetTaxData(int nbLine)
        {
            List<Tax> result = new List<Tax>();
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.taxesDataTable>(_taxesTableAdapter.get_data_tax)).DataTableTypeToTax();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Tax>> GetTaxDataById(int id)
        {
            using (taxesTableAdapter _taxesTableAdapter = new taxesTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.taxesDataTable>(_taxesTableAdapter.get_data_tax_by_id, id)).DataTableTypeToTax();
        }

        public async Task<List<Bill>> GetBillData(int nbLine)
        {
            List<Bill> result = new List<Bill>();
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.billsDataTable>(_billsTableAdapter.get_data_bill)).DataTableTypeToBill();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Bill>> GetBillDataByCommandList(List<Command> commandList)
        {
            List<Bill> result = new List<Bill>();
            foreach (Command command in commandList)
            {
                var billFoundList = await GetBillDataByCommandId(command.ID);
                if (billFoundList.Count() > 0)
                    result.Add(billFoundList.First());
            }
            return result;
        }

        public async Task<List<Bill>> GetBillDataByCommandId(int commandId)
        {
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.billsDataTable>(_billsTableAdapter.get_filter_bill_by_command_id, commandId)).DataTableTypeToBill();
        }

        public async Task<List<Bill>> GetBillDataById(int id)
        {
            using (billsTableAdapter _billsTableAdapter = new billsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.billsDataTable>(_billsTableAdapter.get_data_bill_by_id, id)).DataTableTypeToBill();
        }

        public async Task<Bill> GetLastBill()
        {
            using (GateWayCommand _commandGateWay = new GateWayCommand())
            {
                _commandGateWay.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                return await _commandGateWay.GetLastBill();
            }

            //return await Task.Factory.StartNew(() => { return DALHelper.LastBill(); });
        }

        public async Task<List<Delivery>> GetDeliveryData(int nbLine)
        {
            List<Delivery> result = new List<Delivery>();
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.deliveriesDataTable>(_deliveriesTableAdapter.get_data_delivery)).DataTableTypeToDelivery();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Delivery>> GetDeliveryDataByCommandList(List<Command> commandList)
        {
            List<Delivery> result = new List<Delivery>();
            foreach (Command command in commandList)
            {
                var deliveryFoundList = await GetDeliveryDataByCommandId(command.ID);
                if (deliveryFoundList.Count() > 0)
                    result.Add(deliveryFoundList.First());
            }
            return result;
        }

        public async Task<List<Delivery>> GetDeliveryDataByCommandId(int commandId)
        {
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.deliveriesDataTable>(_deliveriesTableAdapter.get_filter_delivery_by_command_id, commandId)).DataTableTypeToDelivery();
        }

        public async Task<List<Delivery>> GetDeliveryDataById(int id)
        {
            using (deliveriesTableAdapter _deliveriesTableAdapter = new deliveriesTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.deliveriesDataTable>(_deliveriesTableAdapter.get_data_delivery_by_id, id)).DataTableTypeToDelivery();
        }

        public async Task<List<Command>> searchCommand(Command command, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return command.CommandTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Tax_command>> searchTax_command(Tax_command Tax_command, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Tax_command.Tax_commandTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Command_item>> searchCommand_item(Command_item Command_item, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Command_item.Command_itemTypeToFilterDataTable(filterOperator); });

        }

        public async Task<List<Tax>> searchTax(Tax Tax, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Tax.TaxTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Bill>> searchBill(Bill Bill, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Bill.BillTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Delivery>> searchDelivery(Delivery Delivery, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Delivery.DeliveryTypeToFilterDataTable(filterOperator); });
        }

        public void GeneratePdfCommand(ParamCommandToPdf paramCommandToPdf)
        {
            using (GateWayCommand gateWayCommand = new GateWayCommand())
                gateWayCommand.GeneratePdfCommand(paramCommandToPdf);
        }

        public void GeneratePdfQuote(ParamCommandToPdf paramCommandToPdf)
        {
            using (GateWayCommand gateWayCommand = new GateWayCommand())
                gateWayCommand.GeneratePdfQuote(paramCommandToPdf);
        }

        public void GeneratePdfDelivery(ParamDeliveryToPdf paramDeliveryToPdf)
        {
            using (GateWayCommand gateWayCommand = new GateWayCommand())
                gateWayCommand.GeneratePdfDelivery(paramDeliveryToPdf);
        }

        public void Dispose()
        {
            /*DALHelper.emptyTable<commandsTableAdapter>();
            DALHelper.emptyTable<command_itemsTableAdapter>();
            DALHelper.emptyTable<tax_commandsTableAdapter>();
            DALHelper.emptyTable<deliveriesTableAdapter>();
            DALHelper.emptyTable<billsTableAdapter>();
            DALHelper.emptyTable<taxesTableAdapter>();*/
            //_commandsTableAdapter.Dispose();
        }

        public async Task<List<Command>> searchCommandFromWebService(Command command, string filterOperator)
        {
            List<Command> gateWayResultList = new List<Command>();
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayCommand.searchCommandFromWebService(command, filterOperator);
            }
            return gateWayResultList;
        }

        public async Task<List<Command_item>> searchCommand_itemFromWebService(Command_item Command_item, string filterOperator)
        {
            List<Command_item> gateWayResultList = new List<Command_item>();
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayCommand.searchCommand_itemFromWebService(Command_item, filterOperator);
            }
            return gateWayResultList;
        }

        public async Task<List<Bill>> searchBillFromWebService(Bill Bill, string filterOperator)
        {
            List<Bill> gateWayResultList = new List<Bill>();
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayCommand.searchBillFromWebService(Bill, filterOperator);

            }
            return gateWayResultList;
        }

        public async Task<List<Delivery>> searchDeliveryFromWebService(Delivery Delivery, string filterOperator)
        {
            List<Delivery> gateWayResultList = new List<Delivery>();
            using (GateWayCommand gateWayCommand = new GateWayCommand())
            {
                gateWayCommand.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayCommand.searchDeliveryFromWebService(Delivery, filterOperator);

            }
            return gateWayResultList;
        }


        //----------------------------------------------------------------------------------------//
        //----------------------------------------------------------------------------------------//

        public void UpdateCommandDependencies(List<Command> commands, bool isActiveProgress = false)
        {
            int loadUnit = 25;
            lock (_lock) _isLodingDataFromWebServiceToLocal = true;

            ConcurrentBag<Command> commandList;
            ConcurrentBag<Command_item> command_itemList = new ConcurrentBag<Command_item>();
            ConcurrentBag<Item> itemList = new ConcurrentBag<Item>();
            ConcurrentBag<Provider_item> provider_itemList = new ConcurrentBag<Provider_item>();
            ConcurrentBag<Provider> providerList = new ConcurrentBag<Provider>();
            ConcurrentBag<Item_delivery> item_deliveryList = new ConcurrentBag<Item_delivery>();
            ConcurrentBag<Delivery> deliveryList = new ConcurrentBag<Delivery>();
            ConcurrentBag<Tax_item> tax_itemList = new ConcurrentBag<Tax_item>();
            ConcurrentBag<Tax> taxList = new ConcurrentBag<Tax>();
            ConcurrentBag<Bill> billList = new ConcurrentBag<Bill>();
            ConcurrentBag<Client> clientList = new ConcurrentBag<Client>();
            ConcurrentBag<Contact> contactList = new ConcurrentBag<Contact>();
            ConcurrentBag<Address> addressList = new ConcurrentBag<Address>();
            ConcurrentBag<Tax_command> tax_commandList = new ConcurrentBag<Tax_command>();

            DALItem dalItem = new DALItem();
            dalItem.AuthenticatedUser = AuthenticatedUser;
            dalItem.GateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            dalItem.IsLodingDataFromWebServiceToLocal = true;

            DALClient dalClient = new DALClient();
            dalClient.AuthenticatedUser = AuthenticatedUser;
            dalClient.GateWayClient.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
            dalClient.IsLodingDataFromWebServiceToLocal = true;
                        
            commandList = new ConcurrentBag<Command>(commands);
              
            // Address loading
            if (commandList.Count > 0)
            {
                var addressfoundList = new NotifyTaskCompletion<List<Address>>(dalClient.GateWayClient.GetAddressDataByCommandList(commandList.ToList())).Task.Result;
                addressList = new ConcurrentBag<Address>(addressList.Concat(new ConcurrentBag<Address>(addressfoundList)));
                var savedAddressList = new NotifyTaskCompletion<List<Address>>(dalClient.UpdateAddress(addressList.ToList())).Task.Result;
            }
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);

            // Tax_command Loading
            if (commandList.Count > 0)
            {
                var tax_commandfoundList = new NotifyTaskCompletion<List<Tax_command>>(GateWayCommand.GetTax_commandDataByCommandList(commandList.ToList())).Task.Result; // await GateWayCommand.GetTax_commandDataByCommandList(new List<Command>(commandList.Skip(i * loadUnit).Take(loadUnit)));
                tax_commandList = new ConcurrentBag<Tax_command>(tax_commandList.Concat(new ConcurrentBag<Tax_command>(tax_commandfoundList)));
                List<Tax_command> savedTax_commandList = new NotifyTaskCompletion<List<Tax_command>>(UpdateTax_command(tax_commandList.ToList())).Task.Result;
            }

            // Tax Loading
            var taxFoundList = new NotifyTaskCompletion<List<Tax>>(GateWayCommand.GetTaxData(999)).Task.Result; // await GateWayCommand.GetTax_commandDataByCommandList(new List<Command>(commandList.Skip(i * loadUnit).Take(loadUnit)));
            taxList = new ConcurrentBag<Tax>(new ConcurrentBag<Tax>(taxFoundList));
            List<Tax> savedTaxList = new NotifyTaskCompletion<List<Tax>>(UpdateTax(taxList.ToList())).Task.Result;
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);


            // Bills Loading
            if (commandList.Count > 0)
            {
                List<Bill> billfoundList = new NotifyTaskCompletion<List<Bill>>(GateWayCommand.GetBillDataByCommandList(commandList.ToList())).Task.Result; // await GateWayCommand.GetBillDataByCommandList(new List<Command>(commandList.Skip(i * loadUnit).Take(loadUnit)));
                billList = new ConcurrentBag<Bill>(billList.Concat(new ConcurrentBag<Bill>(billfoundList)));
                List<Bill> savedBillList = new NotifyTaskCompletion<List<Bill>>(UpdateBill(billList.ToList())).Task.Result;
            }
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);

            // Delivery Loading
            if (commandList.Count > 0)
            {
                List<Delivery> deliveryfoundList = new NotifyTaskCompletion<List<Delivery>>(GateWayCommand.GetDeliveryDataByCommandList(commandList.ToList())).Task.Result; //await GateWayCommand.GetDeliveryDataByCommandList(new List<Command>(commandList.Skip(i * loadUnit).Take(loadUnit)));
                deliveryList = new ConcurrentBag<Delivery>(deliveryList.Concat(new ConcurrentBag<Delivery>(deliveryfoundList)));
                List<Delivery> savedDeliveryList = new NotifyTaskCompletion<List<Delivery>>(UpdateDelivery(deliveryList.ToList())).Task.Result;
            }
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);

            // Command_item Loading
            for (int i = 0; i < (commandList.Count() / loadUnit) || loadUnit > commandList.Count() && i == 0; i++)
            {
                ConcurrentBag<Command_item> command_itemFoundList = new ConcurrentBag<Command_item>(new NotifyTaskCompletion<List<Command_item>>(GateWayCommand.GetCommand_itemByCommandList(commandList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await GateWayCommand.GetCommand_itemByCommandList(new List<Command>(commandList.Skip(i * loadUnit).Take(loadUnit)));
                command_itemList = new ConcurrentBag<Command_item>(command_itemList.Concat(new ConcurrentBag<Command_item>(command_itemFoundList)));
            }
            //);
            var savedCommand_itemList = new ConcurrentBag<Command_item>(new NotifyTaskCompletion<List<Command_item>>(UpdateCommand_item(command_itemList.ToList())).Task.Result);
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);


            // Item Loading
            for (int i = 0; i < (command_itemList.Count() / loadUnit) || loadUnit > command_itemList.Count() && i == 0; i++)
            {
                ConcurrentBag<Item> itemFoundList = new ConcurrentBag<Item>(new NotifyTaskCompletion<List<Item>>(dalItem.GateWayItem.GetItemDataByCommand_itemList(command_itemList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetItemDataByCommand_itemList(new List<Command_item>(command_itemList.Skip(i * loadUnit).Take(loadUnit)));
                itemList = new ConcurrentBag<Item>(itemList.Concat(new ConcurrentBag<Item>(itemFoundList)));
            }           
            var savedItemList = new ConcurrentBag<Item>(new NotifyTaskCompletion<List<Item>>(dalItem.UpdateItem(itemList.ToList())).Task.Result);
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);


            // Provider_item Loading
            for (int i = 0; i < (itemList.Count() / loadUnit) || loadUnit > itemList.Count() && i == 0; i++)
            {
                ConcurrentBag<Provider_item> provider_itemFoundList = new ConcurrentBag<Provider_item>(new NotifyTaskCompletion<List<Provider_item>>(dalItem.GateWayItem.GetProvider_itemDataByItemList(itemList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetProvider_itemDataByItemList(new List<Item>(itemList.Skip(i * loadUnit).Take(loadUnit)));
                provider_itemList = new ConcurrentBag<Provider_item>(provider_itemList.Concat(new ConcurrentBag<Provider_item>(provider_itemFoundList)).OrderBy(x => x.Provider_name).Distinct());
            }

            var savedProvider_itemList = new ConcurrentBag<Provider_item>(new NotifyTaskCompletion<List<Provider_item>>(dalItem.UpdateProvider_item(provider_itemList.ToList())).Task.Result);
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);


            // Provider Loading
            if (provider_itemList.Count > 0)
            {
                List<Provider> providerFoundList = new NotifyTaskCompletion<List<Provider>>(dalItem.GateWayItem.GetProviderDataByProvider_itemList(provider_itemList.ToList())).Task.Result; // await dalItem.GateWayItem.GetProviderDataByProvider_itemList(new List<Provider_item>(provider_itemList.Skip(i * loadUnit).Take(loadUnit)));
                providerList = new ConcurrentBag<Provider>(providerList.Concat(new ConcurrentBag<Provider>(providerFoundList)).OrderBy(x => x.Name).Distinct());
                List<Provider> savedProviderList = new NotifyTaskCompletion<List<Provider>>(dalItem.UpdateProvider(providerList.ToList())).Task.Result;
            }
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);


            // Item_delivery Loading
            if (deliveryList.Count > 0)
            {
                List<Item_delivery> item_deliveryFoundList = new NotifyTaskCompletion<List<Item_delivery>>(dalItem.GateWayItem.GetItem_deliveryDataByDeliveryList(deliveryList.ToList())).Task.Result; // await dalItem.GateWayItem.GetItem_deliveryDataByDeliveryList(new List<Delivery>(deliveryList.Skip(i * loadUnit).Take(loadUnit)));
                item_deliveryList = new ConcurrentBag<Item_delivery>(item_deliveryList.Concat(new ConcurrentBag<Item_delivery>(item_deliveryFoundList)));
                List<Item_delivery> savedItem_deliveryList = new NotifyTaskCompletion<List<Item_delivery>>(dalItem.UpdateItem_delivery(item_deliveryList.ToList())).Task.Result;
            }
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);
            
            // Tax_item Loading
            for (int i = 0; i < (itemList.Count() / loadUnit) || loadUnit > itemList.Count() && i == 0; i++)
            {
                ConcurrentBag<Tax_item> tax_itemFoundList = new ConcurrentBag<Tax_item>(new NotifyTaskCompletion<List<Tax_item>>(dalItem.GateWayItem.GetTax_itemDataByItemList(itemList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result); // await dalItem.GateWayItem.GetTax_itemDataByItemList(new List<Item>(itemList.Skip(i * loadUnit).Take(loadUnit)));
                tax_itemList = new ConcurrentBag<Tax_item>(tax_itemList.Concat(new ConcurrentBag<Tax_item>(tax_itemFoundList)));
            }
            var savedTax_itemList = new ConcurrentBag<Tax_item>(new NotifyTaskCompletion<List<Tax_item>>(dalItem.UpdateTax_item(tax_itemList.ToList())).Task.Result);
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);


            // Client Loading
            if (billList.Count > 0)
            {
                List<Client> clientFoundList = new NotifyTaskCompletion<List<Client>>(dalClient.GateWayClient.GetClientDataByBillList(billList.ToList())).Task.Result; // await dalClient.GateWayClient.GetClientDataByBillList(new List<Bill>(billList.Skip(i * loadUnit).Take(loadUnit)));
                clientList = new ConcurrentBag<Client>(clientList.Concat(new ConcurrentBag<Client>(clientFoundList)));
                List<Client> savedClientList = new NotifyTaskCompletion<List<Client>>(dalClient.UpdateClient(clientList.ToList())).Task.Result;
            }
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);
            
            // Contacts Loading
            if (clientList.Count > 0)
            {
                List<Contact> contactFoundList = new NotifyTaskCompletion<List<Contact>>(dalClient.GateWayClient.GetContactDataByClientList(clientList.ToList())).Task.Result; // await dalClient.GateWayClient.GetContactDataByClientList(new List<Client>(clientList.Skip(i * loadUnit).Take(loadUnit)));
                contactList = new ConcurrentBag<Contact>(contactList.Concat(new ConcurrentBag<Contact>(contactFoundList)));
                List<Contact> savedContactList = new NotifyTaskCompletion<List<Contact>>(dalClient.UpdateContact(contactList.ToList())).Task.Result;
            }

            // saving commands
            if (commandList.Count > 0)
            {
                var savedCommandList = new NotifyTaskCompletion<List<Command>>(UpdateCommand(commandList.ToList())).Task.Result;
            }
            if (isActiveProgress) lock (_lock) _progressBarFunc(_progressBarFunc(0) + 100 / _progressStep);
            lock (_lock) _isLodingDataFromWebServiceToLocal = false;
        }

    } /* end class BLCommande */
}