using System;
using System.Collections.Generic;
using System.ComponentModel;
using Entity = QCBDManagementCommon.Entities;
using QCBDManagementCommon.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementBusiness;
using QCBDManagementWPF.Models;
using QCBDManagementWPF.Classes;
using QCBDManagementCommon.Enum;
using System.Collections.Concurrent;
using QCBDManagementCommon.Classes;
using System.IO;
using System.Xml.Serialization;
using QCBDManagementWPF.Interfaces;
using System.Windows.Threading;

namespace QCBDManagementWPF.ViewModel
{
    public class CommandViewModel : BindBase
    {
        private string _navigTo;
        private Func<Object, Object> _currentSideBarViewModelFunc;
        private Func<Object, Object> _currentViewModelFunc;
        public NotifyTaskCompletion<List<Entity.Command>> CommandTask { get; set; }
        public NotifyTaskCompletion<List<CommandModel>> CommandModelTask { get; set; }
        public NotifyTaskCompletion<List<Entity.Tax>> TaxTask { get; set; }
        private string _title;
        private CommandSearch _commandSearch;
        private string _blockCommandVisibility;
        private string _blockSearchResultVisibility;

        //----------------------------[ POCOs ]------------------

        private Entity.Command _command;
        private List<Entity.Tax> _taxesList;

        //----------------------------[ Models ]------------------

        private CommandDetailViewModel _commandDetailViewModel;
        private List<CommandModel> _commandModelList;
        private List<CommandModel> _waitValidCommands;
        private List<CommandModel> _waitValidClientCommands;
        private List<CommandModel> _inProcessCommands;
        private List<CommandModel> _waitPayCommands;
        private List<CommandModel> _closedCommands;
        private ClientModel _selectedClient;
        private Func<string, object> _getObjectFromMainWindowViewModel;

        //----------------------------[ Commands ]------------------

        public CommandSideBarViewModel CommandSideBarViewModel { get; set; }
        public Command.ButtonCommand<string> NavigCommand { get; set; }
        public Command.ButtonCommand<CommandModel> GetCurrentCommandCommand { get; set; }
        public Command.ButtonCommand<CommandModel> DeleteCommand { get; set; }
        public Command.ButtonCommand<object> SearchCommand { get; set; }


        public CommandViewModel()
        {
            instances();
            instancesPoco();
            instancesModel();
            instancesCommand();
            initEvents();
        }

        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onSelectedCommandChange;
            PropertyChanged += onNavigToChange;
            PropertyChanged += onBlockSearchResultVisibilityChange;
            PropertyChanged += onStartupChange;
            PropertyChanged += onDialogChange;
            TaxTask.PropertyChanged += onTaxTaskCompletion_getTax;
        }

        private void instances()
        {
            CommandTask = new NotifyTaskCompletion<List<QCBDManagementCommon.Entities.Command>>();
            CommandModelTask = new NotifyTaskCompletion<List<CommandModel>>();
            TaxTask = new NotifyTaskCompletion<List<QCBDManagementCommon.Entities.Tax>>();
            _title = "";
            _commandSearch = new CommandSearch();
            _blockCommandVisibility = "Visible";
            _blockSearchResultVisibility = "Hidden";
        }

        private void instancesPoco()
        {
            _taxesList = new List<QCBDManagementCommon.Entities.Tax>();
            _command = new Entity.Command();
        }

        private void instancesModel()
        {
            _waitValidCommands = new List<CommandModel>();
            _waitValidClientCommands = new List<CommandModel>();
            _commandModelList = new List<CommandModel>();
            _inProcessCommands = new List<CommandModel>();
            _waitPayCommands = new List<CommandModel>();
            _closedCommands = new List<CommandModel>();
            _commandDetailViewModel = new CommandDetailViewModel();
            CommandSideBarViewModel = new CommandSideBarViewModel();
            _selectedClient = new ClientModel();
        }

