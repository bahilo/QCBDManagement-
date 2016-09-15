using QCBDManagementCommon.Entities;
using QCBDManagementDAL.App_Data;
using QCBDManagementDAL.App_Data.QCBDDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using QCBDManagementCommon.Classes;

namespace QCBDManagementDAL.Helper.ChannelHelper
{

    public static class DALHelper
    {

        //====================================================================================
        //=====================================[ Params ]=====================================
        //====================================================================================

        //private static Action _actionToPerform;
        //private static CancellationTokenSource cancelSource = new CancellationTokenSource(120000);
        //private static Random rd = new Random();
        private static string ErrorMessage;
        //private static TaskCompletionSource<object> taskCompletionSource;

        //====================================================================================
        //===============================[ Do/Redo Actions ]==================================
        //====================================================================================

        /*public static async Task<TResult> redoActionAsync<TInput, TResult>(this Func<TInput, TResult> func, TInput param, string message = null)
        {
            CancellationTokenSource cancelSource = new CancellationTokenSource(3000);
            return await Task.Delay(1000, cancelSource.Token).ContinueWith((tsk) =>
            {
                Debug.WriteLine("{0} / param = {1}", message, param);
                return func(param);
            });
        }

        public static async Task<TResult> redoActionAsync<TResult>(this Func<TResult> func, string message = null)
        {
            CancellationTokenSource cancelSource = new CancellationTokenSource(3000);
            return await Task.Delay(1000, cancelSource.Token).ContinueWith((tsk) =>
            {
                Debug.WriteLine(message);
                return func();
            });
        }

        public static Task redoActionAsync(this Action action, string message = null)
        {

            CancellationToken token = cancelSource.Token;
            return Task.Delay(rd.Next(1, 5) * 1000, token).ContinueWith((tsk) =>
            {
                Debug.WriteLine(message);
                doActionAsync(action);
            }, token);
        }*/

        public static Task<TResult> doActionAsync<TInput, TResult>(this Func<TInput, TResult> func, TInput param, [CallerMemberName] string callerName = null) where TResult : new()
        {
            TResult result = new TResult();
            QCBDManagementCommon.Classes.NotifyTaskCompletion<TResult> ntc = new QCBDManagementCommon.Classes.NotifyTaskCompletion<TResult>();
            ntc.PropertyChanged += onActionComplete_checkIfExceptionDetected;
            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            Task.Factory
                    .StartNew(() =>
                    {
                        TResult taskResult = new TResult();
                        try
                        {
                            taskResult = (TResult)func(param);
                            taskCompletionSource.SetResult(taskResult);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = string.Format("Custom [{0}]: One Error occured - {1}", callerName, ex.InnerException.Message);
                            taskCompletionSource.SetException(new Exception(ErrorMessage));
                        }
                        return taskResult;
                    });

            ntc.initializeNewTask(taskCompletionSource.Task);
            return ntc.Task;
         }

        public static Task<TResult> doActionAsync<TResult>(this Func<TResult> func, [CallerMemberName] string callerName = null) where TResult : new()
        {
            TResult result = new TResult();
            QCBDManagementCommon.Classes.NotifyTaskCompletion<TResult> ntc = new QCBDManagementCommon.Classes.NotifyTaskCompletion<TResult>();
            ntc.PropertyChanged += onActionComplete_checkIfExceptionDetected;
            TaskCompletionSource<TResult> taskCompletionSource = new TaskCompletionSource<TResult>();
            Task.Factory
                    .StartNew(() =>
                    {
                        TResult taskResult = new TResult();
                        try
                        {
                            taskResult = (TResult)func();
                            taskCompletionSource.SetResult(taskResult);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = string.Format("Custom [{0}]: One Error occured - {1}", callerName, ex.InnerException.Message);
                            taskCompletionSource.SetException(new Exception(ErrorMessage));
                        }
                        return taskResult;
                    });

            ntc.initializeNewTask(taskCompletionSource.Task);
            return ntc.Task;
         }

        public static void doActionAsync(this System.Action action, [CallerMemberName]string callerName = null)
        {
            QCBDManagementCommon.Classes.NotifyTaskCompletion<object> ntc = new QCBDManagementCommon.Classes.NotifyTaskCompletion<object>();
            ntc.PropertyChanged += onActionComplete_checkIfExceptionDetected;
            TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();

            Task<object>.Factory.StartNew(() =>
            {
                try
                {
                    action();
                    taskCompletionSource.SetResult(null);
                }
                catch (Exception ex)
                {
                    ErrorMessage = string.Format("Custom [{0}]: One Error occured - {1}", callerName, (ex.InnerException != null)? ex.InnerException.Message : ex.Message);
                    taskCompletionSource.SetException(new Exception(ErrorMessage));                    
                }
                return null;
            });

            ntc.initializeNewTask(taskCompletionSource.Task);
        }

        private static void onActionComplete_checkIfExceptionDetected(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsFaulted"))
            {
                Log.write(ErrorMessage, "ERR");
            }
        }


        //====================================================================================
        //===============================[ Sql Commands ]=====================================
        //====================================================================================

