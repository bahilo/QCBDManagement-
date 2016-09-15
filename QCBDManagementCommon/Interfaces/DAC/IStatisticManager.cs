using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.DAC
{
    public interface IStatisticManager: BL.IStatisticManager, IDisposable, INotifyPropertyChanged
    {
        void progressBarManagement(Func<double, double> progressBarFunc);

        void initializeCredential(Agent user);
    } /* end interface IStatisticManager */
}