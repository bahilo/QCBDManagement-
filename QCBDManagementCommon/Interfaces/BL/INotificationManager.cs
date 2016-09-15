// FILE: D:/Just IT Training/BillManagment/Classes//INotificationManager.cs

// In this section you can add your own using directives
// section -64--88-0-12--3914362f:15397d27317:-8000:0000000000000DE7 begin
// section -64--88-0-12--3914362f:15397d27317:-8000:0000000000000DE7 end

using QCBDManagementCommon;
using QCBDManagementCommon.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
/// <summary>
///  An interface defining operations expected of ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementCommon.Interfaces.BL
{
    public interface INotificationManager
    {
        // Operations

        Task<List<Notification>> InsertNotification(List<Notification> notificationList);

        Task<List<Notification>> UpdateNotification(List<Notification> notificationList);

        Task<List<Notification>> DeleteNotification(List<Notification> notificationList);

        Task<List<Notification>> SearchNotification(Notification notification, string filterOperator);

        Task<List<Notification>> GetDataNotification(int nbLine);

        Task<List<Notification>> GetNotificationDataById(int id);
    } /* end interface INotificationManager */
}