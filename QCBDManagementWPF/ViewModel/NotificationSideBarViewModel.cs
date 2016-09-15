using QCBDManagementWPF.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.ViewModel
{
    public class NotificationSideBarViewModel
    {
        public ButtonCommand<string> UtilitiesCommand;

        public NotificationSideBarViewModel()
        {
            UtilitiesCommand = new ButtonCommand<string>(executeUtilityAction, canExecuteUtilityAction);
        }

        private bool canExecuteUtilityAction(string arg)
        {
            throw new NotImplementedException();
        }

        private void executeUtilityAction(string obj)
        {
            switch (obj)
            {
                case "email-unpaid":
                    InputDialog.show("Send email for unpaid bill");
                    break;
            }
        }
    }
}
