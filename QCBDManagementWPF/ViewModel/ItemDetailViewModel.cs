using QCBDManagementBusiness;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Interfaces;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.ViewModel
{
    public class ItemDetailViewModel : BindBase
    {
        private HashSet<string> _itemFamilyList;
        private HashSet<string> _itemBrandList;
        private HashSet<string> _itemRefList;
        private HashSet<Provider> _providerList;
        private Func<object, object> _page;
        private string _title;

        //----------------------------[ Models ]------------------

        private ItemModel _selectedItemModel;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> btnValidCommand { get; set; }
        public ButtonCommand<string> btnDeleteCommand { get; set; }
        public ButtonCommand<object> SearchCommand { get; set; }


        public ItemDetailViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }



        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            _title = "Item Description";
        }

        private void instances()
        {
            _itemFamilyList = new HashSet<string>();
            _itemBrandList = new HashSet<string>();
            _itemRefList = new HashSet<string>();
            _providerList = new HashSet<Provider>();
        }

        private void instancesModel()
        {
            _selectedItemModel = new ItemModel();
        }

        private void instancesCommand()
        {
            btnValidCommand = new ButtonCommand<string>(saveItem, canSaveItem);
            btnDeleteCommand = new ButtonCommand<string>(deleteItem, canDeleteItem);
            SearchCommand = new ButtonCommand<object>(searchItem, canSearchItem);
        }


        //----------------------------[ Properties ]------------------

        public HashSet<Provider> AllProviderList
        {
            get { return _providerList; }
            set { setProperty(ref _providerList, value, "AllProviderList"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public HashSet<string> ItemRefList
        {
            get { return _itemRefList; }
            set { setProperty(ref _itemRefList, value, "ItemRefList"); }
        }

        public HashSet<string> FamilyList
        {
            get { return _itemFamilyList; }
            set { setProperty(ref _itemFamilyList, value, "FamilyList"); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public ItemModel SelectedItemModel
        {
            get { return _selectedItemModel; }
            set { setProperty(ref _selectedItemModel, value, "SelectedItemModel"); }
        }

        public HashSet<string> BrandList
        {
            get { return _itemBrandList; }
            set { setProperty(ref _itemBrandList, value, "BrandList"); }
        }



        //----------------------------[ Actions ]------------------

        private async Task<List<Provider>> loadProviderFromProvider_item(List<Provider_item> provider_itemFoundList, int userSourceId)
        {
            List<Provider> returnResult = new List<Provider>();
            foreach (var provider_item in provider_itemFoundList)
            {
                Provider searchProvider = new Provider();
                searchProvider.Source = userSourceId;
                searchProvider.Name = provider_item.Provider_name;
                var providerFoundList = await Bl.BlItem.searchProvider(searchProvider, "AND");
                if (providerFoundList.Count > 0)
                    foreach (var provider in providerFoundList)
                        returnResult.Add(provider);
            }
            return returnResult;
        }

        private async Task<List<Provider>> getEntryNewProvider()
        {
            List<Provider> returnResult = new List<Provider>();
            if (!String.IsNullOrEmpty(SelectedItemModel.TxtNewProvider))
            {
                // Check that the new Provider doesn't exist
                var searchProvider = new Provider();
                searchProvider.Name = SelectedItemModel.TxtNewProvider;
                var providerFoundList = await Bl.BlItem.searchProvider(searchProvider, "AND");
                if (providerFoundList.Count == 0)
                {
                    var provider = new Provider();
                    provider.Name = SelectedItemModel.TxtNewProvider;
                    provider.Source = Bl.BlSecurity.GetAuthenticatedUser().ID;
                    SelectedItemModel.SelectedProvider = provider;
                    returnResult.Add(provider);
                }
            }
            else if (SelectedItemModel.SelectedProvider.ID != 0)
                returnResult.Add(SelectedItemModel.SelectedProvider);

            return returnResult;
        }

        private async Task<List<Provider_item>> updateProvider_itemTable(Item item, Provider provider)
        {
            // creating a new reccord in the table provider_item to link the item with its providers
            var provider_itemToSaveList = new List<Provider_item>();
            var provider_item = new Provider_item();
            var returnResult = new List<Provider_item>();

            provider_item.Provider_name = provider.Name;
            var provider_itemFoundList = await Bl.BlItem.searchProvider_item(provider_item, "AND");
            provider_item.Item_ref = item.Ref;
            provider_itemToSaveList.Add(provider_item);

            if (!string.IsNullOrEmpty(provider.Name) && !string.IsNullOrEmpty(item.Ref))
                // Processing in case of a new Item
                if (provider_itemFoundList.Count == 0)
                    returnResult = await Bl.BlItem.InsertProvider_item(provider_itemToSaveList);

                //in case an update
                else
                {
                    // retriving and updating the current provider_item to update
                    var provider_itemToUpdateList =
                            from p_i in provider_itemToSaveList
                            where p_i.Provider_name == SelectedItemModel.SelectedProvider.Name
                            select new Provider_item { ID = p_i.ID, Item_ref = p_i.Item_ref, Provider_name = provider.Name };

                    // saving into database
                    returnResult = await Bl.BlItem.UpdateProvider_item(provider_itemToUpdateList.ToList());
                }

            return returnResult;
        }

        private async void processEntryNewBrand()
        {
            // Check that the new Brand doesn't exist
            if (!String.IsNullOrEmpty(SelectedItemModel.TxtNewBrand))
            {
                var searchItemBrand = new Item();
                searchItemBrand.Type = SelectedItemModel.TxtNewBrand;
                var itemBrandFoundList = await Bl.BlItem.searchItem(searchItemBrand, "AND");
                if (itemBrandFoundList.Count == 0)
                {
                    SelectedItemModel.TxtType = SelectedItemModel.TxtNewBrand;
                }
            }
        }

        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _page = navigObject;
        }

        private async void processEntryNewFamily()
        {
            // Check that the The new family doesn't exist
            if (!String.IsNullOrEmpty(SelectedItemModel.TxtNewFamily))
            {
                var searchItemFamily = new Item();
                searchItemFamily.Type_sub = SelectedItemModel.TxtNewFamily;
                var itemFamilyFoundList = await Bl.BlItem.searchItem(searchItemFamily, "AND");
                if (itemFamilyFoundList.Count == 0)
                {
                    SelectedItemModel.TxtType_sub = SelectedItemModel.TxtNewFamily;
                }
            }
        }

        //----------------------------[ Event Handler ]------------------


        //----------------------------[ Action Commands ]------------------

        private async void deleteItem(string obj)
        {
            Dialog.showSearch("Deleting...");
            await Bl.BlItem.DeleteProvider(SelectedItemModel.ProviderList);
            var notSavedList =  await Bl.BlItem.DeleteItem(new List<Item> { SelectedItemModel.Item  });
            if (notSavedList.Count > 0)
                await Dialog.show("Item deleted successfully!");
            Dialog.IsDialogOpen = false;
            _page(new ItemViewModel());
        }

        private bool canDeleteItem(string arg)
        {
            bool isDelete = securityCheck(QCBDManagementCommon.Enum.EAction.Item, QCBDManagementCommon.Enum.ESecurity._Delete);
            if (isDelete)
                return true;

            return false;
        }

        private async void saveItem(string obj)
        {
            Dialog.showSearch("Please wait while we are dealing with your request...");
            var providerToSaveList = new List<Provider>();
            var auto_refToSaveList = new List<Auto_ref>();
            var itemToSaveList = new List<Item>();
            var provider_itemToSaveList = new List<Provider_item>();

            // we check that the item doesn't already exist
            string newRef = "";
            var searchItem = new Item();
            searchItem.Ref = SelectedItemModel.Item.Ref;
            var itemFoundList = await Bl.BlItem.searchItemFromWebService(searchItem, "AND");
            if (itemFoundList.Count == 0)
            {
                // creating a new reference via the automatic reference system
                var auto_reflist = await Bl.BlItem.GetAuto_refData(999);
                var auto_ref = (auto_reflist.Count > 0) ? auto_reflist[0] : new Auto_ref();
                newRef = "QCBD" + auto_ref.RefId;
                newRef += " : " + SelectedItemModel.Item.Name;
                auto_ref.RefId++;
                auto_refToSaveList.Add(auto_ref);

                // Process the field New Brand in case of updated by user
                processEntryNewBrand();
                // Process the field New Family in case of updated by user
                processEntryNewFamily();

                SelectedItemModel.Item.Name = newRef;
                SelectedItemModel.Item.Source = Bl.BlSecurity.GetAuthenticatedUser().ID;
                SelectedItemModel.Item.Erasable = EItem.No.ToString();
                itemToSaveList.Add(SelectedItemModel.Item);

                var providerSavedList = await Bl.BlItem.InsertProvider(await getEntryNewProvider());
                foreach (var auto_refToSave in auto_refToSaveList)
                {
                    if (auto_refToSave.ID == 0)
                        await Bl.BlItem.InsertAuto_ref(auto_refToSaveList);
                    else
                        await Bl.BlItem.UpdateAuto_ref(auto_refToSaveList);
                }
                var itemSavedList = await Bl.BlItem.InsertItem(itemToSaveList);
                if (itemSavedList.Count > 0)
                    await Dialog.show("Item has been created successfully!");
                var provider_itemResultList = updateProvider_itemTable(itemSavedList[0], ((providerSavedList.Count > 0) ? providerSavedList[0] : new Provider()));

                SelectedItemModel.ProviderList = await loadProviderFromProvider_item(await provider_itemResultList, SelectedItemModel.Item.Source);
            }
            // Otherwise update the current item
            else
            {
                itemToSaveList.Add(SelectedItemModel.Item);
                var providerSavedList = await Bl.BlItem.UpdateProvider(await getEntryNewProvider());
                itemToSaveList = await Bl.BlItem.UpdateItem(itemToSaveList);
                provider_itemToSaveList = await updateProvider_itemTable(itemToSaveList[0], ((providerSavedList.Count > 0) ? providerSavedList[0] : new Provider()));
                
                // update of the item providers of the selected item
                SelectedItemModel.ProviderList = await loadProviderFromProvider_item(provider_itemToSaveList, SelectedItemModel.Item.Source);

                if (itemToSaveList.Count > 0)
                    await Dialog.show("Item has been updated successfully!");
            }

            Dialog.IsDialogOpen = false;
        }

        private bool canSaveItem(string arg)
        {
            bool isUpdate = securityCheck(QCBDManagementCommon.Enum.EAction.Item, QCBDManagementCommon.Enum.ESecurity._Update);
            bool isWrite = securityCheck(QCBDManagementCommon.Enum.EAction.Item, QCBDManagementCommon.Enum.ESecurity._Write);
            if (isUpdate && isWrite)
                return true;

            return false;
        }

        private async void searchItem(object obj)
        {
            var itemFoundList = await Bl.BlItem.searchItem(new Item { Ref = SelectedItemModel.Item.Ref }, "AND");
            if (itemFoundList.Count > 0)
            {
                ItemModel itemModel = new ItemModel();
                itemModel.Item = itemFoundList[0];
                SelectedItemModel = itemModel;
            }
        }

        private bool canSearchItem(object arg)
        {
            return true;
        }

    }
}
