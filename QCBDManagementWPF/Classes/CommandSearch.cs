using Entity = QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Enum;
using System.ComponentModel;

namespace QCBDManagementWPF.Classes
{
    public class CommandSearch : System.ComponentModel.INotifyPropertyChanged
    {
        private int _commandId;
        private int _billId;
        private int _clientId;
        private List<string> _statusList;
        private string _selectedStatus;
        private List<Entity.Agent> _agents;
        private Entity.Agent _selectedAgent;
        private string _companyName;
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _isDeepSearch;

        public event PropertyChangedEventHandler PropertyChanged;

        public CommandSearch()
        {
            _statusList = new List<string>
            {
                EStatusCommand.Quote.ToString(),                  //devis
                EStatusCommand.Pre_Command.ToString(),             //preco
                EStatusCommand.Command.ToString(),                //command
                EStatusCommand.Command_Close.ToString(),           // close
                EStatusCommand.Pre_Credit.ToString(),              // preavoir
                EStatusCommand.Credit.ToString(),                 // avoir
                EStatusCommand.Credit_CLose.ToString(),            // a_close
                EStatusCommand.Pre_Client_Validation.ToString(),    // revalid
                EStatusCommand.Bill_Command.ToString(),            // facture
                EStatusCommand.Bill_Credit.ToString(),              // a_facture
                EStatusCommand.Billed.ToString(),                    //f
                EStatusCommand.Not_Billed.ToString()               //nf}
            };
            _startDate = DateTime.Now;
            _endDate = DateTime.Now;
        }

        public int CommandId
        {
            get { return _commandId; }
            set { _commandId = value; onPropertyChange("CommandId"); }
        }

        public int BillId
        {
            get { return _billId; }
            set { _billId = value; onPropertyChange("BillId"); }
        }

        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; onPropertyChange("SelectedStatus"); }
        }

        public List<string> StatusList
        {
            get { return _statusList; }
            set { _statusList = value; onPropertyChange("StatusList"); }
        }

        public Entity.Agent SelectedAgent
        {
            get { return _selectedAgent; }
            set { _selectedAgent = value; onPropertyChange("SelectedAgent"); }
        }

        public List<Entity.Agent> AgentList
        {
            get { return _agents; }
            set { _agents = value; onPropertyChange("AgentList"); }
        }

        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; onPropertyChange("CompanyName"); }
        }

        public int ClientId
        {
            get { return _clientId; }
            set { _clientId = value; onPropertyChange("ClientId"); }
        }

        public string StartDate
        {
            get { return _startDate.ToString(); }
            set {  _startDate = Utility.convertToDateTime(value); onPropertyChange("StartDate"); }
        }

        public string EndDate
        {
            get { return _endDate.ToString(); }
            set { _endDate = Utility.convertToDateTime(value); onPropertyChange("EndDate"); }
        }

        public bool IsDeepSearch
        {
            get { return _isDeepSearch; }
            set { _isDeepSearch = value; onPropertyChange("IsDeepSearch"); }
        }

        public void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
