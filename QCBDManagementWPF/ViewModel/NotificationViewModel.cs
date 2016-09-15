using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QCBDManagementBusiness;
using QCBDManagementWPF.Classes;

namespace QCBDManagementWPF.ViewModel
{
    public class NotificationViewModel : BindBase
    {
        private BusinessLogic _bl;
        private Notification _notification;
        private List<Notification> _notifications;
        private string _title;

        public NotificationSideBarViewModel NotificationSideBarViewModel { get; set; }
        //public ButtonCommand ButtonCommand{ get; set; }

        public NotificationViewModel()
        {
            _title = "Notification Management";
            _notification = new Notification();
            _notifications = new List<Notification>();
            NotificationSideBarViewModel = new NotificationSideBarViewModel();
        }

        public string TxtID
        {
            get { return _notification.ID.ToString(); }
            set { _notification.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string Title
        {
            get { return _title; }
            set { setProperty(ref _title, value, "Title"); }
        }

        public string TxtBillId
        {
            get { return _notification.BillId.ToString(); }
            set { _notification.BillId = Convert.ToInt32(value); onPropertyChange("TxtBillId"); }
        }

        public string TxtReminder1
        {
            get
            {
                return _notification.Reminder1;
            }
            set
            {
                _notification.Reminder1= value;
            }
        }

        public string TxtReminder2
        {
            get
            {
                return _notification.Reminder2;
            }
            set
            {
                _notification.Reminder2 = value;
            }
        }

        public string TxtDate
        {
            get
            {
                return _notification.Date.ToString();
            }
            set
            {
                DateTime res;
                DateTime.TryParse(value, out res);
                _notification.Date= res;
            }
        }
        
        public void action()
        {

        }


        public bool isValid()
        {
            return true;
        }

        internal void setLogicAccess(BusinessLogic bl)
        {
            _bl = bl;
        }



    }
}
