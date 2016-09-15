using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Diagnostics;

namespace QCBDManagementWPF.Classes
{
    public class GeneralInfos : BindBase
    {
        private int _SelectedlistSize;
        private List<int> _listSizeList;

        public GeneralInfos()
        {
            _listSizeList = new List<int>();
            generateListSizeList();
        }

        public List<int> ListSizeList
        {
            get { return _listSizeList; }
            set { setProperty(ref _listSizeList, value, "ListSizeList"); }
        }

        /*public string TxtSelectedListSize
        {
            get { return _SelectedlistSize.ToString(); }
            set { setProperty(ref _SelectedlistSize, Convert.ToInt32(value), "TxtSelectedListSize"); }
        }*/

        public int TxtSelectedListSize
        {
            get { return _SelectedlistSize; }
            set { setProperty(ref _SelectedlistSize, value, "TxtSelectedListSize"); }
        }

        private void generateListSizeList()
        {
            for (int i = 1; i < 200; i++)
            {
                _listSizeList.Add(i);
            }
        }




        //======================[ Bank Infos ]=====================

        public class Bank : BindBase
        {
            private List<string> _bankfilterList;
            private Dictionary<string, Pair<int, string>> _bankDictionary;
            private List<Infos> _infosList;

            public Bank(List<Infos> infosList)
            {
                _bankDictionary = new Dictionary<string, Pair<int, string>>();
                _infosList = new List<Infos>();
                _bankfilterList = new List<string> {
                    "sort_code",                //=> code_banque
                    "account_number",           //=> num_compte
                    "acount_key_number",        //=> cle_rib
                    "bank_name",                //=> nom_banque
                    "branch_code",              //=> guichet
                    "iban",                     //=> IBAN
                    "bic",                      //=> BIC  
                    "bank_address",             //=> adresse_banque     
                };
                fillBankDictionaryFromInfos(infosList);
            }

            public List<Infos> InfosList
            {
                get { return _infosList; }
                set { setProperty(ref _infosList, value, "InfosList"); }
            }

