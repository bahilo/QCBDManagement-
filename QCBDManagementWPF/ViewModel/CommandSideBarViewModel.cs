using QCBDManagementWPF.Command;
using System;
using System.Collections.Generic;
using Entity = QCBDManagementCommon.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementCommon.Enum;
using QCBDManagementWPF.Models;
using QCBDManagementWPF.Classes;
using System.ComponentModel;
using QCBDManagementBusiness;
using QCBDManagementCommon.Classes;

namespace QCBDManagementWPF.ViewModel
{
    public class CommandSideBarViewModel : BindBase
    {
        private Func<Object, Object> _page;
        private string _navigTo;
        private Func<string, object> _getObjectFromMainWindowViewModel;
        private Cart _cart;
        private NotifyTaskCompletion<List<Entity.Command_item>> _command_itemTask_updateItem;
        private NotifyTaskCompletion<List<Entity.Command_item>> _command_itemTask_updateCommand_Item;
        private NotifyTaskCompletion<object> _quoteTask;

        //----------------------------[ Models ]------------------

        private MainWindowViewModel _mainWindowViewModel;
        private CommandModel _selectedCommandModel;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> UtilitiesCommand { get; set; }
        public ButtonCommand<string> SetupCommandCommand { get; set; }


        public CommandSideBarViewModel()
        {
            instances();
            instancesModel();
            instancesCommand();
            initEvents();
        }


        //----------------------------[ Initialization ]------------------

        private void initEvents()
        {
            PropertyChanged += onNavigToChange;
            PropertyChanged += onSelectedCommandModelChange;
            PropertyChanged += onGetObjectFromMainWindowViewModelChange;
            //_command_itemTask_updateItem.PropertyChanged += onCommand_itemTaskCompletion;
            //_command_itemTask_updateCommand_Item.PropertyChanged += onCommand_itemTaskCompletion_updateCommand_item;
        }

        private void instances()
        {
            _command_itemTask_updateItem = new NotifyTaskCompletion<List<Entity.Command_item>>();
            _command_itemTask_updateCommand_Item = new NotifyTaskCompletion<List<Entity.Command_item>>();
            _quoteTask = new NotifyTaskCompletion<object>();
            //_itemTaskList = new List<NotifyTaskCompletion<List<Entity.Item>>>();
        }

        private void instancesModel()
        {
            _selectedCommandModel = new CommandModel();

        }

        private void instancesCommand()
        {
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
            SetupCommandCommand = new Command.ButtonCommand<string>(executeSetupAction, canExecuteSetupAction);

        }

        //----------------------------[ Properties ]------------------

        public BusinessLogic Bl
        {
            get { return _startup.Bl; }
            set { _startup.Bl = value; onPropertyChange("Bl"); }
        }

        public CommandModel SelectedCommandModel
        {
            get { return _selectedCommandModel; }
            set { setProperty(ref _selectedCommandModel, value, "SelectedCommandModel"); }
        }

