using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QCBDManagementBusiness;
using System.Xml.Serialization;
using QCBDManagementCommon.Classes;

namespace QCBDManagementWPF.Models
{
    public class BillModel : BindBase
    {
        private Bill _bill;
        private double _taxValue;
        private decimal _amount;
        private decimal _amountAfterTax;
        private bool _isConstructorRefVisible;
        
        private BusinessLogic _bl;
        

        public BillModel()
        {
            _bill = new Bill();

            PropertyChanged += onTaxValueChange;
        }

        private void onTaxValueChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtTaxValue"))
            {
                _amountAfterTax = _amount + (decimal)_taxValue * _amount;
            }
        }

        [XmlIgnore]
        public BusinessLogic Bl
        {
            get { return _bl; }
            set { setProperty(ref _bl, value, "Bl"); }
        }

        public bool IsConstructorRefVisible
        {
            get { return _isConstructorRefVisible; }
            set { setProperty(ref _isConstructorRefVisible, value, "IsConstructorRefVIsible"); }
        }

        public string TxtAmountAfterTax
        {
            get { return _amountAfterTax.ToString(); }
            set { decimal converted; if (decimal.TryParse(value, out converted)) { _amountAfterTax = converted; } else _amountAfterTax = 0; onPropertyChange("TxtAmountAfterTax"); }
        }

        public string TxtAmount
        {
            get { return _amount.ToString(); }
            set { decimal converted; if (decimal.TryParse(value, out converted)) { _amount = converted; } else _amount = 0; onPropertyChange("TxtAmount");}
        }

        public string TxtTaxValue
        {
            get { return _taxValue.ToString(); }
            set { double converted; if (double.TryParse(value, out converted)) { _taxValue = converted; } else _taxValue = 0; onPropertyChange("TxtTaxValue"); }
        }

        public Bill Bill
        {
            get { return _bill; }
            set { setProperty(ref _bill, value, "Bill"); }
        }

        public string TxtID
        {
            get { return _bill.ID.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _bill.ID = converted; } else _bill.ID = 0; onPropertyChange("TxtID");}
        }

        public string TxtClientId
        {
            get { return _bill.ClientId.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _bill.ClientId = converted; } else _bill.ClientId = 0; onPropertyChange("TxtClientId");}
        }

        public string TxtCommandId
        {
            get { return _bill.CommandId.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _bill.CommandId = converted; } else _bill.CommandId = 0; onPropertyChange("TxtCommandId"); }
        }

        public string TxtPayMod
        {
            get { return _bill.PayMod; }
            set { _bill.PayMod = value; onPropertyChange("TxtPayMod"); }
        }

        public string TxtPay
        {
            get { return _bill.Pay.ToString(); }
            set { decimal converted; if (decimal.TryParse(value, out converted)) { _bill.Pay = converted; } else _bill.Pay = 0; onPropertyChange("TxtPay"); }
        }

        public string TxtPayReceived
        {
            get { return _bill.PayReceived.ToString(); }
            set { decimal converted; if (decimal.TryParse(value, out converted)) { _bill.PayReceived = converted; } else _bill.PayReceived = 0; onPropertyChange("TxtPayReceived"); }
        }

        public string TxtPrivateComment
        {
            get { return _bill.Comment1; }
            set { _bill.Comment1 = value; onPropertyChange("TxtPrivateComment"); }
        }

        public string TxtPublicComment
        {
            get { return _bill.Comment2; }
            set { _bill.Comment2 = value; onPropertyChange("TxtPublicComment"); }
        }

        public string TxtDate
        {
            get { return _bill.Date.ToString(); }
            set { _bill.Date = Utility.convertToDateTime(value); onPropertyChange("TxtDate"); }
        }

        public string TxtDateLimit
        {
            get { return _bill.DateLimit.ToString(); }
            set { _bill.DateLimit = Utility.convertToDateTime(value); onPropertyChange("TxtDateLimit"); }
        }

        public string TxtPayDate
        {
            get { return _bill.PayDate.ToString("MM/dd/yyyy"); }
            set { _bill.PayDate = Utility.convertToDateTime(value, true); onPropertyChange("TxtPayDate"); }
        }

        public List<BillModel> BillListToModelViewList(List<Bill> BillList)
        {
            List<BillModel> output = new List<BillModel>();
            foreach (Bill bill in BillList)
            {
                BillModel billModel = new BillModel();
                billModel.Bill = bill;
                output.Add(billModel);
            }
            return output;
        }


    }
}