            public string TxtSortCode
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[0])) ? _bankDictionary[_bankfilterList[0]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[0])) { _bankDictionary[_bankfilterList[0]].PairValue = value; onPropertyChange("TxtSortCode"); } }
            }

            public string TxtAccountNumber
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[1])) ? _bankDictionary[_bankfilterList[1]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[1])) { _bankDictionary[_bankfilterList[1]].PairValue = value; onPropertyChange("TxtAccountNumber"); } }
            }

            public string TxtAccountKeyNumber
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[2])) ? _bankDictionary[_bankfilterList[2]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[2])) { _bankDictionary[_bankfilterList[2]].PairValue = value; onPropertyChange("TxtAccountKeyNumber"); } }
            }

            public string TxtBankName
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[3])) ? _bankDictionary[_bankfilterList[3]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[3])) { _bankDictionary[_bankfilterList[3]].PairValue = value; onPropertyChange("TxtBankName"); } }
            }

            public string TxtAgencyCode
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[4])) ? _bankDictionary[_bankfilterList[4]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[4])) { _bankDictionary[_bankfilterList[4]].PairValue = value; onPropertyChange("TxtAgencyCode"); } }
            }

            public string TxtIBAN
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[5])) ? _bankDictionary[_bankfilterList[5]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[5])) { _bankDictionary[_bankfilterList[5]].PairValue = value; onPropertyChange("TxtIBAN"); } }
            }

            public string TxtBIC
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[6])) ? _bankDictionary[_bankfilterList[6]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[6])) { _bankDictionary[_bankfilterList[6]].PairValue = value; onPropertyChange("TxtBIC"); } }
            }

            public string TxtBankAddress
            {
                get { return (_bankDictionary.ContainsKey(_bankfilterList[7])) ? _bankDictionary[_bankfilterList[7]].PairValue : ""; }
                set { if (_bankDictionary.ContainsKey(_bankfilterList[7])) { _bankDictionary[_bankfilterList[7]].PairValue = value; onPropertyChange("TxtBankAddress"); } }
            }

            private void fillBankDictionaryFromInfos(List<Infos> infosList)
            {
                foreach (var filter in _bankfilterList)
                {
                    var match = infosList.Where(x => x.Name.Equals(filter)).ToList();
                    Pair<int, string> pair = new Pair<int, string>();
                    if (match.Count() > 0)
                    {
                        pair.PairID = match[0].ID;
                        pair.PairValue = match[0].Value;
                        _bankDictionary[filter] = pair;
                    }
                    else
                    {
                        pair.PairValue = "";
                        _bankDictionary[filter] = pair;
                    }

                }
                /*foreach (Infos infos in infosList)
                {
                    if (_bankfilterList.Contains(infos.Name))
                        _bankDictionary[infos.Name] = infos.Value;

                }*/
            }

            public List<Infos> extractInfosListFromBankDictionary()
            {
                _infosList = new List<Infos>();
                foreach (var bank in _bankDictionary)
                {
                    Infos bankInfos = new Infos();
                    bankInfos.Name = bank.Key;
                    bankInfos.ID = bank.Value.PairID;
                    bankInfos.Value = bank.Value.PairValue;
                    InfosList.Add(bankInfos);
                }
                return InfosList;
            }
        }




        //======================[ Address/Contact Infos ]=====================


        public class Contact : BindBase
        {
            private List<string> _contactfilterList;
            private Dictionary<string, Pair<int, string>> _contactDictionary;
            private List<Infos> _infosList;

            public Contact(List<Infos> infosList)
            {
                _infosList = new List<Infos>();
                _contactDictionary = new Dictionary<string, Pair<int, string>>();
                _contactfilterList = new List<string> {
                    "company_name",         //=> nom_societe
                    "address",              //=> adresse
                    "phone",                //=> tel
                    "fax",                  //=> fax
                    "email",                //=> email
                    "tax_code",             //=> num_tva    
                };
                fillContactDictionaryFromInfos(infosList);
            }

            public string TxtCompanyName
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[0])) ? _contactDictionary[_contactfilterList[0]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[0]].PairValue = value; onPropertyChange("TxtCompanyName"); } }
            }

            public string TxtAddress
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[1])) ? _contactDictionary[_contactfilterList[1]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[1]].PairValue = value; onPropertyChange("TxtAddress"); } }
            }

            public string TxtPhone
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[2])) ? _contactDictionary[_contactfilterList[2]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[2]].PairValue = value; onPropertyChange("TxtPhone"); } }
            }

            public string TxtFax
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[3])) ? _contactDictionary[_contactfilterList[3]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[3]].PairValue = value; onPropertyChange("TxtFax"); } }
            }

            public string TxtEmail
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[4])) ? _contactDictionary[_contactfilterList[4]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[4]].PairValue = value; onPropertyChange("TxtEmail"); } }
            }

            public string TxtTaxCode
            {
                get { return (_contactDictionary.ContainsKey(_contactfilterList[5])) ? _contactDictionary[_contactfilterList[5]].PairValue : ""; }
                set { if (_contactDictionary.Count > 0) { _contactDictionary[_contactfilterList[5]].PairValue = value; onPropertyChange("TxtTaxCode"); } }
            }

            public List<Infos> InfosList
            {
                get { return _infosList; }
                set { setProperty(ref _infosList, value, "InfosList"); }
            }

            private void fillContactDictionaryFromInfos(List<Infos> infosList)
            {
                foreach (var filter in _contactfilterList)
                {
                    var match = infosList.Where(x => x.Name.Equals(filter)).ToList();
                    Pair<int, string> pair = new Pair<int, string>();
                    if (match.Count() > 0)
                    {
                        pair.PairID = match[0].ID;
                        pair.PairValue = match[0].Value;
                        _contactDictionary[filter] = pair;
                    }
                    else
                    {
                        pair.PairValue = "";
                        _contactDictionary[filter] = pair;
                    }
                }
                /*if (match.Count() > 0)
                {
                    _contactDictionary[filter].PairValue = match[0].Value;
                    _contactDictionary[filter].PairID = match[0].ID;
                }                        
                else
                    _contactDictionary[filter].PairValue = "";                    

            foreach (Infos infos in infosList)
            {
                if (_contactfilterList.Contains(infos.Name))
                    _contactDictionary[infos.Name] = infos.Value;
            }*/
            }

            public List<Infos> extractInfosListFromContactDictionary()
            {
                _infosList = new List<Infos>();
                foreach (var contact in _contactDictionary)
                {
                    Infos contactInfos = new Infos();
                    contactInfos.Name = contact.Key;
                    contactInfos.ID = contact.Value.PairID;
                    contactInfos.Value = contact.Value.PairValue;
                    InfosList.Add(contactInfos);
                }

                return InfosList;
            }
        }




        //======================[ File writer ]=====================


        public class FileWriter : BindBase
        {
            private string _content;
            private string _subject;
            private string _path;
            private string _fullPath;
            private string _fileName;
            private string _ftpHost;
            private string _remotePath;
            private string _localPath;
            private string _fileNameWithoutExtension;

            public FileWriter(string fileName)
            {
                _fileNameWithoutExtension = fileName;
                _content = "Message here";
                _path = Directory.GetCurrentDirectory() + @"\Docs\Files\";
                //setup();
                read();
            }

            public string TxtContent
            {
                get { return _content; }
                set { _content = value; onPropertyChange("TxtContent"); }
            }

            public string TxtSubject
            {
                get { return _subject; }
                set { _subject = value; onPropertyChange("TxtSubject"); }
            }

            public string TxtFileName
            {
                get { return _fileName; }
                set { _fileName = value; onPropertyChange("TxtFileName"); }
            }

            public string TxtFileNameWithoutExtension
            {
                get { return _fileNameWithoutExtension; }
                set { setProperty(ref _fileNameWithoutExtension, value, "TxtFileNameWithoutExtension"); }
            }

            public string TxtFileFullPath
            {
                get { return _fullPath; }
                set { _fullPath = value; onPropertyChange("TxtFileFullPath"); }
            }
            
            public string TxtFtpUrl { get; private set; }

            private void setup()
            {
                _ftpHost = "ftp://ftpperso.free.fr";
                _remotePath = string.Format(@"/{0}/{1}/", "dev", "mails");
                _localPath = Directory.GetCurrentDirectory() + string.Format(@"\{0}\{1}\", "Docs", "Mails");

                TxtFileName = TxtFileNameWithoutExtension + ".txt";
                TxtFtpUrl = _ftpHost + _remotePath + TxtFileName;
                TxtFileFullPath = Path.Combine(_localPath, TxtFileName);

                if (!Directory.Exists(_localPath))
                    Directory.CreateDirectory(_localPath);

                /*switch (typeOfFile)
                {
                    case "reminder-1":                        
                        TxtFileName = "reminder_1.txt";
                        break;
                    case "reminder-2":
                        TxtFileName = "reminder_2.txt";
                        break;
                    case "bill":
                        TxtFileName = "bill.txt";
                        break;
                    case "command-confirmation":
                        TxtFileName = "command_confirmation.txt";
                        break;
                    case "command-validation":
                        TxtFileName = "command_validation.txt";
                        break;
                    case "quote":
                        TxtFileName = "quote.txt";
                        break;
                    case "legal-information":
                        TxtFileName = "legal_information.txt";
                        break;
                }*/

                TxtFileFullPath = Path.Combine(_path, TxtFileName);
                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
            }

            public bool save()
            {
                bool isSavedSuccessfully = false;

                /*if (!File.Exists(TxtFileFullPath))
                    File.Create(TxtFileFullPath);*/

                File.WriteAllText(TxtFileFullPath, TxtContent);

                try
                {
                    isSavedSuccessfully = Utility.uploadFIle(TxtFtpUrl, TxtFileFullPath, "sodpagnekita", "bahilo225");
                }
                catch (Exception e)
                {
                    Debug.WriteLine("[ERR] - " + e.Message);
                    //Task.Delay(1000).ContinueWith((t) => { save(); });
                }

                return isSavedSuccessfully;
            }

            private void read()
            {
                bool isFileFound = false;
                setup();
                if (TxtFtpUrl != null && TxtFileFullPath != null)
                    try
                    {
                        //updateImageSource(isClosingImageStream: true);
                        isFileFound = Utility.downloadFIle(TxtFtpUrl, TxtFileFullPath, "sodpagnekita", "bahilo225");
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("[ERR] - " + e.Message);
                        //Task.Delay(1000).ContinueWith((t) => { read(); });
                    }

                if (File.Exists(TxtFileFullPath))
                    TxtContent = File.ReadAllText(TxtFileFullPath);

            }
        }


        //======================[ Pair ]=====================


        public class Pair<T1, T2>
        {
            public T1 PairID { get; set; }
            public T2 PairValue { get; set; }
        }


        //======================[ Email ]=====================


        public class Email : BindBase
        {
            string _textContent;

            public Email()
            {
                _textContent = "";
            }

            public string TextContent
            {
                get { return _textContent; }
                set { setProperty(ref _textContent, value, "TextContent"); }
            }
        }


    }
}