        public string NavigTo
        {
            get { return _navigTo; }
            set { setProperty(ref _navigTo, value, "NavigTo"); }
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


        private void updateCommand()
        {
            UtilitiesCommand.raiseCanExecuteActionChanged();
            SetupCommandCommand.raiseCanExecuteActionChanged();
        }

        private void generateAllBillsPdf()
        {
            foreach (var billModel in SelectedCommandModel.BillModelList)
            {
                Bl.BlCommand
                    .GeneratePdfCommand(new QCBDManagementCommon
                        .Structures
                            .ParamCommandToPdf
                    {
                        BillId = billModel.Bill.ID,
                        CommandId = SelectedCommandModel.Command.ID
                    });
            }
        }

        /// <summary>
        /// Navigate through the application
        /// </summary>
        /// <param name="obj"></param>
        public void executeNavig(string obj)
        {
            switch (obj)
            {
                case "select-client":
                    _page(new ClientViewModel());
                    break;
            }
        }

        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _page = navigObject;
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


        private void onGetObjectFromMainWindowViewModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "GetObjectFromMainWindowViewModel"))
            {
                _mainWindowViewModel = _getObjectFromMainWindowViewModel("main") as MainWindowViewModel;
                if (_mainWindowViewModel != null)
                {
                    _mainWindowViewModel.PropertyChanged += onCurrentPageChange_updateCommand;
                    _mainWindowViewModel.CommandViewModel.CommandDetailViewModel.PropertyChanged += onCommand_ItemModelListChange;
                }
            }
        }

        private void onCommand_ItemModelListChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Command_ItemModelList"))
                SelectedCommandModel.Command_ItemList = _mainWindowViewModel.CommandViewModel.CommandDetailViewModel.Command_ItemModelList;
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

        private void onCurrentPageChange_updateCommand(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CurrentViewModel"))
                updateCommand();
        }

        private void onSelectedCommandModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedCommandModel"))
                updateCommand();
        }

        //----------------------------[ Action Commands ]------------------


        private bool canExecuteUtilityAction(string arg)
        {
            if ((_page(null) as CommandDetailViewModel) == null)
                return false;

            if ((_page(null) as CommandDetailViewModel) != null
                && SelectedCommandModel.Command.ID == 0
                && string.IsNullOrEmpty(SelectedCommandModel.TxtStatus))
                return false;

            if (!SelectedCommandModel.TxtStatus.Equals(EStatusCommand.Quote.ToString())
                && (arg.Equals("convert-quoteToCommand")
                || arg.Equals("convert-quoteToCredit")
                || arg.Equals("open-email")))
                return false;

            if (SelectedCommandModel.TxtStatus.Equals(EStatusCommand.Quote.ToString())
                && (arg.Equals("close-command")
                   || arg.Equals("valid-command")
                   || arg.Equals("convert-commandToQuote")))
                return false;


            if ((SelectedCommandModel.TxtStatus.Equals(EStatusCommand.Command.ToString()) || !SelectedCommandModel.TxtStatus.Equals(EStatusCommand.Pre_Command.ToString()))
                && arg.Equals("valid-command"))
                return false;

            if ((SelectedCommandModel.TxtStatus.Equals(EStatusCommand.Command.ToString()) || !SelectedCommandModel.TxtStatus.Equals(EStatusCommand.Pre_Credit.ToString()))
                && arg.Equals("valid-credit"))
                return false;

            if (SelectedCommandModel.TxtStatus.Equals(EStatusCommand.Command_Close.ToString())
                && arg.Equals("close-command"))
                return false;

            return true;
        }

        private async void executeUtilityAction(string obj)
        {
            _mainWindowViewModel = _getObjectFromMainWindowViewModel("main") as MainWindowViewModel;
            if (_mainWindowViewModel != null)
            {
                CommandDetailViewModel commandDetail = _mainWindowViewModel.CommandViewModel.CommandDetailViewModel;
                switch (obj)
                {
                    case "convert-quoteToCommand":
                        //updateCommandStatus(EStatusCommand.Pre_Command);
                        commandDetail.updateCommandStatus(EStatusCommand.Pre_Command);

                        if (commandDetail.CommandSelected.TxtStatus.Equals(EStatusCommand.Pre_Command.ToString()))
                            await Dialog.show("Quote successfully Converted to Command");
                        break;
                    case "valid-command":
                        //updateCommandStatus(EStatusCommand.Command);
                        commandDetail.updateCommandStatus(EStatusCommand.Command);
                        if (commandDetail.CommandSelected.TxtStatus.Equals(EStatusCommand.Command.ToString()))
                            await Dialog.show("Command successfully Validated");
                        break;
                    case "valid-credit":
                        //updateCommandStatus(EStatusCommand.Command);
                        commandDetail.updateCommandStatus(EStatusCommand.Credit);
                        if (commandDetail.CommandSelected.TxtStatus.Equals(EStatusCommand.Credit.ToString()))
                            await Dialog.show("Command successfully Validated");
                        break;
                    case "convert-commandToQuote":
                        //updateCommandStatus(EStatusCommand.Quote);
                        if (await Dialog.show("Do you really want to convert into quote?"))
                        {
                            commandDetail.updateCommandStatus(EStatusCommand.Quote);
                            if (commandDetail.CommandSelected.TxtStatus.Equals(EStatusCommand.Quote.ToString()))
                                await Dialog.show("Command successfully Converted to Quote");
                        }                         
                        break;
                    case "convert-quoteToCredit":
                        if (await Dialog.show("Do you really want to convert into credit?"))
                        {
                            commandDetail.updateCommandStatus(EStatusCommand.Pre_Credit);
                            if (commandDetail.CommandSelected.TxtStatus.Equals(EStatusCommand.Pre_Credit.ToString()))
                                await Dialog.show("Command successfully Converted to Quote");
                        }
                        break;
                    case "close-command":
                        //updateCommandStatus(EStatusCommand.Command_Close);
                        if (await Dialog.show("Do you really want to close this command?"))
                        {
                            commandDetail.updateCommandStatus(EStatusCommand.Command_Close);
                            if (commandDetail.CommandSelected.TxtStatus.Equals(EStatusCommand.Command_Close.ToString()))
                                await Dialog.show("Command successfully Closed");
                        }
                        break;
                }
                _page(commandDetail);
            }
        }

        /// <summary>
        /// set the value of the next page.
        /// </summary>
        /// <param name="obj"></param>
        private void executeSetupAction(string obj)
        {
            NavigTo = obj;
        }

        private bool canExecuteSetupAction(string arg)
        {
            if ((_page(null) as QuoteViewModel) == null)
                return false;

            if ((_page(null) as QuoteViewModel) != null
                && Cart.Client != null && Cart.Client.Client.ID != 0
                && arg.Equals("select-client"))
                return false;

            return true;
        }




    }
}