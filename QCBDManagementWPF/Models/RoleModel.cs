using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.Models
{
    public class RoleModel : BindBase
    {
        private Role _role;
        private ActionModel _actionModel;
        private bool _isModified;

        public RoleModel()
        {
            _role = new Role();
            _actionModel = new ActionModel();
        }

        public bool IsModified
        {
            get { return _isModified; }
            set { _isModified = value; onPropertyChange("IsModified"); }
        }

        public Role Role
        {
            get { return _role; }
            set { _role = value; onPropertyChange("Role"); }
        }

        public string TxtID
        {
            get { return _role.ID.ToString(); }
            set { _role.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string TxtName
        {
            get { return _role.Name; }
            set { _role.Name = value; onPropertyChange("TxtName"); }
        }

        public List<QCBDManagementCommon.Entities.Action> ActionList
        {
            get { return _role.ActionList; }
            set { _role.ActionList = value; onPropertyChange("ActionList"); }
        }

        public ActionModel ActionModel
        {
            get { return _actionModel; }
            set { _actionModel = value; onPropertyChange("ActionModel"); }
        }
    }
}
