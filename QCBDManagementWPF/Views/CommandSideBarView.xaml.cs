using QCBDManagementWPF.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace QCBDManagementWPF.Views
{
    /// <summary>
    /// Interaction logic for CommandSideBarView.xaml
    /// </summary>
    public partial class CommandSideBarView : UserControl
    {
        public CommandSideBarView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var Parent = FindParent.FindChildParent<Window>(this);
            if (Parent != null)
                this.DataContext = (MainWindowViewModel)Parent.DataContext;
        }
    }
}
