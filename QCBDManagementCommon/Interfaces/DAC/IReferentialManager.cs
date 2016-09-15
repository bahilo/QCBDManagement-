using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.DAC
{
    public interface IReferentialManager : BL.IReferentialManager, IDisposable, INotifyPropertyChanged
    {
        //Agent AuthenticatedUser { get; set; }

        void initializeCredential(Agent user);

        void progressBarManagement(Func<double, double> progressBarFunc);
    } /* end interface IReferentialManager */
}