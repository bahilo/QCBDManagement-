using QCBDManagementCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBDManagementCommon.Interfaces.BL
{
    public interface IInfosManager
    {
        Task<List<Infos>> InsertInfos(List<Infos> listInfos);

        Task<List<Infos>> UpdateInfos(List<Infos> listInfos);

        Task<List<Infos>> DeleteInfos(List<Infos> listInfos);

        Task<List<Infos>> GetInfosData(int nbLine);

        Task<List<Infos>> searchInfos(Infos Infos, string filterOperator);

        Task<List<Infos>> searchInfosFromWebService(Infos infos, string filterOperator);

        Task<List<Infos>> GetInfosDataById(int id);
    }
}
