// FILE: D:/Just IT Training/BillManagment/Classes//IStatisticManager.cs

// In this section you can add your own using directives
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:0000000000000A26 begin
// section -64--88-0-12--60c16d7a:1535b1c1c11:-8000:0000000000000A26 end

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
    public interface IStatisticManager
    {
        // Operations

        Task<List<Statistic>> InsertStatistic(List<Statistic> statisticList);

        Task<List<Statistic>> UpdateStatistic(List<Statistic> statisticList);

        Task<List<Statistic>> DeleteStatistic(List<Statistic> statisticList);

        Task<List<Statistic>> GetStatisticData(int nbLine);

        Task<List<Statistic>> SearchStatisitc(Statistic statistic, string filterOperator);

        Task<List<Statistic>> GetStatisticDataById(int id);

        Task<List<Statistic>> searchStatisticFromWebService(Statistic statisitic, string filterOperator);
    } /* end interface IStatisticManager */
}