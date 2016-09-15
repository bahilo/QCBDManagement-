using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.Classes
{
    public class ToDo : INotifyPropertyChanged
    {
        string _task;
        bool _isDone;
        DateTime _endDate;

        public event PropertyChangedEventHandler PropertyChanged;

        public ToDo()
        {
            _endDate = new DateTime();
            PropertyChanged += onIsDoneChange;
        }

        private void onIsDoneChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsDone"))
            {
                if (IsDone)
                    EndDate = DateTime.Now;
                else
                    EndDate = new DateTime();
            }
        }

        public void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string TxtTask
        {
            get { return _task; }
            set { _task = value; onPropertyChange("TxtTask"); }
        }

        public bool IsDone
        {
            get { return _isDone; }
            set { _isDone = value; onPropertyChange("IsDone"); }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; onPropertyChange("EndDate"); }
        }
    }
}
