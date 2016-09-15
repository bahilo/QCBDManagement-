using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.DAC;
using QCBDManagementDAL.App_Data;
using QCBDManagementDAL.App_Data.QCBDDataSetTableAdapters;
using QCBDManagementDAL.Helper.ChannelHelper;
using QCBDManagementGateway.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Concurrent;
using QCBDManagementCommon.Classes;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementDAL.Core
{
    public class DALReferential : IReferentialManager
    {
        private GateWayReferential _gateWayReferential;
        private bool _isLodingDataFromWebServiceToLocal;
        private int _loadSize;
        private object _lock = new object();
        private int _progressStep;
        private Func<double, double> _rogressBarFunc;

        public event PropertyChangedEventHandler PropertyChanged;

        public Agent AuthenticatedUser { get; set; }


        public DALReferential()
        {
            _loadSize = Convert.ToInt32(ConfigurationManager.AppSettings["load_size"]);
            _progressStep = Convert.ToInt32(ConfigurationManager.AppSettings["progress_step"]);
            _gateWayReferential = new GateWayReferential();
            _gateWayReferential.PropertyChanged += onCredentialChange_loadReferentialDataFromWebService;
        }

        private void onCredentialChange_loadReferentialDataFromWebService(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Credential"))
            {
                DALHelper.doActionAsync(retrieveGateWayDataReferential);
            }
        }

        public bool IsLodingDataFromWebServiceToLocal
        {
            get { return _isLodingDataFromWebServiceToLocal; }
            set { _isLodingDataFromWebServiceToLocal = value; onPropertyChange("IsLodingDataFromWebServiceToLocal"); }
        }

        public void initializeCredential(Agent user)
        {
            if (!string.IsNullOrEmpty(user.Login) && !string.IsNullOrEmpty(user.HashedPassword))
            {
                AuthenticatedUser = user;
                //_loadSize = (AuthenticatedUser.ListSize > 0) ? AuthenticatedUser.ListSize : _loadSize;
                _gateWayReferential.initializeCredential(AuthenticatedUser);
            }
        }

        private void retrieveGateWayDataReferential()
        {
            object _lock = new object();

            lock (_lock) _isLodingDataFromWebServiceToLocal = true;
            try
            {
                ConcurrentBag<Infos> infosList = new ConcurrentBag<Infos>(new NotifyTaskCompletion<List<Infos>>(_gateWayReferential.GetInfosData(_loadSize)).Task.Result);
                List<Infos> savedInfosList = new NotifyTaskCompletion<List<Infos>>(UpdateInfos(infosList.ToList())).Task.Result;
            }
            finally
            {
                lock (_lock)
                {
                    IsLodingDataFromWebServiceToLocal = false;
                    _rogressBarFunc(_rogressBarFunc(0) + 100 / _progressStep);
                    Log.write("Referential loaded!", "TES");
                }
            }
        }

        public void progressBarManagement(Func<double, double> progressBarFunc)
        {
            _rogressBarFunc = progressBarFunc;
        }

        public void onPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<List<Infos>> InsertInfos(List<Infos> listInfos)
        {
            List<Infos> result = new List<Infos>();
            List<Infos> gateWayResultList = new List<Infos>();
            using (infosTableAdapter _infossTableAdapter = new infosTableAdapter())
            using (GateWayReferential gateWayReferential = new GateWayReferential())
            {
                gateWayReferential.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayReferential.InsertInfos(listInfos) : listInfos;

                _isLodingDataFromWebServiceToLocal = true;
                result = await UpdateInfos(gateWayResultList);
                _isLodingDataFromWebServiceToLocal = false;
            }
            return result;
        }

        public async Task<List<Infos>> DeleteInfos(List<Infos> listInfos)
        {
            List<Infos> result = listInfos;
            List<Infos> gateWayResultList = new List<Infos>();
            using (infosTableAdapter _infosTableAdapter = new infosTableAdapter())
            using (GateWayReferential gateWayReferential = new GateWayReferential())
            {
                gateWayReferential.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayReferential.DeleteInfos(listInfos) : listInfos;
                if (gateWayResultList.Count == 0)
                {
                    foreach (Infos infos in listInfos)
                    {
                        int returnValue = _infosTableAdapter.delete_data_infos(infos.ID);
                        if (returnValue > 0)
                            result.Remove(infos);
                    }
                }

            }
            return result;
        }

        public List<Language> DeleteLanguageInfos(List<Language> languageList)
        {
            List<Language> result = languageList;
            using (LanguagesTableAdapter _languagesTableAdapter = new LanguagesTableAdapter())
            {
                foreach (Language lang in languageList)
                {
                    int returnValue = _languagesTableAdapter.delete_data_language(lang.ID);
                    if (returnValue > 0)
                        result.Remove(lang);
                }
            }
            return result;
        }

        public async Task<List<Infos>> UpdateInfos(List<Infos> listInfos)
        {
            List<Infos> result = new List<Infos>();
            List<Infos> gateWayResultList = new List<Infos>();
            using (infosTableAdapter _InfosTableAdapter = new infosTableAdapter())
            using (GateWayReferential gateWayReferential = new GateWayReferential())
            {
                gateWayReferential.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = (!_isLodingDataFromWebServiceToLocal) ? await gateWayReferential.UpdateInfos(listInfos) : listInfos;
                foreach (var Infos in gateWayResultList)
                {
                    int returnValue = _InfosTableAdapter
                            .update_data_infos(
                                Infos.Name,
                                Infos.Value,
                                Infos.ID);

                    if (returnValue > 0)
                        result.Add(Infos);
                }
            }
            return result;
        }

        public List<Language> UpdateLanguageInfos(List<Language> languageList)
        {
            List<Language> result = new List<Language>();
            using (LanguagesTableAdapter _languagesTableAdapter = new LanguagesTableAdapter())
            {
                foreach (var lang in languageList)
                {
                    int returnValue = _languagesTableAdapter
                            .Update_language(
                                    lang.Lang_table,
                                    lang.Table_column,
                                    lang.ColumnId,
                                    lang.Lang,
                                    lang.CultureInfo_name,
                                    lang.CultureInfo_fullName,
                                    lang.Content,
                                    lang.ID);

                    if (returnValue > 0)
                        result.Add(lang);
                }
            }
            return result;
        }

        public async Task<List<Infos>> GetInfosData(int nbLine)
        {
            List<Infos> result = new List<Infos>();
            using (infosTableAdapter _infosTableAdapter = new infosTableAdapter())
                result = (await DALHelper.doActionAsync<QCBDDataSet.infosDataTable>(_infosTableAdapter.get_data_infos)).DataTableTypeToInfos();

            if (nbLine.Equals(999) || result.Count == 0)
                return result;

            return result.GetRange(0, nbLine);
        }

        public async Task<List<Infos>> GetInfosDataById(int id)
        {
            using (infosTableAdapter _infosTableAdapter = new infosTableAdapter())
                return (await DALHelper.doActionAsync<int, QCBDDataSet.infosDataTable>(_infosTableAdapter.get_data_infos_by_id, id)).DataTableTypeToInfos();
        }

        public async Task<List<Infos>> searchInfos(Infos Infos, string filterOperator)
        {
            return await Task.Factory.StartNew(() => { return Infos.InfosTypeToFilterDataTable(filterOperator); });
        }

        public async Task<List<Infos>> searchInfosFromWebService(Infos infos, string filterOperator)
        {
            List<Infos> gateWayResultList = new List<Infos>();
            using (GateWayReferential gateWayReferential = new GateWayReferential())
            {
                gateWayReferential.setServiceCredential(AuthenticatedUser.Login, AuthenticatedUser.HashedPassword);
                gateWayResultList = await gateWayReferential.searchInfosFromWebService(infos, filterOperator);

            }
            return gateWayResultList;
        }

        public Task<List<Language>> searchLanguageInfos(Language language, string filterOperator)
        {
            return Task.Factory.StartNew(() => { return language.langauageTypeToFilterDataTable(filterOperator); });
        }

        public void Dispose()
        {

        }
    } /* end class BlReferential */
}