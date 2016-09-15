using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementGateway.Helper.ChannelHelper;
using QCBDManagementGateway.QCBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementGateway.Core
{
    public class GateWayStatistic : IStatisticManager, INotifyPropertyChanged
    {
        private QCBDManagementWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public GateWayStatistic()
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
                setServiceCredential(value.Login, value.HashedPassword);
                onPropertyChange("Credential");
            }
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            throw new NotImplementedException();
        }

        public void setServiceCredential(string login, string password)
        {
            _channel.Close();
            _channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");
            _channel.ClientCredentials.UserName.UserName = login;
            _channel.ClientCredentials.UserName.Password = password;
        }

        public  async Task<List<Statistic>> InsertStatistic(List<Statistic> statisticList)
        {
            var formatListStatisticToArray = statisticList.StatisticTypeToArray();
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = (await _channel.insert_data_statisticAsync(formatListStatisticToArray)).ArrayTypeToStatistic();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public  async Task<List<Statistic>> UpdateStatistic(List<Statistic> statisticList)
        {
            var formatListStatisticToArray = statisticList.StatisticTypeToArray();
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = (await _channel.update_data_statisticAsync(formatListStatisticToArray)).ArrayTypeToStatistic();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public  async Task<List<Statistic>> DeleteStatistic(List<Statistic> statisticList)
        {
            var formatListStatisticToArray = statisticList.StatisticTypeToArray();
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = (await _channel.delete_data_statisticAsync(formatListStatisticToArray)).ArrayTypeToStatistic();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public  async Task<List<Statistic>> GetStatisticData(int nbLine)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = (await _channel.get_data_statisticAsync(nbLine.ToString())).ArrayTypeToStatistic().OrderBy(x => x.ID).ToList();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException ex)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public  async Task<List<Statistic>> SearchStatisitc(Statistic statistic, string filterOperator)
        {
            var formatListStatisticToArray = statistic.StatisticTypeToFilterArray(filterOperator);
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = (await _channel.get_filter_statisticAsync(formatListStatisticToArray)).ArrayTypeToStatistic();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public  async Task<List<Statistic>> GetStatisticDataById(int id)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = (await _channel.get_data_statistic_by_idAsync(id.ToString())).ArrayTypeToStatistic();
            }
            catch (FaultException)
            {
                Dispose();
                throw;
            }
            catch (CommunicationException)
            {
                _channel.Abort();
                throw;
            }
            catch (TimeoutException)
            {
                _channel.Abort();
            }
            return result;
        }

        public Task<List<Statistic>> searchStatisticFromWebService(Statistic statisitic, string filterOperator)
        {
            return SearchStatisitc(statisitic, filterOperator);
        }

        public void Dispose()
        {
            
        }
    } /* end class BLStatisitc */
}