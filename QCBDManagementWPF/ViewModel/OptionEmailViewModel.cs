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
        GeneralInfos.FileWriter _quoteEmailFile;
        GeneralInfos.FileWriter _reminderOneEmailFile;
        GeneralInfos.FileWriter _reminderTwoEmailFile;
        GeneralInfos.FileWriter _billEmailFile;
        GeneralInfos.FileWriter _commandConfirmationEmailFile;
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
            _quoteEmailFile = new GeneralInfos.FileWriter("quote");
            _reminderOneEmailFile = new GeneralInfos.FileWriter("reminder_1");
            _reminderTwoEmailFile = new GeneralInfos.FileWriter("reminder_2");
            _billEmailFile = new GeneralInfos.FileWriter("bill");
            _commandConfirmationEmailFile = new GeneralInfos.FileWriter("command_confirmation");
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


        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public GeneralInfos.FileWriter CommandConfirmationEmailFile
        {
            get { return _commandConfirmationEmailFile; }
            set { setProperty(ref _commandConfirmationEmailFile, value, "CommandConfirmationEmailFile"); }
        }

        public GeneralInfos.FileWriter BillEmailFile
        {
            get { return _billEmailFile; }
            set { setProperty(ref _billEmailFile, value, "BillEmailFile"); }
        }

        public GeneralInfos.FileWriter ReminderTwoEmailFile
        {
            get { return _reminderTwoEmailFile; }
            set { setProperty(ref _reminderTwoEmailFile, value, "ReminderTwoEmailFile"); }
        }

        public GeneralInfos.FileWriter ReminderOneEmailFile
        {
            get { return _reminderOneEmailFile; }
            set { setProperty(ref _reminderOneEmailFile, value, "ReminderEmailFile"); }
        }

        public GeneralInfos.FileWriter QuoteEmailFile
        {
            get { return _quoteEmailFile; }
            set { setProperty(ref _quoteEmailFile, value, "QuoteEmailFile"); }
        }

        //----------------------------[ Actions ]------------------



        //----------------------------[ Event Handler ]------------------



        //----------------------------[ Action Commands ]------------------



        private void eraseContent(string obj)
        {
            switch (obj)
            {
                case "bill":
                    _billEmailFile.TxtContent = "";
                    break;
                case "reminder-2":
                    _reminderOneEmailFile.TxtContent = "";
                    break;
                case "reminder-1":
                    _reminderTwoEmailFile.TxtContent = "";
                    break;
                case "command-confirmation":
                    _commandConfirmationEmailFile.TxtContent = "";
                    break;
                case "quote":
                    _quoteEmailFile.TxtContent = "";
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

        private void updateEmailFiles(string obj)
        {
            switch (obj)
            {
                case "bill":
                    if (_billEmailFile.save())
                        InputDialog.show("Bill Email saved Successfully!");
                    break;
                case "reminder-2":
                    if (_reminderOneEmailFile.save())
                        InputDialog.show("Bill first reminder Email saved Successfully!");
                    break;
                case "reminder-1":
                    if (_reminderTwoEmailFile.save())
                        InputDialog.show("Bill second reminder Email saved Successfully!");
                    break;
                case "command-confirmation":
                    if (_commandConfirmationEmailFile.save())
                        InputDialog.show("Command validation confirmation Email saved Successfully!");
                    break;
                case "quote":
                    if (_quoteEmailFile.save())
                        InputDialog.show("Quote Email saved Successfully!");
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
