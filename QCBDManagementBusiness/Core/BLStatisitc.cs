using QCBDManagementCommon.Classes;
using QCBDManagementCommon.Entities;
using QCBDManagementCommon.Interfaces.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/// <summary>
///  A class that represents ...
/// 
///  @see OtherClasses
///  @author your_name_here
/// </summary>
namespace QCBDManagementBusiness.Core
{
    public class BLStatisitc : IStatisticManager
    {
        // Attributes

        public QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DAC;

        public BLStatisitc(QCBDManagementCommon.Interfaces.DAC.IDataAccessManager DataAccessComponent)
        {
            DAC = DataAccessComponent;
        }

        public async Task<List<Statistic>> InsertStatistic(List<Statistic> statisticList)
        {
            if (statisticList == null || statisticList.Count == 0)
                return new List<Statistic>();

            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.InsertStatistic(statisticList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Statistic>> DeleteStatistic(List<Statistic> statisticList)
        {
            if (statisticList == null || statisticList.Count == 0)
                return new List<Statistic>();
            
            if (statisticList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Deleting statistics(count = " + statisticList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.DeleteStatistic(statisticList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Statistic>> UpdateStatistic(List<Statistic> statisticList)
        {
            if (statisticList == null || statisticList.Count == 0)
                return new List<Statistic>();

            if (statisticList.Where(x => x.ID == 0).Count() > 0)
                Log.write("Updating statistics(count = " + statisticList.Where(x => x.ID == 0).Count() + ") with ID = 0", "WAR");

            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.UpdateStatistic(statisticList);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public  async Task<List<Statistic>> GetStatisticData(int nbLine)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.GetStatisticData(nbLine);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public  async Task<List<Statistic>> GetStatisticDataById(int id)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.GetStatisticDataById(id);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public async Task<List<Statistic>> searchStatisticFromWebService(Statistic statistic, string filterOperator)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.searchStatisticFromWebService(statistic, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }

        public  async Task<List<Statistic>> SearchStatisitc(Statistic statistic, string filterOperator)
        {
            List<Statistic> result = new List<Statistic>();
            try
            {
                result = await DAC.DALStatistic.SearchStatisitc(statistic, filterOperator);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
            }
            return result;
        }
    } /* end class BLStatisitc */
}