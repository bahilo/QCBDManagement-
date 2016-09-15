using QCBDManagementBusiness;
using QCBDManagementBusiness.Core;
using QCBDManagementCommon.Entities;
using QCBDManagementDAL.Core;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QCBDManagementWPF.Views
{
    public partial class ClientView : UserControl
    {
        public ClientView()
        {
            InitializeComponent();
        }

        private void ClientWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var parent = FindParent.FindChildParent<Window>(this);
            if (parent != null)
            {
                if (!((MainWindowViewModel)parent.DataContext).IsThroughContext)
                    ((MainWindowViewModel)parent.DataContext).ClientViewModel.loadClients();
                this.DataContext = (MainWindowViewModel)parent.DataContext;
            }
        }
    }
}
