using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.ViewModel
{
    public class ItemSideBarViewModel : BindBase
    {
        
        private Func<object, object> _currentViewModel;

        //----------------------------[ Models ]------------------

        private ItemModel _selectedItem;

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> SetupItemCommand { get; set; }
        public ButtonCommand<string> UtilitiesCommand { get; set; }



        public ItemSideBarViewModel()
        {
            instances();
            instancesCommand();
        }

        //----------------------------[ Initialization ]------------------
        
        private void instances()
        {
            _selectedItem = new ItemModel();
        }

        private void instancesCommand()
        {
            SetupItemCommand = new ButtonCommand<string>(executeSetupAction, canExecuteSetupAction);
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
        }

        //----------------------------[ Properties ]------------------

        public ItemModel SelectedItem
        {
            get { return _selectedItem; }
            set { setProperty(ref _selectedItem, value, "SelectedItem"); }
        }

        //----------------------------[ Actions ]------------------
                
        public void mainNavigObject(Func<Object, Object> navigObject)
        {
            _currentViewModel = navigObject;
        }
        
        //----------------------------[ Event Handler ]------------------


        //----------------------------[ Action Commands ]------------------

        private async void executeUtilityAction(string obj)
        {
            switch (obj)
            {
                case "update-item":
                    await Dialog.show("Update Item");
                    break;
            }
        }

        private bool canExecuteUtilityAction(string arg)
        {
            return false;
        }

        private void executeSetupAction(string obj)
        {
            switch (obj)
            {
                case "new-item":
                    SelectedItem.IsRefModifyEnable = true;
                    SelectedItem.Item = new QCBDManagementCommon.Entities.Item();
                    _currentViewModel(new ItemDetailViewModel());
                    break;
            }
        }

        private bool canExecuteSetupAction(string arg)
        {
            bool isUpdate = securityCheck(QCBDManagementCommon.Enum.EAction.Item, QCBDManagementCommon.Enum.ESecurity._Update);
            bool isWrite = securityCheck(QCBDManagementCommon.Enum.EAction.Item, QCBDManagementCommon.Enum.ESecurity._Write);
            if ((!isUpdate || !isWrite)
                && arg.Equals("new-item"))
                return false;

            return true;
        }  

    }
}
