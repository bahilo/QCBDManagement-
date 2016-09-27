using QCBDManagementWPF.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementBusiness;
using Entity = QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Models;
using QCBDManagementCommon.Enum;
using System.ComponentModel;
using QCBDManagementWPF.Interfaces;

namespace QCBDManagementWPF.ViewModel
{
    public class QuoteViewModel: BindBase
    {
        //private Cart _cart;
        private string _navigTo;
        private bool _isCurrentPage;
        private Func<Object, Object> _currentSideBarViewModelFunc;
        private Func<Object, Object> _currentViewModelFunc;
        private string _missingCLientMessage;
        private Func<string, object> _getObjectFromMainWindowViewModel;
        private Cart _cart;
        private string _title;
        private string _defaultClientMissingMessage;

        //----------------------------[ Models ]------------------

        private CommandModel _commandModel;
        private CommandViewModel _commandViewModel;
        private CommandDetailViewModel _quoteDetailViewModel;
        private List<CommandModel> _quoteModelList;
        private CommandSideBarViewModel _quoteSideBarViewModel;
        private ClientModel _selectedClient;
        private ItemModel _itemModel;


        //----------------------------[ Commands ]------------------

        public Command.ButtonCommand<string> NavigCommand { get; set; }
        public Command.ButtonCommand<CommandModel> GetCurrentCommandCommand { get; set; }
        public Command.ButtonCommand<string> ValidCartToQuoteCommand { get; set; }
        public Command.ButtonCommand<CommandModel> DeleteCommand { get; set; }


