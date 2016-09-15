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
            set { setProperty(ref _amountAfterTax, Convert.ToDecimal(value), "TxtAmountAfterTax"); }
        }

        public string TxtAmount
        {
            get { return _amount.ToString(); }
            set { setProperty(ref _amount, Convert.ToDecimal(value), "TxtAmount"); }
        }

        public string TxtTaxValue
        {
            get { return _taxValue.ToString(); }
            set { setProperty(ref _taxValue, Convert.ToDouble(value), "TxtTaxValue"); }
        }

        public Bill Bill
        {
            get { return _bill; }
            set { setProperty(ref _bill, value, "Bill"); }
        }

        public string TxtID
        {
            get { return _bill.ID.ToString(); }
            set { _bill.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string TxtClientId
        {
            get { return _bill.ClientId.ToString(); }
            set { _bill.ClientId = Convert.ToInt32(value); onPropertyChange("TxtClientId"); }
        }

        public string TxtCommandId
        {
            get { return _bill.CommandId.ToString(); }
            set { _bill.CommandId = Convert.ToInt32(value); onPropertyChange("TxtCommandId"); }
        }

        public string TxtPayMod
        {
            get { return _bill.PayMod; }
            set { _bill.PayMod = value; onPropertyChange("TxtPayMod"); }
        }

        public string TxtPay
        {
            get { return _bill.Pay.ToString(); }
            set { _bill.Pay = Convert.ToDecimal(value); onPropertyChange("TxtPay"); }
        }

        public string TxtPayReceived
        {
            get { return _bill.PayReceived.ToString(); }
            set { _bill.PayReceived = Convert.ToDecimal(value); onPropertyChange("TxtPayReceived"); }
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
            set { _bill.Date = Convert.ToDateTime(value); onPropertyChange("TxtDate"); }
        }

        public string TxtDateLimit
        {
            get { return _bill.DateLimit.ToString(); }
            set { _bill.DateLimit = Convert.ToDateTime(value); onPropertyChange("TxtDateLimit"); }
        }

        public string TxtPayDate
        {
            get { return _bill.PayDate.ToString(); }
            set { _bill.PayDate = Utility.convertToDateTime(value); onPropertyChange("TxtPayDate"); }
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
