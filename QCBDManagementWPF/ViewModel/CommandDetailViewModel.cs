using QCBDManagementBusiness;
using Entity = QCBDManagementCommon.Entities;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementCommon.Structures;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementCommon.Classes;
using System.Collections.Concurrent;
using QCBDManagementWPF.Interfaces;
using System.Windows.Threading;
using System.Threading;
using System.Globalization;

namespace QCBDManagementWPF.ViewModel
{
    public class CommandDetailViewModel : BindBase
    {
        private decimal _totalPurchase;
        private decimal _totalAfterTax;
        private decimal _totalPercentProfit;
        private decimal _totalProfit;
        private decimal _totalTaxAmount;
        private decimal _totalBeforeTax;
        private GeneralInfos.FileWriter _mailFile;
        private ParamCommandToPdf _paramQuoteToPdf;
        private ParamCommandToPdf _paramCommandToPdf;
        private ParamDeliveryToPdf _paramDeliveryToPdf;
        public NotifyTaskCompletion<List<Command_item>> Command_ItemTask { get; set; }
        public NotifyTaskCompletion<List<Item>> ItemTask { get; set; }
        public NotifyTaskCompletion<List<ItemModel>> ItemModelTask { get; set; }
        public NotifyTaskCompletion<List<Command_itemModel>> BillTask { get; set; }
        private NotifyTaskCompletion<List<Command_item>> _command_itemTask_updateItem;
        private NotifyTaskCompletion<List<Command_item>> _command_itemTask_updateCommand_Item;
        //private NotifyTaskCompletion<object> _quoteTask;
        private Func<string, object> _getObjectFromMainWindowViewModel;
        private Func<object, object> _page;

        #region [ POCOs ]
        //----------------------------[ POCOs ]------------------

        private List<Tax> _taxes;

        #endregion

        #region [ Models ]
        //----------------------------[ Models ]------------------

        private List<Command_itemModel> _command_itemList;
        private List<Item_deliveryModel> _item_deliveryModelBillingInProcessList;
        private CommandModel _commandSelected;
        //private List<Item_deliveryModel> _item_deliveryModelReceiptList;
        private BillModel _selectedBillToSend;
        private List<Item_deliveryModel> _item_ModelDeliveryInProcess;
        private List<Item_deliveryModel> _item_deliveryModelCreatedList;
        #endregion

        #region [ Commands ]
        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> UpdateCommand_itemListCommand { get; set; }
        public ButtonCommand<Item_deliveryModel> CancelDeliveryReceiptCreationCommand { get; set; }
        public ButtonCommand<Item_deliveryModel> CancelDeliveryReceiptCreatedCommand { get; set; }
        public ButtonCommand<BillModel> CancelBillCreatedCommand { get; set; }
        public ButtonCommand<DeliveryModel> GenerateDeliveryReceiptCreatedPdfCommand { get; set; }
        public ButtonCommand<BillModel> GeneratePdfCreatedBillCommand { get; set; }
        public ButtonCommand<Command_itemModel> DeliveryReceiptCreationCommand { get; set; }
        public ButtonCommand<Command_itemModel> BillCreationCommand { get; set; }
        public ButtonCommand<Command_itemModel> DeleteItemCommand { get; set; }
        public ButtonCommand<object> BilledCommand { get; set; }
        public ButtonCommand<Address> DeliveryAddressSelectionCommand { get; set; }
        public ButtonCommand<Tax> TaxCommand { get; set; }
        public ButtonCommand<object> GeneratePdfCreatedQuoteCommand { get; set; }
        public ButtonCommand<string> SendEmailCommand { get; set; }
        public ButtonCommand<BillModel> UpdateBillCommand { get; set; }
        public ButtonCommand<object> UpdateCommentCommand { get; set; }

        #endregion

        public CommandDetailViewModel()
        {
            instances();
            instancesPoco();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        #region [ Initialization ]
        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onCommandSelectedChange;
            PropertyChanged += onCommand_itemModelWorkFlowChange;
            Command_ItemTask.PropertyChanged += onCommand_itemTaskComplete_getCommandModel;
            //_command_itemTask_updateItem.PropertyChanged += onCommand_itemTaskCompletion;
            //_command_itemTask_updateCommand_Item.PropertyChanged += onCommand_itemTaskCompletion_updateCommand_item;
        }

        private void instances()
        {
            _selectedBillToSend = new BillModel();
            Command_ItemTask = new NotifyTaskCompletion<List<Command_item>>();
            ItemTask = new NotifyTaskCompletion<List<Item>>();
            ItemModelTask = new NotifyTaskCompletion<List<ItemModel>>();
            _command_itemTask_updateItem = new NotifyTaskCompletion<List<Command_item>>();
            _command_itemTask_updateCommand_Item = new NotifyTaskCompletion<List<Command_item>>();
            //_quoteTask = new NotifyTaskCompletion<object>();
            _paramDeliveryToPdf = new ParamDeliveryToPdf();
            _paramQuoteToPdf = new ParamCommandToPdf(EStatusCommand.Quote, 2);
            _paramCommandToPdf = new ParamCommandToPdf(EStatusCommand.Command);
            _paramCommandToPdf.Currency = _paramQuoteToPdf.Currency = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
            _paramCommandToPdf.Lang = _paramQuoteToPdf.Lang = _paramDeliveryToPdf.Lang = CultureInfo.CurrentCulture.Name.Split('-').FirstOrDefault() ?? "en";

            _mailFile = new GeneralInfos.FileWriter("");
            //_quoteEmailFile.TxtSubject = "** CODSIMEX – Votre devis n°{BILL_ID} **";
        }

        private void instancesPoco()
        {
            _taxes = new List<Tax>();
        }

        private void instancesModel()
        {
            _commandSelected = new CommandModel();
            _command_itemList = new List<Command_itemModel>();
            _item_deliveryModelBillingInProcessList = new List<Item_deliveryModel>();
            //_item_deliveryModelReceiptList = new List<Item_deliveryModel>();
            _item_deliveryModelCreatedList = new List<Item_deliveryModel>();
            _item_ModelDeliveryInProcess = new List<Item_deliveryModel>();
        }

        private void instancesCommand()
        {
            UpdateCommand_itemListCommand = new ButtonCommand<string>(updateCommand_itemData, canUpdateCommand_itemData);
            CancelDeliveryReceiptCreationCommand = new ButtonCommand<Item_deliveryModel>(cancelDeliveryReceiptInProcess, canCancelDeliveryReceiptInProcess);
            GenerateDeliveryReceiptCreatedPdfCommand = new ButtonCommand<DeliveryModel>(generateDeliveryReceiptPdf, canGenerateDeliveryReceiptPdf);
            CancelDeliveryReceiptCreatedCommand = new ButtonCommand<Item_deliveryModel>(cancelDeliveryReceiptCreated, canCancelDeliveryReceiptCreated);
            DeliveryReceiptCreationCommand = new ButtonCommand<Command_itemModel>(createDeliveryReceipt, canCreateDeliveryReceipt);
            BillCreationCommand = new ButtonCommand<Command_itemModel>(createBill, canCreateBill);
            CancelBillCreatedCommand = new ButtonCommand<BillModel>(cancelBillCreated, canCancelBillCreated);
            GeneratePdfCreatedBillCommand = new ButtonCommand<BillModel>(generateCommandBillPdf, canGenerateCommandBillPdf);
            DeleteItemCommand = new ButtonCommand<Command_itemModel>(deleteItem, canDeleteItem);
            BilledCommand = new ButtonCommand<object>(billing, canBilling);
            DeliveryAddressSelectionCommand = new ButtonCommand<Address>(selectDeliveryAddress, canSelectDeliveryAddress);
            TaxCommand = new ButtonCommand<Tax>(saveNewTax, canSaveNewTax);
            GeneratePdfCreatedQuoteCommand = new ButtonCommand<object>(generateQuotePdf, canGenerateQuotePdf);
            SendEmailCommand = new ButtonCommand<string>(sendEmail, canSendEmail);
            UpdateBillCommand = new ButtonCommand<BillModel>(updateBill, canUpdateBill);
            UpdateCommentCommand = new ButtonCommand<object>(updateComment, canUpdateComment);
        }
        #endregion