        private void instancesCommand()
        {
            NavigCommand = new Command.ButtonCommand<string>(executeNavig, canExecuteNavig);
            GetCurrentCommandCommand = new Command.ButtonCommand<CommandModel>(saveSelectedCommand, canSaveSelectedCommand);
            DeleteCommand = new Command.ButtonCommand<CommandModel>(deleteCommand, canDeleteCommand);
            SearchCommand = new Command.ButtonCommand<object>(searchCommand, canSearchCommand);
        }

        //----------------------------[ Properties ]------------------

        public CommandSearch CommandSearch
        {
            get { return _commandSearch; }
            set { setProperty(ref _commandSearch, value, "CommandSearch"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public ClientModel SelectedClient
        {
            get { return (_selectedClient != null) ? _selectedClient : new ClientModel(); }
            set
            {
                QCBDManagementWPF.Helper.WPFHelper.onUIThread(() =>
                {
                    setProperty(ref _selectedClient, value, "SelectedClient");
                });
            }

        }

        /*public bool IsCurrentPage
        {
            get { return _isCurrentPage; }
            set { _isCurrentPage = false; setProperty(ref _isCurrentPage, value, "IsCurrentPage"); }
        }*/

        public List<Entity.Tax> TaxList
        {
            get { return _taxesList; }
            set { setProperty(ref _taxesList, value, "TaxList"); }
        }

        public CommandDetailViewModel CommandDetailViewModel
        {
            get { return _commandDetailViewModel; }
            set { setProperty(ref _commandDetailViewModel, value, "CommandDetailViewModel"); }
        }

        public CommandModel SelectedCommandModel
        {
            get { return CommandDetailViewModel.CommandSelected; }
            set { CommandDetailViewModel.CommandSelected = value; CommandSideBarViewModel.SelectedCommandModel = value; onPropertyChange("SelectedCommandModel"); }
        }

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public string NavigTo
        {
            get { return _navigTo; }
            set { setProperty(ref _navigTo, value, "NavigTo"); }
        }

        public List<CommandModel> WaitValidClientCommandList
        {
            get { return getCommandModelListFilterBy("WaitValidClientCommandList"); }
        }

        public List<CommandModel> CommandModelList
        {
            get { return _commandModelList; }
            set { _commandModelList = value; onPropertyChange("CommandModelList"); updateCommandModelListBinding(); }
        }

        public List<CommandModel> InProcessCommandList
        {
            get { return getCommandModelListFilterBy("InProcessCommandList"); }
        }

        public List<CommandModel> WaitValidCommandList
        {
            get { return getCommandModelListFilterBy("WaitValidCommandList"); }
        }

        public List<CommandModel> ClosedCommandList
        {
            get { return getCommandModelListFilterBy("ClosedCommandList"); }
        }

        public List<CommandModel> WaitPayCommandList
        {
            get { return getCommandModelListFilterBy("WaitPayCommandList"); }
        }

        public Func<string, object> GetObjectFromMainWindowViewModel
        {
            get { return _getObjectFromMainWindowViewModel; }
            set { setProperty(ref _getObjectFromMainWindowViewModel, value, "GetObjectFromMainWindowViewModel"); }
        }

        public string BlockSearchResultVisibility
        {
            get { return _blockSearchResultVisibility; }
            set { setProperty(ref _blockSearchResultVisibility, value, "BlockSearchResultVisibility"); }
        }

        public string BlockCommandVisibility
        {
            get { return _blockCommandVisibility; }
            set { setProperty(ref _blockCommandVisibility, value, "BlockCommandVisibility"); }
        }

        //----------------------------[ Actions ]------------------

        /// <summary>
        /// Convert generic list of command into a command model
        /// </summary>
        /// <param name="CommandList"></param>
        /// <returns></returns>
        private Task<List<CommandModel>> CommandListToModelList(List<Entity.Command> CommandList)
        {
            return Task.Factory.StartNew(async () =>
            {
                List<CommandModel> output = new List<CommandModel>();
                ConcurrentBag<CommandModel> concurrentCommandModelList = new ConcurrentBag<CommandModel>();
                foreach (var command in CommandList)
                //Parallel.ForEach(CommandList, async (command) =>
                {
                    CommandModel cmdvm = new CommandModel();

                    var resultAgent = await Bl.BlAgent.GetAgentDataById(command.AgentId);
                    cmdvm.AgentModel.Agent = (resultAgent.Count > 0) ? resultAgent[0] : new Entity.Agent();

                    var resultClient = await Bl.BlClient.GetClientDataById(command.ClientId);
                    cmdvm.CLientModel.Client = (resultClient.Count > 0) ? resultClient[0] : new Entity.Client();

                    var tax_command = new Entity.Tax_command();
                    tax_command.CommandId = command.ID;
                    var resultSearchCommandTaxList = await Bl.BlCommand.searchTax_command(tax_command, "AND");
                    cmdvm.Tax_command = (resultSearchCommandTaxList.Count > 0) ? resultSearchCommandTaxList[0] : new Entity.Tax_command();

                    Entity.Tax taxFound = TaxList.Where(x => x.ID == cmdvm.Tax_command.TaxId).OrderBy(x => x.Date_insert).LastOrDefault();// await Bl.BlCommand.GetTaxDataById(cmdvm.Tax_command.TaxId);
                    cmdvm.Tax = (taxFound != null) ? taxFound : new Entity.Tax();

                    cmdvm.Command = command;
                    concurrentCommandModelList.Add(cmdvm);
                }
                //);
                output = new List<CommandModel>(concurrentCommandModelList);
                return output;
            }).Result;

            //return task.Result;
        }

        /// <summary>
        /// Load all commands in defferent sections according to their status
        /// </summary>
        public async void loadCommands()
        {
            //var main = GetObjectFromMainWindowViewModel("main") as MainWindowViewModel;
            //if (main != null)
            //{
            //    await main.MainWindow.onUIThreadAsync(() =>
            //    {
            var main = GetObjectFromMainWindowViewModel("main") as MainWindowViewModel;
            if (main != null)
            {
                await main.MainWindow.onUIThreadAsync(async() =>
                {
                    Dialog.showSearch("Loading...");
                    TaxList = await Bl.BlCommand.GetTaxData(999);
                    CommandSearch.AgentList = await Bl.BlAgent.GetAgentData(-999);

                    if (SelectedClient.Client.ID != 0)
                    {
                        Title = string.Format("Orders for the Company {0}", SelectedClient.Client.Company);
                        
                        //CommandModelTask.initializeNewTask(CommandListToModelList(CommandTask.Result));
                        CommandModelList = (await CommandListToModelList(await Bl.BlCommand.searchCommandFromWebService(new Entity.Command { ClientId = SelectedClient.Client.ID }, "AND"))).OrderByDescending(x => x.Command.ID).ToList();
                        SelectedClient = new ClientModel();
                    }
                    else
                    {
                        Title = "Orders Management";
                        CommandModelList = (await CommandListToModelList(await Bl.BlCommand.searchCommand(new QCBDManagementCommon.Entities.Command { AgentId = Bl.BlSecurity.GetAuthenticatedUser().ID }, "AND"))).OrderByDescending(x => x.Command.ID).ToList();
                        //CommandTask.initializeNewTask(Bl.BlCommand.searchCommand(new QCBDManagementCommon.Entities.Command { AgentId = Bl.BlSecurity.GetAuthenticatedUser().ID }, "AND"));
                    }
                    BlockSearchResultVisibility = "Hidden";
                    Dialog.IsDialogOpen = false;
                });
            }
            
            //});
            //}

        }

        /// <summary>
        /// get the sideBar management Function
        /// </summary>
        /// <param name="sideBarManagement"></param>
        internal void sideBarContentManagement(Func<object, object> sideBarManagement)
        {
            _currentSideBarViewModelFunc = sideBarManagement;
        }

        public void setSideBar(ref CommandSideBarViewModel sideBar)
        {
            CommandSideBarViewModel = sideBar;
        }

        internal void setObjectAccessorFromMainWindowViewModel(Func<string, object> getObject)
        {
            GetObjectFromMainWindowViewModel = getObject;
            CommandDetailViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
            CommandSideBarViewModel.setObjectAccessorFromMainWindowViewModel(getObject);
        }

        private void updateCommandModelListBinding()
        {
            onPropertyChange("WaitValidClientCommandList");
            onPropertyChange("InProcessCommandList");
            onPropertyChange("WaitValidCommandList");
            onPropertyChange("ClosedCommandList");
            onPropertyChange("WaitPayCommandList");
        }

        private List<CommandModel> getCommandModelListFilterBy(string filterName)
        {
            object _lock = new object();
            ConcurrentBag<CommandModel> result = new ConcurrentBag<CommandModel>();
            lock (_lock)
                if (CommandModelList != null && CommandModelList.Count > 0)
                {
                    switch (filterName)
                    {
                        case "WaitValidClientCommandList":
                            result = new ConcurrentBag<CommandModel>(CommandModelList.Where(x => x.TxtStatus.Equals(EStatusCommand.Pre_Client_Validation.ToString())).ToList());
                            break;
                        case "InProcessCommandList":
                            result = new ConcurrentBag<CommandModel>(CommandModelList.Where(x => x.TxtStatus.Equals(EStatusCommand.Command.ToString()) || x.TxtStatus.Equals(EStatusCommand.Credit.ToString())).ToList());
                            break;
                        case "WaitValidCommandList":
                            result = new ConcurrentBag<CommandModel>(CommandModelList.Where(x => x.TxtStatus.Equals(EStatusCommand.Pre_Command.ToString()) || x.TxtStatus.Equals(EStatusCommand.Pre_Credit.ToString())).ToList());
                            break;
                        case "ClosedCommandList":
                            result = new ConcurrentBag<CommandModel>(CommandModelList.Where(x => x.TxtStatus.Equals(EStatusCommand.Command_Close.ToString()) || x.TxtStatus.Equals(EStatusCommand.Credit_CLose.ToString())).ToList());
                            break;
                        case "WaitPayCommandList":
                            result = new ConcurrentBag<CommandModel>(CommandModelList.Where(x => x.TxtStatus.Equals(EStatusCommand.Bill_Command.ToString()) || x.TxtStatus.Equals(EStatusCommand.Bill_Credit.ToString())).ToList());
                            break;
                    }
                }

            return result.ToList();
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

        public override void Dispose()
        {
            Bl.BlCommand.Dispose();
            PropertyChanged -= onSelectedCommandChange;
            PropertyChanged -= onNavigToChange;
            PropertyChanged -= onBlockSearchResultVisibilityChange;
            PropertyChanged -= onStartupChange;
            PropertyChanged -= onDialogChange;
            TaxTask.PropertyChanged -= onTaxTaskCompletion_getTax;
            CommandDetailViewModel.Dispose();
            CommandSideBarViewModel.Dispose();
        }

        //----------------------------[ Event Handler ]------------------

        private void onStartupChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Startup"))
            {
                CommandDetailViewModel.Startup = Startup;
            }
        }

        private void onDialogChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Dialog"))
            {
                CommandDetailViewModel.Dialog = Dialog;
                CommandSideBarViewModel.Dialog = Dialog;
            }
        }

        /// <summary>
        /// Navigate to the next page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onNavigToChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "NavigTo"))
            {
                executeNavig(NavigTo);
            }
        }

        /// <summary>
        /// Set the default page to navigate to when the selected command is saved
        /// in this case when the button detail is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onSelectedCommandChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "SelectedCommandModel"))
            {
                NavigTo = "command-detail";
            }
        }

