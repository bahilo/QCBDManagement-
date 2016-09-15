using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.Models
{
    public class Cart_itemModel : BindBase
    {
        private List<Item_deliveryModel> _item_deliveryModelList;
        private decimal _totalPurchasePrice;
        private decimal _totalSellingPrice; // PT
        private decimal _total;
        private decimal _cartTotalPurchasePrice; // PAT
        private decimal _cartTotalSellingPrice; //PTT
        private bool _isSelected;
        private bool _isModifyEnable;
        private Item _item;
        private int _quantity;

        public Cart_itemModel()
        {
            _item = new Item();
            PropertyChanged += onItemOrQuantityChange;
        }

        public Item Item
        {
            get { return _item; }
            set { _item = value; onPropertyChange("Item"); }
        }

        public string TxtCartTotalPurchasePrice
        {
            get { return _cartTotalPurchasePrice.ToString(); }
            set { if (!string.IsNullOrEmpty(value)) { setProperty(ref _cartTotalPurchasePrice, Convert.ToDecimal(value), "TxtCartTotalPurchasePrice"); } }
        }

        public string TxtCartTotalSellingPrice
        {
            get { return _cartTotalSellingPrice.ToString(); }
            set { if (!string.IsNullOrEmpty(value)) { setProperty(ref _cartTotalSellingPrice, Convert.ToDecimal(value), "TxtCartTotalSellingPrice"); } }
        }

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

        public string TxtTotalPurchasePrice
        {
            get { return _totalPurchasePrice.ToString(); }
            set { setProperty(ref _totalPurchasePrice, Convert.ToDecimal(value), "TxtTotalPurchasePrice"); }
        }

        public string TxtTotalSellingPrice
        {
            get { return _totalSellingPrice.ToString(); }
            set { setProperty(ref _totalSellingPrice, Convert.ToDecimal(value), "TxtTotalSellingPrice"); }
        }

        public string TxtTotal
        {
            get { return _total.ToString(); }
            set { setProperty(ref _total, Convert.ToDecimal(value), "TxtTotal"); }
        }

        public bool IsItemSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; onPropertyChange("IsItemSelected"); }
        }

        public string TxtQuantity
        {
            get { return _quantity.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _quantity = converted; onPropertyChange("TxtQuantity"); } }
        }

        private void onItemOrQuantityChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "TxtQuantity"))
            {
                TxtTotalSellingPrice = Convert.ToString(_quantity * Item.Price_sell);
                TxtTotalPurchasePrice = Convert.ToString(_quantity * Item.Price_purchase);
            }
        }
    }
}
