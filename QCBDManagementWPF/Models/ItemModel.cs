using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace QCBDManagementWPF.Models
{
    public class ItemModel: BindBase
    {       
        private List<Item_deliveryModel> _item_deliveryModelList;
        private List<Provider> _providerList;
        //private List<Provider> _providerList;
        //private decimal _totalPurchasePrice;
        //private decimal _totalSellingPrice; // PT
        //private decimal _total;
        private string _selectedBrand;
        private string _newBrand;
        private string _selectedFamily;
        private string _newFamily;
        private string _newProvider;
        private Provider _selectedProvider;
        //private decimal _cartTotalPurchasePrice; // PAT
        //private decimal _cartTotalSellingPrice; //PTT
        private bool _isSelected;
        private bool _isModifyEnable;
        private bool _isSearchByItemName;
        private bool _isExactMatch;
        private bool _isDeepSearch;
        private Item _item;

        public ItemModel()
        {
            _item_deliveryModelList = new List<Item_deliveryModel>();
            _providerList = new List<Provider>();
            _selectedProvider = new Provider();
            _item = new Item();

            PropertyChanged += onItemChange;
            //PropertyChanged += onNewProviderChange;
            //PropertyChanged += onNewBrandChange;
            //PropertyChanged += onNewFamilyChange;
        }

        private void onItemChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Item"))
            {
                foreach (var item_deliveryModel in Item_deliveryModelList)
                {
                    item_deliveryModel.Item = Item;
                }
            }
        }

        public string SelectedBrand
        {
            get { return _selectedBrand; }
            set { setProperty(ref _selectedBrand, value, "SelectedBrand"); }
        }

        public string SelectedFamily
        {
            get { return _selectedFamily; }
            set { setProperty(ref _selectedFamily, value, "SelectedFamily"); }
        }
        public bool IsSearchByItemName
        {
            get { return _isSearchByItemName; }
            set { _isSearchByItemName = value; onPropertyChange("IsSearchByItemName"); }
        }

        public bool IsExactMatch
        {
            get { return _isExactMatch; }
            set { _isExactMatch = value; onPropertyChange("IsExactMatch"); }
        }

        public bool IsDeepSearch
        {
            get { return _isDeepSearch; }
            set { _isDeepSearch = value; onPropertyChange("IsDeepSearch"); }
        }

        public bool IsRefModifyEnable
        {
            get { return _isModifyEnable; }
            set { setProperty(ref _isModifyEnable, value, "IsModifyEnable"); }
        }

        public List<Provider> ProviderList
        {
            get { return _providerList; }
            set { setProperty(ref _providerList, value, "ProviderList"); }
        }

        public Provider SelectedProvider
        {
            get { return _selectedProvider; }
            set { setProperty(ref _selectedProvider, value, "SelectedProvider"); }
        }/**/

        public Item Item
        {
            get { return _item; }
            set { _item = value; onPropertyChange("Item"); }
        }

        public List<Item_deliveryModel> Item_deliveryModelList
        {
            get { return _item_deliveryModelList; }
            set { setProperty(ref _item_deliveryModelList, value, "Item_deliveryModelList"); }
        }

        public string TxtNewProvider
        {
            get { return _newProvider; }
            set { setProperty(ref _newProvider, value, "TxtNewProvider"); }
        }

        public string TxtNewBrand
        {
            get { return _newBrand; }
            set { setProperty(ref _newBrand, value, "TxtNewBrand"); }
        }

        public string TxtNewFamily
        {
            get { return _newFamily; }
            set { setProperty(ref _newFamily, value, "TxtNewFamily"); }
        }

        /*public string TxtCartTotalPurchasePrice
        {
            get { return _cartTotalPurchasePrice.ToString(); }
            set { if (!string.IsNullOrEmpty(value)) { setProperty(ref _cartTotalPurchasePrice, Convert.ToDecimal(value), "TxtCartTotalPurchasePrice"); } }
        }

        public string TxtCartTotalSellingPrice
        {
            get { return _cartTotalSellingPrice.ToString(); }
            set { if (!string.IsNullOrEmpty(value)) { setProperty(ref _cartTotalSellingPrice, Convert.ToDecimal(value), "TxtCartTotalSellingPrice"); } }
        }*/

        public string TxtID
        {
            get { return _item.ID.ToString(); }
            set { if (!string.IsNullOrEmpty(value)) { _item.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); } }
        }

        public string TxtRef
        {
            get { return Item.Ref; }
            set { Item.Ref = value; onPropertyChange("TxtRef"); }
        }

        public string TxtName
        {
            get { return Item.Name; }
            set { Item.Name = value; onPropertyChange("TxtName"); }
        }

        public string TxtType
        {
            get { return Item.Type; }
            set { Item.Type = value; onPropertyChange("TxtType"); }
        }

        public string TxtType_sub
        {
            get { return Item.Type_sub; }
            set { Item.Type_sub = value; onPropertyChange("TxtType_sub"); }
        }

        public string TxtPrice_purchase
        {
            get { return Item.Price_purchase.ToString(); }
            set { if (!string.IsNullOrEmpty(value)) { Item.Price_purchase = Convert.ToDecimal(value); onPropertyChange("TxtPrice_purchase"); } }
        }

        public string TxtPrice_sell
        {
            get { return Item.Price_sell.ToString(); }
            set { if (!string.IsNullOrEmpty(value)) { Item.Price_sell = Convert.ToDecimal(value); onPropertyChange("TxtPrice_sell"); } }
        }

        public string TxtSource
        {
            get { return Item.Source.ToString(); }
            set { if (!string.IsNullOrEmpty(value)) { Item.Source = Convert.ToInt32(value); onPropertyChange("TxtSource"); } }
        }

        public string TxtComment
        {
            get { return Item.Comment; }
            set { Item.Comment = value; onPropertyChange("TxtComment"); }
        }

        public string TxtErasable
        {
            get { return Item.Erasable; }
            set { Item.Erasable = value; onPropertyChange("TxtErasable"); }
        }

        /*public string TxtTotalPurchasePrice
        {
            get { return _totalPurchasePrice.ToString(); }
            set { setProperty(ref _totalPurchasePrice, Convert.ToDecimal(value),"TxtTotalPurchasePrice"); }
        }

        public string TxtTotalSellingPrice
        {
            get { return _totalSellingPrice.ToString(); }
            set { setProperty(ref _totalSellingPrice,Convert.ToDecimal(value),"TxtTotalSellingPrice"); }
        }

        public string TxtTotal
        {
            get { return _total.ToString(); }
            set { setProperty(ref _total, Convert.ToDecimal(value),"TxtTotal"); }
        }

        public string TxtQuantity
        {
            get { return _item; }
            set { if (!string.IsNullOrEmpty(value)) { _item_deliveryModelList.TxtQuantity_delivery = value; onPropertyChange("TxtQuantity"); } }
        }*/

        public bool IsItemSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; onPropertyChange("IsItemSelected"); }
        }

        /*private void onItemOrQuantityChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "Item") || string.Equals(e.PropertyName, "TxtQuantity"))
            {
                TxtTotalSellingPrice = Convert.ToString(Convert.ToInt32(_item_deliveryModelList.TxtQuantity_delivery) * _item_deliveryModelList.Item.Price_sell);
                TxtTotalPurchasePrice = Convert.ToString(Convert.ToInt32(_item_deliveryModelList.TxtQuantity_delivery) * _item_deliveryModelList.Item.Price_purchase);
            }
        }*/

        

    }
}
