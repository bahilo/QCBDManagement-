using QCBDManagementBusiness.Helper;
using QCBDManagementCommon;
using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.BL;
using QCBDManagementCommon.Structures;
//using QCBDManagementCommon.Interfaces.DAC;
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
    public class BLCommand : ICommandManager
    {
        // Attributes

        public QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DAC;

        public BLCommand(QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }
        
        public async Task<List<Command>> InsertCommand(List<Command> commandList)
        {
            if (commandList == null || commandList.Count == 0)
                return new List<Command>();

            List<Command> result = new List<Command>();
            try
            {
                result = await DAC.DALCommand.InsertCommand(commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Bill>> InsertCommandBill(List<Bill> billList)
        {
            if (billList == null || billList.Count == 0)
                return new List<Bill>();

            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALCommand.InsertBill(billList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Delivery>> InsertCommandDelivery(List<Delivery> deliveryList)
        {
            if (deliveryList == null || deliveryList.Count == 0)
                return new List<Delivery>();

            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALCommand.InsertDelivery(deliveryList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax>> InsertCommandTax(List<Tax> taxList)
        {
            if (taxList == null || taxList.Count == 0)
                return new List<Tax>();

            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALCommand.InsertTax(taxList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_command>> InsertTax_command(List<Tax_command> tax_commandList)
        {
            if (tax_commandList == null || tax_commandList.Count == 0)
                return new List<Tax_command>();

            List<Tax_command> result = new List<Tax_command>();
            try
            {
                result = await DAC.DALCommand.InsertTax_command(tax_commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command_item>> InsertCommand_item(List<Command_item> command_itemList)
        {
            if (command_itemList == null || command_itemList.Count == 0)
                return new List<Command_item>();

            List<Command_item> result = new List<Command_item>();
            try
            {
                result = await DAC.DALCommand.InsertCommand_item(command_itemList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax>> InsertTax(List<Tax> taxList)
        {
            if (taxList == null || taxList.Count == 0)
                return new List<Tax>();

            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALCommand.InsertTax(taxList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Bill>> InsertBill(List<Bill> billList)
        {
            if (billList == null || billList.Count == 0)
                return new List<Bill>();

            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALCommand.InsertBill(billList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Delivery>> InsertDelivery(List<Delivery> deliveryList)
        {
            if (deliveryList == null || deliveryList.Count == 0)
                return new List<Delivery>();


            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALCommand.InsertDelivery(deliveryList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command>> UpdateCommand(List<Command> commandList)
        {
            if (commandList == null || commandList.Count == 0)
                return new List<Command>();

            if (commandList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating commands(count = " + commandList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Command> result = new List<Command>();
            try
            {
                result = await DAC.DALCommand.UpdateCommand(commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }
        

        public async Task<List<Tax_command>> UpdateTax_command(List<Tax_command> tax_commandList)
        {
            if (tax_commandList == null || tax_commandList.Count == 0)
                return new List<Tax_command>();

            if (tax_commandList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating tax_commands(count = " + tax_commandList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Tax_command> result = new List<Tax_command>();
            try
            {
                result = await DAC.DALCommand.UpdateTax_command(tax_commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command_item>> UpdateCommand_item(List<Command_item> command_itemList)
        {
            if (command_itemList == null || command_itemList.Count == 0)
                return new List<Command_item>();

            if (command_itemList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating command_items(count = " + command_itemList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Command_item> result = new List<Command_item>();
            try
            {
                result = await DAC.DALCommand.UpdateCommand_item(command_itemList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax>> UpdateTax(List<Tax> taxList)
        {
            if (taxList == null || taxList.Count == 0)
                return new List<Tax>();

            if (taxList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating Taxes(count = " + taxList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALCommand.UpdateTax(taxList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Bill>> UpdateBill(List<Bill> billList)
        {
            if (billList == null || billList.Count == 0)
                return new List<Bill>();

            if (billList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating bills(count = " + billList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALCommand.UpdateBill(billList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Delivery>> UpdateDelivery(List<Delivery> deliveryList)
        {
            if (deliveryList == null || deliveryList.Count == 0)
                return new List<Delivery>();

            if (deliveryList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating deliveries(count = " + deliveryList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALCommand.UpdateDelivery(deliveryList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command>> DeleteCommand(List<Command> commandList)
        {
            if (commandList == null || commandList.Count == 0)
                return new List<Command>();

            if (commandList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting commands(count = " + commandList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Command> result = new List<Command>();
            try
            {
                result = await DAC.DALCommand.DeleteCommand(commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        
        public async Task<List<Tax_command>> DeleteTax_command(List<Tax_command> tax_commandList)
        {
            if (tax_commandList == null || tax_commandList.Count == 0)
                return new List<Tax_command>();

            if (tax_commandList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting tax_commands(count = " + tax_commandList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Tax_command> result = new List<Tax_command>();
            try
            {
                result = await DAC.DALCommand.DeleteTax_command(tax_commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command_item>> DeleteCommand_item(List<Command_item> command_itemList)
        {
            if (command_itemList == null || command_itemList.Count == 0)
                return new List<Command_item>();

            if (command_itemList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting command_items(count = " + command_itemList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Command_item> result = new List<Command_item>();
            try
            {
                result = await DAC.DALCommand.DeleteCommand_item(command_itemList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax>> DeleteTax(List<Tax> taxList)
        {
            if (taxList == null || taxList.Count == 0)
                return new List<Tax>();

            if (taxList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting Taxes(count = " + taxList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALCommand.DeleteTax(taxList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Bill>> DeleteBill(List<Bill> billList)
        {
            if (billList == null || billList.Count == 0)
                return new List<Bill>();

            if (billList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting bills(count = " + billList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALCommand.DeleteBill(billList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Delivery>> DeleteDelivery(List<Delivery> deliveryList)
        {
            if (deliveryList == null || deliveryList.Count == 0)
                return new List<Delivery>();

            if (deliveryList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting deliveries(count = " + deliveryList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALCommand.DeleteDelivery(deliveryList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command>> GetCommandData(int nbLine)
        {
            List<Command> result = new List<Command>();
            try
            {
                result = await DAC.DALCommand.GetCommandData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command>> GetCommandDataById(int id)
        {
            List<Command> result = new List<Command>();
            try
            {
                result = await DAC.DALCommand.GetCommandDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_command>> GetTax_commandData(int nbLine)
        {
            List<Tax_command> result = new List<Tax_command>();
            try
            {
                result = await DAC.DALCommand.GetTax_commandData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_command>> GetTax_commandDataByCommandList(List<Command> commandList)
        {
            List<Tax_command> result = new List<Tax_command>();
            try
            {
                result = await DAC.DALCommand.GetTax_commandDataByCommandList(commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_command>> GetTax_commandDataById(int id)
        {
            List<Tax_command> result = new List<Tax_command>();
            try
            {
                result = await DAC.DALCommand.GetTax_commandDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command_item>> GetCommand_itemData(int nbLine)
        {
            List<Command_item> result = new List<Command_item>();
            try
            {
                result = await DAC.DALCommand.GetCommand_itemData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command_item>> GetCommand_itemByCommandList(List<Command> listCommand)
        {
            List<Command_item> result = new List<Command_item>();
            try
            {
                result = await DAC.DALCommand.GetCommand_itemByCommandList(listCommand);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command_item>> GetCommand_itemDataById(int id)
        {
            List<Command_item> result = new List<Command_item>();
            try
            {
                result = await DAC.DALCommand.GetCommand_itemDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax>> GetTaxData(int nbLine)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALCommand.GetTaxData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax>> GetTaxDataById(int id)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALCommand.GetTaxDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Bill>> GetBillData(int nbLine)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALCommand.GetBillData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Bill>> GetBillDataByCommandList(List<Command> commandList)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALCommand.GetBillDataByCommandList(commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Bill>> GetBillDataById(int id)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALCommand.GetBillDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<Bill> GetLastBill()
        {
            Bill result = new Bill();
            try
            {
                result = await DAC.DALCommand.GetLastBill();
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Delivery>> GetDeliveryData(int nbLine)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALCommand.GetDeliveryData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Delivery>> GetDeliveryDataByCommandList(List<Command> commandList)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALCommand.GetDeliveryDataByCommandList(commandList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Delivery>> GetDeliveryDataById(int id)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALCommand.GetDeliveryDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command>> searchCommand(Command command, string filterOperator)
        {
            List<Command> result = new List<Command>();
            try
            {
                result = await DAC.DALCommand.searchCommand(command, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

         
        public async Task<List<Tax_command>> searchTax_command(Tax_command Tax_command, string filterOperator)
        {
            List<Tax_command> result = new List<Tax_command>();
            try
            {
                result = await DAC.DALCommand.searchTax_command(Tax_command, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command_item>> searchCommand_item(Command_item Command_item, string filterOperator)
        {
            List<Command_item> result = new List<Command_item>();
            try
            {
                result = await DAC.DALCommand.searchCommand_item(Command_item, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax>> searchTax(Tax Tax, string filterOperator)
        {
            List<Tax> result = new List<Tax>();
            try
            {
                result = await DAC.DALCommand.searchTax(Tax, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Bill>> searchBill(Bill Bill, string filterOperator)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALCommand.searchBill(Bill, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Delivery>> searchDelivery(Delivery Delivery, string filterOperator)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALCommand.searchDelivery(Delivery, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }
        public async Task<List<Command>> searchCommandFromWebService(Command command, string filterOperator)
        {
            List<Command> result = new List<Command>();
            if (command == null)
                return result;
            try
            {
                result = await DAC.DALCommand.searchCommandFromWebService(command, filterOperator);
                //DAC.DALCommand.UpdateCommandDependencies(result);
                var localSearchResultList = await searchCommand(command, filterOperator);
                if (localSearchResultList.Count == 0)
                    DAC.DALCommand.UpdateCommandDependencies(result);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Command_item>> searchCommand_itemFromWebService(Command_item Command_item, string filterOperator)
        {
            List<Command_item> result = new List<Command_item>();
            try
            {
                result = await DAC.DALCommand.searchCommand_itemFromWebService(Command_item, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Bill>> searchBillFromWebService(Bill Bill, string filterOperator)
        {
            List<Bill> result = new List<Bill>();
            try
            {
                result = await DAC.DALCommand.searchBillFromWebService(Bill, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Delivery>> searchDeliveryFromWebService(Delivery Delivery, string filterOperator)
        {
            List<Delivery> result = new List<Delivery>();
            try
            {
                result = await DAC.DALCommand.searchDeliveryFromWebService(Delivery, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public void GeneratePdfCommand(ParamCommandToPdf paramCommandToPdf)
        {
            try
            {
                DAC.DALCommand.GeneratePdfCommand(paramCommandToPdf);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
        }

        public void GeneratePdfQuote(ParamCommandToPdf paramCommandToPdf)
        {
            try
            {
                DAC.DALCommand.GeneratePdfQuote(paramCommandToPdf);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
        }

        public void GeneratePdfDelivery(ParamDeliveryToPdf paramDeliveryToPdf)
        {
            try
            {
                DAC.DALCommand.GeneratePdfDelivery(paramDeliveryToPdf);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
        }

        public void Dispose()
        {
            DAC.DALCommand.Dispose();
        }        


        // Operations




    } /* end class BLCommande */
}