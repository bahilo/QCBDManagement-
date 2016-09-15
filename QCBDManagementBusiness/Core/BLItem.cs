using QCBDManagementBusiness.Helper;
using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
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
    public class BLItem : IItemManager
    {
        // Attributes

        public QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DAC { get; set; }

        public BLItem(QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }
        
        public async Task<List<Item>> InsertItem(List<Item> itemList)
        {
            if (itemList == null  || itemList.Count == 0)
                return new List<Item>();

            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.InsertItem(itemList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider>> InsertProvider(List<Provider> listProvider)
        {
            if (listProvider == null || listProvider.Count == 0)
                return new List<Provider>();

            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.InsertProvider(listProvider);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider_item>> InsertProvider_item(List<Provider_item> listProvider_item)
        {
            if (listProvider_item == null || listProvider_item.Count == 0)
                return new List<Provider_item>();

            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.InsertProvider_item(listProvider_item);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }
        

        public async Task<List<Item_delivery>> InsertItem_delivery(List<Item_delivery> listItem_delivery)
        {
            if (listItem_delivery == null || listItem_delivery.Count == 0)
                return new List<Item_delivery>();

            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.InsertItem_delivery(listItem_delivery);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Auto_ref>> InsertAuto_ref(List<Auto_ref> listAuto_ref)
        {
            if (listAuto_ref == null || listAuto_ref.Count == 0)
                return new List<Auto_ref>();

            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = await DAC.DALItem.InsertAuto_ref(listAuto_ref);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_item>> InsertTax_item(List<Tax_item> listTax_item)
        {
            if (listTax_item == null || listTax_item.Count == 0)
                return new List<Tax_item>();

            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.InsertTax_item(listTax_item);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item>> DeleteItem(List<Item> itemList)
        {
            if (itemList == null || itemList.Count == 0)
                return new List<Item>();

            if (itemList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting items(count = " + itemList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.DeleteItem(itemList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider>> DeleteProvider(List<Provider> listProvider)
        {
            if (listProvider == null || listProvider.Count == 0)
                return new List<Provider>();

            if (listProvider.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting providers(count = " + listProvider.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.DeleteProvider(listProvider);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider_item>> DeleteProvider_item(List<Provider_item> listProvider_item)
        {
            if (listProvider_item == null || listProvider_item.Count == 0)
                return new List<Provider_item>();

            if (listProvider_item.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting provider_items(count = " + listProvider_item.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.DeleteProvider_item(listProvider_item);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        
        public async Task<List<Item_delivery>> DeleteItem_delivery(List<Item_delivery> listItem_delivery)
        {
            if (listItem_delivery == null || listItem_delivery.Count == 0)
                return new List<Item_delivery>();

            if (listItem_delivery.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting item_deliveries(count = " + listItem_delivery.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.DeleteItem_delivery(listItem_delivery);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Auto_ref>> DeleteAuto_ref(List<Auto_ref> listAuto_ref)
        {
            if (listAuto_ref == null || listAuto_ref.Count == 0)
                return new List<Auto_ref>();

            if (listAuto_ref.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting auto_refs(count = " + listAuto_ref.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = await DAC.DALItem.DeleteAuto_ref(listAuto_ref);
            }
            catch (Exception ex )
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_item>> DeleteTax_item(List<Tax_item> listTax_item)
        {
            if (listTax_item == null || listTax_item.Count == 0)
                return new List<Tax_item>();

            if (listTax_item.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting tax_items(count = " + listTax_item.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.DeleteTax_item(listTax_item);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            // BLHelper.check("Delete", "Count", 0, "DeleteTax_item", result.Count);
            return result;
        }

        public async Task<List<Item>> UpdateItem(List<Item> itemList)
        {
            if (itemList == null || itemList.Count == 0)
                return new List<Item>();

            if (itemList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating items(count = " + itemList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.UpdateItem(itemList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider>> UpdateProvider(List<Provider> listProvider)
        {
            if (listProvider == null || listProvider.Count == 0)
                return new List<Provider>();

            if (listProvider.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating providers(count = " + listProvider.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");
            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.UpdateProvider(listProvider);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider_item>> UpdateProvider_item(List<Provider_item> listProvider_item)
        {
            if (listProvider_item == null || listProvider_item.Count == 0)
                return new List<Provider_item>();

            if (listProvider_item.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating provider_items(count = " + listProvider_item.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.UpdateProvider_item(listProvider_item);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        

        public async Task<List<Item_delivery>> UpdateItem_delivery(List<Item_delivery> listItem_delivery)
        {
            if (listItem_delivery == null || listItem_delivery.Count == 0)
                return new List<Item_delivery>();

            if (listItem_delivery.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating item_deliveries(count = " + listItem_delivery.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.UpdateItem_delivery(listItem_delivery);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Auto_ref>> UpdateAuto_ref(List<Auto_ref> listAuto_ref)
        {
            if (listAuto_ref == null || listAuto_ref.Count == 0)
                return new List<Auto_ref>();

            if (listAuto_ref.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating auto_refs(count = " + listAuto_ref.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = await DAC.DALItem.UpdateAuto_ref(listAuto_ref);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_item>> UpdateTax_item(List<Tax_item> listTax_item)
        {
            if (listTax_item != null || listTax_item.Count == 0)
                return new List<Tax_item>();

            if (listTax_item.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating tax_items(count = " + listTax_item.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.UpdateTax_item(listTax_item);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item>> GetItemData(int nbLine)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.GetItemData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item>> GetItemDataByCommand_itemList(List<Command_item> command_itemList)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.GetItemDataByCommand_itemList(command_itemList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item>> GetItemDataById(int id)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.GetItemDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider>> GetProviderData(int nbLine)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.GetProviderData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider>> GetProviderDataByProvider_itemList(List<Provider_item> provider_itemList)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.GetProviderDataByProvider_itemList(provider_itemList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider>> GetProviderDataById(int id)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.GetProviderDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemData(int nbLine)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.GetProvider_itemData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemDataByItemList(List<Item> itemList)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.GetProvider_itemDataByItemList(itemList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider_item>> GetProvider_itemDataById(int id)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.GetProvider_itemDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item_delivery>> GetItem_deliveryData(int nbLine)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.GetItem_deliveryData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataByDeliveryList(List<Delivery> deliveryList)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.GetItem_deliveryDataByDeliveryList(deliveryList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item_delivery>> GetItem_deliveryDataById(int id)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.GetItem_deliveryDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Auto_ref>> GetAuto_refData(int nbLine)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = await DAC.DALItem.GetAuto_refData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Auto_ref>> GetAuto_refDataById(int id)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = await DAC.DALItem.GetAuto_refDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_item>> GetTax_itemData(int nbLine)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.GetTax_itemData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_item>> GetTax_itemDataByItemList(List<Item> itemList)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.GetTax_itemDataByItemList(itemList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_item>> GetTax_itemDataById(int id)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.GetTax_itemDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item>> searchItem(Item item, string filterOperator)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.searchItem(item, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider>> searchProvider(Provider Provider, string filterOperator)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.searchProvider(Provider, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider_item>> searchProvider_item(Provider_item Provider_item, string filterOperator)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.searchProvider_item(Provider_item, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item_delivery>> searchItem_delivery(Item_delivery Item_delivery, string filterOperator)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.searchItem_delivery(Item_delivery, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Auto_ref>> searchAuto_ref(Auto_ref Auto_ref, string filterOperator)
        {
            List<Auto_ref> result = new List<Auto_ref>();
            try
            {
                result = await DAC.DALItem.searchAuto_ref(Auto_ref, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Tax_item>> searchTax_item(Tax_item Tax_item, string filterOperator)
        {
            List<Tax_item> result = new List<Tax_item>();
            try
            {
                result = await DAC.DALItem.searchTax_item(Tax_item, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item>> searchItemFromWebService(Item item, string filterOperator)
        {
            List<Item> result = new List<Item>();
            try
            {
                result = await DAC.DALItem.searchItemFromWebService(item, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider>> searchProviderFromWbService(Provider Provider, string filterOperator)
        {
            List<Provider> result = new List<Provider>();
            try
            {
                result = await DAC.DALItem.searchProviderFromWebService(Provider, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Provider_item>> searchProvider_itemFromWebService(Provider_item Provider_item, string filterOperator)
        {
            List<Provider_item> result = new List<Provider_item>();
            try
            {
                result = await DAC.DALItem.searchProvider_itemFromWebService(Provider_item, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Item_delivery>> searchItem_deliveryFromWebService(Item_delivery Item_delivery, string filterOperator)
        {
            List<Item_delivery> result = new List<Item_delivery>();
            try
            {
                result = await DAC.DALItem.searchItem_deliveryFromWebService(Item_delivery, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public Task<List<Provider>> searchProviderFromWebService(Provider Provider, string filterOperator)
        {
            throw new NotImplementedException();
        }
    } /* end class BLItem */
}