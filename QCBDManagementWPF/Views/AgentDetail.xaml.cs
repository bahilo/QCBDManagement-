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
using System.ComponentModel;
using QCBDManagementWPF.Models;

namespace QCBDManagementWPF.Views
{
    public partial class AgentDetail : UserControl
    {

        public AgentDetail()
        {
            InitializeComponent();
        }
        
        private void AgentDetailWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var parent = FindParent.FindChildParent<Window>(this);
            if (parent != null)
            {
                this.DataContext = (MainWindowViewModel)parent.DataContext;
                pwdBox.Password = ((MainWindowViewModel)this.DataContext).AgentViewModel.AgentDetailViewModel.SelectedAgentModel.TxtHashedPassword;
                pwdBoxVerification.Password = ((MainWindowViewModel)this.DataContext).AgentViewModel.AgentDetailViewModel.SelectedAgentModel.TxtHashedPassword;
                pwdBox.LostFocus += ((MainWindowViewModel)this.DataContext).AgentViewModel.AgentDetailViewModel.onPwdBoxPasswordChange_updateTxtClearPassword;
                pwdBoxVerification.LostFocus += ((MainWindowViewModel)this.DataContext).AgentViewModel.AgentDetailViewModel.onPwdBoxVerificationPasswordChange_updateTxtClearPasswordVerification;
            }
        }
    }
}
