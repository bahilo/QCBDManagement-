using QCBDManagementCommon.Classes;
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
    public class TaxModel : BindBase
    {
        private Tax _tax;
        private List<int> _taxFloatList;
        private List<int> _taxIntegerList;
        private int _taxIntegerSelected;
        private int _taxFloatSelected;

        public TaxModel()
        {
            _tax = new Tax();
            _taxFloatList = new List<int>();
            _taxIntegerList = new List<int>();
            generateTaxValue();
            PropertyChanged += onTaxValueCange_splitIntoTaxIntegerAndTaxFloat;
            PropertyChanged += onTaxFloatSelectedOrTaxIntegerSelectedChange_createTaxValue;
        }

        public Tax Tax
        {
            get { return _tax; }
            set { setProperty(ref _tax, value, "Tax"); }
        }

        private void onTaxFloatSelectedOrTaxIntegerSelectedChange_createTaxValue(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtTaxIntegerSelected") || e.PropertyName.Equals("TxtTaxFloatSelected"))
            {
                double taxValueParsed;
                TxtTaxValue = (double.TryParse(TxtTaxIntegerSelected + "." + TxtTaxFloatSelected, out taxValueParsed)) ? taxValueParsed.ToString() : 0.ToString();
            }
        }

        private void onTaxValueCange_splitIntoTaxIntegerAndTaxFloat(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtTaxValue"))
            {
                var taxIntegerString = ((int)_tax.Value).ToString();
                if (!taxIntegerString.Equals(TxtTaxIntegerSelected))
                    TxtTaxIntegerSelected = taxIntegerString;

                if ((_tax.Value - _taxIntegerSelected) > 0)
                {
                    var taxFloatString = (_tax.Value - _taxIntegerSelected).ToString().Substring(2);
                    if (!taxFloatString.Equals(TxtTaxFloatSelected))
                        TxtTaxFloatSelected = taxFloatString;
                }

            }
        }

        public string TxtTaxValue
        {
            get { return _tax.Value.ToString(); }
            set { _tax.Value = Convert.ToDouble(value); onPropertyChange("TxtTaxValue"); }
        }

        public string TxtTaxIntegerSelected
        {
            get { return _taxIntegerSelected.ToString(); }
            set { int parsedValue; if (int.TryParse(value, out parsedValue)) { _taxIntegerSelected = parsedValue; onPropertyChange("TxtTaxIntegerSelected"); } }
        }

        public string TxtTaxFloatSelected
        {
            get { return _taxFloatSelected.ToString(); }
            set { int parsedValue; if (int.TryParse(value, out parsedValue)) { _taxFloatSelected = parsedValue; onPropertyChange("TxtTaxFloatSelected"); } }
        }

        public List<int> TaxIntegerList
        {
            get { return _taxIntegerList; }
            set { _taxIntegerList = value; onPropertyChange("TaxIntegerList"); }
        }

        public List<int> TaxFloatList
        {
            get { return _taxFloatList; }
            set { _taxFloatList = value; onPropertyChange("TaxFloatList"); }
        }

        public string TxtTaxType
        {
            get { return _tax.Type.ToString(); }
            set { _tax.Type = value; onPropertyChange("TxtTaxType"); }
        }

        public string TxtDate
        {
            get { return _tax.Date_insert.ToString("dd/MM/yyyy"); }
            set { _tax.Date_insert = (Utility.convertToDateTime(value).Equals(Utility.DateTimeMinValueInSQL2005))? DateTime.Now: Utility.convertToDateTime(value) ; onPropertyChange("TxtDate"); }
        }

        public string TxtComment
        {
            get { return _tax.Comment; }
            set { _tax.Comment = value; onPropertyChange("TxtComment"); }
        }

        private void generateTaxValue()
        {
            for (int i = 0; i < 100; i++)
                _taxIntegerList.Add(i);

            for (int i = 0; i < 10; i++)
                _taxFloatList.Add(i);


        }

    }
}