        public static DataTable getDataTableFromSqlQuery<T>(string sql) where T : new()
        {
            DataTable table = new DataTable();
            object _lock = new object();
            string _constr = "";
            SqlCommand cmd = new SqlCommand();

            lock (_lock)
            {
                _constr = ConfigurationManager.ConnectionStrings["QCBDManagementDAL.Properties.Settings.QCBDDatabaseConnectionString"].ConnectionString;
                try
                {
                    using (cmd = new SqlCommand(sql, new SqlConnection(_constr)))
                    {
                        if (typeof(T).Equals(typeof(QCBDDataSet.billsDataTable)))
                            table = new QCBDDataSet.billsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.tax_commandsDataTable)))
                            table = new QCBDDataSet.tax_commandsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.taxesDataTable)))
                            table = new QCBDDataSet.taxesDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.deliveriesDataTable)))
                            table = new QCBDDataSet.deliveriesDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.commandsDataTable)))
                            table = new QCBDDataSet.commandsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.command_itemsDataTable)))
                            table = new QCBDDataSet.command_itemsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.clientsDataTable)))
                            table = new QCBDDataSet.clientsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.contactsDataTable)))
                            table = new QCBDDataSet.contactsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.addressesDataTable)))
                            table = new QCBDDataSet.addressesDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.itemsDataTable)))
                            table = new QCBDDataSet.itemsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.item_deliveriesDataTable)))
                            table = new QCBDDataSet.item_deliveriesDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.providersDataTable)))
                            table = new QCBDDataSet.providersDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.provider_itemsDataTable)))
                            table = new QCBDDataSet.provider_itemsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.agentsDataTable)))
                            table = new QCBDDataSet.agentsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.auto_refsDataTable)))
                            table = new QCBDDataSet.auto_refsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.tax_itemsDataTable)))
                            table = new QCBDDataSet.tax_itemsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.actionRecordsDataTable)))
                            table = new QCBDDataSet.actionRecordsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.actionRecordsDataTable)))
                            table = new QCBDDataSet.actionRecordsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.rolesDataTable)))
                            table = new QCBDDataSet.rolesDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.agent_rolesDataTable)))
                            table = new QCBDDataSet.agent_rolesDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.role_actionsDataTable)))
                            table = new QCBDDataSet.role_actionsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.actionsDataTable)))
                            table = new QCBDDataSet.actionsDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.privilegesDataTable)))
                            table = new QCBDDataSet.privilegesDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.infosDataTable)))
                            table = new QCBDDataSet.infosDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.LanguagesDataTable)))
                            table = new QCBDDataSet.LanguagesDataTable();
                        else if (typeof(T).Equals(typeof(QCBDDataSet.statisticsDataTable)))
                            table = new QCBDDataSet.statisticsDataTable();

                        cmd.Connection.Open();
                        cmd.CommandTimeout = 0;
                        table.Load(cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection));

                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    cmd.Connection.Close();
                }
            }
            return table;
        }

        //====================================================================================
        //===============================[ Empty Tables ]======================================
        //====================================================================================

        /*public static void emptyTable<T>() where T : new()
        {
            object _lock = new object();
            var _constr = ConfigurationManager.ConnectionStrings["QCBDManagementDAL.Properties.Settings.QCBDDatabaseConnectionString"].ConnectionString;
            string sql = "TRUNCATE TABLE ";
            if (typeof(T).Equals(typeof(clientsTableAdapter)))
                sql += "clients";
            if (typeof(T).Equals(typeof(contactsTableAdapter)))
                sql += "contacts";
            if (typeof(T).Equals(typeof(addressesTableAdapter)))
                sql += "addresses";
            if (typeof(T).Equals(typeof(commandsTableAdapter)))
                sql += "commands";
            if (typeof(T).Equals(typeof(command_itemsTableAdapter)))
                sql += "command_items";
            if (typeof(T).Equals(typeof(taxesTableAdapter)))
                sql += "taxes";
            if (typeof(T).Equals(typeof(deliveriesTableAdapter)))
                sql += "deliveries";
            if (typeof(T).Equals(typeof(billsTableAdapter)))
                sql += "bills";
            if (typeof(T).Equals(typeof(itemsTableAdapter)))
                sql += "items";
            if (typeof(T).Equals(typeof(provider_itemsTableAdapter)))
                sql += "provider_items";
            if (typeof(T).Equals(typeof(item_deliveriesTableAdapter)))
                sql += "item_deliveries";
            if (typeof(T).Equals(typeof(providersTableAdapter)))
                sql += "providers";
            if (typeof(T).Equals(typeof(tax_commandsTableAdapter)))
                sql += "tax_commands";
            if (typeof(T).Equals(typeof(tax_itemsTableAdapter)))
                sql += "tax_items";

            lock (_lock)
                using (SqlCommand cmd = new SqlCommand(sql, new SqlConnection(_constr)))
                {
                    //cmd.CommandTimeout = 0;
                    cmd.Connection.Open();
                    cmd.ExecuteReader();
                    cmd.Connection.Close();
                }
        }*/
        //====================================================================================
        //===============================[ Auto_ref ]===========================================
        //====================================================================================

        public static List<Auto_ref> DataTableTypeToAuto_ref(this QCBDDataSet.auto_refsDataTable Auto_refDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<Auto_ref> returnList = new List<Auto_ref>();

            foreach (var Auto_refQCBD in Auto_refDataTable)
            {
                Auto_ref Auto_ref = new Auto_ref();
                Auto_ref.ID = Auto_refQCBD.ID;
                Auto_ref.RefId = Auto_refQCBD.RefId;

                lock (_lock) returnList.Add(Auto_ref);
            }

            return returnList;
        }

        public static QCBDDataSet.auto_refsDataTable Auto_refTypeToDataTable(this List<Auto_ref> Auto_refList)
        {
            object _lock = new object();
            QCBDDataSet.auto_refsDataTable returnQCBDDataTable = new QCBDDataSet.auto_refsDataTable();

            foreach (var Auto_ref in Auto_refList)
            {
                QCBDDataSet.auto_refsRow Auto_refQCBD = returnQCBDDataTable.Newauto_refsRow();
                Auto_refQCBD.ID = Auto_ref.ID;
                Auto_refQCBD.RefId = Auto_ref.RefId;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(Auto_refQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(Auto_refQCBD);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        public static List<Auto_ref> Auto_refTypeToFilterDataTable(this Auto_ref Auto_ref, string filterOperator)
        {
            if (Auto_ref != null)
            {
                string baseSqlString = "SELECT * FROM Auto_refs WHERE ";
                string defaultSqlString = "SELECT * FROM Auto_refs WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Auto_ref.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Auto_ref.ID);
                if (Auto_ref.RefId != 0)
                    query = string.Format(query + " {0} RefId LIKE '{1}' ", filterOperator, Auto_ref.RefId);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToAuto_ref((QCBDDataSet.auto_refsDataTable)getDataTableFromSqlQuery<QCBDDataSet.auto_refsDataTable>(baseSqlString));

            }
            return new List<Auto_ref>();
        }

        //====================================================================================
        //===============================[ Statistic ]===========================================
        //====================================================================================

        public static List<Statistic> DataTableTypeToStatistic(this QCBDDataSet.statisticsDataTable StatisticDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<Statistic> returnList = new List<Statistic>();

            foreach (var statisticQCBD in StatisticDataTable)
            {
                Statistic statistic = new Statistic();
                statistic.ID = statisticQCBD.ID;
                statistic.BillId = statisticQCBD.BillId;
                statistic.Bill_date = statisticQCBD.Bill_datetime;
                statistic.Company = statisticQCBD.Company;
                statistic.Date_limit = statisticQCBD.Date_limit;
                statistic.Income = statisticQCBD.Income;
                statistic.Income_percent = statisticQCBD.Income_percent;
                statistic.Pay_date = statisticQCBD.Pay_datetime;
                statistic.Pay_received = statisticQCBD.Pay_received;
                statistic.Price_purchase_total = statisticQCBD.Price_purchase_total;
                statistic.Tax_value = statisticQCBD.Tax_value;
                statistic.Total = statisticQCBD.Total;
                statistic.Total_tax_included = statisticQCBD.Total_tax_included;

                lock (_lock) returnList.Add(statistic);
            }

            return returnList;
        }

        public static QCBDDataSet.statisticsDataTable StatisticTypeToDataTable(this List<Statistic> StatisticList)
        {
            object _lock = new object();
            QCBDDataSet.statisticsDataTable returnQCBDDataTable = new QCBDDataSet.statisticsDataTable();

            foreach (var statistic in StatisticList)
            {
                QCBDDataSet.statisticsRow statisticQCBD = returnQCBDDataTable.NewstatisticsRow();
                statisticQCBD.ID = statistic.ID;
                statisticQCBD.BillId = statistic.BillId;
                statisticQCBD.Bill_datetime = statistic.Bill_date;
                statisticQCBD.Company = statistic.Company;
                statisticQCBD.Date_limit = statistic.Date_limit;
                statisticQCBD.Income = statistic.Income;
                statisticQCBD.Income_percent = statistic.Income_percent;
                statisticQCBD.Pay_datetime = statistic.Pay_date;
                statisticQCBD.Pay_received = statistic.Pay_received;
                statisticQCBD.Price_purchase_total = statistic.Price_purchase_total;
                statisticQCBD.Tax_value = statistic.Tax_value;
                statisticQCBD.Total = statistic.Total;
                statisticQCBD.Total_tax_included = statistic.Total_tax_included;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(statisticQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(statisticQCBD);
                    }
                }


            }

            return returnQCBDDataTable;
        }

        public static List<Statistic> StatisticTypeToFilterDataTable(this Statistic Statistic, string filterOperator)
        {
            if (Statistic != null)
            {
                string baseSqlString = "SELECT * FROM Statistics WHERE ";
                string defaultSqlString = "SELECT * FROM Statistics WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Statistic.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Statistic.ID);
                if (Statistic.BillId != 0)
                    query = string.Format(query + " {0} BillId LIKE '{1}' ", filterOperator, Statistic.BillId);
                if (Statistic.Pay_received != 0)
                    query = string.Format(query + " {0} Pay_received LIKE '{1}' ", filterOperator, Statistic.Pay_received);
                if (Statistic.Price_purchase_total != 0)
                    query = string.Format(query + " {0} Price_purchase_total LIKE '{1}' ", filterOperator, Statistic.Price_purchase_total);
                if (Statistic.Total != 0)
                    query = string.Format(query + " {0} Total LIKE '{1}' ", filterOperator, Statistic.Total);
                if (Statistic.Total_tax_included != 0)
                    query = string.Format(query + " {0} Total_tax_included LIKE '{1}' ", filterOperator, Statistic.Total_tax_included);
                if (Statistic.Tax_value != 0)
                    query = string.Format(query + " {0} Tax_value LIKE '{1}' ", filterOperator, Statistic.Tax_value);
                if (Statistic.Income != 0)
                    query = string.Format(query + " {0} Income LIKE '{1}' ", filterOperator, Statistic.Income);
                if (Statistic.Income_percent != 0)
                    query = string.Format(query + " {0} Income_percent LIKE '{1}' ", filterOperator, Statistic.Income_percent);
                if (string.IsNullOrEmpty(Statistic.Company))
                    query = string.Format(query + " {0} Company LIKE '{1}' ", filterOperator, Statistic.Company);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToStatistic((QCBDDataSet.statisticsDataTable)getDataTableFromSqlQuery<QCBDDataSet.statisticsDataTable>(baseSqlString));

            }
            return new List<Statistic>();
        }

        //====================================================================================
        //===============================[ Infos ]===========================================
        //====================================================================================

        public static List<Infos> DataTableTypeToInfos(this QCBDDataSet.infosDataTable InfosDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<Infos> returnList = new List<Infos>();

            foreach (var InfosQCBD in InfosDataTable)
            {
                Infos Infos = new Infos();
                Infos.ID = InfosQCBD.ID;
                Infos.Name = InfosQCBD.Name;
                Infos.Value = InfosQCBD.Value;

                lock (_lock) returnList.Add(Infos);
            }
            return returnList;
        }

        public static QCBDDataSet.infosDataTable InfosTypeToDataTable(this List<Infos> InfosList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.infosDataTable returnQCBDDataTable = new QCBDDataSet.infosDataTable();

            foreach (var Infos in InfosList)
            {
                QCBDDataSet.infosRow InfosQCBD = returnQCBDDataTable.NewinfosRow();
                InfosQCBD.ID = Infos.ID;
                InfosQCBD.Name = Infos.Name;
                InfosQCBD.Value = Infos.Value;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(InfosQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(InfosQCBD);
                        idList.Add(InfosQCBD.ID);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        public static List<Infos> InfosTypeToFilterDataTable(this Infos Infos, string filterOperator)
        {
            if (Infos != null)
            {
                string baseSqlString = "SELECT * FROM Infos WHERE ";
                string defaultSqlString = "SELECT * FROM Infos WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Infos.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Infos.ID);
                if (!string.IsNullOrEmpty(Infos.Name))
                    query = string.Format(query + " {0} Name LIKE '{1}' ", filterOperator, Infos.Name.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Infos.Value))
                    query = string.Format(query + " {0} Value LIKE '{1}' ", filterOperator, Infos.Value.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToInfos((QCBDDataSet.infosDataTable)getDataTableFromSqlQuery<QCBDDataSet.infosDataTable>(baseSqlString));

            }
            return new List<Infos>();
        }

        //====================================================================================
        //===============================[ Language ]===========================================
        //====================================================================================

        public static List<Language> DataTableTypeToLanguage(this QCBDDataSet.LanguagesDataTable LanguageDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<Language> returnList = new List<Language>();

            foreach (var LanguageQCBD in LanguageDataTable)
            {
                Language Language = new Language();
                Language.ID = LanguageQCBD.ID;
                Language.Lang_table = LanguageQCBD.Lang_table;
                Language.Table_column = LanguageQCBD.Table_column;
                Language.ColumnId = LanguageQCBD.ColumnId;
                Language.Lang = LanguageQCBD.Lang;
                Language.CultureInfo_name = LanguageQCBD.CultureInfo_name;
                Language.CultureInfo_fullName = LanguageQCBD.CultureInfo_fullName;
                Language.Content = LanguageQCBD.Content;

                lock (_lock) returnList.Add(Language);
            }

            return returnList;
        }

        public static QCBDDataSet.LanguagesDataTable LanguageTypeToDataTable(this List<Language> LanguageList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.LanguagesDataTable returnQCBDDataTable = new QCBDDataSet.LanguagesDataTable();

            foreach (var Language in LanguageList)
            {
                QCBDDataSet.LanguagesRow LanguageQCBD = returnQCBDDataTable.NewLanguagesRow();
                LanguageQCBD.ID = Language.ID;
                LanguageQCBD.Lang_table = Language.Lang_table;
                LanguageQCBD.Table_column = Language.Table_column;
                LanguageQCBD.ColumnId = Language.ColumnId;
                LanguageQCBD.Lang = Language.Lang;
                LanguageQCBD.Content = Language.Content;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(LanguageQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(LanguageQCBD);
                        idList.Add(LanguageQCBD.ID);
                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Language> langauageTypeToFilterDataTable(this Language language, string filterOperator)
        {
            if (language != null)
            {
                string baseSqlString = "SELECT * FROM Languages WHERE ";
                string defaultSqlString = "SELECT * FROM Languages WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (language.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, language.ID);
                if (!string.IsNullOrEmpty(language.Lang_table))
                    query = string.Format(query + " {0} Lang_Table LIKE '{1}' ", filterOperator, language.Lang_table);
                if (!string.IsNullOrEmpty(language.Table_column))
                    query = string.Format(query + " {0} Table_column LIKE '{1}' ", filterOperator, language.Table_column);
                if (!string.IsNullOrEmpty(language.ColumnId))
                    query = string.Format(query + " {0} ColumnId LIKE '{1}' ", filterOperator, language.ColumnId);
                if (!string.IsNullOrEmpty(language.Lang))
                    query = string.Format(query + " {0} Lang LIKE '{1}' ", filterOperator, language.Lang);
                if (!string.IsNullOrEmpty(language.Content))
                    query = string.Format(query + " {0} Content LIKE '{1}' ", filterOperator, language.Content);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToLanguage((QCBDDataSet.LanguagesDataTable)getDataTableFromSqlQuery<QCBDDataSet.LanguagesDataTable>(baseSqlString));

            }
            return new List<Language>();
        }


        //====================================================================================
        //===============================[ ActionRecord ]===========================================
        //====================================================================================

        public static List<ActionRecord> DataTableTypeToActionRecord(this QCBDDataSet.actionRecordsDataTable ActionRecordDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<ActionRecord> returnList = new List<ActionRecord>();

            foreach (var ActionRecordQCBD in ActionRecordDataTable)
            {
                ActionRecord ActionRecord = new ActionRecord();
                ActionRecord.ID = ActionRecordQCBD.ID;
                ActionRecord.TargetName = ActionRecordQCBD.TargetName;
                ActionRecord.AgentId = ActionRecordQCBD.AgentId;
                ActionRecord.TargetId = ActionRecordQCBD.TargetId;
                ActionRecord.Action = ActionRecordQCBD.Action;

                lock (_lock) returnList.Add(ActionRecord);
            }

            return returnList;
        }

        public static QCBDDataSet.actionRecordsDataTable ActionRecordTypeToDataTable(this List<ActionRecord> ActionRecordList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.actionRecordsDataTable returnQCBDDataTable = new QCBDDataSet.actionRecordsDataTable();

            foreach (var ActionRecord in ActionRecordList)
            {
                QCBDDataSet.actionRecordsRow ActionRecordQCBD = returnQCBDDataTable.NewactionRecordsRow();
                ActionRecordQCBD.ID = ActionRecord.ID;
                ActionRecordQCBD.TargetName = ActionRecord.TargetName;
                ActionRecordQCBD.AgentId = ActionRecord.AgentId;
                ActionRecordQCBD.TargetId = ActionRecord.TargetId;
                ActionRecordQCBD.Action = ActionRecord.Action;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(ActionRecordQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(ActionRecordQCBD);
                        idList.Add(ActionRecordQCBD.ID);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        public static List<ActionRecord> ActionRecordTypeToFilterDataTable(this ActionRecord ActionRecord, string filterOperator)
        {
            if (ActionRecord != null)
            {
                string baseSqlString = "SELECT * FROM ActionRecords WHERE ";
                string defaultSqlString = "SELECT * FROM ActionRecords WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (ActionRecord.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, ActionRecord.ID);
                if (ActionRecord.AgentId != 0)
                    query = string.Format(query + " {0} AgentId LIKE '{1}' ", filterOperator, ActionRecord.AgentId);
                if (ActionRecord.TargetId != 0)
                    query = string.Format(query + " {0} TargetId LIKE '{1}' ", filterOperator, ActionRecord.TargetId);
                if (!string.IsNullOrEmpty(ActionRecord.TargetName))
                    query = string.Format(query + " {0} TargetName LIKE '{1}' ", filterOperator, ActionRecord.TargetName);
                if (!string.IsNullOrEmpty(ActionRecord.Action))
                    query = string.Format(query + " {0} Action LIKE '{1}' ", filterOperator, ActionRecord.Action);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToActionRecord((QCBDDataSet.actionRecordsDataTable)getDataTableFromSqlQuery<QCBDDataSet.actionRecordsDataTable>(baseSqlString));

            }
            return new List<ActionRecord>();
        }


        //====================================================================================
        //===============================[ Privilege ]===========================================
        //====================================================================================

        public static List<Privilege> DataTableTypeToPrivilege(this QCBDDataSet.privilegesDataTable PrivilegeDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<Privilege> returnList = new List<Privilege>();

            foreach (var PrivilegeQCBD in PrivilegeDataTable)
            {
                Privilege Privilege = new Privilege();
                Privilege.ID = PrivilegeQCBD.ID;
                Privilege.Role_actionId = PrivilegeQCBD.Role_actionId;
                Privilege.IsDelete = (PrivilegeQCBD._Delete == 1) ? true : false;
                Privilege.IsRead = (PrivilegeQCBD._Delete == 1) ? true : false;
                Privilege.IsSendMail = (PrivilegeQCBD.SendEmail == 1) ? true : false;
                Privilege.IsUpdate = (PrivilegeQCBD._Update == 1) ? true : false;
                Privilege.IsWrite = (PrivilegeQCBD._Write == 1) ? true : false;
                Privilege.Date = PrivilegeQCBD.Date;

                lock (_lock) returnList.Add(Privilege);
            }

            return returnList;
        }

        public static QCBDDataSet.privilegesDataTable PrivilegeTypeToDataTable(this List<Privilege> PrivilegeList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.privilegesDataTable returnQCBDDataTable = new QCBDDataSet.privilegesDataTable();

            foreach (var Privilege in PrivilegeList)
            {
                QCBDDataSet.privilegesRow PrivilegeQCBD = returnQCBDDataTable.NewprivilegesRow();
                PrivilegeQCBD.ID = Privilege.ID;
                PrivilegeQCBD.Role_actionId = Privilege.Role_actionId;
                PrivilegeQCBD._Delete = (Privilege.IsDelete) ? 1 : 0;
                PrivilegeQCBD._Read = (Privilege.IsRead) ? 1 : 0;
                PrivilegeQCBD.SendEmail = (Privilege.IsSendMail) ? 1 : 0;
                PrivilegeQCBD._Update = (Privilege.IsUpdate) ? 1 : 0;
                PrivilegeQCBD._Write = (Privilege.IsWrite) ? 1 : 0;
                PrivilegeQCBD.Date = Privilege.Date;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(PrivilegeQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(PrivilegeQCBD);
                        idList.Add(PrivilegeQCBD.ID);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        public static List<Privilege> PrivilegeTypeToFilterDataTable(this Privilege Privilege, string filterOperator)
        {
            if (Privilege != null)
            {
                string baseSqlString = "SELECT * FROM Privileges WHERE ";
                string defaultSqlString = "SELECT * FROM Privileges WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Privilege.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Privilege.ID);
                if (Privilege.Role_actionId != 0)
                    query = string.Format(query + " {0} Role_actionId LIKE '{1}' ", filterOperator, Privilege.Role_actionId);

                query = string.Format(query + " {0} _Write LIKE '{1}' ", filterOperator, ((Privilege.IsWrite) ? 1 : 0));

                query = string.Format(query + " {0} _Read LIKE '{1}' ", filterOperator, ((Privilege.IsRead) ? 1 : 0));

                query = string.Format(query + " {0} _Delete LIKE '{1}' ", filterOperator, ((Privilege.IsDelete) ? 1 : 0));

                query = string.Format(query + " {0} _Update LIKE '{1}' ", filterOperator, ((Privilege.IsUpdate) ? 1 : 0));

                query = string.Format(query + " {0} SendMail LIKE '{1}' ", filterOperator, ((Privilege.IsSendMail) ? 1 : 0));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToPrivilege((QCBDDataSet.privilegesDataTable)getDataTableFromSqlQuery<QCBDDataSet.privilegesDataTable>(baseSqlString));

            }
            return new List<Privilege>();
        }

        //====================================================================================
        //===============================[ Agent ]===========================================
        //====================================================================================

        public static List<Agent> DataTableTypeToAgent(this QCBDDataSet.agentsDataTable agentDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<Agent> returnList = new List<Agent>();

            foreach (var agentQCBD in agentDataTable)
            {
                Agent agent = new Agent();
                agent.ID = agentQCBD.ID;
                agent.FirstName = agentQCBD.FirstName;
                agent.LastName = agentQCBD.LastName;
                agent.Login = agentQCBD.Login;
                agent.HashedPassword = agentQCBD.Password;
                agent.Phone = agentQCBD.Phone;
                agent.Status = agentQCBD.Status;
                agent.Admin = agentQCBD.Admin;
                agent.Email = agentQCBD.Email;
                agent.Fax = agentQCBD.Fax;
                agent.ListSize = agentQCBD.ListSize;

                lock (_lock) returnList.Add(agent);
            }

            return returnList;
        }

        internal static object NotificationTypeToDataTable(this object listNotification)
        {
            throw new NotImplementedException();
        }

        public static QCBDDataSet.agentsDataTable AgentTypeToDataTable(this List<Agent> agentList, QCBDDataSet.agentsDataTable returnQCBDDataTable)
        {
            object _lock = new object();
            bool isUpdate;
            //QCBDDataSet.agentsDataTable returnQCBDDataTable = new QCBDDataSet.agentsDataTable();
            returnQCBDDataTable.AcceptChanges();
            foreach (var agent in agentList)
            {
                QCBDDataSet.agentsRow agentQCBD;
                if (returnQCBDDataTable.FindByID(agent.ID) == null)
                {
                    agentQCBD = returnQCBDDataTable.NewagentsRow();
                    returnQCBDDataTable.IDColumn.AutoIncrement = false;
                    agentQCBD.ID = agent.ID;
                    isUpdate = false;
                }
                else
                {
                    agentQCBD = returnQCBDDataTable.FindByID(agent.ID);
                    isUpdate = true;
                }

                //Debug.WriteLine("before change row state: "+agentQCBD.RowState);

                agentQCBD.FirstName = agent.FirstName;
                agentQCBD.LastName = agent.LastName;
                agentQCBD.Login = agent.Login;
                agentQCBD.Password = agent.HashedPassword;
                agentQCBD.Phone = agent.Phone;
                agentQCBD.Status = agent.Status;
                agentQCBD.Admin = agent.Admin;
                agentQCBD.Email = agent.Email;
                agentQCBD.Fax = agent.Fax;
                agentQCBD.ListSize = agent.ListSize;

                lock (_lock)
                {
                    //var agentFoundList = AgentTypeToFilterDataTable(new Agent { ID = agentQCBD.ID }, "AND");
                    if (!returnQCBDDataTable.Rows.Contains(agentQCBD))
                        returnQCBDDataTable.Rows.Add(agentQCBD);
                    //if (!returnQCBDDataTable.Rows.Contains(agentQCBD.ID))
                    //{
                    //returnQCBDDataTable.Rows.Add(agentQCBD);
                    /*if(agentFoundList.Count() > 0)
                    {
                        agentQCBD.AcceptChanges();
                        agentQCBD.SetModified();
                    }*/

                    //Debug.WriteLine("After change row state: " + agentQCBD.RowState);
                    //}
                }
            }
            returnQCBDDataTable.IDColumn.AutoIncrement = true;
            return returnQCBDDataTable;
        }

        public static List<Agent> AgentTypeToFilterDataTable(this Agent agent, string filterOperator)
        {
            if (agent != null)
            {
                string baseSqlString = "SELECT * FROM Agents WHERE ";
                string defaultSqlString = "SELECT * FROM Agents WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (agent.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, agent.ID);
                if (agent.ListSize != 0)
                    query = string.Format(query + " {0} ListSize LIKE '{1}' ", filterOperator, agent.ListSize);
                if (!string.IsNullOrEmpty(agent.LastName))
                    query = string.Format(query + " {0} LastName LIKE '{1}' ", filterOperator, agent.LastName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.FirstName))
                    query = string.Format(query + " {0} FirstName LIKE '{1}' ", filterOperator, agent.FirstName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Phone))
                    query = string.Format(query + " {0} Phone LIKE '{1}' ", filterOperator, agent.Phone.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Fax))
                    query = string.Format(query + " {0} Fax LIKE '{1}' ", filterOperator, agent.Fax.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Email))
                    query = string.Format(query + " {0} Email LIKE '{1}' ", filterOperator, agent.Email.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Login))
                    query = string.Format(query + " {0} Login LIKE '{1}' ", filterOperator, agent.Login.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.HashedPassword))
                    query = string.Format(query + " {0} Password LIKE '{1}' ", filterOperator, agent.HashedPassword.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Admin))
                    query = string.Format(query + " {0} Admin LIKE '{1}' ", filterOperator, agent.Admin.Replace("'", "''"));
                if (!string.IsNullOrEmpty(agent.Status))
                    query = string.Format(query + " {0} Status LIKE '{1}' ", filterOperator, agent.Status.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToAgent((QCBDDataSet.agentsDataTable)DALHelper.getDataTableFromSqlQuery<QCBDDataSet.agentsDataTable>(baseSqlString));

            }
            return new List<Agent>();
        }
        //====================================================================================
        //===============================[ Role ]===========================================
        //====================================================================================

        public static List<Role> DataTableTypeToRole(this QCBDDataSet.rolesDataTable RoleDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<Role> returnList = new List<Role>();

            foreach (var RoleQCBD in RoleDataTable)
            {
                Role Role = new Role();
                Role.ID = RoleQCBD.ID;
                Role.Name = RoleQCBD.Name;

                lock (_lock) returnList.Add(Role);
            }

            return returnList;
        }

        public static QCBDDataSet.rolesDataTable RoleTypeToDataTable(this List<Role> RoleList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.rolesDataTable returnQCBDDataTable = new QCBDDataSet.rolesDataTable();

            foreach (var Role in RoleList)
            {
                QCBDDataSet.rolesRow RoleQCBD = returnQCBDDataTable.NewrolesRow();
                RoleQCBD.ID = Role.ID;
                RoleQCBD.Name = Role.Name;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(RoleQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(RoleQCBD);
                        idList.Add(RoleQCBD.ID);
                    }
                }


            }

            return returnQCBDDataTable;
        }

        public static List<Role> RoleTypeToFilterDataTable(this Role Role, string filterOperator)
        {
            if (Role != null)
            {
                string baseSqlString = "SELECT * FROM Roles WHERE ";
                string defaultSqlString = "SELECT * FROM Roles WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Role.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Role.ID);
                if (string.IsNullOrEmpty(Role.Name))
                    query = string.Format(query + " {0} Name LIKE '{1}' ", filterOperator, Role.Name.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToRole((QCBDDataSet.rolesDataTable)getDataTableFromSqlQuery<QCBDDataSet.rolesDataTable>(baseSqlString));

            }
            return new List<Role>();
        }

        //====================================================================================
        //===============================[ Action ]===========================================
        //====================================================================================

        public static List<QCBDManagementCommon.Entities.Action> DataTableTypeToAction(this QCBDDataSet.actionsDataTable ActionDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<QCBDManagementCommon.Entities.Action> returnList = new List<QCBDManagementCommon.Entities.Action>();

            foreach (var ActionQCBD in ActionDataTable)
            {
                QCBDManagementCommon.Entities.Action Action = new QCBDManagementCommon.Entities.Action();
                Action.ID = ActionQCBD.ID;
                Action.Name = ActionQCBD.Name;

                lock (_lock) returnList.Add(Action);
            }

            return returnList;
        }

        public static QCBDDataSet.actionsDataTable ActionTypeToDataTable(this List<QCBDManagementCommon.Entities.Action> ActionList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.actionsDataTable returnQCBDDataTable = new QCBDDataSet.actionsDataTable();

            foreach (var Action in ActionList)
            {
                QCBDDataSet.actionsRow ActionQCBD = returnQCBDDataTable.NewactionsRow();
                ActionQCBD.ID = Action.ID;
                ActionQCBD.Name = Action.Name;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(ActionQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(ActionQCBD);
                        idList.Add(ActionQCBD.ID);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        public static List<QCBDManagementCommon.Entities.Action> ActionTypeToFilterDataTable(this QCBDManagementCommon.Entities.Action Action, string filterOperator)
        {
            if (Action != null)
            {
                string baseSqlString = "SELECT * FROM Actions WHERE ";
                string defaultSqlString = "SELECT * FROM Actions WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Action.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Action.ID);
                if (!string.IsNullOrEmpty(Action.Name))
                    query = string.Format(query + " {0} Name LIKE '{1}' ", filterOperator, Action.Name.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToAction((QCBDDataSet.actionsDataTable)getDataTableFromSqlQuery<QCBDDataSet.actionsDataTable>(baseSqlString));

            }
            return new List<QCBDManagementCommon.Entities.Action>();
        }

        //====================================================================================
        //===============================[ Agent_role ]===========================================
        //====================================================================================

        public static List<Agent_role> DataTableTypeToAgent_role(this QCBDDataSet.agent_rolesDataTable Agent_roleDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<Agent_role> returnList = new List<Agent_role>();

            foreach (var Agent_roleQCBD in Agent_roleDataTable)
            {
                Agent_role Agent_role = new Agent_role();
                Agent_role.ID = Agent_roleQCBD.ID;
                Agent_role.RoleId = Agent_roleQCBD.RoleId;
                Agent_role.AgentId = Agent_roleQCBD.AgentId;

                lock (_lock) returnList.Add(Agent_role);
            }

            return returnList;
        }

        public static QCBDDataSet.agent_rolesDataTable Agent_roleTypeToDataTable(this List<Agent_role> Agent_roleList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.agent_rolesDataTable returnQCBDDataTable = new QCBDDataSet.agent_rolesDataTable();

            foreach (var Agent_role in Agent_roleList)
            {
                QCBDDataSet.agent_rolesRow Agent_roleQCBD = returnQCBDDataTable.Newagent_rolesRow();
                Agent_roleQCBD.ID = Agent_role.ID;
                Agent_roleQCBD.RoleId = Agent_role.RoleId;
                Agent_roleQCBD.AgentId = Agent_role.AgentId;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(Agent_roleQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(Agent_roleQCBD);
                        idList.Add(Agent_roleQCBD.ID);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        public static List<Agent_role> Agent_roleTypeToFilterDataTable(this Agent_role Agent_role, string filterOperator)
        {
            if (Agent_role != null)
            {
                string baseSqlString = "SELECT * FROM Agent_roles WHERE ";
                string defaultSqlString = "SELECT * FROM Agent_roles WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Agent_role.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Agent_role.ID);
                if (Agent_role.RoleId != 0)
                    query = string.Format(query + " {0} RoleId LIKE '{1}' ", filterOperator, Agent_role.RoleId);
                if (Agent_role.AgentId != 0)
                    query = string.Format(query + " {0} AgentId LIKE '{1}' ", filterOperator, Agent_role.AgentId);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToAgent_role((QCBDDataSet.agent_rolesDataTable)getDataTableFromSqlQuery<QCBDDataSet.agent_rolesDataTable>(baseSqlString));

            }
            return new List<Agent_role>();
        }

        //====================================================================================
        //===============================[ Role_action ]===========================================
        //====================================================================================

        public static List<Role_action> DataTableTypeToRole_action(this QCBDDataSet.role_actionsDataTable Role_actionDataTable)
        {
            //private QCBDDataSet QCBDDataSet;
            object _lock = new object(); List<Role_action> returnList = new List<Role_action>();

            foreach (var Role_actionQCBD in Role_actionDataTable)
            {
                Role_action Role_action = new Role_action();
                Role_action.RoleId = Role_actionQCBD.RoleId;
                Role_action.ActionId = Role_actionQCBD.ActionId;

                lock (_lock) returnList.Add(Role_action);
            }

            return returnList;
        }

        public static QCBDDataSet.role_actionsDataTable Role_actionTypeToDataTable(this List<Role_action> Role_actionList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.role_actionsDataTable returnQCBDDataTable = new QCBDDataSet.role_actionsDataTable();

            foreach (var Role_action in Role_actionList)
            {
                QCBDDataSet.role_actionsRow Role_actionQCBD = returnQCBDDataTable.Newrole_actionsRow();
                Role_actionQCBD.ID = Role_action.ID;
                Role_actionQCBD.RoleId = Role_action.RoleId;
                Role_actionQCBD.ActionId = Role_action.ActionId;

                lock (_lock)
                {
                    if (!returnQCBDDataTable.Rows.Contains(Role_actionQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(Role_actionQCBD);
                        idList.Add(Role_actionQCBD.ID);
                    }
                }


            }

            return returnQCBDDataTable;
        }

        public static List<Role_action> Role_actionTypeToFilterDataTable(this Role_action Role_action, string filterOperator)
        {
            if (Role_action != null)
            {
                string baseSqlString = "SELECT * FROM Role_actions WHERE ";
                string defaultSqlString = "SELECT * FROM Role_actions WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Role_action.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Role_action.ID);
                if (Role_action.ActionId != 0)
                    query = string.Format(query + " {0} ActionId LIKE '{1}' ", filterOperator, Role_action.ActionId);
                if (Role_action.RoleId != 0)
                    query = string.Format(query + " {0} RoleId LIKE '{1}' ", filterOperator, Role_action.RoleId);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToRole_action((QCBDDataSet.role_actionsDataTable)getDataTableFromSqlQuery<QCBDDataSet.role_actionsDataTable>(baseSqlString));

            }
            return new List<Role_action>();
        }


        //====================================================================================
        //===============================[ Command ]===========================================
        //====================================================================================

        public static List<Command> DataTableTypeToCommand(this QCBDDataSet.commandsDataTable CommandDataTable)
        {
            object _lock = new object(); List<Command> returnList = new List<Command>();

            //foreach (var CommandQCBD in CommandDataTable)
            Parallel.ForEach(CommandDataTable, (CommandQCBD) =>
            {
                Command Command = new Command();
                Command.ID = CommandQCBD.ID;
                Command.AgentId = CommandQCBD.AgentId;
                Command.BillAddress = CommandQCBD.BillAddress;
                Command.ClientId = CommandQCBD.ClientId;
                Command.Comment1 = CommandQCBD.Comment1;
                Command.Comment2 = CommandQCBD.Comment2;
                Command.Comment3 = CommandQCBD.Comment3;
                Command.Status = CommandQCBD.Status;
                Command.Date = CommandQCBD.Date;
                Command.DeliveryAddress = CommandQCBD.DeliveryAddress;
                Command.Tax = CommandQCBD.Tax;

                lock (_lock) returnList.Add(Command);
            });

            return returnList;
        }

        public static QCBDDataSet.commandsDataTable CommandTypeToDataTable(this List<Command> CommandList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.commandsDataTable returnQCBDDataTable = new QCBDDataSet.commandsDataTable();
            if (CommandList != null)
            {
                foreach (var Command in CommandList)
                {
                    QCBDDataSet.commandsRow CommandQCBD = returnQCBDDataTable.NewcommandsRow();
                    CommandQCBD.ID = Command.ID;
                    CommandQCBD.AgentId = Command.AgentId;
                    CommandQCBD.BillAddress = Command.BillAddress;
                    CommandQCBD.ClientId = Command.ClientId;
                    CommandQCBD.Comment1 = Command.Comment1;
                    CommandQCBD.Comment2 = Command.Comment2;
                    CommandQCBD.Comment3 = Command.Comment3;
                    CommandQCBD.Status = Command.Status;
                    CommandQCBD.Date = Command.Date;
                    CommandQCBD.DeliveryAddress = Command.DeliveryAddress;
                    CommandQCBD.Tax = Command.Tax;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(CommandQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(CommandQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(CommandQCBD);
                            idList.Add(CommandQCBD.ID);
                        }
                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Command> CommandTypeToFilterDataTable(this Command command, string filterOperator)
        {
            string baseSqlString = "SELECT * FROM Commands WHERE ";
            string defaultSqlString = "SELECT * FROM Commands WHERE 1=0 ";
            string orderBy = " ORDER BY ID DESC";
            object _lock = new object(); string query = "";

            if (command != null)
            {
                if (command.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, command.ID);
                if (command.AgentId != 0)
                    query = string.Format(query + " {0} AgentId LIKE '{1}' ", filterOperator, command.AgentId);
                if (!string.IsNullOrEmpty(command.Comment1))
                    query = string.Format(query + " {0} Comment1 LIKE '{1}' ", filterOperator, command.Comment1.Replace("'", "''"));
                if (!string.IsNullOrEmpty(command.Comment2))
                    query = string.Format(query + " {0} Comment2 LIKE '{1}' ", filterOperator, command.Comment2.Replace("'", "''"));
                if (!string.IsNullOrEmpty(command.Comment3))
                    query = string.Format(query + " {0} Comment3 LIKE '{1}' ", filterOperator, command.Comment3.Replace("'", "''"));
                if (!string.IsNullOrEmpty(command.Status))
                    query = string.Format(query + " {0} Status LIKE '{1}' ", filterOperator, command.Status.Replace("'", "''"));
                /*if (command.Date != null)
                    query = string.Format(query + " {0} Date LIKE '{1}' ", filterOperator, command.Date);*/
                if (command.Tax != 0)
                    query = string.Format(query + " {0} Tax LIKE '{1}' ", filterOperator, command.Tax);
                if (command.ClientId != 0)
                    query = string.Format(query + " {0} ClientId LIKE '{1}' ", filterOperator, command.ClientId);
                if (command.BillAddress != 0)
                    query = string.Format(query + " {0} BillAddress LIKE '{1}' ", filterOperator, command.BillAddress);
                if (command.DeliveryAddress != 0)
                    query = string.Format(query + " {0} DeliveryAddress LIKE '{1}' ", filterOperator, command.DeliveryAddress);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length) + orderBy;
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToCommand((QCBDDataSet.commandsDataTable)getDataTableFromSqlQuery<QCBDDataSet.commandsDataTable>(baseSqlString));
            }
            return new List<Command>();
        }


        //====================================================================================
        //===============================[ Tax_command ]======================================
        //====================================================================================

        public static List<Tax_command> DataTableTypeToTax_command(this QCBDDataSet.tax_commandsDataTable Tax_CommandDataTableList)
        {
            object _lock = new object(); List<Tax_command> returnList = new List<Tax_command>();

            foreach (var Tax_commandQCBD in Tax_CommandDataTableList)
            {
                Tax_command Tax_command = new Tax_command();
                Tax_command.ID = Tax_commandQCBD.ID;
                Tax_command.CommandId = Tax_commandQCBD.CommandId;
                Tax_command.Date_insert = Tax_commandQCBD.Date_insert;
                Tax_command.Target = Tax_commandQCBD.Target;
                Tax_command.Tax_value = Tax_commandQCBD.Tax_value;
                Tax_command.TaxId = Tax_commandQCBD.TaxId;

                lock (_lock) returnList.Add(Tax_command);
            }

            return returnList;
        }

        public static QCBDDataSet.tax_commandsDataTable Tax_commandTypeToDataTable(this List<Tax_command> Tax_commandList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.tax_commandsDataTable returnQCBDDataTable = new QCBDDataSet.tax_commandsDataTable();

            foreach (var Tax_command in Tax_commandList)
            {
                QCBDDataSet.tax_commandsRow Tax_commandQCBD = returnQCBDDataTable.Newtax_commandsRow();
                Tax_commandQCBD.ID = Tax_command.ID;
                Tax_commandQCBD.CommandId = Tax_command.CommandId;
                Tax_commandQCBD.Date_insert = Tax_command.Date_insert;
                Tax_commandQCBD.Target = Tax_command.Target;
                Tax_commandQCBD.Tax_value = Tax_command.Tax_value;
                Tax_commandQCBD.TaxId = Tax_command.TaxId;

                //lock(_lock) returnQCBDDataTable.Rows.Add(Tax_commandQCBD);
                lock (_lock)
                {
                    if (!idList.Contains(Tax_commandQCBD.ID))
                    {
                        returnQCBDDataTable.Rows.Add(Tax_commandQCBD);
                        idList.Add(Tax_commandQCBD.ID);
                    }
                }
            }

            return returnQCBDDataTable;
        }

        public static List<Tax_command> Tax_commandTypeToFilterDataTable(this Tax_command Tax_command, string filterOperator)
        {
            string baseSqlString = "SELECT * FROM Tax_commands WHERE ";
            string defaultSqlString = "SELECT * FROM Tax_commands WHERE 1=0 ";
            object _lock = new object(); string query = "";

            if (Tax_command != null)
            {
                if (Tax_command.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Tax_command.ID);
                if (Tax_command.CommandId != 0)
                    query = string.Format(query + " {0} CommandId LIKE '{1}' ", filterOperator, Tax_command.CommandId);
                if (Tax_command.TaxId != 0)
                    query = string.Format(query + " {0} TaxId LIKE '{1}' ", filterOperator, Tax_command.TaxId);
                if (Tax_command.Tax_value != 0)
                    query = string.Format(query + " {0} Tax_value LIKE '{1}' ", filterOperator, Tax_command.Tax_value);
                if (!string.IsNullOrEmpty(Tax_command.Target))
                    query = string.Format(query + " {0} Target LIKE '{1}' ", filterOperator, Tax_command.Target);
                /*if (Tax_command.Date_insert != null)
                    query = string.Format(query + " {0} Date_insert LIKE '{1}' ", filterOperator, Tax_command.Date_insert);*/

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToTax_command((QCBDDataSet.tax_commandsDataTable)getDataTableFromSqlQuery<QCBDDataSet.tax_commandsDataTable>(baseSqlString));

            }

            return new List<Tax_command>();
        }

        //====================================================================================
        //===============================[ Client ]===========================================
        //====================================================================================

        public static List<Client> DataTableTypeToClient(this QCBDDataSet.clientsDataTable ClientDataTable)
        {
            object _lock = new object(); List<Client> returnList = new List<Client>();
            if (ClientDataTable != null)
            {
                //foreach (var ClientQCBD in ClientDataTable)
                Parallel.ForEach(ClientDataTable, (ClientQCBD) =>
                {
                    Client Client = new Client();
                    Client.ID = ClientQCBD.ID;
                    Client.FirstName = ClientQCBD.FirstName;
                    Client.LastName = ClientQCBD.LastName;
                    Client.AgentId = ClientQCBD.AgentId;
                    Client.Comment = ClientQCBD.Comment;
                    Client.Phone = ClientQCBD.Phone;
                    Client.Status = ClientQCBD.Status;
                    Client.Company = ClientQCBD.Company;
                    Client.Email = ClientQCBD.Email;
                    Client.Fax = ClientQCBD.Fax;
                    Client.CompanyName = ClientQCBD.CompanyName;
                    Client.CRN = ClientQCBD.CRN;
                    Client.MaxCredit = ClientQCBD.MaxCredit;
                    Client.Rib = ClientQCBD.Rib;
                    Client.PayDelay = ClientQCBD.PayDelay;

                    lock (_lock) returnList.Add(Client);
                });
            }
            return returnList;
        }

        public static QCBDDataSet.clientsDataTable ClientTypeToDataTable(this List<Client> ClientList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.clientsDataTable returnQCBDDataTable = new QCBDDataSet.clientsDataTable();
            if (ClientList != null)
            {
                foreach (var Client in ClientList)
                {
                    QCBDDataSet.clientsRow ClientQCBD = returnQCBDDataTable.NewclientsRow();
                    ClientQCBD.ID = Client.ID;
                    ClientQCBD.FirstName = Client.FirstName;
                    ClientQCBD.LastName = Client.LastName;
                    ClientQCBD.AgentId = Client.AgentId;
                    ClientQCBD.Comment = Client.Comment;
                    ClientQCBD.Phone = Client.Phone;
                    ClientQCBD.Status = Client.Status;
                    ClientQCBD.Company = Client.Company;
                    ClientQCBD.Email = Client.Email;
                    ClientQCBD.Fax = Client.Fax;
                    ClientQCBD.CompanyName = Client.CompanyName;
                    ClientQCBD.CRN = Client.CRN;
                    ClientQCBD.MaxCredit = Client.MaxCredit;
                    ClientQCBD.Rib = Client.Rib;
                    ClientQCBD.PayDelay = Client.PayDelay;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(ClientQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(ClientQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(ClientQCBD);
                            idList.Add(ClientQCBD.ID);
                        }
                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Client> ClientTypeToFilterDataTable(this Client client, string filterOperator)
        {
            if (client != null)
            {
                string baseSqlString = "SELECT * FROM Clients WHERE ";
                string defaultSqlString = "SELECT * FROM Clients WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (client.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, client.ID);
                if (client.AgentId != 0)
                    query = string.Format(query + " {0} AgentId LIKE '{1}' ", filterOperator, client.AgentId);
                if (!string.IsNullOrEmpty(client.FirstName))
                    query = string.Format(query + " {0} LastName LIKE '%{1}%' ", filterOperator, client.LastName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.LastName))
                    query = string.Format(query + " {0} FirstName LIKE '%{1}%' ", filterOperator, client.FirstName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Company))
                    query = string.Format(query + " {0} Company LIKE '%{1}%' ", filterOperator, client.Company.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Email))
                    query = string.Format(query + " {0} Email LIKE '%{1}%' ", filterOperator, client.Email.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Phone))
                    query = string.Format(query + " {0} Phone LIKE '{1}' ", filterOperator, client.Phone.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Fax))
                    query = string.Format(query + " {0} Fax LIKE '{1}' ", filterOperator, client.Fax.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Rib))
                    query = string.Format(query + " {0} Rib LIKE '{1}' ", filterOperator, client.Rib.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Rib))
                    query = string.Format(query + " {0} Rib LIKE '{1}' ", filterOperator, client.Rib.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.CRN))
                    query = string.Format(query + " {0} CRN LIKE '%{1}%' ", filterOperator, client.CRN.Replace("'", "''"));
                if (client.PayDelay > 0)
                    query = string.Format(query + " {0} PayDelay LIKE '{1}' ", filterOperator, client.PayDelay);
                if (!string.IsNullOrEmpty(client.Comment))
                    query = string.Format(query + " {0} Comment LIKE '%{1}%' ", filterOperator, client.Comment.Replace("'", "''"));
                if (!string.IsNullOrEmpty(client.Status))
                    query = string.Format(query + " {0} Status LIKE '{1}' ", filterOperator, client.Status.Replace("'", "''"));
                if (client.MaxCredit > 0)
                    query = string.Format(query + " {0} MaxCredit LIKE '{1}' ", filterOperator, client.MaxCredit);
                if (!string.IsNullOrEmpty(client.CompanyName))
                    query = string.Format(query + " {0} CompanyName LIKE '{1}' ", filterOperator, client.CompanyName.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToClient((QCBDDataSet.clientsDataTable)getDataTableFromSqlQuery<QCBDDataSet.clientsDataTable>(baseSqlString));

            }
            return new List<Client>();
        }

        //====================================================================================
        //===============================[ Contact ]===========================================
        //====================================================================================

        public static List<Contact> DataTableTypeToContact(this QCBDDataSet.contactsDataTable ContactDataTable)
        {
            object _lock = new object(); List<Contact> returnList = new List<Contact>();
            if (ContactDataTable != null)
            {
                foreach (var ContactQCBD in ContactDataTable)
                {
                    Contact Contact = new Contact();
                    Contact.ID = ContactQCBD.ID;
                    Contact.Cellphone = ContactQCBD.Cellphone;
                    Contact.ClientId = ContactQCBD.ClientId;
                    Contact.Comment = ContactQCBD.Comment;
                    Contact.Email = ContactQCBD.Email;
                    Contact.Phone = ContactQCBD.Phone;
                    Contact.Fax = ContactQCBD.Fax;
                    Contact.Firstname = ContactQCBD.Firstname;
                    Contact.LastName = ContactQCBD.LastName;
                    Contact.Position = ContactQCBD.Position;

                    lock (_lock) returnList.Add(Contact);
                }
            }
            return returnList;
        }

        public static QCBDDataSet.contactsDataTable ContactTypeToDataTable(this List<Contact> ContactList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.contactsDataTable returnQCBDDataTable = new QCBDDataSet.contactsDataTable();
            if (ContactList != null)
            {
                foreach (var Contact in ContactList)
                {
                    QCBDDataSet.contactsRow ContactQCBD = returnQCBDDataTable.NewcontactsRow();
                    ContactQCBD.ID = Contact.ID;
                    ContactQCBD.Position = Contact.Position;
                    ContactQCBD.LastName = Contact.LastName;
                    ContactQCBD.Firstname = Contact.Firstname;
                    ContactQCBD.Comment = Contact.Comment;
                    ContactQCBD.Phone = Contact.Phone;
                    ContactQCBD.ClientId = Contact.ClientId;
                    ContactQCBD.Cellphone = Contact.Cellphone;
                    ContactQCBD.Email = Contact.Email;
                    ContactQCBD.Fax = Contact.Fax;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(ContactQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(ContactQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(ContactQCBD);
                            idList.Add(ContactQCBD.ID);
                        }
                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Contact> ContactTypeToFilterDataTable(this Contact Contact, string filterOperator)
        {
            if (Contact != null)
            {
                string baseSqlString = "SELECT * FROM Contacts WHERE ";
                string defaultSqlString = "SELECT * FROM Contacts WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Contact.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Contact.ID);
                if (Contact.ClientId != 0)
                    query = string.Format(query + " {0} ClientId LIKE '{1}' ", filterOperator, Contact.ClientId);
                if (!string.IsNullOrEmpty(Contact.Firstname))
                    query = string.Format(query + " {0} Firstname LIKE '{1}' ", filterOperator, Contact.Firstname.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.LastName))
                    query = string.Format(query + " {0} LastName LIKE '{1}' ", filterOperator, Contact.LastName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Position))
                    query = string.Format(query + " {0} Position LIKE '{1}' ", filterOperator, Contact.Position.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Email))
                    query = string.Format(query + " {0} Email LIKE '{1}' ", filterOperator, Contact.Email.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Phone))
                    query = string.Format(query + " {0} Phone LIKE '{1}' ", filterOperator, Contact.Phone.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Cellphone))
                    query = string.Format(query + " {0} Cellphone LIKE '{1}' ", filterOperator, Contact.Cellphone.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Fax))
                    query = string.Format(query + " {0} Fax LIKE '{1}' ", filterOperator, Contact.Fax.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Contact.Comment))
                    query = string.Format(query + " {0} Comment LIKE '{1}' ", filterOperator, Contact.Comment.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToContact((QCBDDataSet.contactsDataTable)getDataTableFromSqlQuery<QCBDDataSet.contactsDataTable>(baseSqlString));

            }
            return new List<Contact>();
        }



        //====================================================================================
        //===============================[ Address ]===========================================
        //====================================================================================

        public static List<Address> DataTableTypeToAddress(this QCBDDataSet.addressesDataTable AddressesDataTable)
        {
            object _lock = new object(); List<Address> returnList = new List<Address>();
            if (AddressesDataTable != null)
            {
                foreach (var AddressQCBD in AddressesDataTable)
                {
                    Address Address = new Address();
                    Address.ID = AddressQCBD.ID;
                    Address.AddressName = AddressQCBD.Address;
                    Address.ClientId = AddressQCBD.ClientId;
                    Address.Comment = AddressQCBD.Comment;
                    Address.Email = AddressQCBD.Email;
                    Address.Phone = AddressQCBD.Phone;
                    Address.CityName = AddressQCBD.CityName;
                    Address.Country = AddressQCBD.Country;
                    Address.LastName = AddressQCBD.LastName;
                    Address.FirstName = AddressQCBD.FirstName;
                    Address.Name = AddressQCBD.Name;
                    Address.Name2 = AddressQCBD.Name2;
                    Address.Postcode = AddressQCBD.Postcode;

                    lock (_lock) lock (_lock) returnList.Add(Address);
                }
            }
            return returnList;
        }

        public static QCBDDataSet.addressesDataTable AddressTypeToDataTable(this List<Address> AddressList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.addressesDataTable returnQCBDDataTable = new QCBDDataSet.addressesDataTable();
            if (AddressList != null)
            {
                foreach (var Address in AddressList)
                {
                    QCBDDataSet.addressesRow AddressQCBD = returnQCBDDataTable.NewaddressesRow();
                    AddressQCBD.ID = Address.ID;
                    AddressQCBD.Address = Address.AddressName;
                    AddressQCBD.ClientId = Address.ClientId;
                    AddressQCBD.Comment = Address.Comment;
                    AddressQCBD.Email = Address.Email;
                    AddressQCBD.Phone = Address.Phone;
                    AddressQCBD.CityName = Address.CityName;
                    AddressQCBD.Country = Address.Country;
                    AddressQCBD.LastName = Address.LastName;
                    AddressQCBD.FirstName = Address.FirstName;
                    AddressQCBD.Name = Address.Name;
                    AddressQCBD.Name2 = Address.Name2;
                    AddressQCBD.Postcode = Address.Postcode;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(AddressQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(AddressQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(AddressQCBD);
                            idList.Add(AddressQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Address> AddressTypeToFilterDataTable(this Address Address, string filterOperator)
        {
            if (Address != null)
            {
                string baseSqlString = "SELECT * FROM Addresses WHERE ";
                string defaultSqlString = "SELECT * FROM Addresses WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Address.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Address.ID);
                if (Address.ClientId != 0)
                    query = string.Format(query + " {0} ClientId LIKE '{1}' ", filterOperator, Address.ClientId);
                if (!string.IsNullOrEmpty(Address.Name))
                    query = string.Format(query + " {0} Name LIKE '{1}' ", filterOperator, Address.Name.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Name2))
                    query = string.Format(query + " {0} Name2 LIKE '{1}' ", filterOperator, Address.Name2.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.CityName))
                    query = string.Format(query + " {0} CityName LIKE '{1}' ", filterOperator, Address.CityName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.AddressName))
                    query = string.Format(query + " {0} Address LIKE '{1}' ", filterOperator, Address.AddressName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Postcode))
                    query = string.Format(query + " {0} Postcode LIKE '{1}' ", filterOperator, Address.Postcode.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Country))
                    query = string.Format(query + " {0} Country LIKE '{1}' ", filterOperator, Address.Country.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Comment))
                    query = string.Format(query + " {0} Comment LIKE '{1}' ", filterOperator, Address.Comment.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.FirstName))
                    query = string.Format(query + " {0} FirstName LIKE '{1}' ", filterOperator, Address.FirstName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.LastName))
                    query = string.Format(query + " {0} LastName LIKE '{1}' ", filterOperator, Address.LastName.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Phone))
                    query = string.Format(query + " {0} Phone LIKE '{1}' ", filterOperator, Address.Phone.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Address.Email))
                    query = string.Format(query + " {0} Email LIKE '{1}' ", filterOperator, Address.Email.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToAddress((QCBDDataSet.addressesDataTable)getDataTableFromSqlQuery<QCBDDataSet.addressesDataTable>(baseSqlString));

            }
            return new List<Address>();
        }


        //====================================================================================
        //===============================[ Bill ]===========================================
        //====================================================================================

        public static List<Bill> DataTableTypeToBill(this QCBDDataSet.billsDataTable BillDataTable)
        {
            object _lock = new object(); List<Bill> returnList = new List<Bill>();
            if (BillDataTable != null)
            {
                foreach (var BillQCBD in BillDataTable)
                {
                    Bill Bill = new Bill();
                    Bill.ID = BillQCBD.ID;
                    Bill.ClientId = BillQCBD.ClientId;
                    Bill.CommandId = BillQCBD.CommandId;
                    Bill.Comment1 = BillQCBD.Comment1;
                    Bill.Comment2 = BillQCBD.Comment2;
                    Bill.Date = BillQCBD.Date;
                    Bill.DateLimit = BillQCBD.DateLimit;
                    Bill.Pay = BillQCBD.Pay;
                    Bill.PayDate = BillQCBD.DatePay;
                    Bill.PayMod = BillQCBD.PayMod;
                    Bill.PayReceived = BillQCBD.PayReceived;

                    lock (_lock) returnList.Add(Bill);
                }
            }
            return returnList;
        }

        public static QCBDDataSet.billsDataTable BillTypeToDataTable(this List<Bill> BillList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.billsDataTable returnQCBDDataTable = new QCBDDataSet.billsDataTable();
            if (BillList != null)
            {
                foreach (var Bill in BillList)
                {
                    QCBDDataSet.billsRow BillQCBD = returnQCBDDataTable.NewbillsRow();
                    BillQCBD.ID = Bill.ID;
                    BillQCBD.ClientId = Bill.ClientId;
                    BillQCBD.CommandId = Bill.CommandId;
                    BillQCBD.Comment1 = Bill.Comment1;
                    BillQCBD.Comment2 = Bill.Comment2;
                    BillQCBD.Date = Bill.Date;
                    BillQCBD.DateLimit = Bill.DateLimit;
                    BillQCBD.Pay = Bill.Pay;
                    BillQCBD.DatePay = Bill.PayDate;
                    BillQCBD.PayMod = Bill.PayMod;
                    BillQCBD.PayReceived = Bill.PayReceived;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(BillQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(BillQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(BillQCBD);
                            idList.Add(BillQCBD.ID);
                        }
                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Bill> BillTypeToFilterDataTable(this Bill Bill, string filterOperator)
        {
            if (Bill != null)
            {
                string baseSqlString = "SELECT * FROM Bills WHERE ";
                string defaultSqlString = "SELECT * FROM Bills WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Bill.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Bill.ID);
                if (Bill.ClientId != 0)
                    query = string.Format(query + " {0} ClientId LIKE '{1}' ", filterOperator, Bill.ClientId);
                if (Bill.CommandId != 0)
                    query = string.Format(query + " {0} CommandId LIKE '{1}' ", filterOperator, Bill.CommandId);
                if (Bill.Pay != 0)
                    query = string.Format(query + " {0} Pay LIKE '{1}' ", filterOperator, Bill.Pay);
                if (!string.IsNullOrEmpty(Bill.PayMod))
                    query = string.Format(query + " {0} PayMod LIKE '{1}' ", filterOperator, Bill.PayMod);
                if (Bill.PayReceived != 0)
                    query = string.Format(query + " {0} PayReceived LIKE '{1}' ", filterOperator, Bill.PayReceived);
                if (!string.IsNullOrEmpty(Bill.Comment2))
                    query = string.Format(query + " {0} Comment2 LIKE '{1}' ", filterOperator, Bill.Comment2.Replace("'", "''"));
                /*if (Bill.Date != null)
                    query = string.Format(query + " {0} Date LIKE '{1}' ", filterOperator, Bill.Date);*/
                if (!string.IsNullOrEmpty(Bill.Comment1))
                    query = string.Format(query + " {0} Comment1 LIKE '{1}' ", filterOperator, Bill.Comment1.Replace("'", "''"));
                /*if (Bill.DateLimit != null)
                    query = string.Format(query + " {0} DateLimit LIKE '{1}' ", filterOperator, Bill.DateLimit);
                if (Bill.PayDate != null)
                    query = string.Format(query + " {0} DatePay LIKE '{1}' ", filterOperator, Bill.PayDate);*/

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToBill((QCBDDataSet.billsDataTable)DALHelper.getDataTableFromSqlQuery<QCBDDataSet.billsDataTable>(baseSqlString));

            }
            return new List<Bill>();
        }

        public static Bill LastBill()
        {
            string baseSqlString = "SELECT TOP 1 * FROM Bills ORDER BY ID DESC ";

            var FoundList = DataTableTypeToBill((QCBDDataSet.billsDataTable)DALHelper.getDataTableFromSqlQuery<QCBDDataSet.billsDataTable>(baseSqlString));
            if (FoundList.Count > 0)
                return FoundList[0];

            return new Bill();
        }

        //====================================================================================
        //===============================[ Delivery ]===========================================
        //====================================================================================

        public static List<Delivery> DataTableTypeToDelivery(this QCBDDataSet.deliveriesDataTable DeliveryDataTable)
        {
            object _lock = new object(); List<Delivery> returnList = new List<Delivery>();
            if (DeliveryDataTable != null)
            {
                foreach (var DeliveryQCBD in DeliveryDataTable)
                {
                    Delivery Delivery = new Delivery();
                    Delivery.ID = DeliveryQCBD.ID;
                    Delivery.BillId = DeliveryQCBD.BillId;
                    Delivery.CommandId = DeliveryQCBD.CommandId;
                    Delivery.Date = DeliveryQCBD.Date;
                    Delivery.Package = DeliveryQCBD.Package;
                    Delivery.Status = DeliveryQCBD.Status;

                    lock (_lock) returnList.Add(Delivery);
                }
            }
            return returnList;
        }

        public static QCBDDataSet.deliveriesDataTable DeliveryTypeToDataTable(this List<Delivery> DeliveryList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.deliveriesDataTable returnQCBDDataTable = new QCBDDataSet.deliveriesDataTable();
            if (DeliveryList != null)
            {
                foreach (var Delivery in DeliveryList)
                {
                    QCBDDataSet.deliveriesRow DeliveryQCBD = returnQCBDDataTable.NewdeliveriesRow();
                    DeliveryQCBD.ID = Delivery.ID;
                    DeliveryQCBD.BillId = Delivery.BillId;
                    DeliveryQCBD.CommandId = Delivery.CommandId;
                    DeliveryQCBD.Date = Delivery.Date;
                    DeliveryQCBD.Package = Delivery.Package;
                    DeliveryQCBD.Status = Delivery.Status;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(DeliveryQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(DeliveryQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(DeliveryQCBD);
                            idList.Add(DeliveryQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Delivery> DeliveryTypeToFilterDataTable(this Delivery Delivery, string filterOperator)
        {
            if (Delivery != null)
            {
                string baseSqlString = "SELECT * FROM Deliveries WHERE ";
                string defaultSqlString = "SELECT * FROM Deliveries WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Delivery.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Delivery.ID);
                if (!string.IsNullOrEmpty(Delivery.Status))
                    query = string.Format(query + " {0} Status LIKE '{1}' ", filterOperator, Delivery.Status);
                if (Delivery.CommandId != 0)
                    query = string.Format(query + " {0} CommandId LIKE '{1}' ", filterOperator, Delivery.CommandId);
                if (Delivery.BillId != 0)
                    query = string.Format(query + " {0} BillId LIKE '{1}' ", filterOperator, Delivery.BillId);
                if (Delivery.Package != 0)
                    query = string.Format(query + " {0} Package LIKE '{1}' ", filterOperator, Delivery.Package);
                /*if (Delivery.Date != null)
                    query = string.Format(query + " {0} Date LIKE '{1}' ", filterOperator, Delivery.Date);*/

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToDelivery((QCBDDataSet.deliveriesDataTable)DALHelper.getDataTableFromSqlQuery<QCBDDataSet.deliveriesDataTable>(baseSqlString));

            }
            return new List<Delivery>();
        }

        //====================================================================================
        //================================[ Command_item ]====================================
        //====================================================================================

        public static List<Command_item> DataTableTypeToCommand_item(this QCBDDataSet.command_itemsDataTable Command_itemDataTable)
        {
            object _lock = new object(); List<Command_item> returnList = new List<Command_item>();
            if (Command_itemDataTable != null)
            {
                //foreach (var Command_itemQCBD in Command_itemDataTable)
                Parallel.ForEach(Command_itemDataTable, (Command_itemQCBD) =>
                {
                    Command_item Command_item = new Command_item();
                    Command_item.ID = Command_itemQCBD.ID;
                    Command_item.CommandId = Command_itemQCBD.CommandId;
                    Command_item.Comment_Purchase_Price = Command_itemQCBD.Comment_Purchase_Price;
                    Command_item.Item_ref = Command_itemQCBD.Item_ref;
                    Command_item.Order = Command_itemQCBD.Order;
                    Command_item.Price = Command_itemQCBD.Price;
                    Command_item.Price_purchase = Command_itemQCBD.Price_purchase;
                    Command_item.Quantity = Command_itemQCBD.Quantity;
                    Command_item.Quantity_current = Command_itemQCBD.Quantity_current;
                    Command_item.Quantity_delivery = Command_itemQCBD.Quantity_delivery;

                    lock (_lock) returnList.Add(Command_item);
                });
            }
            var test = returnList.Where(x => x.CommandId == 3410).ToList(); ;
            return returnList;
        }

        public static QCBDDataSet.command_itemsDataTable Command_itemTypeToDataTable(this List<Command_item> Command_itemList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.command_itemsDataTable returnQCBDDataTable = new QCBDDataSet.command_itemsDataTable();
            if (Command_itemList != null)
            {
                foreach (var Command_item in Command_itemList)
                {
                    QCBDDataSet.command_itemsRow Command_itemQCBD = returnQCBDDataTable.Newcommand_itemsRow();
                    Command_itemQCBD.ID = Command_item.ID;
                    Command_itemQCBD.CommandId = Command_item.CommandId;
                    Command_itemQCBD.Comment_Purchase_Price = Command_item.Comment_Purchase_Price;
                    Command_itemQCBD.Item_ref = Command_item.Item_ref;
                    Command_itemQCBD.Order = Command_item.Order;
                    Command_itemQCBD.Price = Command_item.Price;
                    Command_itemQCBD.Price_purchase = Command_item.Price_purchase;
                    Command_itemQCBD.Quantity = Command_item.Quantity;
                    Command_itemQCBD.Quantity_current = Command_item.Quantity_current;
                    Command_itemQCBD.Quantity_delivery = Command_item.Quantity_delivery;

                    lock (_lock)
                    {
                        if (!idList.Contains(Command_itemQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(Command_itemQCBD);
                            idList.Add(Command_itemQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Command_item> Command_itemTypeToFilterDataTable(this Command_item Command_item, string filterOperator)
        {
            if (Command_item != null)
            {
                string baseSqlString = "SELECT * FROM Command_items WHERE ";
                string defaultSqlString = "SELECT * FROM Command_items WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Command_item.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Command_item.ID);
                if (Command_item.CommandId != 0)
                    query = string.Format(query + " {0} CommandId LIKE '{1}' ", filterOperator, Command_item.CommandId);
                if (Command_item.Quantity != 0)
                    query = string.Format(query + " {0} Quantity LIKE '{1}' ", filterOperator, Command_item.Quantity);
                if (Command_item.Quantity_delivery != 0)
                    query = string.Format(query + " {0} Quantity_delivery LIKE '{1}' ", filterOperator, Command_item.Quantity_delivery);
                if (!string.IsNullOrEmpty(Command_item.Item_ref))
                    query = string.Format(query + " {0} Item_ref LIKE '{1}' ", filterOperator, Command_item.Item_ref);
                if (Command_item.ItemId != 0)
                    query = string.Format(query + " {0} ItemId LIKE '{1}' ", filterOperator, Command_item.ItemId);
                if (Command_item.Quantity_current != 0)
                    query = string.Format(query + " {0} Quantity_current LIKE '{1}' ", filterOperator, Command_item.Quantity_current);
                if (Command_item.Price != 0)
                    query = string.Format(query + " {0} Price LIKE '{1}' ", filterOperator, Command_item.Price);
                if (Command_item.Price_purchase != 0)
                    query = string.Format(query + " {0} Price_purchase LIKE '{1}' ", filterOperator, Command_item.Price_purchase);
                if (!string.IsNullOrEmpty(Command_item.Comment_Purchase_Price))
                    query = string.Format(query + " {0} Comment_Purchase_Price LIKE '{1}' ", filterOperator, Command_item.Comment_Purchase_Price.Replace("'", "''"));
                if (Command_item.Order != 0)
                    query = string.Format(query + " {0} [Order] LIKE '{1}' ", filterOperator, Command_item.Order);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToCommand_item((QCBDDataSet.command_itemsDataTable)getDataTableFromSqlQuery<QCBDDataSet.command_itemsDataTable>(baseSqlString));

            }

            return new List<Command_item>();

        }


        //====================================================================================
        //==================================[ Tax ]===========================================
        //====================================================================================

        public static List<Tax> DataTableTypeToTax(this QCBDDataSet.taxesDataTable TaxDataTable)
        {
            object _lock = new object(); List<Tax> returnList = new List<Tax>();
            if (TaxDataTable != null)
            {
                foreach (var TaxQCBD in TaxDataTable)
                {
                    Tax Tax = new Tax();
                    Tax.ID = TaxQCBD.ID;
                    Tax.Tax_current = TaxQCBD.Tax_current;
                    Tax.Type = TaxQCBD.Type;
                    Tax.Value = TaxQCBD.Value;
                    Tax.Date_insert = TaxQCBD.Date_insert;
                    Tax.Comment = TaxQCBD.Comment;

                    lock (_lock) returnList.Add(Tax);
                }
            }
            return returnList;
        }

        public static QCBDDataSet.taxesDataTable TaxTypeToDataTable(this List<Tax> TaxList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.taxesDataTable returnQCBDDataTable = new QCBDDataSet.taxesDataTable();
            if (TaxList != null)
            {
                foreach (var Tax in TaxList)
                {
                    QCBDDataSet.taxesRow TaxQCBD = returnQCBDDataTable.NewtaxesRow();
                    TaxQCBD.ID = Tax.ID;
                    TaxQCBD.Tax_current = Tax.Tax_current;
                    TaxQCBD.Type = Tax.Type;
                    TaxQCBD.Value = Tax.Value;
                    TaxQCBD.Date_insert = Tax.Date_insert;
                    TaxQCBD.Comment = Tax.Comment;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(TaxQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(TaxQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(TaxQCBD);
                            idList.Add(TaxQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Tax> TaxTypeToFilterDataTable(this Tax Tax, string filterOperator)
        {
            if (Tax != null)
            {
                string baseSqlString = "SELECT * FROM Taxes WHERE ";
                string defaultSqlString = "SELECT * FROM Taxes WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Tax.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Tax.ID);
                if (!string.IsNullOrEmpty(Tax.Type))
                    query = string.Format(query + " {0} Type LIKE '{1}' ", filterOperator, Tax.Type);
                /*if (Tax.Date_insert != null)
                    query = string.Format(query + " {0} Date_insert LIKE '{1}' ", filterOperator, Tax.Date_insert);*/
                if (Tax.Value != 0)
                    query = string.Format(query + " {0} Value LIKE '{1}' ", filterOperator, Tax.Value);
                if (Tax.Tax_current != 0)
                    query = string.Format(query + " {0} Tax_current LIKE '{1}' ", filterOperator, Tax.Tax_current);
                if (!string.IsNullOrEmpty(Tax.Comment))
                    query = string.Format(query + " {0} Comment LIKE '{1}' ", filterOperator, Tax.Comment.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToTax((QCBDDataSet.taxesDataTable)DALHelper.getDataTableFromSqlQuery<QCBDDataSet.taxesDataTable>(baseSqlString));

            }
            return new List<Tax>();
        }



        //====================================================================================
        //===============================[ Provider_item ]===========================================
        //====================================================================================

        public static List<Provider_item> DataTableTypeToProvider_item(this QCBDDataSet.provider_itemsDataTable Provider_itemDataTable)
        {
            object _lock = new object(); List<Provider_item> returnList = new List<Provider_item>();
            if (Provider_itemDataTable != null)
            {
                foreach (var Provider_itemQCBD in Provider_itemDataTable)
                {
                    Provider_item Provider_item = new Provider_item();
                    Provider_item.ID = Provider_itemQCBD.ID;
                    Provider_item.Item_ref = Provider_itemQCBD.Item_ref;
                    Provider_item.Provider_name = Provider_itemQCBD.Provider_name;

                    lock (_lock) returnList.Add(Provider_item);
                }
            }
            return returnList;
        }

        public static QCBDDataSet.provider_itemsDataTable Provider_itemTypeToDataTable(this List<Provider_item> Provider_itemList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.provider_itemsDataTable returnQCBDDataTable = new QCBDDataSet.provider_itemsDataTable();
            if (Provider_itemList != null)
            {
                foreach (var Provider_item in Provider_itemList)
                {
                    QCBDDataSet.provider_itemsRow Provider_itemQCBD = returnQCBDDataTable.Newprovider_itemsRow();
                    Provider_itemQCBD.ID = Provider_item.ID;
                    Provider_itemQCBD.Item_ref = Provider_item.Item_ref;
                    Provider_itemQCBD.Provider_name = Provider_item.Provider_name;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(Provider_itemQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(Provider_itemQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(Provider_itemQCBD);
                            idList.Add(Provider_itemQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Provider_item> Provider_itemTypeToFilterDataTable(this Provider_item Provider_item, string filterOperator)
        {
            if (Provider_item != null)
            {
                string baseSqlString = "SELECT * FROM Provider_items WHERE ";
                string defaultSqlString = "SELECT * FROM Provider_items WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Provider_item.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Provider_item.ID);
                if (!string.IsNullOrEmpty(Provider_item.Provider_name))
                    query = string.Format(query + " {0} Provider_name LIKE '{1}' ", filterOperator, Provider_item.Provider_name.Replace("'", "''"));
                if (!string.IsNullOrEmpty(Provider_item.Item_ref))
                    query = string.Format(query + " {0} Item_ref LIKE '{1}' ", filterOperator, Provider_item.Item_ref.Replace("'", "''"));

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToProvider_item((QCBDDataSet.provider_itemsDataTable)DALHelper.getDataTableFromSqlQuery<QCBDDataSet.provider_itemsDataTable>(baseSqlString));

            }
            return new List<Provider_item>();
        }

        //====================================================================================
        //===============================[ Provider ]===========================================
        //====================================================================================

        public static List<Provider> DataTableTypeToProvider(this QCBDDataSet.providersDataTable ProviderDataTable)
        {
            object _lock = new object(); List<Provider> returnList = new List<Provider>();
            if (ProviderDataTable != null)
            {
                foreach (var ProviderQCBD in ProviderDataTable)
                {
                    Provider Provider = new Provider();
                    Provider.ID = ProviderQCBD.ID;
                    Provider.Name = ProviderQCBD.Name;
                    Provider.Source = ProviderQCBD.Source;

                    lock (_lock) returnList.Add(Provider);
                }
            }
            return returnList;
        }

        public static QCBDDataSet.providersDataTable ProviderTypeToDataTable(this List<Provider> ProviderList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.providersDataTable returnQCBDDataTable = new QCBDDataSet.providersDataTable();
            if (ProviderList != null)
            {
                foreach (var Provider in ProviderList)
                {
                    QCBDDataSet.providersRow ProviderQCBD = returnQCBDDataTable.NewprovidersRow();
                    ProviderQCBD.ID = Provider.ID;
                    ProviderQCBD.Name = Provider.Name;
                    ProviderQCBD.Source = Provider.Source;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(ProviderQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(ProviderQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(ProviderQCBD);
                            idList.Add(ProviderQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Provider> ProviderTypeToFilterDataTable(this Provider Provider, string filterOperator)
        {
            if (Provider != null)
            {
                string baseSqlString = "SELECT * FROM Providers WHERE ";
                string defaultSqlString = "SELECT * FROM Providers WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Provider.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Provider.ID);
                if (!string.IsNullOrEmpty(Provider.Name))
                    query = string.Format(query + " {0} Name LIKE '{1}' ", filterOperator, Provider.Name.Replace("'", "''"));
                if (Provider.Source != 0)
                    query = string.Format(query + " {0} Source LIKE '{1}' ", filterOperator, Provider.Source);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToProvider((QCBDDataSet.providersDataTable)DALHelper.getDataTableFromSqlQuery<QCBDDataSet.providersDataTable>(baseSqlString));

            }
            return new List<Provider>();
        }

        //====================================================================================
        //===============================[ Item ]===========================================
        //====================================================================================

        public static List<Item> DataTableTypeToItem(this QCBDDataSet.itemsDataTable ItemDataTable)
        {
            object _lock = new object(); List<Item> returnList = new List<Item>();
            if (ItemDataTable != null)
            {
                //foreach (var ItemQCBD in ItemDataTable)
                Parallel.ForEach(ItemDataTable, (ItemQCBD) =>
                {
                    Item Item = new Item();
                    Item.ID = ItemQCBD.ID;
                    Item.Comment = ItemQCBD.Comment;
                    Item.Erasable = ItemQCBD.Erasable;
                    Item.Name = ItemQCBD.Name;
                    Item.Price_purchase = ItemQCBD.Price_purchase;
                    Item.Price_sell = ItemQCBD.Price_sell;
                    Item.Ref = ItemQCBD.Ref;
                    Item.Type_sub = ItemQCBD.Type_sub;
                    Item.Source = ItemQCBD.Source;
                    Item.Type = ItemQCBD.Type;

                    lock (_lock) returnList.Add(Item);
                });
            }
            return returnList;
        }

        public static QCBDDataSet.itemsDataTable ItemTypeToDataTable(this List<Item> ItemList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.itemsDataTable returnQCBDDataTable = new QCBDDataSet.itemsDataTable();
            if (ItemList != null)
            {
                foreach (var Item in ItemList)
                {
                    QCBDDataSet.itemsRow ItemQCBD = returnQCBDDataTable.NewitemsRow();
                    ItemQCBD.ID = Item.ID;
                    ItemQCBD.Comment = Item.Comment;
                    ItemQCBD.Erasable = Item.Erasable;
                    ItemQCBD.Name = Item.Name;
                    ItemQCBD.Price_purchase = Item.Price_purchase;
                    ItemQCBD.Price_sell = Item.Price_sell;
                    ItemQCBD.Ref = Item.Ref;
                    ItemQCBD.Type_sub = Item.Type_sub;
                    ItemQCBD.Source = Item.Source;
                    ItemQCBD.Type = Item.Type;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(ItemQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(ItemQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(ItemQCBD);
                            idList.Add(ItemQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Item> ItemTypeToFilterDataTable(this Item item, string filterOperator)
        {
            if (item != null)
            {
                string baseSqlString = "SELECT * FROM items WHERE ";
                string defaultSqlString = "SELECT * FROM items WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (item.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, item.ID);
                if (item.Price_purchase != 0)
                    query = string.Format(query + " {0} Price_purchase LIKE '{1}' ", filterOperator, item.Price_purchase);
                if (!string.IsNullOrEmpty(item.Ref))
                    query = string.Format(query + " {0} Ref LIKE '{1}' ", filterOperator, item.Ref.Replace("'", "''"));
                if (!string.IsNullOrEmpty(item.Name))
                    query = string.Format(query + " {0} Name LIKE '%{1}%' ", filterOperator, item.Name.Replace("'", "''"));
                if (!string.IsNullOrEmpty(item.Type))
                    query = string.Format(query + " {0} Type LIKE '{1}' ", filterOperator, item.Type);
                if (!string.IsNullOrEmpty(item.Type_sub))
                    query = string.Format(query + " {0} Type_sub LIKE '{1}' ", filterOperator, item.Type_sub);
                if (item.Price_sell != 0)
                    query = string.Format(query + " {0} Price_sell LIKE '{1}' ", filterOperator, item.Price_sell);
                if (item.Source != 0)
                    query = string.Format(query + " {0} Source LIKE '{1}' ", filterOperator, item.Source);
                if (!string.IsNullOrEmpty(item.Comment))
                    query = string.Format(query + " {0} Comment LIKE '%{1}%' ", filterOperator, item.Comment.Replace("'", "''"));
                if (!string.IsNullOrEmpty(item.Erasable))
                    query = string.Format(query + " {0} Erasable LIKE '{1}' ", filterOperator, item.Erasable);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToItem((QCBDDataSet.itemsDataTable)DALHelper.getDataTableFromSqlQuery<QCBDDataSet.itemsDataTable>(baseSqlString));

            }
            return new List<Item>();
        }




        //====================================================================================
        //===============================[ Item_delivery ]===========================================
        //====================================================================================


        public static List<Item_delivery> DataTableTypeToItem_delivery(this QCBDDataSet.item_deliveriesDataTable Item_deliveryDataTable)
        {
            object _lock = new object(); List<Item_delivery> returnList = new List<Item_delivery>();
            if (Item_deliveryDataTable != null)
            {
                foreach (var Item_deliveryQCBD in Item_deliveryDataTable)
                {
                    Item_delivery Item_delivery = new Item_delivery();
                    Item_delivery.ID = Item_deliveryQCBD.ID;
                    Item_delivery.DeliveryId = Item_deliveryQCBD.DeliveryId;
                    Item_delivery.Item_ref = Item_deliveryQCBD.Item_ref;
                    Item_delivery.Quantity_delivery = Item_deliveryQCBD.Quantity_delivery;

                    lock (_lock) returnList.Add(Item_delivery);
                }
            }
            return returnList;
        }

        public static QCBDDataSet.item_deliveriesDataTable Item_deliveryTypeToDataTable(this List<Item_delivery> Item_deliveryList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.item_deliveriesDataTable returnQCBDDataTable = new QCBDDataSet.item_deliveriesDataTable();
            if (Item_deliveryList != null)
            {
                foreach (var Item_delivery in Item_deliveryList)
                {
                    QCBDDataSet.item_deliveriesRow Item_deliveryQCBD = returnQCBDDataTable.Newitem_deliveriesRow();
                    Item_deliveryQCBD.ID = Item_delivery.ID;
                    Item_deliveryQCBD.DeliveryId = Item_delivery.DeliveryId;
                    Item_deliveryQCBD.Item_ref = Item_delivery.Item_ref;
                    Item_deliveryQCBD.Quantity_delivery = Item_delivery.Quantity_delivery;

                    //lock(_lock) returnQCBDDataTable.Rows.Add(Item_deliveryQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(Item_deliveryQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(Item_deliveryQCBD);
                            idList.Add(Item_deliveryQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Item_delivery> Item_deliveryTypeToFilterDataTable(this Item_delivery Item_delivery, string filterOperator)
        {
            if (Item_delivery != null)
            {
                string baseSqlString = "SELECT * FROM Item_deliveries WHERE ";
                string defaultSqlString = "SELECT * FROM Item_deliveries WHERE 1=0 ";
                object _lock = new object(); string query = "";

                if (Item_delivery.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Item_delivery.ID);
                if (Item_delivery.DeliveryId != 0)
                    query = string.Format(query + " {0} DeliveryId LIKE '{1}' ", filterOperator, Item_delivery.DeliveryId);
                if (!string.IsNullOrEmpty(Item_delivery.Item_ref))
                    query = string.Format(query + " {0} Item_ref LIKE '{1}' ", filterOperator, Item_delivery.Item_ref.Replace("'", "''"));
                if (Item_delivery.Quantity_delivery != 0)
                    query = string.Format(query + " {0} Quantity_delivery LIKE '{1}' ", filterOperator, Item_delivery.Quantity_delivery);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToItem_delivery((QCBDDataSet.item_deliveriesDataTable)DALHelper.getDataTableFromSqlQuery<QCBDDataSet.item_deliveriesDataTable>(baseSqlString));

            }
            return new List<Item_delivery>();
        }

        //====================================================================================
        //===============================[ Tax_item ]===========================================
        //====================================================================================


        public static List<Tax_item> DataTableTypeToTax_item(this QCBDDataSet.tax_itemsDataTable Tax_itemDataTable)
        {
            object _lock = new object(); List<Tax_item> returnList = new List<Tax_item>();
            if (Tax_itemDataTable != null)
            {
                foreach (var Tax_itemQCBD in Tax_itemDataTable)
                {
                    Tax_item Tax_item = new Tax_item();
                    Tax_item.ID = Tax_itemQCBD.ID;
                    Tax_item.Item_ref = Tax_itemQCBD.Item_ref;
                    Tax_item.Tax_value = Tax_itemQCBD.Tax_value;
                    Tax_item.Tax_type = Tax_itemQCBD.Tax_type;
                    Tax_item.TaxId = Tax_itemQCBD.TaxId;

                    lock (_lock) returnList.Add(Tax_item);
                }
            }
            return returnList;
        }

        public static QCBDDataSet.tax_itemsDataTable Tax_itemTypeToDataTable(this List<Tax_item> Tax_itemList)
        {
            object _lock = new object();
            List<int> idList = new List<int>();
            QCBDDataSet.tax_itemsDataTable returnQCBDDataTable = new QCBDDataSet.tax_itemsDataTable();
            if (Tax_itemList != null)
            {
                foreach (var Tax_item in Tax_itemList)
                {
                    QCBDDataSet.tax_itemsRow Tax_itemQCBD = returnQCBDDataTable.Newtax_itemsRow();
                    Tax_itemQCBD.ID = Tax_item.ID;
                    Tax_itemQCBD.Item_ref = Tax_item.Item_ref;
                    Tax_itemQCBD.Tax_value = Tax_item.Tax_value;
                    Tax_itemQCBD.Tax_type = Tax_item.Tax_type;
                    Tax_itemQCBD.TaxId = Tax_item.TaxId;

                    //lock (_lock) returnQCBDDataTable.Rows.Add(Tax_itemQCBD);
                    lock (_lock)
                    {
                        if (!idList.Contains(Tax_itemQCBD.ID))
                        {
                            returnQCBDDataTable.Rows.Add(Tax_itemQCBD);
                            idList.Add(Tax_itemQCBD.ID);
                        }

                    }
                }
            }
            return returnQCBDDataTable;
        }

        public static List<Tax_item> Tax_itemTypeToFilterDataTable(this Tax_item Tax_item, string filterOperator)
        {
            if (Tax_item != null)
            {
                string baseSqlString = "SELECT * FROM tax_items WHERE ";
                string defaultSqlString = "SELECT * FROM tax_items WHERE 1=0 ";
                object _lock = new object(); string query = "";

                /*Tax_itemQCBD.ID = Tax_item.ID;
                    Tax_itemQCBD.Item_ref = Tax_item.Item_ref;
                    Tax_itemQCBD.Tax_type = Tax_item.Tax_type;
                    Tax_itemQCBD.TaxId = Tax_item.TaxId;*/

                if (Tax_item.ID != 0)
                    query = string.Format(query + " {0} ID LIKE '{1}' ", filterOperator, Tax_item.ID);
                if (Tax_item.Tax_value != 0)
                    query = string.Format(query + " {0} Tax_value LIKE '{1}' ", filterOperator, Tax_item.Tax_value);
                if (!string.IsNullOrEmpty(Tax_item.Item_ref))
                    query = string.Format(query + " {0} Item_ref LIKE '{1}' ", filterOperator, Tax_item.Item_ref.Replace("'", "''"));
                if (Tax_item.TaxId != 0)
                    query = string.Format(query + " {0} TaxId LIKE '{1}' ", filterOperator, Tax_item.TaxId);

                lock (_lock)
                    if (!string.IsNullOrEmpty(query))
                        baseSqlString = baseSqlString + query.Substring(query.IndexOf(filterOperator) + filterOperator.Length);
                    else
                        baseSqlString = defaultSqlString;

                return DataTableTypeToTax_item((QCBDDataSet.tax_itemsDataTable)DALHelper.getDataTableFromSqlQuery<QCBDDataSet.tax_itemsDataTable>(baseSqlString));

            }
            return new List<Tax_item>();
        }








    }
}