        private void onTaxTaskCompletion_getTax(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSuccessfullyCompleted"))
            {
                TaxList = TaxTask.Result;
            }
        }

        private void onBlockSearchResultVisibilityChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("BlockSearchResultVisibility"))
            {
                if (BlockSearchResultVisibility.Equals("Visible"))
                    BlockCommandVisibility = "Hidden";
                else
                    BlockCommandVisibility = "Visible";
            }
        }


        //----------------------------[ Action Commands ]------------------

        /// <summary>
        /// Save the selected command
        /// </summary>
        /// <param name="obj"></param>
        private void saveSelectedCommand(CommandModel obj)
        {
            SelectedCommandModel = obj;
        }

        private bool canSaveSelectedCommand(CommandModel arg)
        {
            return true;
        }

        /// <summary>
        /// get the main navigation management Function
        /// </summary>
        /// <param name="navigObject"></param>
        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _currentViewModelFunc = navigObject;
            CommandDetailViewModel.mainNavigObject(navigObject);
            //CommandSideBarViewModel.mainNavigObject(navigObject);
        }

        /// <summary>
        /// Navigate through the application
        /// </summary>
        /// <param name="obj"></param>
        public void executeNavig(string obj)
        {
            _currentSideBarViewModelFunc(CommandSideBarViewModel);
            switch (obj)
            {
                case "command":
                    //contextManagement(this);
                    _currentViewModelFunc(this);
                    break;
                case "command-detail":
                    //contextManagement(CommandDetailViewModel);
                    _currentViewModelFunc(CommandDetailViewModel);
                    break;
            }
        }

        private bool canExecuteNavig(string arg)
        {
            return true;
        }

        public async void deleteCommand(CommandModel obj)
        {
            Bill lastBill = new Bill();
            lastBill = await Bl.BlCommand.GetLastBill();
            List<Bill> billFoundList = await Bl.BlCommand.GetBillDataByCommandList(new List<Entity.Command> { obj.Command });
            if (billFoundList.Count > 0 && billFoundList[0].ID == lastBill.ID)
            {
                Dialog.showSearch("Deleting...");
                //var billFoundList = await Bl.BlCommand.GetBillDataByCommandList(new List<Entity.Command> { obj.Command });
                var command_itemFoundList = await Bl.BlCommand.GetCommand_itemByCommandList(new List<Entity.Command> { obj.Command });
                var deliveryFoundList = await Bl.BlCommand.GetDeliveryDataByCommandList(new List<Entity.Command> { obj.Command });
                var Item_deliveryFoundList = await Bl.BlItem.GetItem_deliveryDataByDeliveryList(deliveryFoundList);
                var tax_commandFoundList = await Bl.BlCommand.GetTax_commandDataByCommandList(new List<Entity.Command> { obj.Command });

                // deleting
                await Bl.BlCommand.DeleteTax_command(tax_commandFoundList);
                await Bl.BlItem.DeleteItem_delivery(Item_deliveryFoundList);
                await Bl.BlCommand.DeleteDelivery(deliveryFoundList);
                await Bl.BlCommand.DeleteBill(billFoundList);
                await Bl.BlCommand.DeleteCommand_item(command_itemFoundList);
                await Bl.BlCommand.DeleteCommand(new List<Entity.Command> { obj.Command });

                CommandModelList.Remove(obj);
                updateCommandModelListBinding();
                Dialog.IsDialogOpen = false;
            }
            else
                await Dialog.show("Command bill is not the last one.");


        }

        public bool canDeleteCommand(CommandModel arg)
        {
            bool isAdmin = securityCheck(EAction.Security, ESecurity.SendEmail)
                             && securityCheck(EAction.Security, ESecurity._Delete)
                                && securityCheck(EAction.Security, ESecurity._Read)
                                    && securityCheck(EAction.Security, ESecurity._Update)
                                        && securityCheck(EAction.Security, ESecurity._Write);
            if (!isAdmin)
                return true;
            return false;
        }

        private async void searchCommand(object obj)
        {
            Dialog.showSearch("Searching...");
            List<Entity.Command> billCommandFoundList = new List<Entity.Command>();
            List<Entity.Command> CLientCommandFoundList = new List<Entity.Command>();
            List<Entity.Command> commandFoundTotal = new List<Entity.Command>();
            List<Entity.Command> commandFoundFilterByDate = new List<Entity.Command>();
            List<Entity.Command> commandFoundList = new List<Entity.Command>();

            var billFoundList = (CommandSearch.IsDeepSearch) ? await Bl.BlCommand.searchBillFromWebService(new Entity.Bill { ID = CommandSearch.BillId }, "AND") : await Bl.BlCommand.GetBillDataById(CommandSearch.BillId);
            if (billFoundList.Count > 0)
                billCommandFoundList = (CommandSearch.IsDeepSearch) ? await Bl.BlCommand.searchCommandFromWebService(new Entity.Command { ID = billFoundList[0].CommandId }, "AND") : await Bl.BlCommand.searchCommand(new Entity.Command { ID = billFoundList[0].CommandId }, "OR");

            var clientFoundList = (CommandSearch.IsDeepSearch) ? await Bl.BlClient.searchClientFromWebService(new Entity.Client { ID = CommandSearch.ClientId, Company = CommandSearch.CompanyName, CompanyName = CommandSearch.CompanyName }, "OR") : await Bl.BlClient.searchClient(new Entity.Client { ID = CommandSearch.ClientId, Company = CommandSearch.CompanyName, CompanyName = CommandSearch.CompanyName }, "OR");
            foreach (var client in clientFoundList)
            {
                var clientCommandFound = (CommandSearch.IsDeepSearch) ? await Bl.BlCommand.searchCommandFromWebService(new Entity.Command { ClientId = client.ID }, "AND") : await Bl.BlCommand.searchCommand(new Entity.Command { ClientId = client.ID }, "OR");
                CLientCommandFoundList = new List<Entity.Command>(CLientCommandFoundList.Concat(clientCommandFound));
            }

            if (CommandSearch.SelectedStatus != null && CommandSearch.SelectedAgent != null)
                commandFoundList = (CommandSearch.IsDeepSearch) ? await Bl.BlCommand.searchCommandFromWebService(new Entity.Command { Status = CommandSearch.SelectedStatus, AgentId = CommandSearch.SelectedAgent.ID }, "OR") : await Bl.BlCommand.searchCommand(new Entity.Command { Status = CommandSearch.SelectedStatus, AgentId = CommandSearch.SelectedAgent.ID }, "OR");
            else if (CommandSearch.SelectedStatus != null)
                commandFoundList = (CommandSearch.IsDeepSearch) ? await Bl.BlCommand.searchCommandFromWebService(new Entity.Command { Status = CommandSearch.SelectedStatus }, "OR") : await Bl.BlCommand.searchCommand(new Entity.Command { Status = CommandSearch.SelectedStatus }, "OR");
            else if (CommandSearch.SelectedAgent != null)
                commandFoundList = (CommandSearch.IsDeepSearch) ? await Bl.BlCommand.searchCommandFromWebService(new Entity.Command { AgentId = CommandSearch.SelectedAgent.ID }, "OR") : await Bl.BlCommand.searchCommand(new Entity.Command { AgentId = CommandSearch.SelectedAgent.ID }, "OR");

            commandFoundTotal = commandFoundList;
            commandFoundTotal = new List<Entity.Command>(commandFoundTotal.Concat(billCommandFoundList));
            commandFoundTotal = new List<Entity.Command>(commandFoundTotal.Concat(CLientCommandFoundList));

            commandFoundFilterByDate = commandFoundTotal;

            if (Utility.convertToDateTime(CommandSearch.StartDate) != Utility.DateTimeMinValueInSQL2005)
                commandFoundFilterByDate = commandFoundFilterByDate.Where(x => x.Date >= Utility.convertToDateTime(CommandSearch.StartDate)).ToList();
            if (Utility.convertToDateTime(CommandSearch.EndDate) != Utility.DateTimeMinValueInSQL2005)
                commandFoundFilterByDate = commandFoundFilterByDate.Where(x => x.Date <= Utility.convertToDateTime(CommandSearch.EndDate)).ToList();

            CommandModelList = await CommandListToModelList(commandFoundFilterByDate);

            BlockSearchResultVisibility = "Visible";

            Dialog.IsDialogOpen = false;
        }

        private bool canSearchCommand(object arg)
        {
            bool isAdmin = securityCheck(EAction.Security, ESecurity.SendEmail)
                             && securityCheck(EAction.Security, ESecurity._Delete)
                                && securityCheck(EAction.Security, ESecurity._Read)
                                    && securityCheck(EAction.Security, ESecurity._Update)
                                        && securityCheck(EAction.Security, ESecurity._Write);
            if (isAdmin)
                return true;
            return false;
        }
    }
}
