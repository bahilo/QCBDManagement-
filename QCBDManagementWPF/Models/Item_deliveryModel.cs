using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.Models
{
    public class Item_deliveryModel : BindBase
    {
        private Item _item;
        private Item_delivery _item_delivery;
        private DeliveryModel _deliveryReceiptList;
        private int _quantity_current;
        private bool _isSelected;

        public Item_deliveryModel()
        {
            _item = new Item();
            _deliveryReceiptList = new DeliveryModel();
            _item_delivery = new Item_delivery();
            _isSelected = true;
        }
        
        public Item_delivery Item_delivery
        {
            get { return _item_delivery; }
            set { setProperty(ref _item_delivery, value, "Item_delivery"); }
        }

        public Item Item
        {
            get { return _item; }
            set { setProperty(ref _item, value, "Item"); }
        }

        public DeliveryModel DeliveryModel
        {
            get { return _deliveryReceiptList; }
            set { setProperty(ref _deliveryReceiptList, value, "DeliveryModelList"); }
        }

        public string TxtID
        {
            get { return _item_delivery.ID.ToString(); }
            set { _item_delivery.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string TxtDeliveryId
        {
            get { return _item_delivery.DeliveryId.ToString(); }
            set { _item_delivery.DeliveryId = Convert.ToInt32(value); onPropertyChange("TxtDeliveryId"); }
        }

        public string TxtItem_ref
        {
            get { return _item_delivery.Item_ref; }
            set { _item_delivery.Item_ref = value; onPropertyChange("TxtItem_ref"); }
        }

        public string TxtQuantity_delivery
        {
            get { return _item_delivery.Quantity_delivery.ToString(); }
            set { _item_delivery.Quantity_delivery = Convert.ToInt32(value); onPropertyChange("TxtQuantity_delivery"); }
        }

        public string TxtQuantity_current
        {
            get { return _quantity_current.ToString(); }
            set { _quantity_current = Convert.ToInt32(value); onPropertyChange("TxtQuantity_current"); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { setProperty(ref _isSelected, value, "IsSelected"); }
        }





    }
}
