using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.DAC
{
    public interface IItemManager : BL.IItemManager, INotifyPropertyChanged, IDisposable
    {
        void initializeCredential(Agent user);

        void progressBarManagement(Func<double, double> progressBarFunc);
    } /* end interface IItemManager */
}