        #region [ Properties ]
        //----------------------------[ Properties ]------------------        

        public BillModel SelectedBillToSend
        {
            get { return _selectedBillToSend; }
            set { setProperty(ref _selectedBillToSend, value, "SelectedBillToSend"); }
        }

        public List<BillModel> BillModelList
        {
            get { return CommandSelected.BillModelList; }
            set { CommandSelected.BillModelList = value; onPropertyChange("BillModelList"); }
        }

        public List<DeliveryModel> DeliveryModelList
        {
            get { return CommandSelected.DeliveryModelList; }
            set { CommandSelected.DeliveryModelList = value; onPropertyChange("DeliveryModelList"); }
        }

        public GeneralInfos.FileWriter EmailFile
        {
            get { return _mailFile; }
            set { setProperty(ref _mailFile, value, "QuoteEmailFile"); }
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            _getObjectFromMainWindowViewModel = getObject;
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public CommandModel CommandSelected
        {
            get { return _commandSelected; }
            set { setProperty(ref _commandSelected, value, "CommandSelected"); }
        }

        public string TxtQuoteValidityInDays
        {
            get { return _paramQuoteToPdf.ValidityDay.ToString(); }
            set { int convertedNumber; if (int.TryParse(value, out convertedNumber)) { _paramQuoteToPdf.ValidityDay = convertedNumber; onPropertyChange("QuoteValidityInDays"); } }
        }

        public bool IsQuote
        {
            get
            {
                if (_paramQuoteToPdf.TypeQuoteOrProformat == EStatusCommand.Quote)
                    return true;

                return false;
            }
            set
            {
                if (value == true)
                {
                    _paramQuoteToPdf.TypeQuoteOrProformat = EStatusCommand.Quote;
                    onPropertyChange("IsQuote");
                }
            }
        }

        public bool IsProForma
        {
            get
            {
                if (_paramQuoteToPdf.TypeQuoteOrProformat == EStatusCommand.Proforma)
                    return true;

                return false;
            }
            set
            {
                if (value == true)
                {
                    _paramQuoteToPdf.TypeQuoteOrProformat = EStatusCommand.Proforma;
                    onPropertyChange("IsProForma");
                }
            }
        }

        public bool IsQuoteReferencesVisible
        {
            get { return _paramQuoteToPdf.IsQuoteConstructorReferencesVisible; }
            set { _paramQuoteToPdf.IsQuoteConstructorReferencesVisible = value; onPropertyChange("IsQuoteReferencesVisible"); }
        }

        public bool IsCommandReferencesVisible
        {
            get { return _paramCommandToPdf.IsCommandConstructorReferencesVisible; }
            set { _paramCommandToPdf.IsCommandConstructorReferencesVisible = value; onPropertyChange("IsCommandReferencesVisible"); }
        }

        public string TxtTotalAfterTax
        {
            get { return _totalAfterTax.ToString(); }
            set { setProperty(ref _totalAfterTax, Convert.ToDecimal(value), "TxtTotalAfterTax"); }
        }

        public string TxtTotalPercentProfit
        {
            get { return _totalPercentProfit.ToString(); }
            set { setProperty(ref _totalPercentProfit, Convert.ToDecimal(value), "TxtTotalPercentProfit"); }
        }

        public string TxtTotalProfit
        {
            get { return _totalProfit.ToString(); }
            set { setProperty(ref _totalProfit, Convert.ToDecimal(value), ""); }
        }

        public string TxtTotalTaxAmount
        {
            get { return _totalTaxAmount.ToString(); }
            set { setProperty(ref _totalTaxAmount, Convert.ToDecimal(value), "TxtTotalTaxAmount"); }
        }

        public Tax Tax
        {
            get { return _commandSelected.Tax; }
            set { _commandSelected.Tax = value; onPropertyChange("Tax"); }
        }

        public string TxtTotalBeforeTax
        {
            get { return _totalBeforeTax.ToString(); }
            set { setProperty(ref _totalBeforeTax, Convert.ToDecimal(value), "TxtTotalBeforeTax"); }
        }

        public string TxtTotalPurchase
        {
            get { return _totalPurchase.ToString(); }
            set { setProperty(ref _totalPurchase, Convert.ToDecimal(value), "TxtTotalPurchase"); }
        }

        public List<Command_itemModel> Command_ItemModelList
        {
            get { return _command_itemList; }
            set { _command_itemList = value; updateDeliveryAndInvoiceListBindingByCallingPropertyChange(); }// setProperty(ref _command_itemList, value, "Command_ItemModelList");
        }

        public List<Item_deliveryModel> Item_ModelDeliveryInProcess
        {
            get { return _item_ModelDeliveryInProcess; }//getCommand_itemModelListFilterBy("Item_ModelDeliveryInProcess")
            set { setProperty(ref _item_ModelDeliveryInProcess, value, "Item_ModelDeliveryInProcess"); }
        }

        public List<Item_deliveryModel> Item_deliveryModelCreatedList
        {
            get { return _item_deliveryModelCreatedList; }//getCommand_itemModelListFilterBy("Item_ModelDeliveryInProcess")
            set { setProperty(ref _item_deliveryModelCreatedList, value, "Item_deliveryModelCreatedList"); }
        }

        public List<Item_deliveryModel> Item_deliveryModelBillingInProcess
        {
            get { return _item_deliveryModelBillingInProcessList; }
            set { setProperty(ref _item_deliveryModelBillingInProcessList, value, "Item_deliveryModelBillingInProcess"); BillCreationCommand.raiseCanExecuteActionChanged(); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); BillCreationCommand.raiseCanExecuteActionChanged(); }
        }

        #endregion

        #region [ Signaling ]
        //---------------------------[ Signaling ]------------------

        public bool IsItemListCommentTextBoxEnabled
        {
            get { return disableUIElementByBoolean("IsItemListCommentTextBoxEnabled"); }
        }

        public bool IsItemListQuantityReceivedTextBoxEnabled
        {
            get { return disableUIElementByBoolean("IsItemListQuantityReceivedTextBoxEnabled"); }
        }

        public bool IsItemListPurchasePriceTextBoxEnable
        {
            get { return disableUIElementByBoolean("IsItemListPurchasePriceTextBoxEnable"); }
        }

        public bool IsItemListSellingPriceTextBoxEnable
        {
            get { return disableUIElementByBoolean("IsItemListSellingPriceTextBoxEnable"); }
        }

