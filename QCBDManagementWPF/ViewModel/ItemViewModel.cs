using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementBusiness;
using QCBDManagementWPF.Classes;
using QCBDManagementCommon.Enum;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using QCBDManagementWPF.Models;
using System.Threading;
using System.Diagnostics;
using QCBDManagementWPF.Interfaces;
using System.Windows.Threading;
using System.Globalization;

namespace QCBDManagementWPF.ViewModel
{
    public class ItemViewModel : BindBase
    {
        private Cart _cart;
        private List<string> _cbSearchCriteriaList;
        private Func<Object, Object> _page;
        private Func<Object, Object> _currentSideBarViewModelFunc;
        private Func<string, object> _getObjectFromMainWindowViewModel;
        private List<Item> _items;
        private string _searchItemName;
        private string _title;
        private bool _isSearchResult;

        //----------------------------[ Models ]------------------

        private ItemModel _itemModel;
        private IEnumerable<ItemModel> _itemsModel;   
        private ItemCreateViewModel _itemCreateViewModel;
        private ItemDetailViewModel _itemDetailViewModel;
        private ItemSideBarViewModel _itemSideBarViewModel;
        

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> checkBoxSearchCommand { get; set; }
        public ButtonCommand<ItemModel> checkBoxToCartCommand { get; set; }
        public ButtonCommand<string> btnSearchCommand { get; set; }
        public ButtonCommand<Cart_itemModel> DeleteFromCartCommand { get; set; }
        public ButtonCommand<string> NavigCommand { get; set; }
        public ButtonCommand<ItemModel> GetCurrentItemCommand { get; set; }
        public ButtonCommand<object> GoToQuoteCommand { get; set; }


