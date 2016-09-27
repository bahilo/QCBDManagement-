using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementDAL.App_Data;
using QCBDManagementDAL.App_Data.QCBDDataSetTableAdapters;
using QCBDManagementDAL.Helper.ChannelHelper;
using QCBDManagementGateway.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class DALStatisitc : IStatisticManager
    {
        public Agent AuthenticatedUser { get; set; }
        private GateWayStatistic _gateWayStatistic;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private int _progressStep;
        private Func<double, double> _rogressBarFunc;
        private object _lock;

        public event PropertyChangedEventHandler PropertyChanged;

        public DALStatisitc()
        {
            _lock = new object();
            _gateWayStatistic = new GateWayStatistic();
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
            _gateWayStatistic.PropertyChanged += onCredentialChange_loadStatisticDataFromWebService;
        }

        public void initializeCredential(Agent user)
        {
            AuthenticatedUser = user;
            //_loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
            _gateWayStatistic.initializeCredential(AuthenticatedUser);
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; onPropertyChange("IsLodingDataFromWebServiceToLocal"); }
        }

        private void onCredentialChange_loadStatisticDataFromWebService(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Credential"))
            {
                DALHelper.doActionAsync(retrieveGateWayData);
                _gateWayStatistic.PropertyChanged -= onCredentialChange_loadStatisticDataFromWebService;
            }
        }

        public void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void retrieveGateWayData()
        {
            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                var statisticList = new NotifyTaskCompletion<List<Statistic>>(_gateWayStatistic.GetStatisticData(_loadSize)).Task.Result;
                if (statisticList.Count > 0)
                {
                    var savedStatisticList = new NotifyTaskCompletion<List<Statistic>>(UpdateStatistic(statisticList)).Task.Result;
                }                
            }
            finally
            {
                lock (_lock)
                {
                    _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
                    IsLodingDataFromWebServiceToLocal = false;
                    Log.write("Statistics loaded!", "TES");
                }
            }
            
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _rogressBarFunc = progressBarFunc;
        }

        public async Task<List<Statistic>> InsertStatistic(List<Statistic> statisticList)
        {
            List<Statistic> result = new List<Statistic>();
            List<Statistic> gateWayResultList = new List<Statistic>();
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
            using (GateWayStatistic gateWayStatistic = new GateWayStatistic())
            {
                gateWayStatistic.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayStatistic.InsertStatistic(statisticList);

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateStatistic(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Statistic>> UpdateStatistic(List<Statistic> statisticList)
        {
            List<Statistic> result = new List<Statistic>();
            List<Statistic> gateWayResultList = new List<Statistic>();
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
            using (GateWayStatistic gateWayStatistic = new GateWayStatistic())
            {
                gateWayStatistic.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayStatistic.UpdateStatistic(statisticList) : statisticList;
                foreach (var statistic in gateWayResultList)
                {
                    int returnResult = _statisticsTableAdapter
                                            .update_data_statistic(
                                                statistic.Bill_date,
                                                statistic.BillId,
                                                statistic.Company,
                                                statistic.Price_purchase_total,
                                                statistic.Total,
                                                statistic.Total_tax_included,
                                                statistic.Income_percent,
                                                statistic.Income,
                                                statistic.Pay_received,
                                                statistic.Date_limit,
                                                statistic.Pay_date,
                                                statistic.Tax_value,
                                                statistic.ID);
                    if (returnResult > 0)
                        result.Add(statistic);
                }
            }
            return result;
        }

        public async Task<List<Statistic>> DeleteStatistic(List<Statistic> statisticList)
        {
            List<Statistic> result = statisticList;
            List<Statistic> gateWayResultList = new List<Statistic>();
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
            using (GateWayStatistic gateWayStatistic = new GateWayStatistic())
            {
                gateWayStatistic.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayStatistic.DeleteStatistic(statisticList);
                if (gateWayResultList.Count == 0)
                    foreach (Statistic statistic in gateWayResultList)
                    {
                        int returnResult = _statisticsTableAdapter.delete_data_statistic(statistic.ID);
                        if (returnResult > 0)
                            result.Remove(statistic);
                    }
            }
            return result;
        }

        public async Task<List<Statistic>> GetStatisticData(int nbLine)
        {
            List<Statistic> result = new List<Statistic>();
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
                //result = (await DALHelper.doActionAsync<QCBDDataSet.statisticsDataTable>(_statisticsTableAdapter.get_data_statistic)).DataTableTypeToStatistic();
                result = _statisticsTableAdapter.get_data_statistic().DataTableTypeToStatistic();
            if (nbLine == 999 || result.Count < nbLine)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Statistic>> GetStatisticDataById(int id)
        {
            using (statisticsTableAdapter _statisticsTableAdapter = new statisticsTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.statisticsDataTable>(_statisticsTableAdapter.get_data_statistic_by_id, id)).DataTableTypeToStatistic();
        }

        public async Task<List<Statistic>> SearchStatisitc(Statistic statistic, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return statistic.StatisticTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Statistic>> searchStatisticFromWebService(Statistic statisitic, string filterOperator)
        {
            List<Statistic> gateWayResultList = new List<Statistic>();
            using (GateWayStatistic gateWayStatistic = new GateWayStatistic())
            {
                gateWayStatistic.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayStatistic.searchStatisticFromWebService(statisitic, filterOperator);
            }
            return gateWayResultList;
        }

        public void Dispose()
        {

        }
    } /* end class BLStatisitc */
}