        public bool IsItemListQuantityTextBoxEnable
        {
            get { return disableUIElementByBoolean("IsItemListQuantityTextBoxEnable"); }
        }

        public string BlockItemListDetailVisibility
        {
            get { return disableUIElementByString("BlockItemListDetailVisibility"); }
        }

        public string BlockEmailVisibility
        {
            get { return disableUIElementByString("BlockEmailVisibility"); }
        }

        public string BlockBillCreationVisibility
        {
            get { return disableUIElementByString("BlockBillCreationVisibility"); }
        }

        public string BlockDeliveryReceiptCreationVisiblity
        {
            get { return disableUIElementByString("BlockDeliveryReceiptCreationVisiblity"); }
        }

        public string BlockDeliveryAddressVisiblity
        {
            get { return disableUIElementByString("BlockDeliveryAddressVisiblity"); }
        }

        public string BlockDeliveryReceiptCreatedVisibility
        {
            get { return disableUIElementByString("BlockDeliveryReceiptCreatedVisibility"); }
        }

        public string BlockBillCreatedVisibility
        {
            get { return disableUIElementByString("BlockBillCreatedVisibility"); }
        }

        public string BlockStepOneVisibility
        {
            get { return disableUIElementByString("BlockStepOneVisibility"); }
        }

        public string BlockStepTwoVisibility
        {
            get { return disableUIElementByString("BlockStepTwoVisibility"); }
        }

        public string BlockStepThreeVisibility
        {
            get { return disableUIElementByString("BlockStepThreeVisibility"); }
        }

        public string BlockDeliveryListToIncludeVisibility
        {
            get { return disableUIElementByString("BlockDeliveryListToIncludeVisibility"); }
        }

        public string BlockBillListBoxVisibility
        {
            get { return disableUIElementByString("BlockBillListBoxVisibility"); }
        }

        #endregion

        #region [ Actions ]
        //----------------------------[ Actions ]------------------

        public async Task<List<Command_itemModel>> Command_ItemListToModelViewList(List<Command_item> CommandItemList)
        {
            List<Command_itemModel> output = new List<Command_itemModel>();
            foreach (Command_item commandItem in CommandItemList)
            {
                Command_itemModel command_item = new Command_itemModel();
                command_item.PropertyChanged += onTotalSelling_PriceOrPrice_purchaseChange;
                command_item.Command_Item = commandItem;

                //load item and its inforamtion (delivery and item_delivery)
                command_item.ItemModel = await loadCommand_itemItem(commandItem.Item_ref, command_item.Command_Item.ItemId);

                output.Add(command_item);
            }

            return output;
        }

        private async Task<List<ItemModel>> ItemListToModelViewList(List<Item> itemList)
        {
            List<ItemModel> output = new List<ItemModel>();
            foreach (Item item in itemList)
            {
                ItemModel itemModel = new ItemModel();
                itemModel.Item = item;

                var itemDelivery = new Item_delivery();
                itemDelivery.Item_ref = item.Ref;
                itemDelivery.ItemId = item.ID;

                // search for the delivery reference of the item
                var item_DeliveryFoundList = (await Bl.BlItem.searchItem_delivery(itemDelivery, "AND")).Select(x => new Item_deliveryModel { Item_delivery = x, Item = item }).ToList();
                if (item_DeliveryFoundList != null && item_DeliveryFoundList.Count > 0)
                    itemModel.Item_deliveryModelList = item_DeliveryFoundList;

                // search for the item's delivery receipt
                foreach (var item_delivery in itemModel.Item_deliveryModelList)
                {
                    var deliveryFoundList = (await Bl.BlCommand.searchDelivery(new Delivery { ID = item_delivery.Item_delivery.DeliveryId }, "AND"));
                    if (deliveryFoundList.Count > 0)
                        item_delivery.DeliveryModel = deliveryFoundList.Select(x => new DeliveryModel { Delivery = x }).FirstOrDefault();
                }
                output.Add(itemModel);
            }
            return output;
        }

        public void loadCommand_items()
        {
            Command_ItemTask.initializeNewTask(Bl.BlCommand.searchCommand_item(new Command_item { CommandId = CommandSelected.Command.ID }, "AND"));
            loadEmail();
        }

