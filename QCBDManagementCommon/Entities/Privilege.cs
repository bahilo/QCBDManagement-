using System;


namespace QCBDManagementCommon.Entities
{
    public class Privilege
    {
        // Attributes

        public int ID {get; set;}

        public int Role_actionId { get; set;}

        public bool IsWrite {get; set;}

        public bool IsRead { get; set;}

        public bool IsUpdate { get; set;}

        public bool IsDelete { get; set;}

        public DateTime Date {get; set;}

        public bool IsSendMail { get; set; }

    } /* end class Privilege */
}