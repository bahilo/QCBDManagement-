using MaterialDesignColors.WpfExample.Domain;
using MaterialDesignThemes.Wpf;
using QCBDManagementBusiness;
using QCBDManagementBusiness.Core;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Enum;
using QCBDManagementDAL.Core;
using QCBDManagementGateway;
using QCBDManagementWPF.Classes;
using QCBDManagementWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QCBDManagementWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel mainWindowViewModel;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void load()
        {
            mainWindowViewModel = new MainWindowViewModel();
            mainWindowViewModel.MainWindow = this;
            this.DataContext = mainWindowViewModel;
        }

        public async Task onUIThreadAsync(System.Action action)
        {
            await this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, action);
        }

        public void onUIThreadSync(System.Action action)
        {
            this.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, action);
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            mainWindowViewModel.Dispose();
        }

        private void DialogHost_Loaded(object sender, RoutedEventArgs e)
        {
            load();
        }
    }


}
