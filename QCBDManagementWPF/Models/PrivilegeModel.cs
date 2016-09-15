using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.Models
{
    public class PrivilegeModel : BindBase
    {
        Privilege _privilege;
        //private bool _isChecked;

        public PrivilegeModel()
        {
            _privilege = new Privilege();
        }

        public Privilege Privilege
        {
            get { return _privilege; }
            set { _privilege = value; onPropertyChange("Privilege"); }
        }

        public string TxtID
        {
            get { return _privilege.ID.ToString(); }
            set { _privilege.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string TxtRole_actionId
        {
            get { return _privilege.Role_actionId.ToString(); }
            set { _privilege.Role_actionId = Convert.ToInt32(value); onPropertyChange("TxtRole_actionId"); }
        }

        public bool IsWrite
        {
            get { return _privilege.IsWrite; }
            set { _privilege.IsWrite = value; onPropertyChange("IsWrite"); }
        }

        public bool IsRead
        {
            get { return _privilege.IsRead; }
            set { _privilege.IsRead = value; onPropertyChange("IsRead"); }
        }

        public bool IsUpdate
        {
            get { return _privilege.IsUpdate; }
            set { _privilege.IsUpdate = value; onPropertyChange("IsUpdate"); }
        }

        public bool IsDelete
        {
            get { return _privilege.IsDelete; }
            set { _privilege.IsDelete = value; onPropertyChange("IsDelete"); }
        }

        public bool IsSendMail
        {
            get { return _privilege.IsSendMail; }
            set { _privilege.IsSendMail = value; onPropertyChange("IsSendMail"); }
        }



    }
}
