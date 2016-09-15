using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Structures
{
    public struct ParamEmail
    {
        private string _subject;
        private int _reminder ;
        private bool _isSendEmail;
        private bool _isCopyToAgent;

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }
                
        public int Reminder
        {
            get { return _reminder; }
            set { _reminder = value; }
        }

        public bool IsSendEmail
        {
            get { return _isSendEmail; }
            set { _isSendEmail = value; }
        }

        public bool IsCopyToAgent
        {
            get { return _isCopyToAgent; }
            set { _isCopyToAgent = value; }
        }
    }
}
