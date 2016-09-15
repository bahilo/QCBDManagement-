using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;

namespace QCBDManagementWPF.Models
{
    public class LanguageModel : BindBase
    {
        Language _language;
        private CultureInfo _cultureInfo;
        private string _defaultContent;

        public LanguageModel()
        {
            _language = new Language();
            PropertyChanged += onTxtCultureInfo_nameChange;
            PropertyChanged += onCultureInfoChange;
        }

        private void onCultureInfoChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CultureInfo"))
            {
                TxtCultureInfo_name = CultureInfo.Name;
                TxtCultureInfo_fullName = CultureInfo.DisplayName;
            }
        }

        private void onTxtCultureInfo_nameChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("TxtCultureInfo_name"))
            {
                TxtLang = TxtCultureInfo_name.Split('-').FirstOrDefault();
                /*CultureInfo = new CultureInfo(TxtCultureInfo_name);
                TxtCultureInfo_fullName = CultureInfo.DisplayName;*/
            }
        }

        public Language Language
        {
            get { return _language; }
            set { setProperty(ref _language, value, "Language"); }
        }

        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set { setProperty(ref _cultureInfo, value, "CultureInfo"); }
        }

        public string TxtID
        {
            get { return _language.ID.ToString(); }
            set { int converted = 0; if (int.TryParse(value, out converted)) { _language.ID = converted; onPropertyChange("TxtID"); } }
        }

        public string TxtLang_table
        {
            get { return _language.Lang_table; }
            set { _language.Lang_table = value; onPropertyChange("TxtLang_table"); }
        }

        public string TxtTable_column
        {
            get { return _language.Table_column; }
            set { _language.Table_column = value; onPropertyChange("TxtTable_column"); }
        }

        public string TxtColumnId
        {
            get { return _language.ColumnId.ToString(); }
            set { _language.ColumnId = value; onPropertyChange("TxtColumnId"); }
        }

        public string TxtLang
        {
            get { return _language.Lang; }
            set { _language.Lang = value; onPropertyChange("TxtLang"); }
        }

        public string TxtContent
        {
            get { return _language.Content; }
            set { _language.Content = value; onPropertyChange("TxtContent"); }
        }

        public string TxtDefaultContent
        {
            get { return _defaultContent; }
            set { _defaultContent = value; onPropertyChange("TxtDefaultContent"); }
        }

        public string TxtCultureInfo_name
        {
            get { return _language.CultureInfo_name; }
            set { _language.CultureInfo_name = value; onPropertyChange("TxtCultureInfo_name"); }
        }

        public string TxtCultureInfo_fullName
        {
            get { return _language.CultureInfo_fullName; }
            set { _language.CultureInfo_fullName = value; onPropertyChange("TxtCultureInfo_fullName"); }
        }



    }
}