        private async void loadEmail()
        {
            if (CommandSelected != null)
            {
                var infos = (await Bl.BlReferential.searchInfos(new Infos { Name = "Company_name" }, "AND")).FirstOrDefault();
                var infosFTP = (await _startup.Bl.BlReferential.searchInfos(new QCBDManagementCommon.Entities.Infos { Name = "ftp_" }, "AND")).ToList();
                string login = infosFTP.Where(x => x.Value == "ftp_login").Select(x => x.Value).FirstOrDefault() ?? "";
                string password = infosFTP.Where(x => x.Value == "ftp_password").Select(x => x.Value).FirstOrDefault() ?? "";
                switch (CommandSelected.TxtStatus)
                {
                    case "Quote":
                        EmailFile = new GeneralInfos.FileWriter("quote", ftpLogin: login, ftpPassword: password);
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Your Quote n°{BILL_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Quote n°{BILL_ID} **";
                        break;
                    case "Pre_Command":
                        EmailFile = new GeneralInfos.FileWriter("command_confirmation", ftpLogin: login, ftpPassword: password);
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Your Command n°{BILL_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Command n°{BILL_ID} **";
                        break;
                    case "Pre_Credit":
                        EmailFile = new GeneralInfos.FileWriter("command_confirmation", ftpLogin: login, ftpPassword: password);
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Your Credit n°{BILL_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Credit n°{BILL_ID} **";
                        break;
                    case "Command":
                        EmailFile = new GeneralInfos.FileWriter("bill", ftpLogin: login, ftpPassword: password);
                        if (infos != null)
                            EmailFile.TxtSubject = "** " + infos.Value + " – Bill n°{BILL_ID} **";
                        else
                            EmailFile.TxtSubject = "** Your Bill n°{BILL_ID} **";
                        break;
                }
            }

        }

        internal void mainNavigObject(Func<object, object> navigObject)
        {
            _page = navigObject;
        }

        /// <summary>
        /// load all bills of the selected command
        /// </summary>
        public async void loadInvoicesAndDeliveryReceipts()
        {
            Item_deliveryModelBillingInProcess = (from c in Command_ItemModelList
                                                  from d in c.ItemModel.Item_deliveryModelList
                                                  where d.DeliveryModel.TxtStatus == EStatusCommand.Not_Billed.ToString()
                                                  select d).ToList();

            Item_ModelDeliveryInProcess = Command_ItemModelList.Where(x => x.Command_Item.Quantity_current > 0).Select(x => new Item_deliveryModel { Item = x.ItemModel.Item, TxtQuantity_current = x.TxtQuantity_current, TxtQuantity_delivery = x.TxtQuantity_delivery }).ToList();

            BillModelList = new BillModel().BillListToModelViewList(await Bl.BlCommand.searchBill(new Bill { CommandId = CommandSelected.Command.ID }, "AND"));
            DeliveryModelList = new DeliveryModel().DeliveryListToModelViewList(await Bl.BlCommand.searchDelivery(new Delivery { CommandId = CommandSelected.Command.ID }, "AND"));

        }

        /// <summary>
        /// Load all delivery receipts for command ready to be sent
        /// </summary>
        /*public async void loadDeliveryReceipts()
        {
            CommandSelected.DeliveryModelList = new DeliveryModel().DeliveryListToModelViewList(await Bl.BlCommand.searchDelivery(new Delivery { CommandId= CommandSelected.Command.ID }, "AND"));
        }*/

        private void totalCalcul()
        {
            if (Command_ItemModelList.Count > 0)
            {
                decimal totalProfit = 0.0m;
                decimal totalBeforeTax = 0.0m;
                decimal totalPurchase = 0.0m;

                foreach (Command_itemModel command_itemModel in Command_ItemModelList)
                {
                    var command_item = command_itemModel.Command_Item;
                    totalProfit += command_item.Quantity * (command_item.Price - command_item.Price_purchase);
                    totalBeforeTax += Convert.ToDecimal(command_itemModel.TxtTotalSelling);
                    totalPurchase += Convert.ToDecimal(command_itemModel.TxtTotalPurchase);
                }
                this.TxtTotalTaxAmount = Convert.ToString(((decimal)CommandSelected.Tax_command.Tax_value / 100) * totalBeforeTax);
                this.TxtTotalProfit = totalProfit.ToString();
                this.TxtTotalBeforeTax = totalBeforeTax.ToString();
                this.TxtTotalAfterTax = string.Format("{0:0.00}", (totalBeforeTax + (totalBeforeTax) * ((decimal)CommandSelected.Tax_command.Tax_value) / 100));
                try
                {
                    this.TxtTotalPercentProfit = string.Format("{0:0.00}", ((totalProfit / totalBeforeTax) * 100));
                }
                catch (DivideByZeroException) { this.TxtTotalPercentProfit = "0"; }

                this.TxtTotalPurchase = string.Format("{0:0.00}", totalPurchase);
            }
        }

        private async void loadAddresses()
        {
            CommandSelected.AddressList = await Bl.BlClient.searchAddress(new Address { ClientId = CommandSelected.CLientModel.Client.ID }, "AND");
        }

        public async Task<ItemModel> loadCommand_itemItem(string itemRef, int itemId)
        {
            var itemFoundList = await Bl.BlItem.searchItem(new Item { ID = itemId, Ref = itemRef }, "AND");
            if (itemFoundList.Count > 0)
                return (await ItemListToModelViewList(new List<Item> { itemFoundList[0] })).FirstOrDefault();
            return new ItemModel();
        }

        private bool disableUIElementByBoolean(string obj)
        {

            if ((CommandSelected.TxtStatus.Equals(EStatusCommand.Bill_Command.ToString()) || CommandSelected.TxtStatus.Equals(EStatusCommand.Bill_Credit.ToString()))
                && (obj.Equals("IsItemListCommentTextBoxEnabled")
                || obj.Equals("IsItemListQuantityReceivedTextBoxEnabled")
                || obj.Equals("IsItemListQuantityTextBoxEnable")
                || obj.Equals("IsItemListSellingPriceTextBoxEnable")
                || obj.Equals("IsItemListPurchasePriceTextBoxEnable")))
                return false;
            if ((CommandSelected.TxtStatus.Equals(EStatusCommand.Command_Close.ToString()) || CommandSelected.TxtStatus.Equals(EStatusCommand.Credit_CLose.ToString()))
                && (obj.Equals("IsItemListCommentTextBoxEnabled")
                || obj.Equals("IsItemListQuantityReceivedTextBoxEnabled")
                || obj.Equals("IsItemListQuantityTextBoxEnable")
                || obj.Equals("IsItemListSellingPriceTextBoxEnable")
                || obj.Equals("IsItemListPurchasePriceTextBoxEnable")))
                return false;

            return true;
        }

        private string disableUIElementByString(string obj)
        {
            if (!CommandSelected.TxtStatus.Equals(EStatusCommand.Command.ToString())
                && obj.Equals("BlockItemListDetailVisibility"))
                return "Collapsed";

            else if (CommandSelected.TxtStatus.Equals(EStatusCommand.Command.ToString())
                && obj.Equals("BlockItemListDetailVisibility"))
                return "VisibleWhenSelected";

            if ((!CommandSelected.TxtStatus.Equals(EStatusCommand.Command.ToString()) && !CommandSelected.TxtStatus.Equals(EStatusCommand.Credit.ToString()))
                && (obj.Equals("BlockDeliveryListToIncludeVisibility")
                || obj.Equals("BlockStepOneVisibility")
                || obj.Equals("BlockStepTwoVisibility")
                || obj.Equals("BlockStepThreeVisibility")))
                return "Hidden";

            /*if (CommandSelected.TxtStatus.Equals(EStatusCommand.Quote.ToString())
                && obj.Equals("BlockBillListBoxVisibility"))
                return "Hidden";*/

            if (CommandSelected.TxtStatus.Equals(EStatusCommand.Quote.ToString())
                && (obj.Equals("BlockDeliveryReceiptCreatedVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")
                || obj.Equals("BlockBillCreatedVisibility")
                || obj.Equals("BlockBillListBoxVisibility")
                ))
                return "Hidden";

            if ((CommandSelected.TxtStatus.Equals(EStatusCommand.Pre_Command.ToString()) || CommandSelected.TxtStatus.Equals(EStatusCommand.Pre_Credit.ToString()))
                && (obj.Equals("BlockEmailVisibility")
                || obj.Equals("BlockDeliveryReceiptCreatedVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")
                || obj.Equals("BlockBillCreatedVisibility")
                ))
                return "Hidden";

            if ((CommandSelected.TxtStatus.Equals(EStatusCommand.Command.ToString()) || CommandSelected.TxtStatus.Equals(EStatusCommand.Credit.ToString()))
                && (obj.Equals("BlockStepOneVisibility") && Item_ModelDeliveryInProcess.Count == 0
                || obj.Equals("BlockStepTwoVisibility") && Item_deliveryModelBillingInProcess.Count == 0
                || obj.Equals("BlockStepThreeVisibility") && CommandSelected.BillModelList.Count == 0
                ))
                return "Hidden";

            if ((CommandSelected.TxtStatus.Equals(EStatusCommand.Bill_Command.ToString()) || CommandSelected.TxtStatus.Equals(EStatusCommand.Bill_Credit.ToString()))
                && (obj.Equals("BlockStepVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")))
                return "Hidden";

            if ((CommandSelected.TxtStatus.Equals(EStatusCommand.Command_Close.ToString()) || CommandSelected.TxtStatus.Equals(EStatusCommand.Credit_CLose.ToString()))
                && (obj.Equals("BlockStepVisibility")
                || obj.Equals("BlockDeliveryReceiptCreationVisiblity")
                || obj.Equals("BlockBillCreationVisibility")))
                return "Hidden";

            return "Visible";
        }


        private void updateStepBinding()
        {
            onPropertyChange("BlockStepOneVisibility");
            onPropertyChange("BlockStepTwoVisibility");
            onPropertyChange("BlockStepThreeVisibility");
        }

        private void updateDeliveryAndInvoiceListBindingByCallingPropertyChange()
        {
            onPropertyChange("Command_ItemModelList");
            onPropertyChange("Item_deliveryModelCreatedList");
            onPropertyChange("Item_ModelDeliveryInProcess");
            onPropertyChange("Item_deliveryModelBillingInProcess");
        }

        public async void updateCommandStatus(EStatusCommand status)
        {
            bool canChangeStatus = true;
            switch (status)
            {
                case EStatusCommand.Command:
                    lockCommand_itemItems();
                    break;
                case EStatusCommand.Quote:
                    cleanUpBeforeConvertingToQuote();
                    break;
                case EStatusCommand.Billed:
                    break;
                case EStatusCommand.Bill_Command:
                    break;
                case EStatusCommand.Bill_Credit:
                    break;
                case EStatusCommand.Pre_Command:
                    break;
                case EStatusCommand.Pre_Credit:
                    createCredit();
                    break;
                case EStatusCommand.Command_Close:
                    canChangeStatus = await Dialog.show("Command_Close: Be careful as it will not be possible to do any change after.");
                    break;
                case EStatusCommand.Credit_CLose:
                    canChangeStatus = await Dialog.show("Credit_CLose: Be careful as it will not be possible to do any change after.");
                    break;
            }
            if (canChangeStatus)
            {
                CommandSelected.TxtStatus = status.ToString();
                CommandSelected.Command.Date = DateTime.Now;
                var savedCommandList = await Bl.BlCommand.UpdateCommand(new List<Entity.Command> { { CommandSelected.Command } });
                if (savedCommandList.Count > 0)
                    CommandSelected.Command = savedCommandList[0];
            }
        }

        private async void cleanUpBeforeConvertingToQuote()
        {
            bool canDelete = false;
            Bill lastBill = await Bl.BlCommand.GetLastBill();
            for (int i = 0; i < BillModelList.Count(); i++)
            {
                if (lastBill.ID - i == BillModelList[i].Bill.ID)
                    canDelete = true;
                else
                {
                    canDelete = false;
                    break;
                }
            }

            if (canDelete)
            {
                var Item_deliveryFoundListToDelete = await Bl.BlItem.GetItem_deliveryDataByDeliveryList(DeliveryModelList.Select(x => x.Delivery).ToList());
                var tax_commandFoundListToDelete = await Bl.BlCommand.GetTax_commandDataByCommandList(new List<Entity.Command> { CommandSelected.Command });

                // deleting
                await Bl.BlCommand.DeleteTax_command(tax_commandFoundListToDelete);
                await Bl.BlItem.DeleteItem_delivery(Item_deliveryFoundListToDelete);
                await Bl.BlCommand.DeleteDelivery(DeliveryModelList.Select(x => x.Delivery).ToList());
                await Bl.BlCommand.DeleteBill(BillModelList.Select(x => x.Bill).ToList());
                foreach (var command_itemToUpdate in CommandSelected.Command_ItemList)
                {
                    command_itemToUpdate.Command_Item.Quantity_current = 0;
                    command_itemToUpdate.Command_Item.Quantity_delivery = 0;
                    await Bl.BlCommand.UpdateCommand_item(new List<Command_item> { command_itemToUpdate.Command_Item });
                }
                BillModelList = new List<BillModel>();
                DeliveryModelList = new List<DeliveryModel>();
                if (CommandSelected.TxtStatus.Equals(EStatusCommand.Credit.ToString()))
                    createCredit(isReset: true);
            }
            else
                await Dialog.show("Command bills are not the latest!");

        }

        private void refreshBindings()
        {
            loadInvoicesAndDeliveryReceipts();
        }


        private async void lockCommand_itemItems()
        {
            List<Item> itemToSaveList = new List<Item>();
            foreach (var command_itemModel in Command_ItemModelList)
            {
                command_itemModel.ItemModel.TxtErasable = EItem.No.ToString();
                itemToSaveList.Add(command_itemModel.ItemModel.Item);
            }
            await Bl.BlItem.UpdateItem(itemToSaveList);
        }


        private async void createCredit(bool isReset = false)
        {
            List<Command_item> command_itemToSave = new List<Command_item>();
            foreach (Command_itemModel command_itemModel in Command_ItemModelList)
            {
                command_itemModel.Command_Item.Price = (!isReset) ? Math.Abs(command_itemModel.Command_Item.Price) * (-1) : Math.Abs(command_itemModel.Command_Item.Price);
                command_itemModel.Command_Item.Price_purchase = (!isReset) ? Math.Abs(command_itemModel.Command_Item.Price_purchase) * (-1) : Math.Abs(command_itemModel.Command_Item.Price);
                command_itemToSave.Add(command_itemModel.Command_Item);
            }
            var savedCommand_item = await Bl.BlCommand.UpdateCommand_item(command_itemToSave);
        }

        public override void Dispose()
        {
            PropertyChanged -= onCommandSelectedChange;
            PropertyChanged -= onCommand_itemModelWorkFlowChange;
            Command_ItemTask.PropertyChanged -= onCommand_itemTaskComplete_getCommandModel;
            foreach (var command_itemModel in Command_ItemModelList)
            {
                command_itemModel.PropertyChanged -= onTotalSelling_PriceOrPrice_purchaseChange;
            }
        }

        #endregion

        #region [ Event Handler ]
        //----------------------------[ Event Handler ]------------------

        private void onCommandSelectedChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CommandSelected"))
            {
                loadCommand_items();
                //loadBills();
                loadAddresses();
                //loadTaxCommand();
            }
        }

        private void onTotalSelling_PriceOrPrice_purchaseChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "TxtTotalSelling") || string.Equals(e.PropertyName, "TxtPrice") || string.Equals(e.PropertyName, "TxtPrice_purchase"))
                totalCalcul();
        }

