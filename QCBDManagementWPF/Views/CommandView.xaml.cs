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
    /// Interaction logic for CommandView.xaml
    /// </summary>
    public partial class CommandView : UserControl
    {
        public CommandView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var parent = FindParent.FindChildParent<Window>(this);
            if (parent != null)
            {       
                if( !((MainWindowViewModel)parent.DataContext).IsThroughContext)
                    ((MainWindowViewModel)parent.DataContext).CommandViewModel.loadCommands();
                             
                this.DataContext = parent.DataContext; 
            }
        }
    }
}
