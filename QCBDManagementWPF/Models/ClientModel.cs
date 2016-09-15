using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace QCBDManagementWPF.Models
{
    public class ClientModel : BindBase
    {
        private Client _client;
        private Address _address;
        private List<Address> _Addresses;
        private Contact _contact;
        private List<Contact> _contacts;
        private AgentModel _agent;        
        private bool _isSearchCompanyChecked;
        private bool _isSearchContactChecked;
        private bool _isSearchClientChecked;
        private bool _isSearchProspectChecked;


        public ClientModel()
        {
            _client = new Client();
            _agent = new AgentModel();
            _Addresses = new List<Address>();
            _contacts = new List<Contact>();
            _contact = new Contact();
            _address = new Address();

            PropertyChanged += onClientChange_updateAllClientObjects;
        }

        private void onClientChange_updateAllClientObjects(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Client"))
            {
                onPropertyChange("TxtID");
                onPropertyChange("TxtAgentId");
                onPropertyChange("TxtFirstName");
                onPropertyChange("TxtLastName");
                onPropertyChange("TxtCompany");
                onPropertyChange("TxtEmail");
                onPropertyChange("TxtPhone");
                onPropertyChange("TxtFax");
                onPropertyChange("TxtRib");
                onPropertyChange("TxtCRN");
                onPropertyChange("TxtPayDelay");
                onPropertyChange("TxtComment");
                onPropertyChange("TxtMaxCredit");
                onPropertyChange("TxtIsProspect");
                onPropertyChange("TxtIsClient");
                onPropertyChange("TxtStatus");
                onPropertyChange("TxtCompanyName");
            }
        }

        public Contact Contact
        {
            get { return (_contact != null)? _contact : new Contact(); }
            set { setProperty(ref _contact, value, "Contact"); }
        }

        public Address Address
        {
            get { return (_address != null)?_address:new Address(); }
            set { setProperty(ref _address, value, "Address"); }
        }

        public List<Contact> ContactList
        {
            get { return _contacts; }
            set { setProperty(ref _contacts, value, "ContactList"); }
        }

        public AgentModel Agent
        {
            get { return _agent; }
            set { setProperty(ref _agent, value, "Agent"); }
        }

        public string TxtID
        {
            get { return _client.ID.ToString(); }
            set { if (!string.IsNullOrEmpty(value)) { _client.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); } }
        }

        public string TxtAgentId
        {
            get {  return _client.AgentId.ToString(); }
            set { if (!string.IsNullOrEmpty(value)) { _client.AgentId = Convert.ToInt32(value); onPropertyChange("TxtAgentId"); } }
        }

        public string TxtAgentName
        {
            get { return (_agent.TxtLastName != null) ? _agent.TxtLastName : ""; }
            set { _agent.TxtLastName = value; onPropertyChange("TxtAgentName"); }
        }

        public string TxtAgentFirstName
        {
            get { return (_agent.TxtFirstName != null) ? _agent.TxtFirstName : ""; }
            set { _agent.TxtFirstName = value; onPropertyChange("TxtAgentFirstName"); }
        }

        public string TxtFirstName
        {
            get { return (_client.FirstName != null) ? _client.FirstName : ""; }
            set { _client.FirstName = value; onPropertyChange("TxtFirstName"); }
        }

        public string TxtLastName
        {
            get { return (_client.LastName != null) ? _client.LastName : ""; }
            set { _client.LastName = value; onPropertyChange("TxtLastName"); }
        }

        public string TxtCompany
        {
            get { return (_client.Company != null) ? _client.Company : ""; }
            set { _client.Company = value; onPropertyChange("TxtCompany"); }
        }

        public string TxtEmail
        {
            get { return (_client.Email != null) ? _client.Email : ""; }
            set { _client.Email = value; onPropertyChange("TxtEmail"); }
        }

        public string TxtPhone
        {
            get { return (_client.Phone != null) ? _client.Phone : ""; }
            set { _client.Phone = value; onPropertyChange("TxtPhone"); }
        }

        public string TxtFax
        {
            get { return (_client.Fax != null) ? _client.Fax : ""; }
            set { _client.Fax = value; onPropertyChange("TxtFax"); }
        }

        public string TxtRib
        {
            get { return (_client.Rib != null) ? _client.Rib : ""; }
            set { _client.Rib = value; onPropertyChange("TxtRib"); }
        }

        public string TxtCRN
        {
            get { return (_client.CRN != null) ? _client.CRN : ""; }
            set { _client.CRN = value; onPropertyChange("TxtCRN"); }
        }

        public string TxtPayDelay
        {
            get { return _client.PayDelay.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _client.PayDelay = converted; } else _client.PayDelay = 0; onPropertyChange("TxtPayDelay"); }
        }

        public string TxtComment
        {
            get { return (_client.Comment != null) ? _client.Comment : ""; }
            set { _client.Comment = value; onPropertyChange("TxtComment"); }
        }

        public string TxtMaxCredit
        {
            get { return _client.MaxCredit.ToString(); }
            set { int converted; if (int.TryParse(value, out converted)) { _client.MaxCredit = converted; } else _client.MaxCredit = 0; onPropertyChange("TxtMaxCredit"); }
        }

        public bool IsProspect
        {
            get
            {
                if (_client.Status == EStatus.Prospect.ToString())
                    return true;

                return false;
            }
            set
            {
                if (value == true)
                {
                    _client.Status = EStatus.Prospect.ToString();
                    onPropertyChange("TxtIsProspect");
                }
            }
        }

        public bool IsClient
        {
            get
            {
                if (_client.Status == EStatus.Client.ToString())
                    return true;

                return false;
            }
            set
            {
                if (value == true)
                {
                    _client.Status = EStatus.Client.ToString();
                    onPropertyChange("TxtIsClient");
                }
            }
        }

        public string TxtStatus
        {
            get { return (_client.Status != null) ? _client.Status : ""; }
            set { _client.Status = value; onPropertyChange("TxtStatus"); }
        }

        public string TxtCompanyName
        {
            get { return (_client.CompanyName != null) ? _client.CompanyName : ""; }
            set { _client.CompanyName = value; onPropertyChange("TxtCompanyName"); }
        }

        public bool IsSearchCompanyChecked
        {
            get { return _isSearchCompanyChecked; }
            set { _isSearchCompanyChecked = value; onPropertyChange("IsSearchCompanyChecked"); }
        }

        public bool IsSearchContactCheked
        {
            get { return _isSearchContactChecked; }
            set { _isSearchContactChecked = value; onPropertyChange("IsSearchContactChecked"); }
        }

        public bool IsSearchClientChecked
        {
            get { return _isSearchClientChecked; }
            set { _isSearchClientChecked = value; onPropertyChange("IsSearchCLientChecked"); }
        }

        public bool IsSearchProspectChecked
        {
            get { return _isSearchProspectChecked; }
            set { _isSearchProspectChecked = value; onPropertyChange("IsSearchProspectChecked"); }
        }                    

        public Client Client
        {
            get { return _client; }
            set { _client = value; onPropertyChange("Client"); }
        }     
        
        public List<Address> AddressList
        {
            get { return _Addresses; }
            set { setProperty(ref _Addresses, value, "AddressList"); }
        }   

    }
}
