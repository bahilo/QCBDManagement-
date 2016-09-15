using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementGateway.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementDAL.Core
{
    public class DALNotification : INotificationManager
    {
        private SqlCommand _sqlCommand;
        public Agent AuthenticatedUser { get; set; }
        private GateWayNotification _gateWayNotification;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private Func<double, double> _rogressBarFunc;

        public DALNotification()
        {
            _gateWayNotification = new GateWayNotification();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
        }

        public void initializeCredential(Agent user)
        {
            AuthenticatedUser = user;
            //_loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
            _gateWayNotification.initializeCredential(user);
        }

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

        public void Dispose()
        {
            _sqlCommand.Connection.Close();
        }
    } /* end class BlNotification */
}