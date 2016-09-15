using QCBDManagementBusiness;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.ViewModel
{
    public class QuoteDetailViewModel : BindBase
    {
        private BusinessLogic _bl;
        private CommandModel _selectedQuoteModel;

        public QuoteDetailViewModel()
        {
            _selectedQuoteModel = new CommandModel();
        }


        public CommandModel SelectedQuoteModel
        {
            get
            {
                return _selectedQuoteModel;
            }
            set
            {
                setProperty(ref _selectedQuoteModel, value, "SelectedQuoteModel");
            }
        }

        public BusinessLogic Bl
        {
            get
            {
                return _bl;
            }
            set
            {
                setProperty(ref _bl, value, "Bl");
            }
        }



    }
}
