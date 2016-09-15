using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementGateway.QCBDServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using QCBDManagementGateway.Helper.ChannelHelper;
using System.ServiceModel;
using System.Linq;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementGateway.Core
{
    public class GateWayReferential : IReferentialManager, INotifyPropertyChanged
    {
        private QCBDManagementWebServicePortTypeClient _channel;

        public event PropertyChangedEventHandler PropertyChanged;

        public GateWayReferential()
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

        public void setServiceCredential(string login, string password)
        {
            _channel.Close();
            _channel = new QCBDManagementWebServicePortTypeClient("QCBDManagementWebServicePort");
            _channel.ClientCredentials.UserName.UserName = login;
            _channel.ClientCredentials.UserName.Password = password;
        }

        public Infos AuthenticatedUser
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        private void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<List<Infos>> DeleteInfos(List<Infos> listInfos)
        {
            var formatListInfosToArray = ServiceHelper.InfosTypeToArray(listInfos);
            List<Infos> result = new List<Infos>();
            try
            {
                result = (await _channel.delete_data_infosAsync(formatListInfosToArray)).ArrayTypeToInfos();
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
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Infos>> InsertInfos(List<Infos> listInfos)
        {
            var formatListInfosToArray = ServiceHelper.InfosTypeToArray(listInfos);
            List<Infos> result = new List<Infos>();
            try
            {
                result = (await _channel.insert_data_infosAsync(formatListInfosToArray)).ArrayTypeToInfos();
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
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Infos>> UpdateInfos(List<Infos> listInfos)
        {
            var formatListInfosToArray = ServiceHelper.InfosTypeToArray(listInfos);
            List<Infos> result = new List<Infos>();
            try
            {
                result = (await _channel.update_data_infosAsync(formatListInfosToArray)).ArrayTypeToInfos();
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
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Infos>> GetInfosData(int nbLine)
        {
            List<Infos> result = new List<Infos>();
            try
            {
                result = (await _channel.get_data_infosAsync(nbLine.ToString())).ArrayTypeToInfos().OrderBy(x => x.ID).ToList();
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
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Infos>> GetInfosDataById(int id)
        {
            List<Infos> result = new List<Infos>();
            try
            {
                result = (await _channel.get_data_infos_by_idAsync(id.ToString())).ArrayTypeToInfos();
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
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Infos>> searchInfos(Infos Infos, string filterOperator)
        {
            var formatListInfosToArray = ServiceHelper.InfosTypeToFilterArray(Infos, filterOperator);
            List<Infos> result = new List<Infos>();
            try
            {
                result = (await _channel.get_filter_infosAsync(formatListInfosToArray)).ArrayTypeToInfos();
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
            /*finally
            {
                _channel.Close();
            }*/
            return result;
        }

        public async Task<List<Infos>> searchInfosFromWebService(Infos infos, string filterOperator)
        {
            return await searchInfos(infos, filterOperator);
        }

        public void Dispose()
        {
            
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            throw new NotImplementedException();
        }
    } /* end class BlReferential */
}