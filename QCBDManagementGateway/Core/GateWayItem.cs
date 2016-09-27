using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementGateway.Helper.ChannelHelper;
using QCBDManagementGateway.QCBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class GateWayItem : IItemManager, INotifyPropertyChanged
    {
        private QCBDManagementWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public GateWayItem()
        {
            _channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");// (binding, endPoint);
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



        public async Task<List<Item>> InsertItem(List<Item> itemList)
        {
            var formatListItemToArray = ServiceHelper.ItemTypeToArray(itemList);
            List<Item> result = new List<Item>();
            try
            {
                
                result = (await _channel.insert_data_itemAsync(formatListItemToArray)).ArrayTypeToItem();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider>> InsertProvider(List<Provider> listProvider)
        {
            var formatListProviderToArray = ServiceHelper.ProviderTypeToArray(listProvider);
            List<Provider> result = new List<Provider>();
            try
            {
                
                result = (await _channel.insert_data_providerAsync(formatListProviderToArray)).ArrayTypeToProvider();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider_item>> InsertProvider_item(List<Provider_item> listProvider_item)
        {
            var formatListProvider_itemToArray = ServiceHelper.Provider_itemTypeToArray(listProvider_item);
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                
                result = (await _channel.insert_data_provider_itemAsync(formatListProvider_itemToArray)).ArrayTypeToProvider_item();
            }
            catch (FaultException)
            {
                Dispose();
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


        public async Task<List<Item_delivery>> InsertItem_delivery(List<Item_delivery> listItem_delivery)
        {
            var formatListItem_deliveryToArray = ServiceHelper.Item_deliveryTypeToArray(listItem_delivery);
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                
                result = (await _channel.insert_data_item_deliveryAsync(formatListItem_deliveryToArray)).ArrayTypeToItem_delivery();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Auto_ref>> InsertAuto_ref(List<Auto_ref> listAuto_ref)
        {
            var formatListAuto_refToArray = listAuto_ref.Auto_refTypeToArray();
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                
                result = (await _channel.insert_data_auto_refAsync(formatListAuto_refToArray)).ArrayTypeToAuto_ref();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Tax_item>> InsertTax_item(List<Tax_item> listTax_item)
        {
            var formatListTax_itemToArray = listTax_item.Tax_itemTypeToArray();
            List<Tax_item> result = new List<Tax_item>();
            try
            {

                result = (await _channel.insert_data_tax_itemAsync(formatListTax_itemToArray)).ArrayTypeToTax_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item>> DeleteItem(List<Item> listItem)
        {
            var formatListItemToArray = ServiceHelper.ItemTypeToArray(listItem);
            List<Item> result = new List<Item>();
            try
            {
                
                result = (await _channel.delete_data_itemAsync(formatListItemToArray)).ArrayTypeToItem();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider>> DeleteProvider(List<Provider> listProvider)
        {
            var formatListProviderToArray = ServiceHelper.ProviderTypeToArray(listProvider);
            List<Provider> result = new List<Provider>();
            try
            {
                
                result = (await _channel.delete_data_providerAsync(formatListProviderToArray)).ArrayTypeToProvider();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider_item>> DeleteProvider_item(List<Provider_item> listProvider_item)
        {
            var formatListProvider_itemToArray = ServiceHelper.Provider_itemTypeToArray(listProvider_item);
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                
                result = (await _channel.delete_data_provider_itemAsync(formatListProvider_itemToArray)).ArrayTypeToProvider_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item_delivery>> DeleteItem_delivery(List<Item_delivery> listItem_delivery)
        {
            var formatListItem_deliveryToArray = ServiceHelper.Item_deliveryTypeToArray(listItem_delivery);
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                
                result = (await _channel.delete_data_item_deliveryAsync(formatListItem_deliveryToArray)).ArrayTypeToItem_delivery();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Auto_ref>> DeleteAuto_ref(List<Auto_ref> listAuto_ref)
        {
            var formatListAuto_refToArray = listAuto_ref.Auto_refTypeToArray();
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                
                result = (await _channel.delete_data_auto_refAsync(formatListAuto_refToArray)).ArrayTypeToAuto_ref();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Tax_item>> DeleteTax_item(List<Tax_item> listTax_item)
        {
            var formatListTax_itemToArray = listTax_item.Tax_itemTypeToArray();
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.delete_data_tax_itemAsync(formatListTax_itemToArray)).ArrayTypeToTax_item();
            }
            catch (FaultException)
            {
                Dispose();
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


        public async Task<List<Item>> UpdateItem(List<Item> listItem)
        {
            var formatListItemToArray = ServiceHelper.ItemTypeToArray(listItem);
            List<Item> result = new List<Item>();
            try
            {
                
                result = (await _channel.update_data_itemAsync(formatListItemToArray)).ArrayTypeToItem();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider>> UpdateProvider(List<Provider> listProvider)
        {
            var formatListProviderToArray = ServiceHelper.ProviderTypeToArray(listProvider);
            List<Provider> result = new List<Provider>();
            try
            {
                
                result = (await _channel.update_data_providerAsync(formatListProviderToArray)).ArrayTypeToProvider();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider_item>> UpdateProvider_item(List<Provider_item> listProvider_item)
        {
            var formatListProvider_itemToArray = ServiceHelper.Provider_itemTypeToArray(listProvider_item);
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                
                result = (await _channel.update_data_provider_itemAsync(formatListProvider_itemToArray)).ArrayTypeToProvider_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item_delivery>> UpdateItem_delivery(List<Item_delivery> listItem_delivery)
        {
            var formatListItem_deliveryToArray = ServiceHelper.Item_deliveryTypeToArray(listItem_delivery);
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                
                result = (await _channel.update_data_item_deliveryAsync(formatListItem_deliveryToArray)).ArrayTypeToItem_delivery();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Auto_ref>> UpdateAuto_ref(List<Auto_ref> listAuto_ref)
        {
            var formatListAuto_refToArray = listAuto_ref.Auto_refTypeToArray();
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                
                result = (await _channel.update_data_auto_refAsync(formatListAuto_refToArray)).ArrayTypeToAuto_ref();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Tax_item>> UpdateTax_item(List<Tax_item> listTax_item)
        {
            var formatListTax_itemToArray = listTax_item.Tax_itemTypeToArray();
            List<Tax_item> result = new List<Tax_item>();
            try
            {

                result = (await _channel.update_data_tax_itemAsync(formatListTax_itemToArray)).ArrayTypeToTax_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item>> GetItemData(int nbLine)
        {
            List<Item> result = new List<Item>();
            try
            {
                
                result = (await _channel.get_data_itemAsync(nbLine.ToString())).ArrayTypeToItem();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item>> GetItemDataByCommand_itemList(List<Command_item> command_itemList)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = (await _channel.get_data_item_by_command_item_listAsync(command_itemList.Command_itemTypeToArray())).ArrayTypeToItem();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item>> GetItemDataById(int id)
        {
            List<Item> result = new List<Item>();
            try
            {
                
                result = (await _channel.get_data_item_by_idAsync(id.ToString())).ArrayTypeToItem();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider>> GetProviderData(int nbLine)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                
                result = (await _channel.get_data_providerAsync(nbLine.ToString())).ArrayTypeToProvider();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider>> GetProviderDataByProvider_itemList(List<Provider_item> provider_itemList)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = (await _channel.get_data_provider_by_provider_item_listAsync(provider_itemList.Provider_itemTypeToArray())).ArrayTypeToProvider();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider>> GetProviderDataById(int id)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                
                result = (await _channel.get_data_provider_by_idAsync(id.ToString())).ArrayTypeToProvider();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider_item>> GetProvider_itemData(int nbLine)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                
                result = (await _channel.get_data_provider_itemAsync(nbLine.ToString())).ArrayTypeToProvider_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider_item>> GetProvider_itemDataByItemList(List<Item> itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = (await _channel.get_data_provider_item_by_item_listAsync(itemList.ItemTypeToArray())).ArrayTypeToProvider_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Provider_item>> GetProvider_itemDataById(int id)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                
                result = (await _channel.get_data_provider_item_by_idAsync(id.ToString())).ArrayTypeToProvider_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item_delivery>> GetItem_deliveryData(int nbLine)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                
                result = (await _channel.get_data_item_deliveryAsync(nbLine.ToString())).ArrayTypeToItem_delivery();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item_delivery>> GetItem_deliveryDataByDeliveryList(List<Delivery> deliveryList)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = (await _channel.get_data_item_delivery_by_delivery_listAsync(deliveryList.DeliveryTypeToArray())).ArrayTypeToItem_delivery();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item_delivery>> GetItem_deliveryDataById(int id)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                
                result = (await _channel.get_data_item_delivery_by_idAsync(id.ToString())).ArrayTypeToItem_delivery();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Auto_ref>> GetAuto_refData(int nbLine)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                
                result = (await _channel.get_data_auto_refAsync(nbLine.ToString())).ArrayTypeToAuto_ref();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Auto_ref>> GetAuto_refDataById(int id)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                
                result = (await _channel.get_data_auto_ref_by_idAsync(id.ToString())).ArrayTypeToAuto_ref();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Tax_item>> GetTax_itemData(int nbLine)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.get_data_tax_itemAsync(nbLine.ToString())).ArrayTypeToTax_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Tax_item>> GetTax_itemDataByItemList(List<Item> itemList)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.get_data_tax_item_by_item_listAsync(itemList.ItemTypeToArray())).ArrayTypeToTax_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Tax_item>> GetTax_itemDataById(int id)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {

                result = (await _channel.get_data_tax_item_by_idAsync(id.ToString())).ArrayTypeToTax_item();
            }
            catch (FaultException)
            {
                Dispose();
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


        public async Task<List<Item>> searchItem(Item item, string filterOperator)
        {
            var formatListItemToArray = ServiceHelper.ItemTypeToFilterArray(item, filterOperator);
            List<Item> result = new List<Item>();
            try
            {
                
                result = (await _channel.get_filter_itemAsync(formatListItemToArray)).ArrayTypeToItem();
            }
            catch (FaultException)
            {
                Dispose();
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
                throw;
            }
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Provider>> searchProvider(Provider Provider, string filterOperator)
        {
            var formatListProviderToArray = ServiceHelper.ProviderTypeToFilterArray(Provider, filterOperator);
            List<Provider> result = new List<Provider>();
            try
            {
                
                result = (await _channel.get_filter_providerAsync(formatListProviderToArray)).ArrayTypeToProvider();
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

        public async Task<List<Provider_item>> searchProvider_item(Provider_item Provider_item, string filterOperator)
        {
            var formatListProvider_itemToArray = ServiceHelper.Provider_itemTypeToFilterArray(Provider_item, filterOperator);
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                
                result = (await _channel.get_filter_provider_itemAsync(formatListProvider_itemToArray)).ArrayTypeToProvider_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item_delivery>> searchItem_delivery(Item_delivery Item_delivery, string filterOperator)
        {
            var formatListItem_deliveryToArray = ServiceHelper.Item_deliveryTypeToFilterArray(Item_delivery, filterOperator);
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                
                result = (await _channel.get_filter_item_deliveryAsync(formatListItem_deliveryToArray)).ArrayTypeToItem_delivery();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Auto_ref>> searchAuto_ref(Auto_ref Auto_ref, string filterOperator)
        {
            var formatListAuto_refToArray = ServiceHelper.Auto_refTypeToFilterArray(Auto_ref, filterOperator);
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {                
                result = (await _channel.get_filter_auto_refAsync(formatListAuto_refToArray)).ArrayTypeToAuto_ref();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Tax_item>> searchTax_item(Tax_item Tax_item, string filterOperator)
        {
            var formatListTax_itemToArray = ServiceHelper.Tax_itemTypeToFilterArray(Tax_item, filterOperator);
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = (await _channel.get_filter_tax_itemAsync(formatListTax_itemToArray)).ArrayTypeToTax_item();
            }
            catch (FaultException)
            {
                Dispose();
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

        public async Task<List<Item>> searchItemFromWebService(Item item, string filterOperator)
        {
            return await searchItem(item, filterOperator);
        }

        public async Task<List<Provider>> searchProviderFromWebService(Provider Provider, string filterOperator)
        {
            return await searchProvider(Provider, filterOperator);
        }

        public async Task<List<Provider_item>> searchProvider_itemFromWebService(Provider_item Provider_item, string filterOperator)
        {
            return await searchProvider_item(Provider_item, filterOperator);
        }

        public async Task<List<Item_delivery>> searchItem_deliveryFromWebService(Item_delivery Item_delivery, string filterOperator)
        {
            return await searchItem_delivery(Item_delivery, filterOperator);
        }

        public void Dispose()
        {
            _channel.Close();
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            throw new NotImplementedException();
        }
    } /* end class BLItem */
}