        public QuoteViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }


        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            _quoteDetailViewModel.PropertyChanged += onSelectedQuoteModelChange;
            PropertyChanged += onGetObjectFromMainWindowViewModelChange;
            PropertyChanged += onStartupChange;
            PropertyChanged += onDialogChange;
        }

        private void instances()
        {
            _defaultClientMissingMessage = @"/!\ No Client Selected";
        }

        private void instancesModel()
        {
            _commandModel = new CommandModel();
            _commandViewModel = new CommandViewModel();
            _quoteSideBarViewModel = new CommandSideBarViewModel();
            _quoteDetailViewModel = new CommandDetailViewModel();
            _selectedClient = new ClientModel();
        }

        private void instancesCommand()
        {
            NavigCommand = new Command.ButtonCommand<string>(executeNavig, canExecuteNavig);
            GetCurrentCommandCommand = new Command.ButtonCommand<CommandModel>(saveSelectedQuote, canSaveSelectedCommand);
            ValidCartToQuoteCommand = new ButtonCommand<string>(createQuote, canCreateQuote);
            DeleteCommand = new Command.ButtonCommand<CommandModel>(deleteCommand, canDeleteCommand);
        }

        //----------------------------[ Properties ]------------------
               
        public ItemModel ItemModel
        {
            get { return _itemModel; }
            set { setProperty(ref _itemModel, value, "ItemModel"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public bool IsCurrentPage
        {
            get { return _isCurrentPage; }
            set { _isCurrentPage = false; setProperty(ref _isCurrentPage, value, "IsCurrentPage"); }
        }

        public CommandSideBarViewModel QuoteSideBarViewModel
        {
            get { return _quoteSideBarViewModel; }
            set { setProperty(ref _quoteSideBarViewModel, value, "QuoteSideBarViewModel"); }
        }
        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }        

        public string MissingCLientMessage
        {
            get { return _missingCLientMessage; }
            set { setProperty(ref _missingCLientMessage, value, "MissingCLientMessage"); }
        }

        public string NavigTo
        {
            get { return _navigTo; }
            set { setProperty(ref _navigTo, value,"NavigTo"); }
        }

        public CommandModel SelectedQuoteModel
        {
            get { return QuoteDetailViewModel.CommandSelected; }
            set { QuoteDetailViewModel.CommandSelected = value; onPropertyChange("SelectedQuoteModel"); }
        }

        public CommandDetailViewModel QuoteDetailViewModel
        {
            get { return _quoteDetailViewModel; }
            set { _quoteDetailViewModel = value; onPropertyChange("QuoteDetailViewModel"); }
        }

        public List<CommandModel> QuoteModelList
        {
            get { return _quoteModelList; }
            set { setProperty(ref _quoteModelList, value, "QuoteModelList"); }
        }

        public ClientModel SelectedClient
        {
            get { return _selectedClient; }
            set { setProperty(ref _selectedClient, value, "SelectedClient"); }
        }

        public Cart Cart
        {
            get { return _cart; }
            set { setProperty(ref _cart, value, "Cart"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }


        //----------------------------[ Actions ]------------------

        public void loadCommands()
        {
            _commandViewModel = _getObjectFromMainWindowViewModel("command") as CommandViewModel;
            
            //-------[ check cart client ]
            if (Cart.Client.Client.ID == 0)
                MissingCLientMessage = _defaultClientMissingMessage;
            else
                MissingCLientMessage = "";

            //-------[ filter on client's quotes ]
            if (SelectedClient.Client.ID != 0)
            {
                _commandViewModel.SelectedClient = SelectedClient;
                SelectedClient = new ClientModel();
            }

            //-------[ retrieve quotes ]
            if (_commandViewModel != null)
            {
                _commandViewModel.PropertyChanged += onCommandModelChange_loadCommand;
                _commandViewModel.loadCommands();
                Title = _commandViewModel.Title.Replace("Command","Quotation");
            }

                
            //SelectedQuoteModel = new CommandModel();
            //var result = await Bl.BlCommand.searchCommand(new Entity.Command { Status=EStatusCommand.Quote.ToString() }, "AND");
            //QuoteModelList = await QuoteListToModelViewList(await Bl.BlCommand.searchCommand(new Entity.Command { Status = EStatusCommand.Quote.ToString() }, "AND"));
        }

        private void onCommandModelChange_loadCommand(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CommandModelList"))
            {
                QuoteModelList = _commandViewModel.CommandModelList.Where(x => x.TxtStatus.Equals(EStatusCommand.Quote.ToString())).ToList();
            }
        }

        private async Task<List<CommandModel>> QuoteListToModelViewList(List<Entity.Command> CommandList)
        {
            List<CommandModel> output = new List<CommandModel>();
            foreach (Entity.Command command in CommandList)
            {
                CommandModel cmdvm = new CommandModel();

                var resultAgent = await Bl.BlAgent.GetAgentDataById(command.AgentId);
                cmdvm.AgentModel.Agent = (resultAgent.Count > 0) ? resultAgent[0] : new Entity.Agent();
                var resultClient = await Bl.BlClient.GetClientDataById(command.ClientId);
                cmdvm.CLientModel.Client = (resultClient.Count > 0) ? resultClient[0] : new Entity.Client();

                cmdvm.Command = command;

                output.Add(cmdvm);
            }
            return output;
        }

        /*private void contextManagement(IState nextPage)
        {
            var mainViewModel = _getObjectFromMainWindowViewModel("main") as MainWindowViewModel;
            if (mainViewModel != null)
            {
                mainViewModel.Context.PreviousState = this;
                mainViewModel.Context.NextState = nextPage;
            }
        }*/

        internal void mainNavigObject(Func<object, object> navigation)
        {
            _currentViewModelFunc = navigation;
            //QuoteSideBarViewModel.mainNavigObject(navigation);
        }

        internal void sideBarContentManagement(Func<object, object> sideBarManagement)
        {
            _currentSideBarViewModelFunc = sideBarManagement;
        }

        /*public void initCart(ref Cart cart)
        {
            if (cart.Client == null)
                MissingCLientMessage = "No Client Selected!";

            _itemViewModel.initCart(ref cart);
        }*/

        public void setSideBar(ref CommandSideBarViewModel sideBar)
        {
            QuoteSideBarViewModel = sideBar;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            GetObjectFromMainWindowViewModel = getObject;
        }

        internal void setInitCart(ref Cart cart)
        {
            Cart = cart;
        }

        //----------------------------[ Event Handler ]------------------

        private void onNavigToChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "NavigTo"))
            {
                executeNavig(NavigTo);
            }
        }

        private void onSelectedQuoteModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedQuoteModel"))
            {
                NavigTo = "quote-detail";
            }
        }

        private void onGetObjectFromMainWindowViewModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "GetObjectFromMainWindowViewModel"))
            {
                var itemViewModelFound = GetObjectFromMainWindowViewModel("item") as ItemViewModel;
                if (itemViewModelFound != null)
                {
                    ItemModel = itemViewModelFound.ItemModel;
                }                    
            }
        }

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                QuoteDetailViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                QuoteDetailViewModel.Startup = Startup;
            }
        }

        //----------------------------[ Action Commands ]------------------

        private void saveSelectedQuote(CommandModel obj)
        {
            SelectedQuoteModel = obj;
            //QuoteDetailViewModel.CommandSelected = obj;
        }

        private bool canSaveSelectedCommand(CommandModel arg)
        {
            return true;
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }

        public void executeNavig(string obj)
        {
            _currentSideBarViewModelFunc(QuoteSideBarViewModel);
            switch (obj)
            {
                case "quote":
                    //contextManagement(this);
                    _currentViewModelFunc(this);
                    break;
                case "quote-detail":
                    //contextManagement(QuoteDetailViewModel);
                    _currentViewModelFunc(QuoteDetailViewModel);
                    break;
                /*case "quote-new":
                    SelectedQuoteModel = new CommandModel();
                    contextManagement(QuoteDetailViewModel);
                    _currentViewModelFunc(QuoteDetailViewModel);
                    break;
                case "quote-update":
                    contextManagement(QuoteDetailViewModel);
                    _currentViewModelFunc(QuoteDetailViewModel);
                    break;*/
                default:
                    goto case "quote";
            }
        }

        private async void createQuote(string obj)
        {
            Dialog.showSearch("Quote creation...");
            CommandModel quote = new CommandModel();
            List<Entity.Command_item> command_itemList = new List<Entity.Command_item>();
            List<Entity.Command> quoteList = new List<Entity.Command>();

            quote.AddressList = Cart.Client.AddressList;
            quote.CLientModel = Cart.Client;
            quote.AgentModel = new AgentModel { Agent = Bl.BlSecurity.GetAuthenticatedUser() };
            quote.TxtDate = DateTime.Now.ToString();
            quote.TxtStatus = EStatusCommand.Quote.ToString();

            quoteList.Add(quote.Command);

            var savedQuoteList = await Bl.BlCommand.InsertCommand(quoteList);
            var savedQuote = (savedQuoteList.Count > 0) ? savedQuoteList[0] : new Entity.Command();

            foreach (var itemModel in Cart.CartItemList)
            {
                var command_item                    = new Command_itemModel();
                command_item.ItemModel.Item         = itemModel.Item;
                command_item.TxtItem_ref           = itemModel.TxtRef;
                command_item.TxtItemId              = itemModel.TxtID;
                command_item.TxtPrice               = itemModel.TxtPrice_sell;
                command_item.TxtPrice_purchase      = itemModel.TxtPrice_purchase;
                command_item.TxtTotalPurchase       = itemModel.TxtTotalPurchasePrice;
                command_item.TxtTotalSelling        = itemModel.TxtTotalSellingPrice;
                command_item.TxtCommandId           = savedQuote.ID.ToString();
                command_item.TxtQuantity            = itemModel.TxtQuantity;
                //command_item.TxtQuantity_current    = itemModel.TxtQuantity;

                command_itemList.Add(command_item.Command_Item);
            }    
            var savedCommandList = await Bl.BlCommand.InsertCommand_item(command_itemList);
            Cart.CartItemList.Clear();
            Cart.Client.Client = new QCBDManagementCommon.Entities.Client();
            if (savedQuoteList.Count > 0)
                await Dialog.show("Quote ID("+savedQuoteList[0].ID+") has been created successfully!");
            //loadCommands();
            Dialog.IsDialogOpen = false;
            _currentViewModelFunc(new QuoteViewModel());
        }

        private bool canCreateQuote(string arg)
        {
            if (Cart.Client != null 
                && Cart.Client.Client.ID != 0
                && Cart.CartItemList.Count() > 0)
            {
                MissingCLientMessage = "";
                return true;                
            }
            else
                MissingCLientMessage = _defaultClientMissingMessage;

            return false;
        }

        private async void deleteCommand(CommandModel obj)
        {
            var command_itemFoundList = await Bl.BlCommand.GetCommand_itemByCommandList(new List<Entity.Command> { obj.Command });
            await Bl.BlCommand.DeleteCommand_item(command_itemFoundList);
            await Bl.BlCommand.DeleteCommand(new List<Entity.Command> { obj.Command });
            //_commandViewModel.deleteCommand(obj);
            _currentViewModelFunc(this);
        }

        private bool canDeleteCommand(CommandModel arg)
        {
            return _commandViewModel.canDeleteCommand(arg);
        }


    }
}
