using QCBDManagementBusiness;
using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace QCBDManagementWPF.ViewModel
{
    public class OptionGeneralViewModel : BindBase
    {
        private Func<object, object> _sideBarManagement;
        private Func<string, object> _getObjectFromMainWindowViewModel;
        private Func<object, object> _page;
        private List<GeneralInfos.Bank> _bankDetails;
        private List<GeneralInfos.Contact> _addressDetails;
        private GeneralInfos _generalInfos;
        private GeneralInfos.FileWriter _legalInformationFileManagement;
        private GeneralInfos.FileWriter _saleGeneralConditionFileManagement;
        private string _validationEmail;
        private string _reminderEmail;
        private string _invoiceEmail;
        private string _quoteEmail;
        private string _email;
        private List<string> _emailfilterList;
        private string _title;

        //----------------------------[ Models ]------------------

        private List<TaxModel> _taxes;
        private TaxModel _taxModel;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<object> UpdateListSizeCommand { get; set; }
        public ButtonCommand<object> UpdateBankDetailCommand { get; set; }
        public ButtonCommand<object> UpdateAddressCommand { get; set; }
        public ButtonCommand<object> AddTaxCommand { get; set; }
        public ButtonCommand<TaxModel> DeleteTaxCommand { get; set; }
        public ButtonCommand<object> UpdateLegalInformationCommand { get; set; }
        public ButtonCommand<object> UpdateSaleGeneralConditionCommand { get; set; }
        public ButtonCommand<object> NewLegalInformationCommand { get; set; }
        public ButtonCommand<object> NewSaleGeneralConditionCommand { get; set; }
        public ButtonCommand<object> UpdateEmailCommand { get; set; }



        public OptionGeneralViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }
        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {

        }

        private void instances()
        {
            _legalInformationFileManagement = new GeneralInfos.FileWriter("legal_information", "text");
            _saleGeneralConditionFileManagement = new GeneralInfos.FileWriter("sale_general_condition", "text");
            _addressDetails = new List<GeneralInfos.Contact>();
            _bankDetails = new List<GeneralInfos.Bank>();
            _generalInfos = new GeneralInfos();
            _emailfilterList = new List<string> { "email", "invoice_email", "quote_email", "reminder_email", "validation_email" };
            _title = "Option Management";
        }

        private void instancesModel()
        {
            _taxModel = new TaxModel();
            _taxes = new List<TaxModel>();
        }

        private void instancesCommand()
        {
            UpdateAddressCommand = new ButtonCommand<object>(updateAddress, canUpdateAddress);
            UpdateBankDetailCommand = new ButtonCommand<object>(updateBankDetail, canUpdateBankDetail);
            UpdateLegalInformationCommand = new ButtonCommand<object>(updateLegalInformation, canUpdateLegalInformation);
            UpdateSaleGeneralConditionCommand = new ButtonCommand<object>(updateSaleGeneralCondition, canUpdateSaleGeneralCondition);
            NewLegalInformationCommand = new ButtonCommand<object>(eraseLegalInformation, canEraseLegalInformation);
            NewSaleGeneralConditionCommand = new ButtonCommand<object>(eraseSaleGeneralCondition, canEraseSaleGeneralCondition);
            UpdateListSizeCommand = new ButtonCommand<object>(updateListSize, canUpdateListSize);
            AddTaxCommand = new ButtonCommand<object>(addTax, canAddTax);
            DeleteTaxCommand = new ButtonCommand<TaxModel>(deleteTax, canDeleteTax);
            UpdateEmailCommand = new ButtonCommand<object>(updateEmail, canUpdateEmail);
        }

        //----------------------------[ Properties ]------------------


        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public GeneralInfos.FileWriter LegalInformationFileManagement
        {
            get { return _legalInformationFileManagement; }
            set { setProperty(ref _legalInformationFileManagement, value, "LegalInformationFileManagement"); }
        }

        public GeneralInfos.FileWriter SaleGeneralConditionFileManagement
        {
            get { return _saleGeneralConditionFileManagement; }
            set { setProperty(ref _saleGeneralConditionFileManagement, value, "SaleGeneralConditionFileManagement"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            private set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }

        public List<int> ListSizeList
        {
            get { return _generalInfos.ListSizeList; }
            set { _generalInfos.ListSizeList = value; onPropertyChange("ListSizeList"); }
        }

        public int TxtSelectedListSize
        {
            get { return _generalInfos.TxtSelectedListSize; }
            set { _generalInfos.TxtSelectedListSize = value; onPropertyChange("TxtListSize"); }
        }

        public TaxModel TaxModel
        {
            get { return _taxModel; }
            set { setProperty(ref _taxModel, value, "Tax"); }
        }

        public string TxtValidationEmail
        {
            get { return _validationEmail; }
            set { setProperty(ref _validationEmail, value, "TxtValidationEmail"); }
        }

        public string TxtReminderEmail
        {
            get { return _reminderEmail; }
            set { setProperty(ref _reminderEmail, value, "TxtReminderEmail"); }
        }

        public string TxtInvoiceEmail
        {
            get { return _invoiceEmail; }
            set { setProperty(ref _invoiceEmail, value, "TxtInvoiceEmail"); }
        }

        public string TxtQuoteEmail
        {
            get { return _quoteEmail; }
            set { setProperty(ref _quoteEmail, value, "TxtQuoteEmail"); }
        }

        public string TxtEmail
        {
            get { return _email; }
            set { setProperty(ref _email, value, "TxtEmail"); }
        }

        public List<TaxModel> TaxList
        {
            get { return _taxes; }
            set { setProperty(ref _taxes, value, "TaxList"); }
        }

        public List<GeneralInfos.Bank> BankDetailList
        {
            get { return _bankDetails; }
            set { setProperty(ref _bankDetails, value, "BankDetailList"); }
        }

        public List<GeneralInfos.Contact> AddressList
        {
            get { return _addressDetails; }
            set { setProperty(ref _addressDetails, value, "AddressList"); }
        }

        public string BlockBankDetailVisibility
        {
            get { return disableUIElementByString("BlockBankDetailVisibility"); }
        }

        public string BlockAddressDetailVisibility
        {
            get { return disableUIElementByString("BlockAddressDetailVisibility"); }
        }

        public string BlockLegalInfosDetailVisibility
        {
            get { return disableUIElementByString("BlockLegalInfosDetailVisibility"); }
        }

        public string BlockTaxDetailVisibility
        {
            get { return disableUIElementByString("BlockTaxDetailVisibility"); }
        }

        //----------------------------[ Actions ]------------------

        public List<TaxModel> TaxListToTaxModelList(List<Tax> taxList)
        {
            List<TaxModel> output = new List<TaxModel>();
            foreach (var tax in taxList)
            {
                TaxModel taxModel = new TaxModel();
                taxModel.Tax = tax;
                output.Add(taxModel);
            }
            return output;
        }

        public async void loadData()
        {
            Dialog.showSearch("Loading...");
            var userListSizeFoundList = _generalInfos.ListSizeList.Where(x => x.Equals(Bl.BlSecurity.GetAuthenticatedUser().ListSize)).ToList();
            _generalInfos.TxtSelectedListSize = (userListSizeFoundList.Count > 0) ? userListSizeFoundList[0] : 0;
            TaxList = TaxListToTaxModelList(await Bl.BlCommand.GetTaxData(999));
            var infosFoundList = await Bl.BlReferential.GetInfosData(999);
            BankDetailList = new List<GeneralInfos.Bank> { new GeneralInfos.Bank(infosFoundList) };
            AddressList = new List<GeneralInfos.Contact> { new GeneralInfos.Contact(infosFoundList) };
            LoadEmail();
            string login = ((await _startup.Bl.BlReferential.searchInfos(new QCBDManagementCommon.Entities.Infos { Name = "ftp_login" }, "OR")).FirstOrDefault() ?? new Infos()).Value;
            string password = ((await _startup.Bl.BlReferential.searchInfos(new QCBDManagementCommon.Entities.Infos { Name = "ftp_password" }, "OR")).FirstOrDefault() ?? new Infos()).Value;
            SaleGeneralConditionFileManagement.TxtLogin = LegalInformationFileManagement.TxtLogin = login;
            SaleGeneralConditionFileManagement.TxtPassword = LegalInformationFileManagement.TxtPassword = password;
            LegalInformationFileManagement.read();
            SaleGeneralConditionFileManagement.read();
            
            Dialog.IsDialogOpen = false;
        }

        private async void LoadEmail()
        {
            List<Infos> insertList = new List<Infos>();
            foreach (string filter in _emailfilterList)
            {
                var infosFoundList = await Bl.BlReferential.searchInfos(new Infos { Name = filter }, "AND");
                if (infosFoundList.Count > 0)
                {
                    switch (filter)
                    {
                        case "email":
                            TxtEmail = infosFoundList[0].Value;
                            break;
                        case "invoice_email":
                            TxtInvoiceEmail = infosFoundList[0].Value;
                            break;
                        case "quote_email":
                            TxtQuoteEmail = infosFoundList[0].Value;
                            break;
                        case "reminder_email":
                            TxtReminderEmail = infosFoundList[0].Value;
                            break;
                        case "validation_email":
                            TxtReminderEmail = infosFoundList[0].Value;
                            break;
                    }
                }
                else
                    insertList.Add(new Infos { Name = filter });
            }
            var infosInsertedList = await Bl.BlReferential.InsertInfos(insertList);
        }

        private string disableUIElementByString(string obj)
        {
            bool isWrite = securityCheck(QCBDManagementCommon.Enum.EAction.Option, QCBDManagementCommon.Enum.ESecurity._Write);
            bool isUpdate = securityCheck(QCBDManagementCommon.Enum.EAction.Option, QCBDManagementCommon.Enum.ESecurity._Update);
            if ((!isWrite || !isUpdate)
                && (obj.Equals("BlockBankDetailVisibility")
                || obj.Equals("BlockTaxDetailVisibility")
                || obj.Equals("BlockAddressDetailVisibility")
                || obj.Equals("BlockLegalInfosDetailVisibility")))
                return "Hidden";

            return "Visible";
        }

        internal void sideBarContentManagement(Func<object, object> sideBarManagement)
        {
            _sideBarManagement = sideBarManagement;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            GetObjectFromMainWindowViewModel = getObject;
        }

        internal void mainNavigObject(Func<object, object> navigation)
        {
            _page = navigation;
        }

        //----------------------------[ Event Handler ]------------------
        

        //----------------------------[ Action Commands ]------------------


        private async void deleteTax(TaxModel obj)
        {
            Dialog.showSearch("Tax deleting...");
            var NotDeletedTax = await Bl.BlCommand.DeleteTax(new List<QCBDManagementCommon.Entities.Tax> { obj.Tax });
            if (NotDeletedTax.Count == 0)
            {
                await Dialog.show("Tax Deleted Successfully!");
                TaxList.Remove(obj);
                TaxList = new List<TaxModel>(TaxList);
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canDeleteTax(TaxModel arg)
        {
            return true;
        }

        private async void addTax(object obj)
        {
            Dialog.showSearch("Tax creation...");
            var savedTaxList = await Bl.BlCommand.InsertTax(new List<QCBDManagementCommon.Entities.Tax> { TaxModel.Tax });
            if (savedTaxList.Count > 0)
            {
                await Dialog.show("Tax added Successfully!");
                TaxList = new List<TaxModel>(TaxList.Concat(TaxListToTaxModelList(savedTaxList)));
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canAddTax(object arg)
        {
            return true;
        }

        private async void updateListSize(object obj)
        {
            Dialog.showSearch("List size updating...");
            var authenticatedUser = Bl.BlSecurity.GetAuthenticatedUser();
            authenticatedUser.ListSize = Convert.ToInt32(_generalInfos.TxtSelectedListSize);
            var savedAgentList = await Bl.BlAgent.UpdateAgent(new List<Agent> { authenticatedUser });
            if (savedAgentList.Count > 0)
                await Dialog.show("List Size saved Successfully!");
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateListSize(object arg)
        {
            return true;
        }

        private async void eraseLegalInformation(object obj)
        {
            LegalInformationFileManagement.TxtContent = "";
            LegalInformationFileManagement.save();
            await Dialog.show("Legal Information content has been erased Successfully!");
        }

        private bool canEraseLegalInformation(object arg)
        {
            return true;
        }

        private async void eraseSaleGeneralCondition(object obj)
        {
            SaleGeneralConditionFileManagement.TxtContent = "";
            SaleGeneralConditionFileManagement.save();
            await Dialog.show("Sale General Condition content has been erased Successfully!");
        }

        private bool canEraseSaleGeneralCondition(object arg)
        {
            return true;
        }

        private async void updateLegalInformation(object obj)
        {
            Dialog.showSearch("Legal information updating...");
            bool isSuccessful = LegalInformationFileManagement.save();
            if (isSuccessful)
                await Dialog.show("Legal Information content has been saved Successfully!");
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateLegalInformation(object arg)
        {
            return true;
        }

        private async void updateSaleGeneralCondition(object obj)
        {
            Dialog.showSearch("Sale General Condition updating...");
            bool isSuccessful = SaleGeneralConditionFileManagement.save();
            if (isSuccessful)
                await Dialog.show("Sale General Condition content has been saved Successfully!");
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateSaleGeneralCondition(object arg)
        {
            return true;
        }

        private async void updateBankDetail(object obj)
        {
            Dialog.showSearch("Bank details updating...");
            var infosList = _bankDetails[0].extractInfosListFromBankDictionary();
            var infosToUpdateList = infosList.Where(x => x.ID != 0).ToList();
            var infosToCreateList = infosList.Where(x => x.ID == 0).ToList();
            var infosUpdatedList = await Bl.BlReferential.UpdateInfos(infosToUpdateList);
            var infosCreatedList = await Bl.BlReferential.InsertInfos(infosToCreateList);
            if (infosUpdatedList.Count > 0 || infosCreatedList.Count > 0)
            {
                await Dialog.show("Bank Detail saved Successfully!");
                List<Infos> savedInfosList = new List<Infos>(infosUpdatedList);
                savedInfosList = new List<Infos>(savedInfosList.Concat(infosCreatedList));
                BankDetailList = new List<GeneralInfos.Bank> { new GeneralInfos.Bank(savedInfosList) };
            }

            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateBankDetail(object arg)
        {
            return true;
        }

        private async void updateAddress(object obj)
        {
            Dialog.showSearch("Address updating...");
            var infosList = _addressDetails[0].extractInfosListFromContactDictionary();
            var infosToUpdateList = infosList.Where(x => x.ID != 0).ToList();
            var infosToCreateList = infosList.Where(x => x.ID == 0).ToList();
            var infosUpdatedList = await Bl.BlReferential.UpdateInfos(infosToUpdateList);
            var infosCreatedList = await Bl.BlReferential.InsertInfos(infosToCreateList);
            if (infosUpdatedList.Count > 0 || infosCreatedList.Count > 0)
            {
                await Dialog.show("Address Detail saved Successfully!");
                List<Infos> savedInfosList = new List<Infos>(infosUpdatedList);
                savedInfosList = new List<Infos>(savedInfosList.Concat(infosCreatedList));
                AddressList = new List<GeneralInfos.Contact> { new GeneralInfos.Contact(savedInfosList) };
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateAddress(object arg)
        {
            return true;
        }

        private async void updateEmail(object obj)
        {
            List<Infos> updateList = new List<Infos>();
            Dialog.showSearch("Updating email...");
            foreach (string filter in _emailfilterList)
            {
                var infosFoundList = await Bl.BlReferential.searchInfos(new Infos { Name = filter }, "AND");
                if (infosFoundList.Count > 0)
                {
                    switch (filter)
                    {
                        case "email":
                            infosFoundList[0].Value = TxtEmail;
                            break;
                        case "invoice_email":
                            infosFoundList[0].Value = TxtInvoiceEmail;
                            break;
                        case "quote_email":
                            infosFoundList[0].Value = TxtQuoteEmail;
                            break;
                        case "reminder_email":
                            infosFoundList[0].Value = TxtReminderEmail;
                            break;
                        case "validation_email":
                            infosFoundList[0].Value = TxtReminderEmail;
                            break;
                    }
                    updateList.Add(infosFoundList[0]);
                }
            }
            var infosUpdatedList = await Bl.BlReferential.UpdateInfos(updateList);
            if (infosUpdatedList.Count > 0)
                await Dialog.show("Email updated successfully!");
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateEmail(object arg)
        {
            return true;
        }
    }
}
