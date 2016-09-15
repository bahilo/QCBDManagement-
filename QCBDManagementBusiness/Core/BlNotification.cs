using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementBusiness.Core
{
    public class BlNotification : INotificationManager
    {
        // Attributes

        public QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DAC;

        public Task<List<Notification>> DeleteNotification(List<Notification> notificationList)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetDataNotification(int nbLine)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> GetNotificationDataById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> InsertNotification(List<Notification> notificationList)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> SearchNotification(Notification notification, string filterOperator)
        {
            throw new NotImplementedException();
        }

        public Task<List<Notification>> UpdateNotification(List<Notification> notificationList)
        {
            throw new NotImplementedException();
        }


        // Operations


    } /* end class BlNotification */
}