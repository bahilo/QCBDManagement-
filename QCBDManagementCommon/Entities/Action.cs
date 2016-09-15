using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Entities
{
    public class Action
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public Privilege Right { get; set; }
    }
}
