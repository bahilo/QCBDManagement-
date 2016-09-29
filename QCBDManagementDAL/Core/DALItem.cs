using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementDAL.App_Data;
using QCBDManagementDAL.App_Data.QCBDDataSetTableAdapters;
using QCBDManagementDAL.Helper.ChannelHelper;
using QCBDManagementGateway.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ... 
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementDAL.Core
{
    public class DALItem : IItemManager
    {
        public Agent AuthenticatedUser { get; set; }
        private Func<double, double> _rogressBarFunc;
        private GateWayItem _gateWayItem;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private object _lock = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        public DALItem()
        {
            _gateWayItem = new GateWayItem();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
            _gateWayItem.PropertyChanged += onCredentialChange_loadItemDataFromWebService;
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; onPropertyChange("IsLodingDataFromWebServiceToLocal"); }
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public GateWayItem GateWayItem
        {
            get { return _gateWayItem; }
        }

        private void onCredentialChange_loadItemDataFromWebService(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Credential"))
            {
                DALHelper.doActionAsync(retrieveGateWayDataItem);                
            }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                _gateWayItem.initializeCredential(AuthenticatedUser);
            }
        }

        private void retrieveGateWayDataItem()
        {
            int loadUnit = 50;

            List<Provider> providerList = new List<Provider>();
            List<Provider_item> provider_itemList = new List<Provider_item>();

            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                var itemList = new NotifyTaskCompletion<List<Item>>(_gateWayItem.GetItemData(_loadSize)).Task.Result;
                if (itemList.Count > 0)
                {
                    List<Item> savedItemList = new NotifyTaskCompletion<List<Item>>(UpdateItem(itemList)).Task.Result;

                    for (int i = 0; i < (savedItemList.Count() / loadUnit) || loadUnit >= savedItemList.Count() && i == 0; i++)
                    {
                        lock (_lock)
                        {
                            List<Provider_item> savedProvider_itemList = new NotifyTaskCompletion<List<Provider_item>>(UpdateProvider_item(new NotifyTaskCompletion<List<Provider_item>>(_gateWayItem.GetProvider_itemDataByItemList(savedItemList.Skip(i * loadUnit).Take(loadUnit).ToList())).Task.Result)).Task.Result;
                            List<Provider> savedProviderList = new NotifyTaskCompletion<List<Provider>>(UpdateProvider(new NotifyTaskCompletion<List<Provider>>(_gateWayItem.GetProviderDataByProvider_itemList(savedProvider_itemList.OrderBy(x => x.Provider_name).Distinct().ToList())).Task.Result)).Task.Result;
                        }
                    }
                }
                
            }
            finally
            {
                lock (_lock)
                {
                    IsLodingDataFromWebServiceToLocal = false;
                    _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
                    Log.write("Item loaded!", "TES");
                }
            }            
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _rogressBarFunc = progressBarFunc;
        }

        public async Task<List<Item>> InsertItem(List<Item> itemList)
        {
            List<Item> result = new List<Item>();
            List<Item> gateWayResultList = new List<Item>();
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.InsertItem(itemList) : itemList;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateItem(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Provider>> InsertProvider(List<Provider> listProvider)
        {
            List<Provider> result = new List<Provider>();
            List<Provider> gateWayResultList = new List<Provider>();
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.InsertProvider(listProvider) : listProvider;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateProvider(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Provider_item>> InsertProvider_item(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = new List<Provider_item>();
            List<Provider_item> gateWayResultList = new List<Provider_item>();
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.InsertProvider_item(listProvider_item) : listProvider_item;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateProvider_item(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }


        public async Task<List<Item_delivery>> InsertItem_delivery(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            List<Item_delivery> gateWayResultList = new List<Item_delivery>();
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.InsertItem_delivery(listItem_delivery) : listItem_delivery;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateItem_delivery(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Auto_ref>> InsertAuto_ref(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            List<Auto_ref> gateWayResultList = new List<Auto_ref>();
            using (auto_refsTableAdapter _auto_refTableAdapter = new auto_refsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.InsertAuto_ref(listAuto_ref) : listAuto_ref;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateAuto_ref(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Tax_item>> InsertTax_item(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = new List<Tax_item>();
            List<Tax_item> gateWayResultList = new List<Tax_item>();
            using (tax_itemsTableAdapter _tax_itemTableAdapter = new tax_itemsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.InsertTax_item(listTax_item) : listTax_item;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateTax_item(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Item>> DeleteItem(List<Item> listItem)
        {
            List<Item> result = listItem;
            List<Item> gateWayResultList = new List<Item>();
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.DeleteItem(listItem) : listItem;
                if (gateWayResultList.Count == 0)
                    foreach (Item item in listItem)
                    {
                        int returnValue = _itemTableAdapter.delete_data_item(item.ID);
                        if (returnValue > 0)
                            result.Remove(item);
                    }
            }
            return result;
        }

        public async Task<List<Provider>> DeleteProvider(List<Provider> listProvider)
        {
            List<Provider> result = listProvider;
            List<Provider> gateWayResultList = new List<Provider>();
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.DeleteProvider(listProvider) : listProvider;
                if (gateWayResultList.Count == 0)
                    foreach (Provider provider in listProvider)
                    {
                        int returnValue = _providersTableAdapter.delete_data_provider(provider.ID);
                        if (returnValue > 0)
                            result.Remove(provider);
                    }
            }
            return result;
        }

        public async Task<List<Provider_item>> DeleteProvider_item(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = listProvider_item;
            List<Provider_item> gateWayResultList = new List<Provider_item>();
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.DeleteProvider_item(listProvider_item) : listProvider_item;
                if (gateWayResultList.Count == 0)
                    foreach (Provider_item provider_item in listProvider_item)
                    {
                        int returnValue = _provider_itemsTableAdapter.delete_data_provider_item(provider_item.ID);
                        if (returnValue > 0)
                            result.Remove(provider_item);
                    }
            }
            return result;
        }


        public async Task<List<Item_delivery>> DeleteItem_delivery(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = listItem_delivery;
            List<Item_delivery> gateWayResultList = new List<Item_delivery>();
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.DeleteItem_delivery(listItem_delivery) : listItem_delivery;
                if (gateWayResultList.Count == 0)
                    foreach (Item_delivery item_delivery in gateWayResultList)
                    {
                        int returnValue = _item_deliveriesTableAdapter.delete_data_item_delivery(item_delivery.ID);
                        if (returnValue > 0)
                            result.Remove(item_delivery);
                    }
            }
            return result;
        }

        public async Task<List<Auto_ref>> DeleteAuto_ref(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = listAuto_ref;
            List<Auto_ref> gateWayResultList = new List<Auto_ref>();
            using (auto_refsTableAdapter _auto_refTableAdapter = new auto_refsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.DeleteAuto_ref(listAuto_ref) : listAuto_ref;
                if (gateWayResultList.Count == 0)
                    foreach (Auto_ref Auto_ref in listAuto_ref)
                    {
                        int returnValue = _auto_refTableAdapter.Delete(Auto_ref.ID, Auto_ref.RefId);
                        if (returnValue > 0)
                            result.Remove(Auto_ref);
                    }
            }
            return result;
        }

        public async Task<List<Tax_item>> DeleteTax_item(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = listTax_item;
            List<Tax_item> gateWayResultList = new List<Tax_item>();
            using (tax_itemsTableAdapter _tax_itemTableAdapter = new tax_itemsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.DeleteTax_item(listTax_item) : listTax_item;
                if (gateWayResultList.Count == 0)
                    foreach (Tax_item Tax_item in listTax_item)
                    {
                        int returnValue = _tax_itemTableAdapter.delete_data_tax_item(Tax_item.ID);
                        if (returnValue > 0)
                            result.Remove(Tax_item);
                    }
            }
            return result;
        }


        public async Task<List<Item>> UpdateItem(List<Item> listItem)
        {
            List<Item> result = new List<Item>();
            List<Item> gateWayResultList = new List<Item>();
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.UpdateItem(listItem) : listItem;

                foreach (var item in gateWayResultList)
                //Parallel.ForEach(gateWayResultList, (item) =>
                {
                    int returnResult = _itemTableAdapter
                                            .update_data_item(
                                                item.Ref,
                                                item.Name,
                                                item.Type,
                                                item.Type_sub,
                                                item.Price_purchase,
                                                item.Price_sell,
                                                item.Source,
                                                item.Comment,
                                                item.Erasable,
                                                item.ID);
                    if (returnResult > 0)
                        result.Add(item);
                }
                //);
            }
            return result;
        }

        public async Task<List<Provider>> UpdateProvider(List<Provider> listProvider)
        {
            List<Provider> result = new List<Provider>();
            List<Provider> gateWayResultList = new List<Provider>();
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.InsertProvider(listProvider) : listProvider;
                foreach (var provider in gateWayResultList)
                {
                    int returnResult = _providersTableAdapter
                                            .update_data_provider(
                                                provider.Name,
                                                provider.Source,
                                                provider.ID);
                    if (returnResult > 0)
                        result.Add(provider);
                }
            }
            return result;
        }

        public async Task<List<Provider_item>> UpdateProvider_item(List<Provider_item> listProvider_item)
        {
            List<Provider_item> result = new List<Provider_item>();
            List<Provider_item> gateWayResultList = new List<Provider_item>();
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.InsertProvider_item(listProvider_item) : listProvider_item;
                foreach (var provider_item in gateWayResultList)
                {
                    int returnResult = _provider_itemsTableAdapter
                                            .update_data_provider_item(
                                                provider_item.Provider_name,
                                                provider_item.Item_ref,
                                                provider_item.ID);

                    if (returnResult > 0)
                        result.Add(provider_item);
                }
            }
            return result;
        }

        public async Task<List<Item_delivery>> UpdateItem_delivery(List<Item_delivery> listItem_delivery)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            List<Item_delivery> gateWayResultList = new List<Item_delivery>();
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.InsertItem_delivery(listItem_delivery) : listItem_delivery;
                foreach (Item_delivery item_delivery in gateWayResultList)
                {
                    int returnResult = _item_deliveriesTableAdapter
                                    .update_data_item_delivery(
                                        item_delivery.DeliveryId,
                                        item_delivery.Item_ref,
                                        item_delivery.Quantity_delivery,
                                        item_delivery.ID);

                    if (returnResult > 0)
                        result.Add(item_delivery);
                }
            }
            return result;
        }

        public async Task<List<Auto_ref>> UpdateAuto_ref(List<Auto_ref> listAuto_ref)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            List<Auto_ref> gateWayResultList = new List<Auto_ref>();
            using (auto_refsTableAdapter _auto_refTableAdapter = new auto_refsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.UpdateAuto_ref(listAuto_ref) : listAuto_ref;
                foreach (var Auto_ref in gateWayResultList)
                {
                    int returnResult = _auto_refTableAdapter
                                    .Update(
                                        Auto_ref.RefId,
                                        Auto_ref.ID,
                                        Auto_ref.RefId);

                    if (returnResult > 0)
                        result.Add(Auto_ref);
                }
            }

            return result;
        }

        public async Task<List<Tax_item>> UpdateTax_item(List<Tax_item> listTax_item)
        {
            List<Tax_item> result = new List<Tax_item>();
            List<Tax_item> gateWayResultList = new List<Tax_item>();
            using (tax_itemsTableAdapter _tax_itemTableAdapter = new tax_itemsTableAdapter())
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayItem.InsertTax_item(listTax_item) : listTax_item;
                foreach (var Tax_item in gateWayResultList)
                {
                    int returnResult = _tax_itemTableAdapter
                        .update_data_tax_item(
                            Tax_item.TaxId,
                            Tax_item.Item_ref,
                            Tax_item.Tax_value,
                            Tax_item.Tax_type,
                            Tax_item.ID);

                    if (returnResult > 0)
                        result.Add(Tax_item);
                }
            }

            return result;
        }

        public async Task<List<Item>> GetItemData(int nbLine)
        {
            List<Item> result = new List<Item>();
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.itemsDataTable>(_itemTableAdapter.get_data_item)).DataTableTypeToItem();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;
            return result.GetRange(0, nbLine);
        }

        public async Task<List<Item>> GetItemDataByCommand_itemList(List<Command_item> command_itemList)
        {
            List<Item> result = new List<Item>();
            using (GateWayItem gateAwayItem = new GateWayItem())
            {
                gateAwayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                result = await gateAwayItem.GetItemDataByCommand_itemList(command_itemList);
            }
            /*foreach (Command_item command_item in command_itemList)
            {
                var itemFound = (await searchItem(new Item { Ref = command_item.Item_ref, ID=command_item.ItemId }, "OR")).OrderByDescending(x => x.ID).FirstOrDefault();
                if (itemFound != null)
                    result.Add(itemFound);
            }*/
            return result;
        }

        public async Task<List<Item>> GetItemDataById(int id)
        {
            using (itemsTableAdapter _itemTableAdapter = new itemsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.itemsDataTable>(_itemTableAdapter.get_data_item_by_id, id)).DataTableTypeToItem();
        }

        public async Task<List<Provider>> GetProviderData(int nbLine)
        {
            List<Provider> result = new List<Provider>();
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.providersDataTable>(_providersTableAdapter.get_data_provider)).DataTableTypeToProvider();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Provider>> GetProviderDataByProvider_itemList(List<Provider_item> provider_itemList)
        {
            List<Provider> result = new List<Provider>();
            using (GateWayItem gateAwayItem = new GateWayItem())
            {
                gateAwayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                result = await gateAwayItem.GetProviderDataByProvider_itemList(provider_itemList);
            }
            /*foreach (Provider_item provider_item in provider_itemList)
            {
                var providerFound = (await searchProvider(new Provider { Name = provider_item.Provider_name }, "AND")).OrderByDescending(x => x.ID).FirstOrDefault();
                if (providerFound != null)
                    result.Add(providerFound);
            }*/
            return result;
        }

        public async Task<List<Provider>> GetProviderDataById(int id)
        {
            using (providersTableAdapter _providersTableAdapter = new providersTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.providersDataTable>(_providersTableAdapter.get_data_provider_by_id, id)).DataTableTypeToProvider();

        }

        public async Task<List<Provider_item>> GetProvider_itemData(int nbLine)
        {
            List<Provider_item> result = new List<Provider_item>();
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.provider_itemsDataTable>(_provider_itemsTableAdapter.get_data_provider_item)).DataTableTypeToProvider_item();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Provider_item>> GetProvider_itemDataByItemList(List<Item> itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            using (GateWayItem gateAwayItem = new GateWayItem())
            {
                gateAwayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                result = await gateAwayItem.GetProvider_itemDataByItemList(itemList);
            }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemDataById(int id)
        {
            using (provider_itemsTableAdapter _provider_itemsTableAdapter = new provider_itemsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.provider_itemsDataTable>(_provider_itemsTableAdapter.get_data_provider_item_by_id, id)).DataTableTypeToProvider_item();
        }
        
        public async Task<List<Item_delivery>> GetItem_deliveryData(int nbLine)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.item_deliveriesDataTable>(_item_deliveriesTableAdapter.get_data_item_delivery)).DataTableTypeToItem_delivery();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataByDeliveryList(List<Delivery> deliveryList)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            using (GateWayItem gateAwayItem = new GateWayItem())
            {
                gateAwayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                result = await gateAwayItem.GetItem_deliveryDataByDeliveryList(deliveryList);
            }
            return result;
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataById(int id)
        {
            using (item_deliveriesTableAdapter _item_deliveriesTableAdapter = new item_deliveriesTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.item_deliveriesDataTable>(_item_deliveriesTableAdapter.get_data_item_delivery_by_id, id)).DataTableTypeToItem_delivery();
        }

        public async Task<List<Auto_ref>> GetAuto_refData(int nbLine)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                result = await gateWayItem.GetAuto_refData(nbLine);
            }
              
            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Auto_ref>> GetAuto_refDataById(int id)
        {
            return await searchAuto_ref(new Auto_ref { ID = id }, "AND");
        }

        public async Task<List<Tax_item>> GetTax_itemData(int nbLine)
        {
            List<Tax_item> result = new List<Tax_item>();
            using (tax_itemsTableAdapter _tax_itemTableAdapter = new tax_itemsTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.tax_itemsDataTable>(_tax_itemTableAdapter.get_data_tax_item)).DataTableTypeToTax_item();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Tax_item>> GetTax_itemDataByItemList(List<Item> itemList)
        {
            List<Tax_item> result = new List<Tax_item>();
            foreach (Item item in itemList)
            {
                var tax_itemFound = (await searchTax_item(new Tax_item { Item_ref = item.Ref, itemId = item.ID }, "OR")).OrderByDescending(x => x.ID).FirstOrDefault();
                if (tax_itemFound != null)
                    result.Add(tax_itemFound);
            }
            return result;
        }

        public async Task<List<Tax_item>> GetTax_itemDataById(int id)
        {
            return await searchTax_item(new Tax_item { ID = id }, "AND");
        }

        public async Task<List<Item>> searchItem(Item item, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return item.ItemTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Provider>> searchProvider(Provider Provider, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Provider.ProviderTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Provider_item>> searchProvider_item(Provider_item Provider_item, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Provider_item.Provider_itemTypeToFilterDataTable(filterOperator); });
        }
        
        public async Task<List<Item_delivery>> searchItem_delivery(Item_delivery Item_delivery, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Item_delivery.Item_deliveryTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Auto_ref>> searchAuto_ref(Auto_ref Auto_ref, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Auto_ref.Auto_refTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Tax_item>> searchTax_item(Tax_item Tax_item, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Tax_item.Tax_itemTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Item>> searchItemFromWebService(Item item, string filterOperator)
        {
            List<Item> gateWayResultList = new List<Item>();
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayItem.searchItemFromWebService(item, filterOperator);

            }
            return gateWayResultList;
        }

        public async Task<List<Provider>> searchProviderFromWebService(Provider Provider, string filterOperator)
        {
            List<Provider> gateWayResultList = new List<Provider>();
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayItem.searchProviderFromWebService(Provider, filterOperator);

            }
            return gateWayResultList;
        }

        public async Task<List<Provider_item>> searchProvider_itemFromWebService(Provider_item Provider_item, string filterOperator)
        {
            List<Provider_item> gateWayResultList = new List<Provider_item>();
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayItem.searchProvider_itemFromWebService(Provider_item, filterOperator);

            }
            return gateWayResultList;
        }

        public async Task<List<Item_delivery>> searchItem_deliveryFromWebService(Item_delivery Item_delivery, string filterOperator)
        {
            List<Item_delivery> gateWayResultList = new List<Item_delivery>();
            using (GateWayItem gateWayItem = new GateWayItem())
            {
                gateWayItem.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayItem.searchItem_deliveryFromWebService(Item_delivery, filterOperator);

            }
            return gateWayResultList;
        }

        public void Dispose()
        {
            _gateWayItem.PropertyChanged -= onCredentialChange_loadItemDataFromWebService;
        }
    } /* end class BLItem */
}