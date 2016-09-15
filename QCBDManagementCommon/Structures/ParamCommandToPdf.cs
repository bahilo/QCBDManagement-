using QCBDManagementCommon.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Structures
{
    public struct ParamCommandToPdf
    {
        private int _commandId;
        private int _billId;
        private EStatusCommand _status;                 // whether it's a command or a quote PDF generation
        private int _quoteValidityDay;                  // allow to calculate the quote expire date 
        private EStatusCommand _typeQuoteOrProformat;   // Whether it's a quote format or pro forma format
        private bool _isQuoteConstructorReferencesVisible;
        private bool _isCommandConstructorReferencesVisible;
        private ParamEmail _paramEmail;
        private string _currency;
        private string _lang;

        public ParamCommandToPdf(int commandId, int idBill)
        {
            _commandId = commandId;
            _billId = idBill;
            _status = EStatusCommand.Command;
            _quoteValidityDay = 2;
            _typeQuoteOrProformat = EStatusCommand.Quote;
            _isQuoteConstructorReferencesVisible = true;
            _isCommandConstructorReferencesVisible = false;
            _paramEmail = new ParamEmail();
            _currency = "";
            _lang = "";
        }

        public ParamCommandToPdf(int commandId, EStatusCommand status)
        {
            _commandId = commandId;
            _billId = 0;
            _status = status;
            _quoteValidityDay = 2;
            _currency = "";
            _lang = "";
            _typeQuoteOrProformat = EStatusCommand.Quote;
            _isQuoteConstructorReferencesVisible = true;
            _isCommandConstructorReferencesVisible = false;
            _paramEmail = new ParamEmail();
        }

        public ParamCommandToPdf(int quoteValidityDay)
        {
            _commandId = 0;
            _billId = 0;
            _status = EStatusCommand.Quote;
            _quoteValidityDay = 2;
            _currency = "";
            _lang = "";
            _typeQuoteOrProformat = EStatusCommand.Quote;
            _isQuoteConstructorReferencesVisible = true;
            _isCommandConstructorReferencesVisible = false;
            _paramEmail = new ParamEmail();
        }

        public ParamCommandToPdf(EStatusCommand status)
        {
            _commandId = 0;
            _billId = 0;
            _status = status;
            _quoteValidityDay = 2;
            _currency = "";
            _lang = "";
            _typeQuoteOrProformat = EStatusCommand.Quote;
            _isQuoteConstructorReferencesVisible = true;
            _isCommandConstructorReferencesVisible = false;
            _paramEmail = new ParamEmail();
        }

        public ParamCommandToPdf(EStatusCommand status, int quoteValidityDay)
        {
            _commandId = 0;
            _billId = 0;
            _status = status;
            _currency = "";
            _lang = "";
            _quoteValidityDay = quoteValidityDay;
            _typeQuoteOrProformat = EStatusCommand.Quote;
            _isQuoteConstructorReferencesVisible = true;
            _isCommandConstructorReferencesVisible = false;
            _paramEmail = new ParamEmail();
        }

        public ParamCommandToPdf(int commandId, EStatusCommand status, int quoteValidityDay)
            : this(commandId, status)
        {
            _quoteValidityDay = quoteValidityDay;
        }

        public ParamCommandToPdf(int commandId, EStatusCommand status, EStatusCommand typeQuoteOrProformat)
            : this(commandId, status)
        {
            _typeQuoteOrProformat = typeQuoteOrProformat;
        }

        public int CommandId
        {
            get { return _commandId; }
            set { _commandId = value; }
        }

        public int BillId
        {
            get { return _billId; }
            set { _billId = value; }
        }

        public EStatusCommand Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int ValidityDay
        {
            get { return _quoteValidityDay; }
            set { _quoteValidityDay = value; }
        }

        public EStatusCommand TypeQuoteOrProformat
        {
            get { return _typeQuoteOrProformat; }
            set { _typeQuoteOrProformat = value; }
        }

        public bool IsQuoteConstructorReferencesVisible
        {
            get { return _isQuoteConstructorReferencesVisible; }
            set { _isQuoteConstructorReferencesVisible = value; }
        }

        public bool IsCommandConstructorReferencesVisible
        {
            get { return _isCommandConstructorReferencesVisible; }
            set { _isCommandConstructorReferencesVisible = value; }
        }

        public ParamEmail ParamEmail
        {
            get { return _paramEmail; }
            set { _paramEmail = value; }
        }

        public string Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }

        public string Lang
        {
            get { return _lang; }
            set { _lang = value; }
        }


    }
}
