using QCBDManagementWPF.Classes;
using System;
using Entity = QCBDManagementCommon.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementWPF.ViewModel;
using System.ComponentModel;
using QCBDManagementCommon.Enum;
using System.Collections.ObjectModel;

namespace QCBDManagementWPF.Models
{
    public class CommandModel : BindBase
    {
        private Entity.Command _command;
        private AgentModel _agentModel;
        private ClientModel _clientModel;
        private Entity.Address _deliveryAddress;
        private Entity.Address _billAddress;
        private List<Command_itemModel> _command_itemList;
        private List<Entity.Address> _addressList;
        private Entity.Tax_command _tax_command;
        private Entity.Tax _tax;
        private List<BillModel> _billModelList;
        private List<DeliveryModel> _deliveryModelList;


        public CommandModel()
        {
            _tax = new Entity.Tax();
            _tax_command = new Entity.Tax_command();
            _addressList = new List<Entity.Address>();
            _billModelList = new List<BillModel>();
            _deliveryModelList = new List<DeliveryModel>();
            _agentModel = new AgentModel();
            _clientModel = new ClientModel();
            _command = new Entity.Command();
            _command_itemList = new List<Command_itemModel>();

            PropertyChanged += onAddressListChange;
            PropertyChanged += onAgentModelChange;
            PropertyChanged += onClientModelChange;
        }


        private void onClientModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CLientModel"))
            {
                TxtClientId = (CLientModel != null) ? CLientModel.TxtID : "0";
            }
        }

        private void onAgentModelChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AgentModel"))
            {
                TxtAgentId = AgentModel.TxtID;
            }
        }

        private void onAddressListChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AddressList"))
            {
               /*var result = AddressList.Where(x => x.Name.Equals(EStatusAddress.Delivery.ToString())).ToList();
                _command.DeliveryAddress = (result.Count > 0) ? result[0].ID: 0;

                result = AddressList.Where(x => x.Name.Equals(EStatusAddress.Bill.ToString())).ToList();
                _command.BillAddress = (result.Count > 0) ? result[0].ID : 0;
                */
                var deliveryAddressFoundList = AddressList.Where(x => x.ID == Command.DeliveryAddress).ToList();
                DeliveryAddress = (deliveryAddressFoundList.Count() > 0) ? deliveryAddressFoundList[0] : new Entity.Address();
                var billAddressFoundList = AddressList.Where(x => x.ID == Command.BillAddress).ToList();
                BillAddress = (billAddressFoundList.Count() > 0) ? billAddressFoundList[0] : new Entity.Address();

            }
        }

        public Entity.Address BillAddress
        {
            get { return _billAddress; }
            set { setProperty(ref _billAddress, value, "BillAddress"); }
        }

        public Entity.Address DeliveryAddress
        {
            get { return _deliveryAddress; }
            set { setProperty(ref _deliveryAddress, value, "DeliveryAddress"); }
        }
        
        public List<DeliveryModel> DeliveryModelList
        {
            get { return _deliveryModelList; }
            set { setProperty(ref _deliveryModelList, value, "DeliveryModelList"); }
        }

        public List<BillModel> BillModelList
        {
            get { return _billModelList; }
            set { setProperty(ref _billModelList, value, "BillModelList"); }
        }

        public List<Command_itemModel> Command_ItemList
        {
            get { return _command_itemList; }
            set { _command_itemList = value; onPropertyChange("Command_ItemList"); }
        }

        public Entity.Command Command
        {
            get { return _command; }
            set { _command = value; onPropertyChange("Command"); }
        }

        public Entity.Tax_command Tax_command
        {
            get { return _tax_command; }
            set { _tax_command = value; onPropertyChange("Tax_command"); }
        }

        public Entity.Tax Tax
        {
            get { return _tax; }
            set { _tax = value; onPropertyChange("Tax"); }
        }

        public List<Entity.Address> AddressList
        {
            get
            { return _addressList; }
            set { setProperty(ref _addressList, value, "AddressList"); }
        }

        public AgentModel AgentModel
        {
            get { return _agentModel;  }
            set { _agentModel = value; onPropertyChange("AgentModel"); }
        }

        public ClientModel CLientModel
        {
            get { return _clientModel;  }
            set { _clientModel = value; onPropertyChange("CLientModel"); }
        }

        public string TxtID
        {
            get {  return _command.ID.ToString(); }
            set { _command.ID = Convert.ToInt32(value);  onPropertyChange("TxtID"); }
        }

        public string TxtAgentId
        {
            get {  return _command.AgentId.ToString(); }
            set { _command.AgentId = Convert.ToInt32(value); onPropertyChange("TxtAgentId"); }
        }

        public string TxtClientId
        {
            get { return _command.ClientId.ToString(); }
            set { _command.ClientId = Convert.ToInt32(value); onPropertyChange("TxtClientId"); }
        }

        public string TxtPrivateComment
        {
            get { return _command.Comment1; }
            set {  _command.Comment1 = value; onPropertyChange("TxtComment1"); }
        }

        public string TxtPublicComment
        {
            get {  return _command.Comment2; }
            set { _command.Comment2 = value; onPropertyChange("TxtComment2"); }
        }

        public string TxtAdminComment
        {
            get { return _command.Comment3; }
            set { _command.Comment3 = value; onPropertyChange("TxtComment3"); }
        }

        public string TxtBillAddress
        {
            get { return _command.BillAddress.ToString(); }
            set { _command.BillAddress = Convert.ToInt32(value); onPropertyChange("TxtBillAddress"); }
        }

        public string TxtDeliveryAddress
        {
            get {  return _command.DeliveryAddress.ToString(); }
            set { _command.DeliveryAddress = Convert.ToInt32(value); onPropertyChange("TxtDeliveryAddress"); }
        }

        public string TxtStatus
        {
            get {  return _command.Status; }
            set { _command.Status = value; onPropertyChange("TxtStatus"); }
        }

        public string TxtDate
        {
            get { return _command.Date.ToString(); }
            set { _command.Date = Convert.ToDateTime(value); onPropertyChange("TxtDate"); }
        }

        public string TxtTaxName
        {
            get { return _command.Tax.ToString(); }
            set { _command.Tax = Convert.ToDouble(value); onPropertyChange("TxtTaxName"); }
        }
        



    }
}