        public ItemViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }



        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onStartupChange;
            PropertyChanged += onDialogChange;
            ItemDetailViewModel.PropertyChanged += onSelectedItemChange;
        }

        private void instances()
        {
            _title = "Catalog Management";
            _cart = new Cart();
            _cbSearchCriteriaList = new List<string>();
            _items = new List<Item>();
        }

        private void instancesModel()
        {
            _itemModel = new ItemModel();
            _itemCreateViewModel = new ItemCreateViewModel();
            _itemDetailViewModel = new ItemDetailViewModel();
            _itemSideBarViewModel = new ItemSideBarViewModel();            
        }

        private void instancesCommand()
        {
            checkBoxSearchCommand = new ButtonCommand<string>(saveSearchChecks, canSaveSearchChecks);
            checkBoxToCartCommand = new ButtonCommand<ItemModel>(saveCartChecks, canSaveCartChecks);
            btnSearchCommand = new ButtonCommand<string>(filterItem, canFilterItem);
            DeleteFromCartCommand = new ButtonCommand<Cart_itemModel>(deleteItemFromCart, canDeleteItemFromCart);
            NavigCommand = new ButtonCommand<string>(executeNavig, canExecuteNavig);
            GetCurrentItemCommand = new ButtonCommand<ItemModel>(saveSelectedItem, canSaveSelectedItem);
            GoToQuoteCommand = new ButtonCommand<object>(gotoQuote, canGoToQuote);
        }

        //----------------------------[ Properties ]------------------

        public string SearchItemName
        {
            get { return _searchItemName; }
            set { setProperty(ref _searchItemName, value, "SearchItemName"); }
        }

        public BusinessLogic Bl
        {
            get {  return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public ItemDetailViewModel ItemDetailViewModel
        {
            get { return _itemDetailViewModel; }
            set { setProperty(ref _itemDetailViewModel, value, "ItemDetailViewModel"); }
        }

        public ItemSideBarViewModel ItemSideBarViewModel
        {
            get { return _itemSideBarViewModel; }
            set { setProperty(ref _itemSideBarViewModel, value, "ItemSideBarViewModel"); }
        }

        public ItemModel ItemModel
        {
            get { return _itemModel; }
            set { setProperty(ref _itemModel, value,"ItemModel"); }
        }

        public List<Item> Items
        {
            get { return _items; }
            set { setProperty(ref _items, value,"Items"); }
        }

        public IEnumerable<ItemModel> ItemModelList
        {
            get { return _itemsModel; }
            set { setProperty(ref _itemsModel, value, "ItemModelList"); }
        }

        public HashSet<string> FamilyList
        {
            get { return ItemDetailViewModel.FamilyList; }
            set { ItemDetailViewModel.FamilyList = value; onPropertyChange("FamilyList"); }
        }

        public HashSet<string> BrandList
        {
            get { return ItemDetailViewModel.BrandList; }
            set { ItemDetailViewModel.BrandList = value; onPropertyChange("BrandList"); }
        }

        public Cart Cart
        {
            get { return _cart; }
            set { setProperty(ref _cart, value, "Cart"); }
        }

        public ItemModel SelectedItemModel
        {
            get { return ItemDetailViewModel.SelectedItemModel; }
            set { ItemDetailViewModel.SelectedItemModel = value; onPropertyChange("SelectedItemModel"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }


        //----------------------------[ Actions ]------------------

        public async void loadItems()
        {
            Dialog.showSearch("Loading...");
            // check if search mode to display only search result
            if (!_isSearchResult)
            {
                ItemModelList = await itemListToModelViewList(await Bl.BlItem.GetItemData(999));
                _cbSearchCriteriaList = new List<string>();
                ItemDetailViewModel.AllProviderList = new HashSet<Provider>(await Bl.BlItem.GetProviderData(999)); // convert provider
                //loadBrandAndFamilyList();
            }
            _isSearchResult = false;
            Dialog.IsDialogOpen = false;
        }
        
        public async Task<List<Item_deliveryModel>> item_deliveryListToModelList(List<Item_delivery> item_deliveryList)
        {
            //var task = Task.Factory.StartNew(async () =>
            //{
            List<Item_deliveryModel> output = new List<Item_deliveryModel>();
            System.Collections.Concurrent.ConcurrentBag<Item_deliveryModel> concurrentItem_deliveryModelList = new System.Collections.Concurrent.ConcurrentBag<Item_deliveryModel>();
            foreach (var item_delivery in item_deliveryList)
            //Parallel.ForEach(item_deliveryList, async (item_delivery) =>
            {
                Item_deliveryModel idm = new Item_deliveryModel();
                idm.Item_delivery = item_delivery;
                var deliveryList = new DeliveryModel().DeliveryListToModelViewList(await Bl.BlCommand.searchDelivery(new Delivery { ID = item_delivery.DeliveryId }, "AND"));
                idm.DeliveryModel = (deliveryList.Count > 0) ? deliveryList[0] : new DeliveryModel();
                concurrentItem_deliveryModelList.Add(idm);

            }
            //);
            output = new List<Item_deliveryModel>(concurrentItem_deliveryModelList);
            return output;
            //});

            //return await task;
        }

        private async Task<List<ItemModel>> itemListToModelViewList(List<Item> itemtList)
        {
            List<ItemModel> output = new List<ItemModel>();
            object _lock = new object();
            ItemDetailViewModel.FamilyList.Clear();
            ItemDetailViewModel.ItemRefList.Clear();
            ItemDetailViewModel.BrandList.Clear();

            foreach (var item in itemtList)
            //Parallel.ForEach(itemtList, async (item) =>
           {
               ItemModel ivm = new ItemModel();

               ivm.Item = item;
               Provider_item searchProvider_item = new Provider_item();

               searchProvider_item.Item_ref = item.Ref;
               var provider_itemFoundList = await Bl.BlItem.searchProvider_item(searchProvider_item, "AND");

               // getting all providers for each item
               ivm.ProviderList = await loadProviderFromProvider_item(provider_itemFoundList, item.Source);
               
                lock (_lock)
               {
                   if (ivm.ProviderList.Count > 0)
                       ivm.SelectedProvider =  ivm.ProviderList.OrderByDescending(x => x.ID).First();
                   ItemDetailViewModel.FamilyList.Add(item.Type_sub);
                   ItemDetailViewModel.ItemRefList.Add(item.Ref);
                   ItemDetailViewModel.BrandList.Add(item.Type);
                   output.Add(ivm);
               }
               
           }
            //);
            return output;
        }

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
                    returnResult.Concat(providerFoundList);
            }
            return returnResult;
        }

        /*private void contextManagement(IState nextPage)
        {
            var mainViewModel = _getObjectFromMainWindowViewModel("main") as MainWindowViewModel;
            if (mainViewModel != null)
            {
                mainViewModel.Context.PreviousState = this;
                mainViewModel.Context.NextState = nextPage;
            }
        }*/
        
        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _page = navigObject;
            ItemDetailViewModel.mainNavigObject(navigObject);
            ItemSideBarViewModel.mainNavigObject(navigObject);
        }

        internal void sideBarContentManagement(Func<object, object> sideBarManagement)
        {
            _currentSideBarViewModelFunc = sideBarManagement;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            _getObjectFromMainWindowViewModel = getObject;
        }

        internal void setInitCart(ref Cart cart)
        {
            Cart = cart;
        }

        //----------------------------[ Event Handler ]------------------

        private void onSelectedItemChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "SelectedItemModel"))
            {
                executeNavig("item-detail");
                ItemSideBarViewModel.SelectedItem = SelectedItemModel;
            }
        }

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                _itemDetailViewModel.Startup = Startup;
                _itemSideBarViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                _itemDetailViewModel.Dialog = Dialog;
                _itemSideBarViewModel.Dialog = Dialog;
            }
        }

        //----------------------------[ Action Commands ]------------------

        private async void filterItem(string obj)
        {
            Dialog.showSearch("Searching...");
            Item item = new Item();
            List<Item> results = new List<Item>();
            string filterOperator = "";
            /*bool isRestrict = false;
            bool isDeep = false;

            foreach (string checkedValue in _cbSearchCriteriaList)
            {
                switch (checkedValue)
                {
                    case "cbName":
                        item.Name = obj;
                        break;
                    case "cbMatch":
                        isRestrict = true;
                        break;
                    case "cbDeep":
                        isDeep = true;
                        break;
                }                
            }*/
            item.Ref = obj;
            item.Name = ItemModel.TxtName;
            item.Type = ItemModel.SelectedBrand;
            item.Type_sub = ItemModel.SelectedFamily;            

            if (ItemModel.IsExactMatch) { filterOperator = "AND"; }
            else { filterOperator = "OR"; }

            if (ItemModel.IsDeepSearch) { results = await Bl.BlItem.searchItemFromWebService(item, filterOperator); }
            else { results = await Bl.BlItem.searchItem(item, filterOperator); }
            
            if (ItemModel.IsSearchByItemName) { results = results.Where(x=> x.Name.IndexOf(obj, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList(); }

            ItemModelList = await itemListToModelViewList(results);
            _isSearchResult = true;          

            ItemModel.SelectedBrand = null;
            ItemModel.SelectedFamily = null;
            ItemModel.IsExactMatch = false;
            ItemModel.IsDeepSearch = false;
            ItemModel.IsSearchByItemName = false;
            ItemModel.TxtName = "";
            Dialog.IsDialogOpen = false;
            _page(this);            
        }

        public void saveSearchChecks(string obj)
        {
            if (!_cbSearchCriteriaList.Contains(obj))
            { _cbSearchCriteriaList.Add(obj); }                
            else { _cbSearchCriteriaList.Remove(obj); }                
        }

        private bool canSaveSearchChecks(string arg)
        {
            return true;
        }        

        private void saveCartChecks(ItemModel obj)
        {
            if (Cart.CartItemList.Where(x=>x.Item.ID == obj.Item.ID).Count() == 0)
            {
                var cart_itemModel = new Cart_itemModel();
                cart_itemModel.Item = obj.Item;
                cart_itemModel.TxtQuantity = 1.ToString();
                Cart.AddItem(cart_itemModel);
            }
            else
            {
                foreach(var cart_itemModel in Cart.CartItemList.Where(x => x.Item.ID == obj.Item.ID).ToList())
                    Cart.RemoveItem(cart_itemModel);
            }
            GoToQuoteCommand.raiseCanExecuteActionChanged();
        }

        private bool canSaveCartChecks(ItemModel arg)
        {
            return true;
        }

        private bool canFilterItem(string arg)
        {
            return true;
        }     
        

        private bool canDeleteItemFromCart(Cart_itemModel arg)
        {
            return true;
        }

        private void deleteItemFromCart(Cart_itemModel obj)
        {
            //foreach (var cart_itemModel in Cart.CartItemList.Where(x => x.Item.ID == obj.Item.ID).ToList())
            Cart.CartItemList.Remove(obj);
            ItemModelList.Where(x=>x.TxtID == obj.TxtID).Single().IsItemSelected = false;
        }

        private void saveSelectedItem(ItemModel obj)
        {
            SelectedItemModel = obj;
        }

        private bool canSaveSelectedItem(ItemModel arg)
        {
            return true;
        }


        public void executeNavig(string obj)
        {
            
            _currentSideBarViewModelFunc(ItemSideBarViewModel);
            switch (obj)
            {
                case "item":
                    //contextManagement(this);
                    _page(this);
                    break;
                case "item-detail":
                    SelectedItemModel.IsRefModifyEnable = false;
                    //contextManagement(ItemDetailViewModel);
                    _page(ItemDetailViewModel);
                    break;
                case "item-update":
                    SelectedItemModel.IsRefModifyEnable = false;
                    //contextManagement(ItemDetailViewModel);
                    _page(_itemDetailViewModel);
                    break;
                default:
                    goto case "item";
            }
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }

        private void gotoQuote(object obj)
        {
            _page(new QuoteViewModel());
            _currentSideBarViewModelFunc(new CommandSideBarViewModel());
        }

        private bool canGoToQuote(object arg)
        {
            bool isRead = securityCheck(EAction.Quote, ESecurity._Read);
            if (isRead && Cart.CartItemList.Count > 0)
                return true;


            return false;
        }



    }
}