        private async void onCommand_itemTaskComplete_getCommandModel(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSuccessfullyCompleted"))
            {
                Dialog.showSearch("Loading items...");
                Command_ItemModelList = await Command_ItemListToModelViewList(Command_ItemTask.Result);//.Where(x=>x.ItemModel.Item.Ref != null).ToList();

                totalCalcul();
                refreshBindings();
                Dialog.IsDialogOpen = false;
            }
        }

        private void onCommand_itemModelWorkFlowChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Item_ModelDeliveryInProcess")
                || e.PropertyName.Equals("Item_deliveryModelBillingInProcess")
                || e.PropertyName.Equals("BillModelList"))
            {
                updateStepBinding();
            }
        }

        #endregion
        //----------------------------[ Action Commands ]------------------

        private async void updateCommand_itemData(string obj)
        {
            Dialog.showSearch("Updating...");
            List<Command_item> command_itemToSave = new List<Command_item>();
            foreach (var command_itemModelNew in Command_ItemModelList)
            {
                int quantityReceived = Convert.ToInt32(command_itemModelNew.TxtQuantity_received);
                int quantity = command_itemModelNew.Command_Item.Quantity;
                int quantityDelivery = command_itemModelNew.Command_Item.Quantity_delivery;
                int quantityCurrent = command_itemModelNew.Command_Item.Quantity_current;

                if (quantityReceived > 0)
                {
                    int quentityPending = quantity - (quantityDelivery + quantityReceived);
                    if (quentityPending >= 0)
                    {
                        // checking that the user is not giving more received Item than expected
                        if (quantityReceived > (quantity - quantityDelivery))
                            quantityReceived = (quantity - quantityDelivery);

                        quantityDelivery += quantityReceived;
                        quantityCurrent += quantityReceived;
                        command_itemModelNew.Command_Item.Quantity_current = quantityCurrent;
                        command_itemModelNew.Command_Item.Quantity_delivery = quantityDelivery;

                    }
                }
                command_itemToSave.Add(command_itemModelNew.Command_Item);
                command_itemModelNew.TxtQuantity_received = 0.ToString();
            }

            var savedCommand_itemList = await Bl.BlCommand.UpdateCommand_item(command_itemToSave);

            updateDeliveryAndInvoiceListBindingByCallingPropertyChange();
            refreshBindings();
            Dialog.IsDialogOpen = false;
            _page(this);
        }

        private bool canUpdateCommand_itemData(string arg)
        {
            bool isUpdate = securityCheck(EAction.Command, ESecurity._Update);
            if (!isUpdate)
                return false;

            if (string.IsNullOrEmpty(CommandSelected.TxtStatus)
                || !CommandSelected.TxtStatus.Equals(EStatusCommand.Command.ToString())
                && !CommandSelected.TxtStatus.Equals(EStatusCommand.Quote.ToString())
                && !CommandSelected.TxtStatus.Equals(EStatusCommand.Pre_Command.ToString())
                && !CommandSelected.TxtStatus.Equals(EStatusCommand.Pre_Credit.ToString()))
                return false;
            return true;
        }

        private void deleteItem(Command_itemModel obj)
        {
            Dialog.showSearch("Deleting...");
            Bl.BlCommand.DeleteCommand_item(new List<Command_item> { obj.Command_Item });
            Command_ItemModelList.Remove(obj);
            updateDeliveryAndInvoiceListBindingByCallingPropertyChange();
            Dialog.IsDialogOpen = false;
        }

        private bool canDeleteItem(Command_itemModel arg)
        {
            bool isDelete = securityCheck(EAction.Command, ESecurity._Delete);
            if (isDelete)
                return true;

            return false;
        }

        private void generateDeliveryReceiptPdf(DeliveryModel obj)
        {
            Dialog.showSearch("Pdf creation...");
            _paramDeliveryToPdf.CommandId = CommandSelected.Command.ID;
            _paramDeliveryToPdf.DeliveryId = obj.Delivery.ID;
            Bl.BlCommand.GeneratePdfDelivery(_paramDeliveryToPdf);
            Dialog.IsDialogOpen = false;
        }

        private bool canGenerateDeliveryReceiptPdf(DeliveryModel arg)
        {
            return true;
        }

        private async void createDeliveryReceipt(Command_itemModel obj)
        {
            Dialog.showSearch("Delivery receipt creation...");
            int first = 0;
            List<Delivery> insertNewDeliveryList = new List<Delivery>();
            List<Delivery> savedDeliveryList = new List<Delivery>();
            List<Command_item> command_itemListToUpdate = new List<Command_item>();

            foreach (var item_deliveryModel in Item_ModelDeliveryInProcess)
            {
                if (item_deliveryModel.TxtQuantity_current != 0.ToString())
                {
                    if (first == 0)
                    {
                        // creation of the delivery receipt
                        Delivery delivery = new Delivery();
                        delivery.CommandId = CommandSelected.Command.ID;
                        delivery.Date = DateTime.Now;
                        delivery.Status = EStatusCommand.Not_Billed.ToString();
                        delivery.Package = item_deliveryModel.DeliveryModel.Delivery.Package;
                        insertNewDeliveryList.Add(delivery);
                        savedDeliveryList = await Bl.BlCommand.InsertDelivery(insertNewDeliveryList);
                        first++;
                    }

                    // creation of the reference of the delivery created inside item_delivery
                    if (savedDeliveryList.Count > 0)
                    {
                        Item_delivery item_delivery = new Item_delivery();
                        item_delivery.DeliveryId = savedDeliveryList[0].ID;
                        item_delivery.Item_ref = item_deliveryModel.Item.Ref;
                        item_delivery.ItemId = item_deliveryModel.Item.ID;
                        item_delivery.Quantity_delivery = Convert.ToInt32(item_deliveryModel.TxtQuantity_delivery);
                        var savedItem_deliveryList = await Bl.BlItem.InsertItem_delivery(new List<Item_delivery> { item_delivery });
                        var command_itemModelFound = (from c in Command_ItemModelList
                                                      where c.ItemModel.Item.ID == item_deliveryModel.Item.ID && c.ItemModel.Item.Ref == item_deliveryModel.Item.Ref
                                                      select c).FirstOrDefault();
                        if (savedItem_deliveryList.Count > 0 && command_itemModelFound != null)
                        {


                            command_itemModelFound.Command_Item.Quantity_current = 0;
                            command_itemModelFound.ItemModel.Item_deliveryModelList.Add(savedItem_deliveryList.Select(x => new Item_deliveryModel { Item_delivery = x, DeliveryModel = new DeliveryModel { Delivery = savedDeliveryList[0] } }).FirstOrDefault());
                            command_itemListToUpdate.Add(command_itemModelFound.Command_Item);
                        }
                    }
                }
            }
            var savedCommand_itemList = await Bl.BlCommand.UpdateCommand_item(command_itemListToUpdate);
            refreshBindings();
            Dialog.IsDialogOpen = false;
        }

        private bool canCreateDeliveryReceipt(Command_itemModel arg)
        {
            bool isUpdate = securityCheck(EAction.Command, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        private async void cancelDeliveryReceiptInProcess(Item_deliveryModel obj)
        {
            Dialog.showSearch("Delivery receipt creation cancelling...");
            var command_itemFound = (from c in Command_ItemModelList
                                     where c.ItemModel.Item.Ref == obj.Item.Ref && c.ItemModel.Item.ID == obj.Item.ID
                                     select c).FirstOrDefault();

            if (command_itemFound != null)
            {
                command_itemFound.Command_Item.Quantity_delivery = Math.Max(0, command_itemFound.Command_Item.Quantity_delivery - command_itemFound.Command_Item.Quantity_current);
                command_itemFound.Command_Item.Quantity_current = 0;
                var command_itemSavedList = await Bl.BlCommand.UpdateCommand_item(new List<Command_item> { command_itemFound.Command_Item });
                Item_ModelDeliveryInProcess.Remove(obj);
                updateDeliveryAndInvoiceListBindingByCallingPropertyChange();
                refreshBindings();
            }
            _page(this);
            Dialog.IsDialogOpen = false;
        }

        private bool canCancelDeliveryReceiptInProcess(Item_deliveryModel arg)
        {
            bool isUpdate = securityCheck(EAction.Command, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        private async void cancelDeliveryReceiptCreated(Item_deliveryModel obj)
        {
            Dialog.showSearch("Delivery receipt created cancelling...");
            // search of the quantity current to reset
            var command_itemModelfound = (from c in Command_ItemModelList
                                          from d in c.ItemModel.Item_deliveryModelList
                                          where d.Item_delivery.DeliveryId == obj.DeliveryModel.Delivery.ID
                                          select c).FirstOrDefault();

            if (command_itemModelfound != null && command_itemModelfound.TxtQuantity_pending != command_itemModelfound.TxtQuantity)
            {
                command_itemModelfound.Command_Item.Quantity_current = obj.Item_delivery.Quantity_delivery;
                command_itemModelfound.ItemModel.Item_deliveryModelList.Remove(obj);
                var savedCommand_itemList = await Bl.BlCommand.UpdateCommand_item(new List<Command_item> { command_itemModelfound.Command_Item });
                await Bl.BlCommand.DeleteDelivery(new List<Delivery> { obj.DeliveryModel.Delivery });
                await Bl.BlItem.DeleteItem_delivery(new List<Item_delivery> { obj.Item_delivery });
            }
            else
            {
                command_itemModelfound.ItemModel.Item_deliveryModelList.Remove(obj);
                await Bl.BlCommand.DeleteDelivery(new List<Delivery> { obj.DeliveryModel.Delivery });
                await Bl.BlItem.DeleteItem_delivery(new List<Item_delivery> { obj.Item_delivery });
            }

            updateDeliveryAndInvoiceListBindingByCallingPropertyChange();
            refreshBindings();
            Dialog.IsDialogOpen = false;
        }

        private bool canCancelDeliveryReceiptCreated(Item_deliveryModel arg)
        {
            bool isUpdate = securityCheck(EAction.Command, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        private async void createBill(Command_itemModel obj)
        {
            Dialog.showSearch("Bill creation...");
            List<Bill> billSavedList = new List<Bill>();
            var command_itemInProcess = new List<Command_itemModel>();
            Client searchClient = new Client();
            decimal totalInvoiceAmount = 0m;

            // get the limit date of pay
            searchClient.ID = CommandSelected.CLientModel.Client.ID;
            var foundClients = await Bl.BlClient.searchClient(searchClient, "AND");
            int payDelay = (foundClients.Count > 0) ? foundClients[0].PayDelay : 0;
            int months = (((DateTime.Now.Day + payDelay) / 30) != 0) ? (DateTime.Now.Day + payDelay) / 30 : 1;
            int days = (((DateTime.Now.Day + payDelay) % 30) != 0) ? (DateTime.Now.Day + payDelay) % 30 : 1;
            DateTime expire = new DateTime(DateTime.Now.Year, DateTime.Now.Month + months, days, 23, 59, 58);

            int first = 0;

            foreach (var item_deliveryModel in Item_deliveryModelBillingInProcess)
            {
                if (item_deliveryModel.IsSelected)
                {
                    // search of the last inserted bill 
                    Bill lastBill = (await Bl.BlCommand.GetLastBill()) ?? new Bill();

                    if (first == 0)
                    {
                        // We increment the bill id ourself 
                        //to make sure the IDs follow each others                      
                        int billId = lastBill.ID + 1;
                        Bill bill = new Bill();
                        bill.ID = billId;
                        bill.CommandId = CommandSelected.Command.ID;
                        bill.ClientId = CommandSelected.Command.ClientId;
                        bill.Date = DateTime.Now;
                        bill.DateLimit = expire;
                        bill.PayReceived = 0m;

                        // we create the bill
                        billSavedList = await Bl.BlCommand.InsertBill(new List<Bill> { bill });
                        first = 1;
                    }

                    // Update of delivery bill status
                    if (billSavedList.Count > 0)
                    {
                        DeliveryModel searchDeliveryModelFound = null;
                        //var searchDeliveryFound = (await Bl.BlCommand.searchDelivery(new Delivery { ID= item_deliveryModel.DeliveryModel.Delivery.ID, Status = EStatusCommand.Not_Billed.ToString() }, "AND")).FirstOrDefault();
                        command_itemInProcess = Command_ItemModelList.Where(x => x.TxtItem_ref == item_deliveryModel.TxtItem_ref).ToList();
                        var deliveryModelFoundList = command_itemInProcess.Select(x => x.ItemModel.Item_deliveryModelList.Where(y => y.DeliveryModel.Delivery.ID == item_deliveryModel.DeliveryModel.Delivery.ID && y.DeliveryModel.TxtStatus == EStatusCommand.Not_Billed.ToString()).Select(z => z.DeliveryModel)).FirstOrDefault().ToList();
                        //.Where(x => x.ItemModel.Item_deliveryModelList.Select()
                        //.Where(y => y.DeliveryModel.Delivery.ID == item_deliveryModel.DeliveryModel.Delivery.ID && y.DeliveryModel.TxtStatus == EStatusCommand.Not_Billed.ToString()).Count() > 0)
                        //    .Select(x => x.ItemModel.Item_deliveryModelList.Select(y => y.DeliveryModel))
                        //        .FirstOrDefault();
                        if (deliveryModelFoundList != null && deliveryModelFoundList.Count() > 0)
                            searchDeliveryModelFound = deliveryModelFoundList.FirstOrDefault();

                        if (searchDeliveryModelFound != null)
                        {
                            searchDeliveryModelFound.Delivery.Status = EStatusCommand.Billed.ToString();
                            searchDeliveryModelFound.Delivery.BillId = billSavedList[0].ID;
                            var savedDeliveryList = await Bl.BlCommand.UpdateDelivery(new List<Delivery> { searchDeliveryModelFound.Delivery });

                            if (savedDeliveryList.Count > 0)
                                searchDeliveryModelFound.Delivery = savedDeliveryList[0];
                        }

                        if (command_itemInProcess.Count > 0)
                            totalInvoiceAmount += command_itemInProcess[0].Command_Item.Price * command_itemInProcess[0].Command_Item.Quantity_delivery;
                    }
                }
            }

            if (first == 1)
            {
                // update of the invoice amount
                var billFound = (await Bl.BlCommand.searchBill(new Bill { ID = billSavedList[0].ID }, "AND")).FirstOrDefault();
                if (billFound != null)
                {
                    billFound.Pay = totalInvoiceAmount;
                    billFound.PayDate = DateTime.Now;
                    var savedBill = await Bl.BlCommand.UpdateBill(new List<Bill> { billFound });
                }
            }
            refreshBindings();
            Dialog.IsDialogOpen = false;
            //_currentViewModelFunc(this);
        }

        private bool canCreateBill(Command_itemModel arg)
        {
            bool isUpdate = securityCheck(EAction.Command, ESecurity._Update);
            if (isUpdate)
                return true;

            if (Item_deliveryModelBillingInProcess.Count > 0)
                return true;
            return false;
        }

        private async void cancelBillCreated(BillModel obj)
        {
            Dialog.showSearch("Bill cancelling...");
            List<Bill> billToDelteList = new List<Bill>();
            List<Delivery> deliveryToUpdateList = new List<Delivery>();

            // Getting the delivery ID for processing
            var command_itemFoundList = (from c in Command_ItemModelList
                                         from d in c.ItemModel.Item_deliveryModelList
                                         where d.DeliveryModel.TxtBillId == obj.TxtID
                                                 && d.DeliveryModel.TxtStatus == EStatusCommand.Billed.ToString()
                                         select c).ToList();
            //int first = 0;
            foreach (var command_item in command_itemFoundList)
            {
                foreach (var item_delivery in command_item.ItemModel.Item_deliveryModelList)
                {
                    item_delivery.DeliveryModel.Delivery.Status = EStatusCommand.Not_Billed.ToString();
                    item_delivery.DeliveryModel.Delivery.BillId = 0;
                    deliveryToUpdateList.Add(item_delivery.DeliveryModel.Delivery);
                }

            }
            await Bl.BlCommand.DeleteBill(new List<Bill> { obj.Bill });
            await Bl.BlCommand.UpdateDelivery(deliveryToUpdateList);
            CommandSelected.BillModelList.Remove(obj);
            refreshBindings();
            //_currentViewModelFunc(this);
            Dialog.IsDialogOpen = false;
        }

        private bool canCancelBillCreated(BillModel arg)
        {
            bool isUpdate = securityCheck(EAction.Command, ESecurity._Update);
            if (isUpdate)
                return true;

            // only the last Invoice can be deleted.
            Bill lastBill = new NotifyTaskCompletion<Bill>(Bl.BlCommand.GetLastBill()).Task.Result;
            if (lastBill.ID == arg.Bill.ID)
                return true;

            return false;
        }

        private async void updateBill(BillModel obj)
        {
            Dialog.showSearch("Bill updating...");
            var savedBillList = await Bl.BlCommand.UpdateBill(new List<Bill> { obj.Bill });
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateBill(BillModel arg)
        {
            return true;
        }


        private void generateCommandBillPdf(BillModel obj)
        {
            Dialog.showSearch("Bill pdf creation...");
            _paramCommandToPdf.BillId = obj.Bill.ID;
            _paramCommandToPdf.CommandId = CommandSelected.Command.ID;
            Bl.BlCommand.GeneratePdfCommand(_paramCommandToPdf);
            Dialog.IsDialogOpen = false;
        }

        private bool canGenerateCommandBillPdf(BillModel arg)
        {
            return true;
        }

        private void generateQuotePdf(object obj)
        {
            Dialog.showSearch("Quote pdf creation...");
            _paramQuoteToPdf.CommandId = CommandSelected.Command.ID;
            Bl.BlCommand.GeneratePdfQuote(_paramQuoteToPdf);
            Dialog.IsDialogOpen = false;
        }

        private bool canGenerateQuotePdf(object arg)
        {
            return true;
        }

        private async void billing(object obj)
        {
            Dialog.showSearch("Billing...");
            updateCommandStatus(EStatusCommand.Bill_Command);
            if (CommandSelected.TxtStatus.Equals(EStatusCommand.Bill_Command.ToString()))
            {
                await Dialog.show("Successfully Billed");
                _page(this);
            }
            Dialog.IsDialogOpen = false;
        }

        private bool canBilling(object arg)
        {
            bool isUpdate = securityCheck(EAction.Command_Billed, ESecurity._Update) && securityCheck(EAction.Command_Billed, ESecurity._Update);
            bool isWrite = securityCheck(EAction.Command_Billed, ESecurity._Write);
            if (isUpdate && isWrite)
                return true;

            return false;
        }

        private async void selectDeliveryAddress(Address obj)
        {
            if (obj != null)
            {
                Dialog.showSearch("Address updating...");
                CommandSelected.TxtDeliveryAddress = obj.ID.ToString();
                var savedCommandList = await Bl.BlCommand.UpdateCommand(new List<Entity.Command> { CommandSelected.Command });
                if (savedCommandList.Count > 0)
                {
                    var deliveryAddressFoundList = CommandSelected.AddressList.Where(x => x.ID == savedCommandList[0].DeliveryAddress).ToList();
                    CommandSelected.DeliveryAddress = (deliveryAddressFoundList.Count() > 0) ? deliveryAddressFoundList[0] : new Address();
                    await Dialog.show("Delivery Address Successfully Saved!");
                }
                Dialog.IsDialogOpen = false;
            }
        }

        private bool canSelectDeliveryAddress(Address arg)
        {
            bool isUpdate = securityCheck(EAction.Command, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        private async void saveNewTax(Tax obj)
        {
            if (obj != null)
            {
                Dialog.showSearch("Tax updating...");
                List<Tax_command> savedCommandList = new List<Tax_command>();
                CommandSelected.Tax = obj;
                CommandSelected.Tax_command.CommandId = CommandSelected.Command.ID;
                CommandSelected.Tax_command.TaxId = obj.ID;
                CommandSelected.Tax_command.Tax_value = obj.Value;
                CommandSelected.Tax_command.Target = EStatusCommand.Command.ToString();
                CommandSelected.Tax_command.Date_insert = DateTime.Now;

                if (CommandSelected.Tax_command.ID == 0)
                    savedCommandList = await Bl.BlCommand.InsertTax_command(new List<Tax_command> { CommandSelected.Tax_command });
                else
                    savedCommandList = await Bl.BlCommand.UpdateTax_command(new List<Tax_command> { CommandSelected.Tax_command });

                totalCalcul();
                Dialog.IsDialogOpen = false;
            }
        }

        private bool canSaveNewTax(Tax arg)
        {
            bool isUpdate = securityCheck(EAction.Command, ESecurity._Update);
            if (isUpdate)
                return true;

            return false;
        }

        private async void sendEmail(string obj)
        {
            Dialog.showSearch("Email sending...");
            var paramEmail = new ParamEmail();
            paramEmail.IsCopyToAgent = await Dialog.show("Do you want to receive an copy of the email?");
            paramEmail.Subject = EmailFile.TxtSubject;
            paramEmail.IsSendEmail = true;

            if (EmailFile.TxtFileNameWithoutExtension.Equals("quote"))
            {
                _paramQuoteToPdf.ParamEmail = paramEmail;
                _paramQuoteToPdf.CommandId = CommandSelected.Command.ID;
                Bl.BlCommand.GeneratePdfQuote(_paramQuoteToPdf);
            }
            else
            {
                _paramCommandToPdf.BillId = SelectedBillToSend.Bill.ID;
                _paramCommandToPdf.CommandId = CommandSelected.Command.ID;
                paramEmail.Reminder = 0;
                _paramCommandToPdf.ParamEmail = paramEmail;

                Bl.BlCommand.GeneratePdfCommand(_paramCommandToPdf);
            }

            Dialog.IsDialogOpen = false;

            //generateCommandBillPdf(SelectedBillToSend);
            /*if (EmailFile.save())
                InputDialog.show("Email sent successfully!");*/
        }

        private bool canSendEmail(string arg)
        {
            bool isSendEmailValidCommand = securityCheck(EAction.Command, ESecurity.SendEmail);
            bool isSendEmailValidPrecommand = securityCheck(EAction.Command_Precommand, ESecurity.SendEmail);
            bool isSendEmailQuote = securityCheck(EAction.Quote, ESecurity.SendEmail);

            if (CommandSelected == null)
                return false;

            if (isSendEmailValidPrecommand && CommandSelected.TxtStatus.Equals(EStatusCommand.Pre_Command.ToString()))
                return true;

            if (isSendEmailQuote && CommandSelected.TxtStatus.Equals(EStatusCommand.Quote.ToString()))
                return true;

            if (isSendEmailValidCommand
                && (CommandSelected.TxtStatus.Equals(EStatusCommand.Command.ToString())
                || CommandSelected.TxtStatus.Equals(EStatusCommand.Credit.ToString())))
                return true;

            return false;
        }

        private async void updateComment(object obj)
        {
            Dialog.showSearch("Comment updaing...");
            var savedCommandList = await Bl.BlCommand.UpdateCommand(new List<QCBDManagementCommon.Entities.Command> { CommandSelected.Command });
            if (savedCommandList.Count > 0)
                await Dialog.show("Comment updated successfully!");
            Dialog.IsDialogOpen = false;
        }

        private bool canUpdateComment(object arg)
        {
            return true;
        }
    }

}
