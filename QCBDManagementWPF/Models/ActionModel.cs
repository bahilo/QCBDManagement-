using Entity = QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.Models
{
    public class ActionModel : BindBase
    {
        Entity.Action _action;
        PrivilegeModel _privilegeModel;
        public ActionModel()
        {
            _action = new Entity.Action();
            _privilegeModel = new PrivilegeModel();
        }

        public Entity.Action Action
        {
            get { return _action; }
            set { setProperty(ref _action, value, "Action"); }
        }

        public string TxtName
        {
            get { return _action.Name; }
            set { _action.Name = value; onPropertyChange("TxtName"); }
        }

        public string TxtID
        {
            get { return _action.ID.ToString(); }
            set { _action.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public PrivilegeModel PrivilegeModel
        {
            get { return _privilegeModel; }
            set { setProperty(ref _privilegeModel, value, "PrivilegeModel"); }
        }
    }
}
