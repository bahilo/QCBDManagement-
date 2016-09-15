using QCBDManagementCommon.Entities;
using QCBDManagementDAL.App_Data;
using QCBDManagementDAL.App_Data.QCBDDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace QCBDManagementWPF.Helper
{
    
    public static class WPFHelper
    {
        public static void onUIThread(System.Action action)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
            }
        }


    }
}
