using MaterialDesignThemes.Wpf;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementWPF.ViewModel
{
    public class ConfirmationViewModel : INotifyPropertyChanged
    {
        string _message;
        bool _response;
        bool _isDialogOpen;

        public event PropertyChangedEventHandler PropertyChanged;

        public ConfirmationViewModel()
        {
            _message = "";
        }

        public void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string TxtMessage
        {
            get { return _message; }
            set { _message = value; onPropertyChange("TxtMessage"); }
        }

        public bool Response
        {
            get { return _response; }
            set { _response = value; onPropertyChange("Response"); }
        }

        public bool IsDialogOpen
        {
            get { return _isDialogOpen; }
            set { _isDialogOpen = value; onPropertyChange("IsDialogOpen"); }
        }

        public async Task<bool> show(string message)
        {
            IsDialogOpen = false;
            TxtMessage = message;
            object result = await DialogHost.Show(this, "RootDialog");
            if ((result as bool?) != null)
                Response = (bool)result;
            return Response;
        }

        public async void showSearch(string message)
        {
            TxtMessage = message;
            IsDialogOpen = false;
            object result = await DialogHost.Show(new Views.ConfirmationSearchView(), "RootDialog");
        }

        public async Task<bool> show(object viewModel)
        {
            IsDialogOpen = false;
            object result = await DialogHost.Show(viewModel, "RootDialog");
            if ((result as bool?) != null)
                Response = (bool)result;

            return Response;
        }
    }
}
