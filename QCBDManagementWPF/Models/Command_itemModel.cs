using QCBDManagementBusiness;
using Entity = QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementCommon.Classes;

namespace QCBDManagementWPF.Models
{
    public class Command_itemModel : BindBase
    {
        private Entity.Command_item _command_item;
        private ItemModel _itemModel;
        private decimal _percentProfit;
        private decimal _profit;
        private decimal _totalPurchase;
        private decimal _totalSelling; // PT
        private int _quantityPending;
        private int _quantityReceived;
        private int _nbPackages;

        public Command_itemModel()
        {
            _command_item = new Entity.Command_item();
            _itemModel = new ItemModel();
            _nbPackages = 1;
            PropertyChanged += onQuantityChange;
            PropertyChanged += onAmountSellOrPurchaseChange;
            //PropertyChanged += onItemModelChange;
            PropertyChanged += onCommand_itemChange;
            //ItemModel.PropertyChanged += onItemModelOrItemChange;
        }

        public string TxtItemId
        {
            get { return _command_item.ItemId.ToString(); }
            set { _command_item.ItemId = Convert.ToInt32(value); onPropertyChange("TxtItemId"); }
        }

        public Entity.Command_item Command_Item
        {
            get { return _command_item; }
            set { setProperty(ref _command_item, value,"Command_Item"); }
        }

        public ItemModel ItemModel
        {
            get { return _itemModel; }
            set { setProperty(ref _itemModel, value, "ItemModel"); }
        }

        public string TxtTotalPurchase
        {
            get { return _totalPurchase.ToString(); }
            set { setProperty(ref _totalPurchase, Convert.ToDecimal(value), "TxtTotalPurchase"); }
        }

        public string TxtTotalSelling
        {
            get { return _totalSelling.ToString(); }
            set { setProperty(ref _totalSelling, Convert.ToDecimal(value), "TxtTotalSelling"); }
        }

        public string TxtProfit
        {
            get { return _profit.ToString(); }
            set { setProperty(ref _profit, Convert.ToDecimal(value),"TxtProfit"); }
        }

        public string TxtPercentProfit
        {
            get { return _percentProfit.ToString(); }
            set { setProperty(ref _percentProfit, Convert.ToDecimal(value),"TxtPercentProfit"); }
        }

        public string TxtID
        {
            get { return _command_item.ID.ToString(); }
            set { _command_item.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string TxtCommandId
        {
            get { return _command_item.CommandId.ToString(); }
            set { _command_item.CommandId = Convert.ToInt32(value); onPropertyChange("TxtCommandId"); }
        }

        public string TxtItem_ref
        {
            get { return _command_item.Item_ref; }
            set { _command_item.Item_ref = value; onPropertyChange("TxtItems_ref"); }
        }

        public string TxtQuantity
        {
            get { return _command_item.Quantity.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _command_item.Quantity = converted; onPropertyChange("TxtQuantity"); } }
        }

        public string TxtQuantity_delivery
        {
            get { return _command_item.Quantity_delivery.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _command_item.Quantity_delivery = converted; onPropertyChange("TxtQuantity_delivery"); } }
        }

        public string TxtQuantity_current
        {
            get { return _command_item.Quantity_current.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _command_item.Quantity_current = converted; onPropertyChange("TxtQuantity_current"); } }
        }

        public string TxtQuantity_received
        {
            get { return _quantityReceived.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) setProperty(ref _quantityReceived, converted, "TxtQuantity_received"); }
        }

        public string TxtQuantity_pending
        {
            get { return (_quantityPending = _command_item.Quantity - Command_Item.Quantity_delivery).ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) setProperty(ref _quantityPending, converted, "TxtQuantity_pending"); }
        }

        public string TxtPrice
        {
            get { return _command_item.Price.ToString(); }
            set { decimal converted; if (decimal.TryParse(value, out converted)) { _command_item.Price = converted; onPropertyChange("TxtPrice"); } }
        }

        public string TxtPrice_purchase
        {
            get { return _command_item.Price_purchase.ToString(); }
            set { decimal converted; if (decimal.TryParse(value, out converted)) { _command_item.Price_purchase = converted; onPropertyChange("TxtPrice_purchase"); } }
        }

        public string TxtComment_Purchase_Price
        {
            get { return _command_item.Comment_Purchase_Price; }
            set { _command_item.Comment_Purchase_Price = value; onPropertyChange("TxtComment_Purchase_Price"); }
        }

        public string TxtOrder
        {
            get { return _command_item.Order.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _command_item.Order = converted; onPropertyChange("TxtOrder"); } }
        }

        public string TxtPackage
        {
            get { return _nbPackages.ToString(); }
            set { int converted; if(int.TryParse(value, out converted))setProperty(ref _nbPackages, converted, "TxtPackage"); }
        }

        private void onQuantityChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "TxtQuantity") || string.Equals(e.PropertyName, "Command_Item"))
            {
                //this.ItemModel.TxtQuantity = TxtQuantity;       
                TxtTotalSelling = Convert.ToString(_command_item.Quantity * _command_item.Price);
                TxtTotalPurchase = Convert.ToString(_command_item.Quantity * _command_item.Price_purchase);
            }
        }

        private void onCommand_itemChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "Command_Item"))
            {
                //loadCommand_itemItem();
                profitCalcul();
            }
        }
        

        private void onAmountSellOrPurchaseChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtPrice") || e.PropertyName.Equals("TxtPrice_purchase"))
            {
                if ( _command_item.Price_purchase != 0 && _command_item.Price != 0)
                    profitCalcul();
            }            
        }

        private void profitCalcul()
        {
            try
            {
                TxtPercentProfit = string.Format("{0:0.00}", (((_command_item.Price - _command_item.Price_purchase) / _command_item.Price) * 100));
            }
            catch (DivideByZeroException)
            {
                TxtPercentProfit = 0.ToString();
            }
            TxtProfit = string.Format("{0:0.00}", (_command_item.Price - _command_item.Price_purchase) * _command_item.Quantity);
        }

        /*private void onItemModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("ItemModel") && ItemModel.Item.ID != 0)
            {
                ItemModel.TxtPrice_purchase = TxtPrice_purchase;
                ItemModel.TxtPrice_sell = TxtPrice;
                ItemModel.TxtQuantity = TxtQuantity;
            }
        }*/



    }
}
