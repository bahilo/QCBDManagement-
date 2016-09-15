using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.DAC
{
    public interface INotificationManager : BL.INotificationManager, IDisposable
    {
        //Agent AuthenticatedUser { get; set; }

        void initializeCredential(Agent user);
        
    } /* end interface INotificationManager */
}