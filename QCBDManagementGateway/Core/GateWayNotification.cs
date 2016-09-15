using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementGateway.QCBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementGateway.Core
{
    public class GateWayNotification : INotificationManager, INotifyPropertyChanged
    {
        private QCBDManagementWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public GateWayNotification()
        {
            _channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");// (binding, endPoint);
        }

        public void initializeCredential(Agent user)
        {
            Credential = user;
        }

        public Agent Credential
        {
            set
            {
                _channel.ClientCredentials.UserName.UserName = value.Login;
                _channel.ClientCredentials.UserName.Password = value.HashedPassword;
                onPropertyChange("Credential");
            }
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
            throw new NotImplementedException();
        }
    } /* end class BlNotification */
}