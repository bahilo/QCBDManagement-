using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Entities
{
    public class Agent_role
    {
        public int ID { get; set; }

        public int RoleId { get; set; }

        public int AgentId { get; set; }

        public int IsOnAllUsers { get; set; }

        public DateTime Date { get; set; }
    }
}
