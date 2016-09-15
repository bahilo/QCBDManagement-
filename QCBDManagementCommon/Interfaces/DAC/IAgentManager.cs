using QCBDManagementCommon.Entities;
using System;
using System.ComponentModel;

namespace QCBDManagementCommon.Interfaces.DAC
{
    public interface IAgentManager : BL.IAgentManager, INotifyPropertyChanged, IDisposable
    {
        void initializeCredential(Agent user);
        
        void progressBarManagement(Func<double, double> progressBarFunc);

    } /* end interface IAgentManager */
}