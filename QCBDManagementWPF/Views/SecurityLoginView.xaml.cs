﻿using QCBDManagementWPF.Classes;
using QCBDManagementWPF.ViewModel;
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
    /// Interaction logic for SecurityLoginView.xaml
    /// </summary>
    public partial class SecurityLoginView : UserControl
    {
        public SecurityLoginView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            pwdBox.Password = ((SecurityLoginViewModel)this.DataContext).TxtClearPassword;
            pwdBox.LostFocus += ((SecurityLoginViewModel)this.DataContext).onPwdBoxPasswordChange_updateTxtClearPassword;
         }
    }
}
