using QCBDManagementCommon.Entities;
using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.Models
{
    public class InfosModel : BindBase
    {
        private Infos _infos;

        public InfosModel()
        {
            _infos = new Infos();
        }

        public string TxtID
        {
            get { return _infos.ID.ToString(); }
            set { _infos.ID = Convert.ToInt32(value); onPropertyChange("TxtID"); }
        }

        public string TxtName
        {
            get { return _infos.Name; }
            set { _infos.Name = value; onPropertyChange("TxtName"); }
        }

        public string TxtValue
        {
            get { return _infos.Value; }
            set { _infos.Value = value; onPropertyChange("TxtValue"); }
        }

    }
}
