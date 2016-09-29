using QCBDManagementBusiness;
using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.ViewModel
{
    public class OptionEmailViewModel : BindBase
    {
        Dictionary<string, GeneralInfos.FileWriter> _emails;
        private string _title;

        //----------------------------[ Models ]------------------

        //----------------------------[ Commands ]------------------

        public ButtonCommand<string> DeleteCommand { get; set; }
        public ButtonCommand<string> UpdateCommand { get; set; }


        public OptionEmailViewModel()
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
            _title = "Email Management";
            _emails = new Dictionary<string, GeneralInfos.FileWriter>();
            _emails["quote"] = new GeneralInfos.FileWriter("quote");
            _emails["reminder_1"] = new GeneralInfos.FileWriter("reminder_1");
            _emails["reminder_2"] = new GeneralInfos.FileWriter("reminder_2");
            _emails["bill"] = new GeneralInfos.FileWriter("bill");
            _emails["command_confirmation"] = new GeneralInfos.FileWriter("quote");
            _emails["quote"] = new GeneralInfos.FileWriter("command_confirmation");
        }

        private void instancesModel()
        {

        }

        private void instancesCommand()
        {
            UpdateCommand = new ButtonCommand<string>(updateEmailFiles, canUpdateEmailFIles);
            DeleteCommand = new ButtonCommand<string>(eraseContent, canEraseContent);
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

        public GeneralInfos.FileWriter CommandConfirmationEmailFile
        {
            get { return _emails["command_confirmation"]; }
            set { _emails["command_confirmation"] = value; onPropertyChange("CommandConfirmationEmailFile"); }
        }

        public GeneralInfos.FileWriter BillEmailFile
        {
            get { return _emails["bill"]; }
            set { _emails["bill"] = value; onPropertyChange("BillEmailFile"); }
        }

        public GeneralInfos.FileWriter ReminderTwoEmailFile
        {
            get { return _emails["reminder_2"]; }
            set { _emails["reminder_2"] = value; onPropertyChange("ReminderTwoEmailFile"); }
        }

        public GeneralInfos.FileWriter ReminderOneEmailFile
        {
            get { return _emails["reminder_1"]; }
            set { _emails["reminder_1"] = value; onPropertyChange("ReminderEmailFile"); }
        }

        public GeneralInfos.FileWriter QuoteEmailFile
        {
            get { return _emails["quote"]; }
            set { _emails["quote"] = value; onPropertyChange("QuoteEmailFile"); }
        }

        //----------------------------[ Actions ]------------------

        public async void loadData()
        {
            Dialog.showSearch("Loading...");
            
            string login = ((await _startup.Bl.BlReferential.searchInfos(new QCBDManagementCommon.Entities.Infos { Name = "ftp_login" }, "OR")).FirstOrDefault() ?? new Infos()).Value;
            string password = ((await _startup.Bl.BlReferential.searchInfos(new QCBDManagementCommon.Entities.Infos { Name = "ftp_password" }, "OR")).FirstOrDefault() ?? new Infos()).Value;

            foreach (var email in _emails)
            {
                email.Value.TxtLogin = login;
                email.Value.TxtPassword = password;
                email.Value.read();
            }

            Dialog.IsDialogOpen = false;
        }

        //----------------------------[ Event Handler ]------------------



        //----------------------------[ Action Commands ]------------------
        
        private void eraseContent(string obj)
        {
            switch (obj)
            {
                case "bill":
                    _emails["bill"].TxtContent = "";
                    break;
                case "reminder-2":
                    _emails["reminder_2"].TxtContent = "";
                    break;
                case "reminder-1":
                    _emails["reminder_1"].TxtContent = "";
                    break;
                case "command-confirmation":
                    _emails["command_confirmation"].TxtContent = "";
                    break;
                case "quote":
                    _emails["quote"].TxtContent = "";
                    break;
            }
        }

        private bool canEraseContent(string arg)
        {
            bool isWrite = securityCheck(QCBDManagementCommon.Enum.EAction.Option, QCBDManagementCommon.Enum.ESecurity._Write);
            bool isUpdate = securityCheck(QCBDManagementCommon.Enum.EAction.Option, QCBDManagementCommon.Enum.ESecurity._Update);
            if (isUpdate && isWrite)
                return true;
            return false;
        }

        private async void updateEmailFiles(string obj)
        {
            switch (obj)
            {
                case "bill":
                    if (_emails["bill"].save())
                        await Dialog.show("Email Bill saved Successfully!");
                    break;
                case "reminder-2":
                    if (_emails["reminder_2"].save())
                        await Dialog.show("Email first Bill reminder saved Successfully!");
                    break;
                case "reminder-1":
                    if (_emails["reminder_1"].save())
                        await Dialog.show("Email second Bill reminder saved Successfully!");
                    break;
                case "command-confirmation":
                    if (_emails["command_confirmation"].save())
                        await Dialog.show("Email validation Order confirmation saved Successfully!");
                    break;
                case "quote":
                    if (_emails["quote"].save())
                        await Dialog.show("Email Quote saved Successfully!");
                    break;
            }
        }

        private bool canUpdateEmailFIles(string arg)
        {
            bool isWrite = securityCheck(QCBDManagementCommon.Enum.EAction.Option, QCBDManagementCommon.Enum.ESecurity._Write);
            bool isUpdate = securityCheck(QCBDManagementCommon.Enum.EAction.Option, QCBDManagementCommon.Enum.ESecurity._Update);
            if (isUpdate && isWrite)
                return true;
            return false;
        }
    }
}
