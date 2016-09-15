using QCBDManagementCommon.Entities;
using System;
namespace QCBDManagementCommon.Interfaces.DAC
{
    public interface IClientManager : BL.IClientManager, IDisposable
    {
        void initializeCredential(Agent user);

        void progressBarManagement(Func<double, double> progressBarFunc);

    } /* end interface IClientManager */
}